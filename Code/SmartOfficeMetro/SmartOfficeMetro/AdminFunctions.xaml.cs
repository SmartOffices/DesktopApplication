using System;
using System.Collections.Generic;
using System.Linq;

using System.Windows;
using System.Windows.Controls;

using SmartOfficeMetro.Model;

/// <summary>
/// Created by Naorin Hossain
/// Edited by Raghav Bhardwaj
/// </summary>
namespace SmartOfficeMetro
{
    /// <summary>
    /// Interaction logic for AdminFunctions.xaml
    /// </summary>
    public partial class AdminFunctions : UserControl
    {
        
        public AdminFunctions()
        {
            InitializeComponent();
            

            foreach(Robot robot in AdminManager.Instance.robot_list)
            {
                Tile_Battery_Info robot_tile = new Tile_Battery_Info(robot);
                robot_tile.HorizontalAlignment = HorizontalAlignment.Stretch;
                robot_tile.Width = Double.NaN;  //to automatically set it to auto width
                stackPanelBattery.Children.Add(robot_tile);
            }
            
            
        }

        private void Disconnect_button_Click(object sender, RoutedEventArgs e)
        {
            String username = ((Button)sender).Tag.ToString();
            SmartOfficeClient.sendMessage(11, username);
        }

        public void Update_UI()
        {
            stackPanelBattery.Children.Clear();
            stackPanelLoggedUser.Children.Clear();
  
            foreach (Robot robot in AdminManager.Instance.robot_list)
            {
                Tile_Battery_Info robot_tile = new Tile_Battery_Info(robot);
                robot_tile.HorizontalAlignment = HorizontalAlignment.Stretch;
                robot_tile.Width = Double.NaN;  //to automatically set it to auto width
                stackPanelBattery.Children.Add(robot_tile);
            }//foreach robot battery

            //logged in user data table
            try
            {
                foreach (String user in AdminManager.Instance.logged_in_users)
                {
                    String id = "";
                    //perform a data search in local table for user ID
                    foreach (List<String> u in UserManager.Instance.user_details)
                    {
                        if (u.ElementAt(1) == user)
                        {
                            id = u.ElementAt(0);
                            break;
                        }//if
                    }//foreach find user id

                    Logged_In_User_Tile tile = new Logged_In_User_Tile(id, user);
                    tile.HorizontalAlignment = HorizontalAlignment.Stretch;
                    tile.Width = Double.NaN;        //so tile width becomes auto instead of specefic pixel numbers

                    stackPanelLoggedUser.Children.Insert(0, tile);

                    
                }//foreach populate logged in users
                stackPanelLoggedUser.Margin = new Thickness(0);
                System.Diagnostics.Debug.WriteLine("I came here once");
            }
            catch(NullReferenceException ex)
            {
                //null reference occuers if the application hasn't heard from the server yet
                // due to maybe latency, in this case we ignore it and wait for another update to happen
            }

        }//update
    }//class
}//namespace
