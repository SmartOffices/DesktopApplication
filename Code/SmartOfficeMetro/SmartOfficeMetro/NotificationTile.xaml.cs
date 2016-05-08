using System;

using System.Windows;
using System.Windows.Controls;

using System.Windows.Input;

using System.Timers;
using System.Threading;

/// <summary>
/// Created By Raghav Bhardwaj
/// </summary>
namespace SmartOfficeMetro
{
    /// <summary>
    /// Interaction logic for NotificationTile.xaml
    /// </summary>
    public partial class NotificationTile : UserControl
    {
        Boolean expanded;
        static int counter;
        System.Timers.Timer expand;
        System.Timers.Timer compress;
        SynchronizationContext context = SynchronizationContext.Current;
        Boolean isFetchEnabled;
        public NotificationTile(String time, String sender, String header, String description, Boolean isFetchEnabled)
        {
            InitializeComponent();
            //Add notification time 
            labelTime.MouseLeftButtonDown += LabelHeader_MouseLeftButtonDown;
            labelTime.Content = time;
            //Add subject description along with click description
            labelHeader.MouseLeftButtonDown += LabelHeader_MouseLeftButtonDown;
            labelHeader.Content = header;

            //Add body description to the tile
            textDescription.Text = description;
            textDescription.MouseLeftButtonDown += LabelHeader_MouseLeftButtonDown;

            //Add sender name and it's assosiated click event
            labelSender.Content = sender;
            labelSender.MouseLeftButtonDown += LabelHeader_MouseLeftButtonDown;

            descriptionPanel.Visibility = Visibility.Collapsed;
            expanded = false;

        //added these timers for the slide animation
            expand = new System.Timers.Timer();
            expand.Interval = 25;
            expand.Elapsed += T_Elapsed;

            compress = new System.Timers.Timer();
            compress.Interval = 25;
            compress.Elapsed += Compress_Elapsed;

            this.isFetchEnabled = isFetchEnabled;
        //If the notification is not from the mail room no need for fetch button
            if(!this.isFetchEnabled)
            {
                buttonFetch.Visibility = Visibility.Collapsed;
            }
            else
            {
            //if from mail room then add event handler
                buttonFetch.Click += ButtonFetch_Click;
            }

        }

        private void ButtonFetch_Click(object sender, RoutedEventArgs e)
        {
        //request for mail from mail room
            SmartOfficeClient.sendMessage(4, "null");
       //only allow user to click fetch once to prevent overload
            buttonFetch.IsEnabled = false;
        }

        private void LabelHeader_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //expander.IsExpanded = !expander.IsExpanded;
            
            counter = 0;
            


            if(!expanded)
            {
                if(this.Height > 150)
                {
                    this.Height = 150;
                }

                try
                {
                    expand.Start();
                }
                catch(Exception ex)
                {
                    this.Height = 150;
                    counter = 0;
                    expand.Stop();
                }
                descriptionPanel.Visibility = Visibility.Visible;
                descriptionPanel.Margin = new Thickness(0, 0, 0, 0);
                expanded = true;
            }
            else
            {
                
                try
                {
                    compress.Start();
                }
                catch(Exception ex)
                {
                    this.Height = 30;
                    counter = 0;
                    compress.Stop();
                }
                descriptionPanel.Visibility = Visibility.Collapsed;
                descriptionPanel.Margin = new Thickness(0, 0, 0, 0);
                expanded = false;
            }

            //MetroWindow w = new MetroWindow();
            //ThemeManager.GetAccent();

        }//button click

        private void Compress_Elapsed(object sender, ElapsedEventArgs e)
        {
            context.Post(new SendOrPostCallback(new Action<object>(o => {
                
                this.Height -= 20;
            })), null);
            counter++;
            if (counter == 6)
                compress.Stop();
        }

        private void T_Elapsed(object sender, ElapsedEventArgs e)
        {
            context.Post(new SendOrPostCallback(new Action<object>(o => {
                this.Height += 20;
            })), null);
            counter++;
            if (counter == 6)
                expand.Stop();
        }

        private void descriptionPanel_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            var scrollbar = (ScrollViewer)sender;
            if(scrollbar.VerticalOffset == scrollbar.ScrollableHeight)
            {
                scrollbar.MoveFocus(new TraversalRequest(FocusNavigationDirection.Left));
            }
        }
    }
}
