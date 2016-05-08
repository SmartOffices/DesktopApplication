using System;
using System.Collections.Generic;
using System.Linq;

using System.Windows;
using System.Windows.Controls;

using System.Windows.Input;

using SmartOfficeMetro.Model;

using System.Data;

/// <summary>
/// Created by Raghav Bhardwaj
/// </summary>
namespace SmartOfficeMetro
{
    /// <summary>
    /// Interaction logic for orderMail.xaml
    /// </summary>
    public partial class MailService : UserControl
    {
        Boolean send_now;
        Boolean send_to_mail_room;
        String destination;
        DateTime pickup_time;
        String subject;
        String note;
        DataTable delivery_history = null;
        public MailService()
        {
            InitializeComponent();


            delivery_history = new DataTable("delivery_history");
            delivery_history.Columns.Add("Delivery ID", typeof(String));
            delivery_history.Columns.Add("Reciver", typeof(String));
            delivery_history.Columns.Add("Time Delivered", typeof(String));
            delivery_history.Columns.Add("Status", typeof(String));
            
            Delivery_History_Tab.MouseLeftButtonDown += Delivery_History_Tab_MouseLeftButtonDown;
            dataGridHistory.DataContext = delivery_history.DefaultView;
            dataGridHistory.ColumnWidth = DataGridLength.Auto;
            

            datePicker.SelectedDate = System.DateTime.Today;
            timePicker.Text = System.DateTime.Today.TimeOfDay.ToString();
            List<List<String>> user_names = new List<List<string>>();
            foreach(List<String> users in UserManager.Instance.user_details)
            {
                user_names.Add(new List<string> { users.ElementAt(0), users.ElementAt(1) });
            }
            comboBoxReciver.ItemsSource = new Func<List<String>>(() => { List<String> dummy = new List<string>(); foreach (List<String> user in UserManager.Instance.user_details) { if (user.ElementAt(1) == "unity" || user.ElementAt(1) == "MAIL") { continue; } else { dummy.Add(user.ElementAt(1)); } } return dummy; })();
        }

        private void Delivery_History_Tab_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Update_Delivery_History();
        }

        public void Update_Delivery_History()
        {
            delivery_history.Rows.Clear(); //clear old data
            String reciver = "";
            String status = "";
            foreach(List<String> row in UserManager.Instance.delivery_history)
            {
                foreach (List<String> user in UserManager.Instance.user_details)
                {   //match user ID to find name
                    if (user.ElementAt(0) == row.ElementAt(1))
                    {
                        reciver = user.ElementAt(3);
                        break;//get reciver name from his/her ID
                    }
                }//foreach
                if(row.ElementAt(5) =="1")
                {
                    status = "Delivered";
                }

                
                delivery_history.Rows.Add(new object[] { row.ElementAt(0), reciver, row.ElementAt(4), status });
                //row.ElementAt(0),reciver,row.ElementAt(5),row.ElementAt(6)
                System.Diagnostics.Debug.WriteLine("I updated delivery rows!");
            }
        }
        

        private void sendNow(object sender, EventArgs e)
        {
            //toggle between states
            datePicker.IsEnabled = !datePicker.IsEnabled;
            timePicker.IsEnabled = !timePicker.IsEnabled;
            send_now = !send_now;
        }

        private void sendToMailRoom(object sender, EventArgs e)
        {
            //toggle between states
            comboBoxReciver.IsEnabled = !comboBoxReciver.IsEnabled;
            send_to_mail_room = !send_to_mail_room;
        }

        private void requestPickup(object sender, RoutedEventArgs e)
        {
            if(send_now)
            {
                //request pickup immidiately
                pickup_time = DateTime.Now;
            }
            else
            {
                pickup_time = DateTime.Parse(datePicker.Text + " " + timePicker.Text);
            }
            if(send_to_mail_room)
            {
                //send to mail room
                destination = "mail_room";
            }
            else
            {
                destination = comboBoxReciver.Text;
            }
            
            Mail mail = new Mail(destination, pickup_time, textBoxSubject.Text, textBoxNote.Text);
            SmartOfficeClient.sendMessage(5, mail); //5 is server code for mail delivery











            // var metroWindow = (_openedViews.First() as MetroWindow);
            
         //  SmartOfficeClient.sendMessage()
        //    var metroWindow = (Application.Current.MainWindow as MetroWindow);

        //    metroWindow.ShowMessageAsync("Thank you!!", "You requested a pickup to Talha Mahmood. The robot is on it's way!", MessageDialogStyle.Affirmative);


            //window.show
            //SmartOfficeClient.sendMessage(new Mail(mailDestination, mailTime, note));
        }
        
    }
}
