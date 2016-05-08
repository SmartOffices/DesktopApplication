using System;
using System.Collections.Generic;
using System.Linq;

using NetworksApi.TCP.CLIENT;
using System.Net;
using System.Net.Sockets;

using MahApps.Metro.Controls;

using MahApps.Metro.Controls.Dialogs;

using System.Threading;
using Newtonsoft.Json;
using SmartOfficeMetro.Model;

/// <summary>
/// Created by Raghav Bhardwaj
/// </summary>

namespace SmartOfficeMetro
{
    
    public class SmartOfficeClient
    {
        public static Client client;
        public static Boolean allow;
        public static Boolean dataReceived;
        JSONObject SentData;
        public static Boolean isConnected;
        //private SmartOfficeClient clientInstance;
        public static SynchronizationContext main_thread;
        int connectionAttempts = 5;
        int currentAttempts;

        public SmartOfficeClient(UserManager manager)
        {
            client = new Client();
            client.ServerIp = "172.31.132.170";
            client.ServerPort = "8888";
            client.ClientName = manager.username;

            client.OnClientConnected += Client_OnClientConnected;
            client.OnClientDisconnected += Client_OnClientDisconnected;
            client.OnClientError += Client_OnClientError;
            client.OnClientFileSending += Client_OnClientFileSending;
            client.OnDataReceived += Client_OnDataReceived;
            client.OnClientConnecting += Client_OnClientConnecting;
            
            client.Connect();
            
        }
        /// <summary>
        /// Sends data to the server
        /// </summary>
        /// <param name="requestType"> 1: Notifications(Any kind) 2: Coffee 3: Login request 4: Mail request 5: delivery request 6: Initial user Data 7: delivery history 8: notification history 9:robot battery status 10: recall robot 11: Disconnect user
        /// </param>
        /// <param name="obj">Desired data</param>
        public static void sendMessage(int requestType, object obj)
        {
            JSONObject SentData = new JSONObject(requestType, obj);
            String message = JsonConvert.SerializeObject(SentData);
            try
            {
                client.Send(message);
            }
            catch(Exception e)  //unable to connect to the client duirng application use// retry
            {
                isConnected = false;     
                client.Connect();
            }
        }

        /// <summary>
        /// Requests login from the server. returns true if attempt was sucessful and details are correct. False otherwise
        /// </summary>
        /// <param name="login">The loginUser object containing the user's username and password</param>
        /// <returns></returns>
        public Boolean requestLogin(Object login, SynchronizationContext context)
        {
            SynchronizationContext _uiContext = context; //get context of main thread of login window

                try
                {
                //requestType 3 = login request
                    JSONObject obj = new JSONObject(3, login);
                    String data = JsonConvert.SerializeObject(obj);
                    client.Send(data);
                    dataReceived = false;
                }
                catch (Exception e)
                {
                //no connection established  
                //need to use the context of the main thread to update UI(show message box) 
                _uiContext.Post(new SendOrPostCallback(new Action<object>(o => 
                {
                    //get current instance of the login window. needed to display message box inside it
                    var mainview0 = System.Windows.Application.Current.Windows.OfType<MetroWindow>().FirstOrDefault();
                    mainview0.ShowMessageAsync("Oops", "Unable to connect to the server, please try again later!", MessageDialogStyle.Affirmative);
                })), null);
                client.Disconnect();
                    return false;
                } //catch

            // If data still hasn't been received, wait for sometime
            while (!SmartOfficeClient.dataReceived || currentAttempts < connectionAttempts)
            {
                System.Threading.Thread.Sleep(500);
                currentAttempts++;
            }

            //If data has still not been received display server error
            if (!dataReceived)
            {
                //get current instance of the main window and use it to display error
                _uiContext.Post(new SendOrPostCallback(new Action<object>(o => 
                {
                    //get current instance of the login window. needed to display message box inside it
                    var mainview0 = System.Windows.Application.Current.Windows.OfType<MetroWindow>().FirstOrDefault();
                    mainview0.ShowMessageAsync("Oops", "Unable to connect to the server, please try again later!", MessageDialogStyle.Affirmative);
                    client.Disconnect();
                })), null);
                return false;
            }// dataReceived
            

            if (allow)
                return true;    //client sucessfully logged in
            else
            {
                _uiContext.Post(new SendOrPostCallback(new Action<object>(o =>
                {
                    //get current instance of the login window. needed to display message box inside it
                    var mainview0 = System.Windows.Application.Current.Windows.OfType<MetroWindow>().FirstOrDefault();
                    mainview0.ShowMessageAsync("Oops", "Your Login information seems to be incorrect. Please try again", MessageDialogStyle.Affirmative);
                    client.Disconnect();
                })), null);
                return false;
            }//else

        }//end login

        public void disconnect()
        {
            client.Disconnect(); 
        }

        private void Client_OnClientConnecting(object Sender, ClientConnectingArguments R)
        {
            System.Diagnostics.Debug.WriteLine("Connecting");
        }

        /// <summary>
        /// Receives data as a JSON packet which is then deserialized to a JSONObject.
        /// Based on the requestType within the JSONObject, the accompnying object is deserialized to
        /// the desired object types
        /// </summary>
        /// <param name="Sender">usually the server is the sender</param>
        /// <param name="R">Received JSON packet</param>
        private void Client_OnDataReceived(object Sender, ClientReceivedArguments R)
        {
            //set boolean to true meaning some data was received and other threads can proceed
            dataReceived = true;
           // System.Diagnostics.Debug.WriteLine(R.ReceivedData);
            JSONObject ReceivedData = JsonConvert.DeserializeObject<JSONObject>(R.ReceivedData);

            //switch case to decide what kind of data was received
           /// 1: Notifications(Any kind) 
           /// 2: Coffee 
           /// 3: Login request 
           /// 4: Mail request 
           /// 5: delivery request 
           /// 6: Initial user Data 
           /// 7: delivery history 
           /// 8: notification history
           /// 9: robot status
           /// 10: recall robot

            
            switch (ReceivedData.requestType)
            {
                case 1:
                    Notification_Handler(ReceivedData.info);
                    break;
                case 2:
                    Coffee_Handler(ReceivedData.info);
                    break;
                case 3:
                    Login_Handler(ReceivedData.info);
                    break;
                case 4:
                    Mail_Request_Handler(ReceivedData.info);
                    break;
                case 5:
                    Delivery_Request_Handler(ReceivedData.info);
                    break;
                case 6:
                    Initial_User_Data_Handler(ReceivedData.info);
                    break;
                case 7:
                    Delivery_History_Handler(ReceivedData.info);
                    break;
                case 8:
                    Notification_History_Handler(ReceivedData.info);
                    break;
                case 9:
                    Admin_Data_Handler(ReceivedData.info);
                    break;
                case 10:
                    Recall_Robot_Handler(ReceivedData.info);
                    break;
                case 12:
                    Disconnect_User_Handler(ReceivedData.info);
                    break;
            }//switch

        }//on data received client

        private void Disconnect_User_Handler(object info)
        {
            main_thread.Post(new SendOrPostCallback(new Action<object>(o => {
                var mainview0 = System.Windows.Application.Current.Windows.OfType<MetroWindow>().Last();
                mainview0.ShowMessageAsync("Notification", info.ToString(), MessageDialogStyle.Affirmative);
            })), null);
        }

        private void Recall_Robot_Handler(object info)
        {
            main_thread.Post(new SendOrPostCallback(new Action<object>(o => {
                var mainview0 = System.Windows.Application.Current.Windows.OfType<MetroWindow>().Last();
                mainview0.ShowMessageAsync("Notification", info.ToString(), MessageDialogStyle.Affirmative);
            })), null);
        }

        private void Notification_Handler(object info)
        {
            UserManager.Instance.current_notifications.Add(info as List<String>);
        }

        private void Admin_Data_Handler(object info)
        {
            // This is where the robots battery status will be recorded
            try
            {
                List<List<String>> robots = JsonConvert.DeserializeObject<List<List<String>>>(info.ToString());
                List<Robot> robot_list = new List<Robot>();
                AdminManager.Instance.battery_degradation += 1;
                foreach (List<string> robot in robots)
                {
                    robot_list.Add(new Robot(robot.ElementAt(0), Double.Parse(robot.ElementAt(1)) - AdminManager.Instance.battery_degradation ));
                }
                AdminManager.Instance.robot_list = robot_list;
                //maybe call a function to update UI here or can update upon load too
            }
            catch (Exception ex)
            {
                /// 2 reasons this catch can occuer:
                /// #1: sometimes the server data gets lost and null values are passed. in that case we simply ignore
                ///     that data packet
                /// #2: the data sent was a list of logged in users and could not be parsed into a robot object.
                ///     In this case we parse it to the logged in users list
                try
                {
                    AdminManager.Instance.logged_in_users = JsonConvert.DeserializeObject<List<String>>(info.ToString());
                }
                catch(Exception e)
                {
                    //if the data was actually sent as null then we ignore it
                }//catch
            }//catch
        }//admin data

        private void Delivery_Request_Handler(object info)
        {
            //User receives a response for his mail. We just need to print it
            main_thread.Post(new SendOrPostCallback(new Action<object>(o => {
                var mainview0 = System.Windows.Application.Current.Windows.OfType<MetroWindow>().Last();
                mainview0.ShowMessageAsync("Notification", info.ToString(), MessageDialogStyle.Affirmative);
            })), null);
        }

        private void Mail_Request_Handler(object info)
        {
            //User receives a response for his mail. We just need to print it
            main_thread.Post(new SendOrPostCallback(new Action<object>(o => {
                var mainview0 = System.Windows.Application.Current.Windows.OfType<MetroWindow>().Last();
                mainview0.ShowMessageAsync("Notification", info.ToString(), MessageDialogStyle.Affirmative);
            })), null);

        }

        private void Notification_History_Handler(object info)
        {
            UserManager.Instance.current_notifications = JsonConvert.DeserializeObject<List<List<String>>>(info.ToString());


        }

        private void Delivery_History_Handler(object info)
        {
            UserManager.Instance.delivery_history = JsonConvert.DeserializeObject<List<List<String>>>(info.ToString());
        }

        private void Initial_User_Data_Handler(object info)
        {
            UserManager.Instance.user_details = JsonConvert.DeserializeObject<List<List<String>>>(info.ToString());
        }

        /// <summary>
        /// Just displays the response sent by the server when user requests for coffee
        /// </summary>
        /// <param name="info">Server response</param>
        private void Coffee_Handler(object info)
        {
            main_thread.Post(new SendOrPostCallback(new Action<object>(o => {
                var mainview0 = System.Windows.Application.Current.Windows.OfType<MetroWindow>().Last();
                mainview0.ShowMessageAsync("Notification", info.ToString(), MessageDialogStyle.Affirmative);
            })), null);
            System.Diagnostics.Debug.WriteLine("I made it past the display Coffee_handler stuff");
        }

        private void Client_OnClientFileSending(object Sender, ClientFileSendingArguments R)
        {
            throw new NotImplementedException();
        }

        private void Client_OnClientError(object Sender, ClientErrorArguments R)
        {
            System.Diagnostics.Debug.WriteLine(R.ErrorMessage);
            isConnected = false;
            if (main_thread != null)//we're actually using the application and not in the initial login phase
            { 
            main_thread.Post(new SendOrPostCallback(new Action<object>(o =>
            {
                try
                {
                    var mainview0 = System.Windows.Application.Current.Windows.OfType<MetroWindow>().Last();
                    mainview0.ShowMessageAsync("Notification", "Looks like the connection to the server has been lost. Please wait while we try to re-establish it", MessageDialogStyle.Affirmative);
                }
                catch(NullReferenceException e) { }
            })), null);
            }
        }

        private void Client_OnClientDisconnected(object Sender, ClientDisconnectedArguments R)
        {
            System.Diagnostics.Debug.WriteLine(R.EventMessage);
        }

        private void Client_OnClientConnected(object Sender, ClientConnectedArguments R)
        {
            System.Diagnostics.Debug.WriteLine(R.EventMessage);
            if(main_thread!=null)//we're actually using the application and not in the initial login phase
            {
               
                    main_thread.Post(new SendOrPostCallback(new Action<object>(o => {
                        try
                        {
                            var mainview0 = System.Windows.Application.Current.Windows.OfType<MetroWindow>().Last();
                            mainview0.ShowMessageAsync("Congratulations!", "We re-established the connection! You may continue to use the application now", MessageDialogStyle.Affirmative);
                        }
                        catch(NullReferenceException e) { }
                    })), null);
               
            }
            isConnected = true;
        }

        public static string GetLocalIPAddress()
        {
            string IP4Address = String.Empty;
            foreach (IPAddress IPA in Dns.GetHostAddresses(Dns.GetHostName()))
            {
                if (IPA.AddressFamily == AddressFamily.InterNetwork)
                {
                    IP4Address = IPA.ToString();
                    break;
                }
            }
            return IP4Address;
        }
        public void Login_Handler(Object login_data)
        {
            String json;
            json = JsonConvert.SerializeObject(login_data);
            loginObject loginObj = JsonConvert.DeserializeObject<loginObject>(json);

            if (loginObj.loginStatus)
                allow = true;
            else
            {
                // if login failed, then no need to populate user data
                allow = false;
                return;
            }
            //if loginStatus was true then start populating user data
            json = JsonConvert.SerializeObject(loginObj.user);
            User user_login_object = JsonConvert.DeserializeObject<User>(json);
            //Get current instance of UserManager and populate all the information in it
            UserManager user = UserManager.Instance;
            user.id = user_login_object.id;
            user.username = user_login_object.username;
            user.password = user_login_object.password;
            user.Name = user_login_object.Name;
            user.phone = user_login_object.phone;
            user.email = user_login_object.email;
            user.department = user_login_object.department;
            user.age = user_login_object.age;
            user.image = user_login_object.image;
            user.current_notifications = new List<List<string>>();
            user.delivery_history = new List<List<string>>();
            user.user_details = new List<List<string>>();

            /*
            System.Diagnostics.Debug.WriteLine(user.id);
            System.Diagnostics.Debug.WriteLine(user.Name);
            System.Diagnostics.Debug.WriteLine(user.phone);
            System.Diagnostics.Debug.WriteLine(user.email);
            System.Diagnostics.Debug.WriteLine(user.department);
            System.Diagnostics.Debug.WriteLine(user.image);
            System.Diagnostics.Debug.WriteLine(user.age);
            System.Diagnostics.Debug.WriteLine(user.password);
            System.Diagnostics.Debug.WriteLine(user.username);
            */ 
        }// Login Handler



    }//client
}//namespace
