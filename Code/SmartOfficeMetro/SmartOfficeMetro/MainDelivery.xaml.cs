
using MahApps.Metro.Controls.Dialogs;

using System.Windows;
using System.Windows.Controls;


/// <summary>
/// Created by Raghav Bhardwaj
/// </summary>
namespace SmartOfficeMetro
{
    /// <summary>
    /// Interaction logic for MainDelivery.xaml
    /// </summary>
    public partial class MainDelivery : UserControl
    {
        // TcpClientTest client;
        MainWindow mainWindow = null;
        public MainDelivery()
        {
            InitializeComponent();
          //  client = new TcpClientTest();
          
        }

        private void orderCoffee(object sender, RoutedEventArgs e)
        {
            mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow.ShowMessageAsync("Coffe comes soon", "da");
        }

        private void mailService(object sender, RoutedEventArgs e)
        {
            if (mainWindow == null)
                mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow.showUserControl(2);
            
        }


    }
}
