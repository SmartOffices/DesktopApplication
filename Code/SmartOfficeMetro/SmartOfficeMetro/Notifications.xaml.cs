using System;
using System.Collections.Generic;
using System.Linq;

using System.Windows;
using System.Windows.Controls;

using System.Windows.Input;

using SmartOfficeMetro.Model;
using System.Threading;


/// <summary>
/// Created By Raghav Bhardwaj
/// </summary>
namespace SmartOfficeMetro
{
    /// <summary>
    /// Interaction logic for Notifications.xaml
    /// </summary>
    public partial class Notifications : UserControl
    {

        static int counter = 0;
        SynchronizationContext context;
        System.Timers.Timer remove;
        public Notifications()
        {
            InitializeComponent();
            
           
            
            updateUI();
            
            remove = new System.Timers.Timer();
            remove.Interval = 50;
            remove.Elapsed += Remove_Elapsed;

            //get main UI thread
            context = SynchronizationContext.Current;
        }

        

        private void Tile_MouseEnter(object sender, MouseEventArgs e)
        {
           // NotificationTile tile = sender as NotificationTile;
            //tile.Background = SystemColors.HighlightBrushKey as System.Windows.Media.Brush;
        }

        private void clearAll_Click(object sender, RoutedEventArgs e)
        {
            /*
            counter = 0;
            Thread t = new Thread(()=>remove.Start());
            t.Start();
            t.Join();
            */
            UserManager.Instance.current_notifications.Clear();
            stackPanelNotifications.Children.Clear();
            
            
             
            
            
        }
        private void Remove_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            context.Post(new SendOrPostCallback(new Action<object>(o => {
                Thickness margin = stackPanelNotifications.Margin;
                margin.Left += 250;
                stackPanelNotifications.Margin = margin;
                this.InvalidateVisual();
            })), null);
            counter++;
            if (counter == 6)
                remove.Stop();
        }
        public void updateUI()
        {
            //Clear old notifications
            stackPanelNotifications.Children.Clear();
            foreach (List<string> notification in UserManager.Instance.current_notifications)
            {
                String Name = "";
                foreach(List<String> user in UserManager.Instance.user_details )
                {   //match user ID to find name
                    if(user.ElementAt(0) == notification.ElementAt(2))
                    {
                        Name = user.ElementAt(3);
                        break;
                    }
                }
                
                NotificationTile tile = new NotificationTile(DateTime.Parse(notification.ElementAt(5)).ToString("MMM d HH:MM"), Name, notification.ElementAt(3), notification.ElementAt(4),notification.ElementAt(2)=="8");
                tile.HorizontalAlignment = HorizontalAlignment.Stretch;
                tile.Width = Double.NaN;        //so tile width becomes auto adjust instead of specefic pixel numbers
                tile.MouseEnter += Tile_MouseEnter;
                stackPanelNotifications.Children.Insert(0,tile);
            }
            
           // UserManager.Instance.current_notifications.Clear();     //Clear current notifications after displaying them
            stackPanelNotifications.Margin = new Thickness(0);
            
        }
    }
}
