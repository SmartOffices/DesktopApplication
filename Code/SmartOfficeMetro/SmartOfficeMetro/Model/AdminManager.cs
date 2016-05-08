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
//Singleton for handling admin functions 
    public sealed class AdminManager 
    {
        private static AdminManager instance = null;
        private static readonly object padlock = new object();

        public List<Robot> robot_list { get; set; }
        public List<String> logged_in_users { get; set; }
        public int battery_degradation { get; set; }
        AdminManager()
        {

        }
        public static AdminManager Instance
        {
            get
            {
                lock(padlock)
                {
                    if (instance == null)
                    {
                        instance = new AdminManager();
                    }
                    return instance;
                }
                
            }
        }//Instance



    }//class
}//namespace