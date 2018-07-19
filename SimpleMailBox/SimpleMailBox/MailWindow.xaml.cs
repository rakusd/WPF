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
    /// Interaction logic for MailWindow.xaml
    /// </summary>
    public partial class MailWindow : Window
    {
        public string To { get; set; }
        public string Body { get; set; }
        public string Titled { get; set; }
        public MailWindow()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = this;
            MainWindow wnd = this.Owner as MainWindow;
            double x = this.ActualWidth;
            double y = this.ActualHeight;
            this.Width = 0.8*wnd.ActualWidth;
            this.Height = 0.8 * wnd.ActualHeight;
            this.Left -= (this.Width - x)/2;
            this.Top -= (this.Height - y)/2;
            
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)//Cancelling sending e-mail
        {
            this.Close();
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)//Sending mail
        {
            MainWindow wnd= this.Owner as MainWindow;
            EmailMessage email = new EmailMessage
            {
                Body = MailBody.Text,
                From = wnd.current.LastName,
                Title = MailTitle.Text,
                To = MailTo.Text,
                Date = DateTime.Now.ToShortDateString()
            };
            wnd.SentMessages.Add(email);

            this.Close();
        }
    }
}
