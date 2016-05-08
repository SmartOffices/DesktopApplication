using System;

using System.Threading;

using System.Windows;
using System.Windows.Controls;

/// <summary>
/// Created by Raghav Bhardwaj
/// </summary>
namespace SmartOfficeMetro
{
    /// <summary>
    /// Interaction logic for Logged_In_User_Tile.xaml
    /// </summary>
    public partial class Logged_In_User_Tile : UserControl
    {
        SynchronizationContext context = SynchronizationContext.Current;
        public Logged_In_User_Tile(String ID, String Name)
        {
            InitializeComponent();
            //Add ID 
            labelID.Content = ID;

            //Add name of user
            labelName.Content = Name;

            buttonDisconnect.Click += ButtonDisconnect_Click;

        }

        private void ButtonDisconnect_Click(object sender, RoutedEventArgs e)
        {
            //request for mail from mail room
            SmartOfficeClient.sendMessage(12, labelName.Content);
            //only allow user to click fetch once to prevent overload
        }

       
    }
}
