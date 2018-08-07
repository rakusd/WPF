using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using wpfTask1;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window,INotifyPropertyChanged
    {
        public EmailUser current { get; set; }
        public bool logged = false;

        private EmailMessage selected;
        public EmailMessage Selected
        {
            get => selected;
            set
            {
                selected = value;
                OnPropertyChanged(nameof(Selected));
            }
        }

        private EmailMessage selectedSent;
        public EmailMessage SelectedSent
        {
            get => selectedSent;
            set
            {
                selectedSent = value;
                OnPropertyChanged(nameof(SelectedSent));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public System.Collections.ObjectModel.ObservableCollection<EmailMessage> messages { get; set; }

        public System.Collections.ObjectModel.ObservableCollection<EmailMessage> SentMessages { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            current = null;
            Selected = null;
        }

        private void EnglishRadioButton_Click(object sender, RoutedEventArgs e)//Changing language to English
        {
            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri("English.xaml", UriKind.RelativeOrAbsolute) });
            this.LanguageDictionary.MergedDictionaries.Clear();
            this.LanguageDictionary.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri("English.xaml", UriKind.RelativeOrAbsolute) });
        }

        private void PolishRadioButton_Click(object sender,RoutedEventArgs e)//Changing language to Polish
        {
            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri("Polish.xaml", UriKind.RelativeOrAbsolute) });
            this.LanguageDictionary.MergedDictionaries.Clear();
            this.LanguageDictionary.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri("Polish.xaml",UriKind.RelativeOrAbsolute) });
        }

        private void Button_Click(object sender, RoutedEventArgs e)//Login/Logout button
        {
            if(!logged)//Logging in
            {
                LoginWindow wnd = new LoginWindow();
                wnd.Owner = this;
                wnd.Owner.Opacity = 0.5;
                wnd.LanguageDictionary.MergedDictionaries.Clear();
                wnd.LanguageDictionary.MergedDictionaries.Add(this.LanguageDictionary);
                wnd.ShowDialog();
                messages = current?.MessagesReceived;
                SentMessages = current?.MessagesSent;
                OnPropertyChanged(nameof(SentMessages));
                

                this.DataContext = this;
                wnd.Owner.Opacity = 1;
                OnPropertyChanged(nameof(current));
                OnPropertyChanged(nameof(selected));
                OnPropertyChanged(nameof(messages));
                if (current != null)
                {
                    MovingColumn.Width = new GridLength(4, GridUnitType.Star);
                    CollectionView collectionView = (CollectionView)CollectionViewSource.GetDefaultView(MyList.ItemsSource);
                    collectionView.Filter = MyFilter;
                    collectionView = (CollectionView)CollectionViewSource.GetDefaultView(MySentList.ItemsSource);
                    collectionView.Filter = MyFilter;
                }
                    
            }
            else//Logging out
            {
                this.NewMailButton.IsEnabled = false;
                this.SearchBox.IsEnabled = false;
                this.TabControl.Visibility = System.Windows.Visibility.Hidden;
                current = null;
                Login.Text = this.FindResource("StrLogin") as string;
                logged = false;
                OnPropertyChanged(nameof(current));
                MovingColumn.Width = new GridLength(1);
                Selected = null;
                SelectedSent = null;
                MyList.SelectedItem = null;
                MySentList.SelectedItem = null;
            }
        }

        private void NewMailButton_Click(object sender,RoutedEventArgs e)//Opening form to send mail
        {
            this.Opacity = 0.5;
            MailWindow wnd = new MailWindow();
            wnd.Owner = this;
            wnd.LanguageDictionary.MergedDictionaries.Clear();
            wnd.LanguageDictionary.MergedDictionaries.Add(this.LanguageDictionary);
            wnd.ShowDialog();

            this.Opacity = 1;
        }

        private void ListBoxSelectionChanged2(object sender,SelectionChangedEventArgs e)
        {
            SelectedSent = MySentList.SelectedItem as EmailMessage;
            if (Selected == null)
                return;
        }
        private void ListBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Selected = MyList.SelectedItem as EmailMessage;
            if (Selected == null)
                return;
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(sender is TabControl tab)
            {
                switch(tab.SelectedIndex)
                {
                    case 0:
                        this.ScrollViewer.Visibility = Visibility.Visible;
                        this.ScrollViewerSent.Visibility = Visibility.Hidden;
                        break;

                    case 1:
                        this.ScrollViewer.Visibility = Visibility.Hidden;
                        this.ScrollViewerSent.Visibility = Visibility.Visible;
                        break;

                }
            }
        }

        private void MyList_KeyDown(object sender, KeyEventArgs e)//deleting selected message from received
        {
            if (e.Key != Key.Delete)
                return;
            if(Selected!=null)
            {
                messages.Remove(Selected);
                Selected = null;
                MyList.SelectedItem = null;
            }
        }

        private void MySentList_KeyDown(object sender, KeyEventArgs e)//deleting selected message from sent
        {
            if (e.Key != Key.Delete)
                return;
            if (SelectedSent != null)
            {
                messages.Remove(SelectedSent);
                SelectedSent = null;
                MySentList.SelectedItem = null;
            }
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(MyList.ItemsSource)?.Refresh();
            CollectionViewSource.GetDefaultView(MySentList.ItemsSource)?.Refresh();
        }

        private bool MyFilter(object item)//function to filter sent or received messages
        {
            string expression = SearchBox.Text;
            if (String.IsNullOrEmpty(expression))
                return true;
            else
            {
                char[] separator = { ' ' };
                string[] lines = expression.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                if(item is EmailMessage em)
                {
                    foreach(string a in lines)
                    {
                        string search = a.ToLowerInvariant();
                        if (em.Date.ToLowerInvariant().Contains(search) || (em.To?.ToLowerInvariant().Contains(search) ?? false) ||
                            (em.From?.ToLowerInvariant().Contains(search) ?? false) || em.Title.ToLowerInvariant().Contains(search))
                            return true;
                    }
                }
            }
            return false;
        }
    }
}
