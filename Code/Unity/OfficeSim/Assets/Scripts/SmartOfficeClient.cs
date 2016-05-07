using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
//using NetworksApi.TCP.CLIENT;
using System.Net;
using System.Net.Sockets;
//using System.Windows.Threading;
using Newtonsoft.Json;
using SmartOfficeMetro.Model;
using NetworksApi.TCP.CLIENT;
//using BMove;
using SmartOfficeMetro;
using UnityEngine;
using BMove;

//MODIFIED FOR UNITY



namespace SmartOfficeMetro
{
    public class Task
    {
        public int Destination;
        public bool Status;
        public Task(int dest)
        {
            Destination = dest;

        }
    }
    public class SmartOfficeClient : MonoBehaviour
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
        public navigationScriptObjectNEW controller;
        public static Queue<LinkedList<Task>> tasks = new Queue<LinkedList<Task>>();
        //  static System.Timers.Timer robot_battery;
        public SmartOfficeClient()
        {

            client = new Client();
			client.ServerIp = "172.31.51.31";
            client.ServerPort = "8888";
            client.ClientName = "unity";

            client.OnClientConnected += Client_OnClientConnected;
            client.OnClientDisconnected += Client_OnClientDisconnected;
            client.OnClientError += Client_OnClientError;
            client.OnClientFileSending += Client_OnClientFileSending;
            client.OnDataReceived += Client_OnDataReceived;
            client.OnClientConnecting += Client_OnClientConnecting;

            client.Connect();
            Thread.Sleep(1500);
            if (isConnected)
            {
                //bot timer to send robot battery
                //    robot_battery = new System.Timers.Timer(9000);
                //    robot_battery.Elapsed += Robot_battery_Elapsed;
                //    robot_battery.Start();

                //initialize botmovement

                Debug.Log("Successfully initialized the unity client");
            }
        }//constructor
        void Start()
        {
            controller = GameObject.FindGameObjectWithTag("Robot1").GetComponent<navigationScriptObjectNEW>();
        }
        private void Robot_battery_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            List<List<String>> robot_details = new List<List<String>>();
            List<String> robo1 = new List<string>();
            robo1.Add("1"); robo1.Add(controller.battery_life.ToString());
            robot_details.Add(robo1);
            sendMessage(11, robot_details);
        }

        /// <summary>
        /// Sends data to the server
        /// </summary>
        /// <param name="requestType"> 1: Notifications(Any kind) 2: Coffee 3: Login request 4: Mail request 5: delivery request 6: Initial user Data 7: delivery history 8: notification history
        /// </param>
        /// <param name="obj">Desired data</param>
        public static void sendMessage(int requestType, object obj)
        {
            JSONObject SentData = new JSONObject(requestType, obj);
            String message = JsonConvert.SerializeObject(SentData);
            //  SendData:
            try
            {
                client.Send(message);
                Debug.Log("Sending data to server: " + message);
            }
            catch (Exception e)  //unable to connect to the client duirng application use// retry
            {

                Debug.LogError("Not connected to the server!");
                Disconnect();
                //  client.Connect();
                // goto SendData;

            }
        }

        public static void Disconnect()
        {
            client.Disconnect();
            //   robot_battery.Stop();
            Debug.Log("umm..Disconnected!");
        }

        void OnApplicationQuit()
        {
            client.Disconnect();
            //   robot_battery.Stop();
            Debug.Log("Hi, Disconnected!");
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
            JSONObject ReceivedData = JsonConvert.DeserializeObject<JSONObject>(R.ReceivedData);
            Debug.LogError("Request Received by server.... Processing...");
            //switch case to decide what kind of data was received
            /// 1: Notifications(Any kind) 
            /// 2: Coffee 
            /// 3: Login request 
            /// 4: Mail request 
            /// 5: delivery request 
            /// 6: Initial user Data 
            /// 7: delivery history 
            /// 8: notification history


            switch (ReceivedData.requestType)
            {
                case 1:
                    break;
                case 2:
                    Debug.LogError("Coffee Request Received by " + ReceivedData.info.ToString() + ".... Processing...");
                    Coffee_Handler(ReceivedData.info);
                    //indx = destinationIndex(R.name);
                    //other.moveBot(indx);
                    break;
                case 3:
                    //    Login_Handler(ReceivedData.info);
                    break;
                case 4:
                    Debug.LogError("Mail Request Received by " + ReceivedData.info.ToString() + ".... Processing...");
                    Mail_Request_Handler(ReceivedData.info);
                    /*
                    indx = destinationIndex("mailroom");
                    other.moveBot(indx);
                    indx = destinationIndex(R.name);
                    other.moveBot(indx);
                    */
                    break;
                case 5:
                    Debug.LogError("Delivery Request Received.... Processing...");
                    Delivery_Request_Handler(ReceivedData.info);
                    //indx = destinationIndex(R.name);
                    //other.moveBot(indx);
                    break;
                case 6:

                    break;
                case 7:

                    break;
                case 8:

                    break;

            }
        }


        //  System.Timers.Timer botTimer = new System.Timers.Timer();
        //   ElapsedEventHandler botTimer_Elapsed =  new ElapsedEventHandler(Battery_Life_Sender);
        //   botTimer.Elapsed+=botTimer_Elapsed;
        //   botTimer.Interval = 240000;                 //Every 4 minutes, battery life will be sent to server
        //   botTimer.Enabled = true;

        //QUEUE
       


        private void Delivery_Request_Handler(object info)
        {

            List<String> delivery_info = JsonConvert.DeserializeObject<List<String>>(info.ToString());
            LinkedList<Task> deliveryTask = new LinkedList<Task>();
            Task person1 = new Task(destinationIndex(delivery_info.ElementAt(0)));
            Task person2 = new Task(destinationIndex(delivery_info.ElementAt(1)));
            deliveryTask.AddLast(person1);
            deliveryTask.AddLast(person2);
            ///execution code here
            //System.Diagnostics.Debug.WriteLine("I made it past the display Coffee_handler stuff");


            tasks.Enqueue(deliveryTask);



            /*
            
            //Get destination for pickup:
            indx1 = destinationIndex(info.ToString().ElementAt(0));
            indx2 = destinationIndex(info.ToString().ElementAt(1));
            //Add Delivery Request to queue
            Task pickup = new Task(indx1);
            Task dropoff = new Task(indx2);
            //Tasks.Enqueue(delivery);
            tasks.Enqueue(pickup);
            pickup.Next = dropoff;
            */



            //delivery_info.Add(sender_name);delivery_info.Add(mail.mailDestination);delivery_info.Add(mail.mailTime.ToString());
            //List<String> delivery_info = JsonConvert.DeserializeObject<List<String>>(info.ToString());
            //List<int> waypoints = new List<int> { destinationIndex(delivery_info.ElementAt(0)), destinationIndex(delivery_info.ElementAt(1)) };
            //controller.waypoint(waypoints);
            //  int index = destinationIndex(delivery_info.ElementAt(0));
            //User receives a response for his mail. We just need to print it
            //controller.index = index;

            //   System.Threading.Thread.Sleep(10000);

            //  bot_movement_object.moveBot(index);
            // int dst = destinationIndex(mail.mailDestination);
            //index = destinationIndex(delivery_info.ElementAt(1));
            // bot_movement_object.moveBot(index);
            // controller.index = index;

        }

        private void Mail_Request_Handler(object info)
        {
            /*
            Task pickup = new Task();

            //String json = JsonConvert.SerializeObject(info);
            //Mail mail = JsonConvert.DesializeObject<Mail>(json);
            //mail.sende
            int indx = destinationIndex("mailroom");
            //controller.index = indx;
            pickup.Destination = indx;

            Task dropoff = new Task();
            // other.moveBot(index);
            indx = destinationIndex(info.ToString());//info is the receivers name
            //controller.index = index;
            dropoff.Destination = indx;
            pickup.Next = dropoff;
            tasks.Enqueue(pickup);
            // other.moveBot(index);

            //User receives a response for his mail. We just need to print it
            /*
            main_thread.Post(new SendOrPostCallback(new Action<object>(o => {
                var mainview0 = System.Windows.Application.Current.Windows.OfType<MetroWindow>().Last();
                mainview0.ShowMessageAsync("Notification", info.ToString(), MessageDialogStyle.Affirmative);
            })), null);
            */

        }



        /// <summary>
        /// Just displays the response sent by the server when user requests for coffee
        /// </summary>
        /// <param name="info">Server response</param>
        private void Coffee_Handler(object info)
        {
         //   int indx = destinationIndex(info.ToString());
            
            Task coffee = new Task(destinationIndex(info.ToString()));
            LinkedList <Task> coffeeTask = new LinkedList<Task>();
            coffeeTask.AddLast(coffee);
            ///execution code here
            //System.Diagnostics.Debug.WriteLine("I made it past the display Coffee_handler stuff");
            
           
            tasks.Enqueue(coffeeTask);
            
           // controller.index = indx;
        }

        private void Client_OnClientFileSending(object Sender, ClientFileSendingArguments R)
        {
            throw new NotImplementedException();
        }

        private void Client_OnClientError(object Sender, ClientErrorArguments R)
        {
            Debug.LogError(R.ErrorMessage);
            //     robot_battery.Stop();
            client.Disconnect();

            System.Diagnostics.Debug.WriteLine(R.ErrorMessage);
        }

        private void Client_OnClientDisconnected(object Sender, ClientDisconnectedArguments R)
        {
            Debug.Log(R.EventMessage);
            System.Diagnostics.Debug.WriteLine(R.EventMessage);
            isConnected = false;
            // robot_battery.Stop();
        }

        private void Client_OnClientConnected(object Sender, ClientConnectedArguments R)
        {
            Debug.Log(R.EventMessage);
            System.Diagnostics.Debug.WriteLine(R.EventMessage);
            isConnected = true;
        }

        public int destinationIndex(string name)
        {
            int ret = 0;
            switch (name.ToLower())
            {
                case "poornima":
                    ret = 1;
                    break;
                case "naorin":
                    ret = 2;
                    break;
                case "raghav":
                    ret = 3;
                    break;
                case "talha":
                    ret = 4;
                    break;
                case "terrell":
                    ret = 5;
                    break;
                case "thinh":
                    ret = 6;
                    break;
                case "mailroom":
                    ret = 0;
                    break;
            }
            return ret;
        }//destination index



    }//client


}//namespace


