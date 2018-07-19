using Microsoft.Build.BuildEngine;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Execution;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WinForms=System.Windows.Forms;
using PluginContracts;
using System.Reflection;

namespace VisualStudent
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public class MyPluginInfo:INotifyPropertyChanged//class used for storing plugins and info which ones are enabled
    {
        public MyPluginInfo(IPlugin p,bool b)
        {
            PluginInfo = p;
            Enabled = b;
        }
        public IPlugin pluginInfo;
        bool enabled;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public IPlugin PluginInfo
        {
            get => pluginInfo;
            set
            {
                pluginInfo = value;
                OnPropertyChanged(nameof(Enabled));
            }
        }
        public bool Enabled
        {
            get => enabled;
            set
            {
                enabled = value;
                OnPropertyChanged(nameof(Enabled));
            }
        }
    }
    public class MyErrorInfo//my class for storing info about compilation errors
    {
        public MyErrorInfo(string s1,string s2,string s3)
        {
            ErrorSource = s1;
            ErrorNumber = s2;
            ErrorDescription = s3;
        }
        public string ErrorNumber { get; set; }
        public string ErrorDescription { get; set; }
        public string ErrorSource { get; set; }
    }
    public class BindableRichTextBox : RichTextBox//my class for being able to bind to Text in RichTextBox
    {
        public static readonly DependencyProperty DocumentProperty =
            DependencyProperty.Register("Document", typeof(FlowDocument),
            typeof(BindableRichTextBox), new FrameworkPropertyMetadata
            (null, new PropertyChangedCallback(OnDocumentChanged)));

        public new FlowDocument Document
        {
            get
            {
                return (FlowDocument)this.GetValue(DocumentProperty);
            }

            set
            {
                this.SetValue(DocumentProperty, value);
            }
        }

        public static void OnDocumentChanged(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            RichTextBox rtb = (RichTextBox)obj;
            rtb.Document = (FlowDocument)args.NewValue;
        }
    }


    public class TreeItem : INotifyPropertyChanged //base class for storing info about directories and files
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class FolderItem /*class for storing info about directories*/ : TreeItem
    {
        public FolderItem() => Elems = new ObservableCollection<TreeItem>();
        private DirectoryInfo info;
        private ObservableCollection<TreeItem> elems;
        private string usableName;
        
        public string UsableName
        {
            get => usableName;
            set
            {
                usableName = value;
                OnPropertyChanged("UsableName");
            }
        }
        public DirectoryInfo Info
        {
            get => info;
            set
            {
                info = value;
                OnPropertyChanged("Info");
            }
        }
        public ObservableCollection<TreeItem> Elems
        {
            get => elems;
            set
            {
                elems = value;
                OnPropertyChanged("Elems");
            }
        }
    }

    public class FileItem/*class for storing info about files*/ : TreeItem
    {
        private FileInfo info;
        public FileInfo Info
        {
            get => info;
            set
            {
                info = value;
                OnPropertyChanged("Info");
            }
        }
    }

    public partial class MainWindow : Window,INotifyPropertyChanged
    {
        public ObservableCollection<MyPluginInfo> PluginsCollection { get; set; }//collection of plugins
        public string Output
        {
            get => output;
            set
            {
                output = value;
                OnPropertyChanged(nameof(Output));
            }
        }
        private string output;

        private FolderItem folders;

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<MyErrorInfo> ErrorsCollection { get; set; }
        protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public FolderItem Folders//main directory of solution
        {
            get => folders;
            set
            {
                folders=value;
                OnPropertyChanged(nameof(Folders));
            }
        }

        private void InitializeCommands()
        {
            ;
        }

        private void InitiliazeProperties()
        {
            Folders = new FolderItem();
            ErrorsCollection = new ObservableCollection<MyErrorInfo>();
            PluginsCollection = new ObservableCollection<MyPluginInfo>();
            PluginsCollection.Add(new MyPluginInfo(new MySyntaxHighlight1(), false));
            PluginsCollection.Add(new MyPluginInfo(new MySyntaxHighLight2(), false));
            GetFilesInExeDirectory();
        }
        public MainWindow()
        {
            InitializeComponent();
            InitiliazeProperties();
            InitializeCommands();
            this.DataContext = this;
        }
        private void GetFilesInExeDirectory()//looks for*.dll files and tries adding them to plugins collection
        {
            foreach (string s in Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll"))
                TryLoadingPlugin(s);
        }
        private void TryLoadingPlugin(string path)
        {
            Assembly asm = Assembly.LoadFrom(path);
            foreach (Type t in asm.GetTypes())
            {
                foreach (Type iface in t.GetInterfaces())
                {
                    if (iface.Equals(typeof(IPlugin)))
                    {
                        PluginsCollection.Add(new MyPluginInfo((IPlugin)Activator.CreateInstance(t), false));
                        break;
                    }
                }
            }
        }

        private bool ExecuteBuild()//Builds currently open project
        {

            bool status=true;
            string assemblyExecutionPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string logFilePath = assemblyExecutionPath.Remove(assemblyExecutionPath.LastIndexOf("\\") + 1) + "build.log";

            string path = (Folders.Elems[0] as FolderItem).Info.FullName;
            path = path.Substring(0, path.LastIndexOf('\\'));
            path = Directory.GetFiles(path, "*.sln")[0];
            //path = Directory.GetFiles(path, "*.csproj")[0];
            try
            {
                FileLogger logger = new FileLogger();
                logger.Parameters = @"logfile=" + logFilePath;

                ProjectCollection pc = new ProjectCollection();
                Dictionary<string, string> GlobalProperty = new Dictionary<string, string>();
                GlobalProperty.Add("Configuration", "Debug");
                GlobalProperty.Add("Platform", "Any CPU");

                BuildRequestData buildRequest = new BuildRequestData(path, GlobalProperty, null, new string[] { "Build" }, null);
                BuildParameters bp = new BuildParameters(pc);
                bp.DetailedSummary = true;
                bp.Loggers = new List<Microsoft.Build.Framework.ILogger> { logger }.AsEnumerable();

                BuildResult buildResult = BuildManager.DefaultBuildManager.Build(bp, buildRequest);

                bool end = false;
                ErrorsCollection.Clear();
                foreach(string s in File.ReadLines(logFilePath,Encoding.Default))
                {
                    if (s.Contains("error"))
                    {
                        status = false;   
                        string[] strings = s.Split(':');
                        if(strings.Length==3)
                        {
                            ErrorsCollection.Add(new MyErrorInfo(strings[0], strings[1], strings[2]));
                            end = true;
                        }
                       
                    }
                    else if (end)
                        break;
                }

                //OutputTextBox.AppendText(File.ReadAllText(logFilePath,Encoding.Default));
                Output = File.ReadAllText(logFilePath, Encoding.Default);
            }
            catch(Exception)
            {
                status = false;
            }
            finally
            {
                File.Delete(logFilePath);
            }
            return status;
        }
            

        private void ExecuteBuildAndRun()//runs currently open project
        {
            if(ExecuteBuild())
            {
                string path = (Folders.Elems[0] as FolderItem).Info.FullName;
                path += @"\bin\Debug";
                path = Directory.GetFiles(path, "*.exe")?[0];

                if (path == null)
                    return;
                Process.Start(path);

            }
        }
        private void Execute_Click(object sender, RoutedEventArgs e)//tries to build or build and run currently open project
        {
            if(Folders.Elems.Count<1)
            {
                MessageBox.Show("You cannot build project if it is not loaded", "An error occured", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            switch(OptionComboBox.SelectedIndex)
            {
                case 0:
                    ExecuteBuild();
                    break;
                case 1:
                    ExecuteBuildAndRun();
                    break;
                default:
                    MessageBox.Show("Error", "Something wrong with comboboxindex", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
            }
        }
        private void Close_Click(object sender,RoutedEventArgs e)//closes open file or asks if user want to close if file was modified
        {
            Button b = sender as Button;
            TabItem tabItem = b.TemplatedParent as TabItem;

            string name = tabItem.Header as string;
            if(name[name.Length-1]=='*')
            {
                if (MessageBoxResult.No == MessageBox.Show("Do you really want to close an unsaved document?", "Close document", MessageBoxButton.YesNo, MessageBoxImage.Question))
                    return;
            }

            int i = FileTab.Items.IndexOf(tabItem);
            if (i == FileTab.SelectedIndex)
                FileTab.SelectedIndex = 0;
            FileTab.Items.RemoveAt(i);
        }
        private void About_Click(object sender, RoutedEventArgs e)=> MessageBox.Show("This is simple C# editor and compiler.", "About", MessageBoxButton.OK, MessageBoxImage.Information);

        private void Exit_Click(object sender, RoutedEventArgs e) => this.Close();

        private void NewFile_Click(object sender, RoutedEventArgs e)//Creates new file
        {
            TabItem item = new TabItem();
            RichTextBox r = new RichTextBox();
            r.TextChanged += R_TextChanged;
            r.SelectionChanged += Colorise;
            item.Content = r;
            item.Header = "New File";
            item.Tag = string.Empty;
            FileTab.Items.Add(item);
            FileTab.SelectedIndex = FileTab.Items.Count - 1;
        }
        private void Colorise(object sender,RoutedEventArgs e)//applies currently to file chosen plug-ins by bolding and coloring selected words
        {
            RichTextBox richTextBox = sender as RichTextBox;
            TabItem tabItem = richTextBox.Parent as TabItem;
            string header = tabItem.Header as string;

            TextRange doc = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
            doc.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Black);
            doc.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Normal);

            foreach (var info in PluginsCollection)
            {
                if (info.Enabled)
                {
                    info.PluginInfo.Do(richTextBox);
                }
            }

            tabItem.Header=header;
        }
        private void R_TextChanged(object sender, TextChangedEventArgs e)
        {
            RichTextBox richTextBox = sender as RichTextBox;
            TabItem tabItem = richTextBox.Parent as TabItem;

            string name = tabItem.Header as string;
            if (name == null)
                return;
            if (name[name.Length - 1] != '*')
                tabItem.Header+="*";

        }

        private void OpenFile_Click(object sender, RoutedEventArgs e)//opens *.cs file for modification
        {
            WinForms.OpenFileDialog openFileDialog = new WinForms.OpenFileDialog();
            openFileDialog.Filter = "C# files (*.cs)|*cs";
            if(WinForms.DialogResult.OK==openFileDialog.ShowDialog())
            {
                TabItem item = new TabItem();
                RichTextBox richTextBox = new RichTextBox();
                richTextBox.TextChanged += R_TextChanged;
                richTextBox.SelectionChanged += Colorise;
                item.Content = richTextBox;
                richTextBox.AppendText(File.ReadAllText(openFileDialog.FileName));
                item.Header = openFileDialog.SafeFileName;
                item.Tag = openFileDialog.FileName;
                FileTab.Items.Add(item);
                FileTab.SelectedIndex = FileTab.Items.Count - 1;
            }
        }
        private void GetFileCount(string path,FolderItem folderItem)//recursive function for building project tree
        {
            if(Directory.Exists(path))
            {
                foreach (string subFolder in Directory.GetDirectories(path))
                {
                    FolderItem newSubFolder = new FolderItem();
                    newSubFolder.Info = new DirectoryInfo(subFolder);
                    newSubFolder.UsableName = newSubFolder.Info.Name;
                    folderItem.Elems.Add(newSubFolder);
     
                    GetFileCount(subFolder, newSubFolder);
                }

                foreach(string fileName in Directory.GetFiles(path,"*.cs"))
                {
                    FileItem newFile = new FileItem();
                    newFile.Info = new FileInfo(fileName);
                    folderItem.Elems.Add(newFile);
                }
            }
            

        }
        private void OpenProject_Click(object sender, RoutedEventArgs e)//opens project (user has to choose directory with *.csproj file in it) and builds project tree
        {
            WinForms.FolderBrowserDialog folderBrowserDialog = new WinForms.FolderBrowserDialog();

            if(WinForms.DialogResult.OK==folderBrowserDialog.ShowDialog())
            {
                string dir = folderBrowserDialog.SelectedPath;
                
                if(Directory.GetFiles(dir,"*.csproj").Length==0)//no .csproj file found in chosen directory
                {
                    MessageBox.Show("This is not proper folder with C# project!", "An error occured", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                //Folders = new FolderItem { Info = new DirectoryInfo(dir) };
                FolderItem folder = new FolderItem { Info = new DirectoryInfo(dir)};
                folder.UsableName ="Project: " +folder.Info.Name;
                Folders.Elems.Insert(0, folder);
                GetFileCount(dir, folder);
                OnPropertyChanged(nameof(Folders));
            }
            
        }
        private void OpenFile(string fullPath,string header)//opens file
        {
            TabItem item = new TabItem();
            RichTextBox richTextBox = new RichTextBox();
            richTextBox.TextChanged += R_TextChanged;
            richTextBox.SelectionChanged+= Colorise;
            item.Content = richTextBox;
            richTextBox.AppendText(File.ReadAllText(fullPath));
            item.Header = header;
            item.Tag = fullPath;
            FileTab.Items.Add(item);
            FileTab.SelectedIndex = FileTab.Items.Count - 1;
        }

        private void ProjectTree_MouseDoubleClick(object sender, MouseButtonEventArgs e)//opens file by doubleclicking on project tree
        {
            if (ProjectTree.SelectedItem is FileItem fileItem)
            {
                string fullPath = fileItem.Info.FullName;
                if (!fullPath.EndsWith(".cs"))
                {
                    MessageBox.Show($"Upss... something went wrong");
                    return;
                }
                else
                    OpenFile(fullPath,fileItem.Info.Name);
            }
            else
                MessageBox.Show($"Upss... something went wrong");
        }

        private void Save_Click(object sender, RoutedEventArgs e)//saves file
        {
            if (!FileTab.HasItems)
                return;
            TabItem tabItem = FileTab.Items[FileTab.SelectedIndex] as TabItem;
            string name = tabItem.Header as string;
            string tag = tabItem.Tag as string;
            if (String.IsNullOrEmpty(tag) || String.IsNullOrWhiteSpace(tag))
            {
                SaveAs_Click(sender, e);
                return;
            }
                

            string path = tabItem.Tag as string;
            using (StreamWriter sw = new StreamWriter(path, false))
            {
                RichTextBox r = tabItem.Content as RichTextBox;
                sw.Write(new TextRange(r.Document.ContentStart, r.Document.ContentEnd).Text);
                if (name[name.Length - 1] == '*')
                    tabItem.Header=(tabItem.Header as string).Substring(0, name.Length - 1);
            }
        }

        private void SaveAs_Click(object sender, RoutedEventArgs e)//saves file as
        {
            if (!FileTab.HasItems)
                return;
            TabItem tabItem = FileTab.Items[FileTab.SelectedIndex] as TabItem;
            string name = tabItem.Header as string;
            RichTextBox r = tabItem.Content as RichTextBox;

            WinForms.SaveFileDialog saveFileDialog = new WinForms.SaveFileDialog();
            saveFileDialog.Filter = "C# files (*.cs)|*.cs";
            saveFileDialog.FileName = name.Substring(0, name.Length - 1);
            saveFileDialog.ShowDialog();
            if(saveFileDialog.FileName!="")
            {
                System.IO.FileStream stream = (System.IO.FileStream)saveFileDialog.OpenFile();
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(new TextRange(r.Document.ContentStart, r.Document.ContentEnd).Text);
                    if (name[name.Length - 1] == '*')
                        tabItem.Header = (tabItem.Header as string).Substring(0, name.Length - 1);
                }
                stream.Close();
            }

        }

        private void NewFile_Executed(object sender, ExecutedRoutedEventArgs e) => NewFile_Click(sender, e);
        private void AllCommands_CanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = true;
        private void SaveFile_Executed(object sender, ExecutedRoutedEventArgs e) => Save_Click(sender, e);
        private void OpenFile_Executed(object sender, ExecutedRoutedEventArgs e) => OpenFile_Click(sender, e);
        private void Execute_Executed(object sender, ExecutedRoutedEventArgs e) => Execute_Click(sender, e);
        private void AllTab_Colorise()//function that applies plug-ins coloring to all currently open files
        {
            foreach(TabItem tab in FileTab.Items)
            {
                string header = tab.Header as string;
                RichTextBox richTextBox = tab.Content as RichTextBox;
                TextRange doc = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
                doc.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Black);
                doc.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Normal);
                foreach (var info in PluginsCollection)
                {
                    if (info.Enabled)
                    {
                        info.PluginInfo.Do(richTextBox);
                    }
                }
                tab.Header = header;
                richTextBox.SelectionBrush = new SolidColorBrush(Colors.Black);
                richTextBox.CaretBrush = new SolidColorBrush(Colors.Black);
            }
        }
        private void MenuItem_Checked(object sender, RoutedEventArgs e) => AllTab_Colorise();
    }
}
