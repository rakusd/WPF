using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using wpfTask1;
namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {       
        
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)//Not logging
        {
            MainWindow wnd = this.Owner as MainWindow;
            wnd.current = null;
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)//Logging in
        {
            EmailUser logged;
            string user = this.Username.Text;
            string password = this.Password.Password;
            bool success = EmailData.GetUserData(user, password, out logged);
            if(!success)
            {
                MessageBox.Show($"{this.FindResource("LoginFail") as string}");
            }
            else
            {
                if(this.Owner is MainWindow wnd)
                { 
                    wnd.current = logged;
                    wnd.TabControl.Visibility = System.Windows.Visibility.Visible;
                    wnd.logged = true;
                    wnd.Login.Text = this.FindResource("StrLogout") as string;
                    wnd.NewMailButton.IsEnabled = true;
                    wnd.SearchBox.IsEnabled = true;
                }
                this.Close();
            }

        }

        private void txtChanged(object sender, RoutedEventArgs e)//Enabling OK button when both username and password are typed
        {
            if (Username.Text.Length > 0 && Password.Password.Length > 0)
                OKButton.IsEnabled = true;
            else
                OKButton.IsEnabled = false;
        }
    }
}
