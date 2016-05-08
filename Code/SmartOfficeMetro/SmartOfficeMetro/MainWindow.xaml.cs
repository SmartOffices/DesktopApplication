using System;
using System.Collections.Generic;

using System.Windows;

using System.Windows.Media.Imaging;

using MahApps.Metro.Controls;
using MahApps.Metro;

using System.Threading;

using SmartOfficeMetro.Model;

/// <summary>
/// Created by Raghav Bhardwaj
/// </summary>
namespace SmartOfficeMetro
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    ///

    

    public partial class MainWindow : MetroWindow
    {
        
        String imageSource;
        public string imgSrc { get { return imageSource; } set { imageSource = value; } }
        private System.Windows.Controls.Control currentControl;
       
        System.Windows.Forms.NotifyIcon notifyIcon;
        System.Windows.Forms.ContextMenu notifyContextMenu;
        Notifications notification_window = null;
        MailService main_delivery_window = null;
        AdminFunctions admin_window = null;

        public MainWindow()
        {
            UserManager.Instance.Name = "";
           // UserManager.Instance.image = "kitten-2.jpg";
           // this.Hide();
           // Login login_window = new Login(this,SynchronizationContext.Current);
         //   login_window.Show();
            
            InitializeComponent();
            buttonAdmin.Visibility = Visibility.Collapsed;
            //set image
            BitmapImage logo = new BitmapImage();
            logo.BeginInit();
            logo.UriSource = new Uri("pack://application:,,,/SmartOfficeMetro;component/../../UserImages/" + UserManager.Instance.image);
            logo.EndInit();
            imageUserIcon.Source = logo;

            imageUserIcon.DataContext = imgSrc;
            //Set welcome message
            labelWelcome.Content = "Welcome, " + UserManager.Instance.Name + "!";

            //notify icon image
            System.Drawing.Icon icon = Properties.Resources.favicon;

            //Init notifyIcon
            notifyIcon = new System.Windows.Forms.NotifyIcon();
            notifyIcon.Icon = Properties.Resources.favicon;
            notifyIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(notifyIcon_MouseDoubleClick);
           
            //Init context menu for notification tray!
            notifyContextMenu = new System.Windows.Forms.ContextMenu();
            System.Windows.Forms.MenuItem item1= new System.Windows.Forms.MenuItem();
            System.Windows.Forms.MenuItem item2 = new System.Windows.Forms.MenuItem();
            System.Windows.Forms.MenuItem item3 = new System.Windows.Forms.MenuItem();
            System.Windows.Forms.MenuItem item4 = new System.Windows.Forms.MenuItem();
            item1.Text = "Exit Application";
            item2.Text = "Order Coffee";
            item3.Text = "Notifications";
            item4.Text = "Fetch Mail";
            notifyContextMenu.MenuItems.Add(item2);
            notifyContextMenu.MenuItems.Add(item3);
            notifyContextMenu.MenuItems.Add(item4);
            notifyContextMenu.MenuItems.Add(item1);
            
            notifyIcon.ContextMenu = notifyContextMenu;

         //deal with a closing event to kill threads
            this.Closed += MainWindow_Closed;

         //pass the main thred for the client to use for UI purposes
            SmartOfficeClient.main_thread = SynchronizationContext.Current;
            
        }

        /// <summary>
        /// Displays the control based on the control type passed 1 Notification window 2 mail service window 3 delivery window
        /// </summary>
        /// <param name="control_type">1 Notification window 2 mail service window 3 delivery window</param>
        public void showUserControl(int control_type)
        {

            /// 1: Notification window
            /// 2: delivery window

            switch (control_type)
            {
                case 1:
                    currentControl = notification_window;
                    break;
                case 2:
                    currentControl = main_delivery_window;
                    break;
                case 3:
                    currentControl = admin_window;
                    break;
            }// switch

        //Init User control dimensions 
            currentControl.Height = dockPanel.Height;
            currentControl.Width = dockPanel.Width;
        //clear current panel
            dockPanel.Children.Clear();
       //add User control
            dockPanel.Children.Add(currentControl);

        }//show user control

        private void showNotifications(object sender, RoutedEventArgs e)
        {
            notification_window.updateUI();
            showUserControl(1);

            
        }

        private void orderCoffee(object sender, RoutedEventArgs e)
        {
            SmartOfficeClient.sendMessage(2, new Destination(UserManager.Instance.id));
        }
        private void showMainDelivery(object sender, RoutedEventArgs e)
        {
            main_delivery_window.Update_Delivery_History();
            showUserControl(2);
            
        }
        private void buttonAdmin_Click(object sender, RoutedEventArgs e)
        {
            admin_window.Update_UI();
            showUserControl(3);
        }
        public void UpdateUI()
        {
            //set image
            BitmapImage logo = new BitmapImage();
            logo.BeginInit();
            logo.UriSource = new Uri("pack://application:,,,/SmartOfficeMetro;component/../../UserImages/" + UserManager.Instance.image);
            logo.EndInit();
            imageUserIcon.Source = logo;

            imageUserIcon.DataContext = imgSrc;
            //Set welcome message
            labelWelcome.Content = "Welcome, " + UserManager.Instance.Name + "!";
//Get initial data from server regarding users, delivery history and notifications
            SmartOfficeClient.sendMessage(6, "null");
            SmartOfficeClient.sendMessage(7, "null");
            SmartOfficeClient.sendMessage(8, "null");
            notification_window = new Notifications();
            main_delivery_window = new MailService();
            //Check to see if user is admin. If yes, add admin features, otherwise ignore them
            System.Diagnostics.Debug.WriteLine("CHECKING USER DEPARTMENT " + UserManager.Instance.department);
            if(UserManager.Instance.department.ToLower() =="admin")
            {
                AdminManager.Instance.robot_list = new List<Robot>();

                //display the admin button
                buttonAdmin.Visibility = Visibility.Visible;
                //create admin window here
                admin_window = new AdminFunctions();
                
            }
        }// end UI update


        /// <summary>
        /// Needed to override default closing method to handle thread closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_Closed(object sender, EventArgs e)
        {
            System.Environment.Exit(System.Environment.ExitCode);
        }

        /// <summary>
        /// Maximize the window when icon in task bar is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void notifyIcon_MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
                this.WindowState = WindowState.Maximized;
        }

        private void radioButtonGreen_Checked(object sender, RoutedEventArgs e)
        {
            ThemeManager.ChangeAppStyle(System.Windows.Application.Current, ThemeManager.GetAccent("Green"), ThemeManager.DetectAppStyle().Item1);
        }

        private void radioButtonBlue_Checked(object sender, RoutedEventArgs e)
        {
            ThemeManager.ChangeAppStyle(System.Windows.Application.Current, ThemeManager.GetAccent("Blue"), ThemeManager.DetectAppStyle().Item1);
        }

        private void radioButtonRed_Checked(object sender, RoutedEventArgs e)
        {
            ThemeManager.ChangeAppStyle(System.Windows.Application.Current, ThemeManager.GetAccent("Red"), ThemeManager.DetectAppStyle().Item1);
            
        }

        /// <summary>
        /// Slide in the settings flyout when the necessary button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void showSettings_Click(object sender, RoutedEventArgs e)
        {
            var flyout = this.Flyouts.Items[0] as Flyout;
            if(flyout == null)
            {
                return;
            }
            //switch between open and close states(true,false)
            flyout.IsOpen = !flyout.IsOpen;
        }// settings flyout

        private void showPopup(object sender, RoutedEventArgs e)
        {
         //   popUp.ShowBalloonTip("Hi", imageSource, Hardcodet.Wpf.TaskbarNotification.BalloonIcon.Info);
        }

        

       
        //To minimize window to taskbar!
        /// <summary>
        /// Had to override default minimize action to show notifications upon minimize
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MetroWindow_StateChanged(object sender, EventArgs e)
        {
            if (this.WindowState == WindowState.Minimized)
            {
                this.ShowInTaskbar = false;
                popUp.ShowBalloonTip("Minimized!","just got minimized!",Hardcodet.Wpf.TaskbarNotification.BalloonIcon.Info);
                notifyIcon.Visible = true;
            }
            else if (this.WindowState == WindowState.Maximized)
            {
                notifyIcon.Visible = false;
                this.ShowInTaskbar = true;
            }
            else
            {
                this.WindowState = WindowState.Maximized; 
            }
        } //state changed


    } // main window
   
}// namespace
