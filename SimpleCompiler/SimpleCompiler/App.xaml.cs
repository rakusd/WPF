using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using PluginContracts;
namespace VisualStudent
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    /// 
    public partial class App : Application
    {
        private static RoutedUICommand newFile_Command = new RoutedUICommand("New File", "NewFile", typeof(MainWindow));
        public static RoutedUICommand NewFile_Command
        {
            get { return newFile_Command; }
        }
        private static RoutedUICommand saveFile_Command = new RoutedUICommand("Save File", "SaveFile", typeof(MainWindow));
        public static RoutedUICommand SaveFile_Command
        {
            get { return saveFile_Command; }
        }
        private static RoutedUICommand openFile_Command = new RoutedUICommand("Open File", "OpenFile", typeof(MainWindow));
        public static RoutedUICommand OpenFile_Command
        {
            get { return openFile_Command; }
        }
        private static RoutedUICommand execute_Command = new RoutedUICommand("Execute", "Execute", typeof(MainWindow));
        public static RoutedUICommand Execute_Command
        {
            get { return execute_Command; }
        }
    }
}
