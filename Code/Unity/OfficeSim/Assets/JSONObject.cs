using System;
namespace SmartOfficeMetro.Model
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
        /// <param name="requestType">1: Notifications(Any kind) 2: Coffee 3: Login request 4: Mail request 5: BLANK </param>
        /// <param name="information">The data to be passed</param>
        public JSONObject(int requestType, Object information)
        {
            this.requestType = requestType;
            info = information;
        }
    }// JSON object

    public class Mail
    {
        public String mailDestination;
        public DateTime mailTime;
        public String subject;
        public String note;
        public Mail(String mailDestination, DateTime mailTime, String subject, String note)
        {
            this.mailDestination = mailDestination;
            this.mailTime = mailTime;
            this.subject = subject;
            this.note = note;
        }
    }//mail

    //this class is just for JSON object handling. Do not confuse it with the UserManager
    public class User
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
    }// user login

    public class Notification
    {
        public String sender;
        public String subject;
        public String description;

        public Notification(String sender, String subject, String description)
        {
            this.sender = sender;
            this.subject = subject;
            this.description = description;
        }
    }//notification
    public class loginObject
    {
        public Boolean loginStatus;
        public User user;
        public loginObject(Boolean loginStatus, User user)
        {
            this.loginStatus = loginStatus;
            this.user = user;
        }
    }//loginObject

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

    public class Robot
    {
        public String id;
        public Double battery_level;

        public Robot(String id, Double battery_level)
        {
            this.id = id;
            this.battery_level = battery_level;
        }
    }//robot


}//namespace
