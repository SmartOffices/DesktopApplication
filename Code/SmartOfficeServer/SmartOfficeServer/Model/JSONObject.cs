using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartOfficeServer.Model
{

    /// <summary>
    /// Converts an object to a JSON object accompnied with a requestType identifier
    /// </summary>
    class JSONObject
    {

        public int requestType;
        public Object info;

        /// <summary>
        /// Converts an object to a JSON object accompnied with a requestType identifier
        /// </summary>
        /// <param name="requestType">1: Notifications(Any kind) 2: Coffee 3: Login request 4: Mail request 5: Delivery request 6: Initial user Data 7: delivery history 8: notification history 9: robot status</param>
        /// <param name="information">The data to be passed</param>
        public JSONObject(int requestType, Object information)
        {
            this.requestType = requestType;
            info = information;     
        }
    }// JSON object

    /// <summary>
    /// Mail object required by JSON to send mail request to the client/server
    /// </summary>
    public class Mail
    {
        public String mailDestination;
        public DateTime mailTime;
        public String subject;
        public String note;
        /// <summary>
        /// Constructor for mail object
        /// </summary>
        /// <param name="mailDestination">person the mail needs to go to</param>
        /// <param name="mailTime">time the mail needs to be sent</param>
        /// <param name="subject">subject of the mail</param>
        /// <param name="note">additional remarks that may need to be added</param>
        public Mail(String mailDestination, DateTime mailTime, String subject, String note)
        {
            this.mailDestination = mailDestination;
            this.mailTime = mailTime;
            this.subject = subject;
            this.note = note;
        }
    }//mail

    class User
    {
        public String id;
        public String username;
        public String password;
        public String Name;
        public String age;
        public String email;
        public String phone;
        public String department;
        public String image;
        public Boolean notification;
        
        public User(String username, String password)
        {
            
            this.username = username;
            this.password = password;
            this.notification = false;
        }

        public TCPServer t
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }
    }// user login

    class Notification
    {
        public String sender;
        public String subject;
        public String description;

        /// <summary>
        /// Constructor that represnets a notification objecy
        /// </summary>
        /// <param name="sender">sender of the notification</param>
        /// <param name="subject">subject of the notification</param>
        /// <param name="description">any added information that needs to be attached</param>
        public Notification(String sender, String subject, String description)
        {
            this.sender = sender;
            this.subject = subject;
            this.description = description;
        }
    }
    class loginObject
    {
        public Boolean loginStatus;
        public User user;
        public loginObject(Boolean loginStatus, User user)
        {
            this.loginStatus = loginStatus;
            this.user = user;
        }
    }//login


    public class Destination
    {
        public String destination;
        public Destination(String destination)
        {
            this.destination = destination;
        }
        public override String ToString()
        {
            return destination;
        }
    }// destination

    /// <summary>
    /// Represents the robots in the office
    /// </summary>
    public class Robot
    {
        public String id;
        public Double battery_level;

        /// <summary>
        /// Constructor for the robots
        /// </summary>
        /// <param name="id">ID assigned to the robot</param>
        /// <param name="battery_level">batery level of the associated robot</param>
        public Robot(String id, Double battery_level)
        {
            this.id = id;
            this.battery_level = battery_level;
        }
    }//robot

    
}
