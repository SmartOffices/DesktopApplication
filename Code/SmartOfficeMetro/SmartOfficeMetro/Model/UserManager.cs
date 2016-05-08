using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Created by Raghav Bhardwaj
/// </summary>
namespace SmartOfficeMetro.Model
{
    /// <summary>
    /// Singleton for handling user's data
    /// </summary>
    public sealed class UserManager
    {
        private static UserManager instance = null;
        private static readonly object padlock = new object();

        public String id { get; set; }
        public String username { get; set; }
        public String password { get; set; }
        public String Name { get; set; }
        public String age { get; set; }
        public String email { get; set; }
        public String phone { get; set; }
        public String department { get; set; }
        public String image { get; set; }
        public Boolean notification { get; set; }
       // public List<Notification> current_notifications { get; set; }
        public List<List<String>> current_notifications { get; set; }
        public List<List<String>> user_details { get; set; }
        public List<List<String>> delivery_history { get; set; }
        UserManager()
        {
            
        }

        public static UserManager Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new UserManager();
                    }
                    return instance;
                }
            }
        }
    }//class

}//namespace
