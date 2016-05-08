/// Raghav Bhardwaj
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SmartOfficeServer
{
    class Program
    {
        public static int Main(string[] args)
        {

            TCPServer Init = new TCPServer();
            Console.WriteLine("Begin Server initialization..............");
            Init.initializeServer();
            Console.WriteLine("Begin database initialization............");
            Init.intializeDatabase();
            Console.WriteLine("Fetching list of users............");
            Init.initializeSettings();
            Console.WriteLine("Ending Initialization...... \nServer setup fully \n==================================");

        Read:
            String input = Console.ReadLine();
            if (input.ToLower() == "disconnect all")
            {

                foreach (String user in Init.logged_in_users)
                {
                    try
                    {
                        Init.server.DisconnectClient(user);
                        Console.WriteLine("Disconnected " + user);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Error disconnecting: " + user);
                    }
                    //Init.server.DisconnectClient()
                }
            }
            goto Read;
            return 0;

        }
    }   
}
