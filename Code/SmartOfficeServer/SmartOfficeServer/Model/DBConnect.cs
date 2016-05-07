using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
namespace SmartOfficeServer.Model
{
    /// <summary>
    /// Used to establish connection with the MySQL database
    /// </summary>
    public class DBConnect
    {
        private MySqlConnection connection = null;
        //private MySqlDataReader reader = null;
        private String server;
        private String port;
        private String username;
        private String password;
        private String database;
        /// <summary>
        /// Constructor for creating db connection
        /// </summary>
        public DBConnect()
        {
            Initialize();
        }

        public TCPServer TCPServer
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }

        /// <summary>
        /// Provide MySQL DB details like server, database name, username, password etc
        /// </summary>
        private void Initialize()
        {
            server = "localhost";
            database = "smartoffice";
            username = "root";
            password = "123";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + username + ";" + "PASSWORD=" + password + ";";

            connection = new MySqlConnection(connectionString);
        }

        //open connection to database
        /// <summary>
        /// Tries to establish connection with the database
        /// </summary>
        /// <returns>True if connection is established, false otherwise</returns>
        public bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
           catch(MySqlException e)
            {
                Console.WriteLine(e.StackTrace);
                Console.WriteLine(e.Message);
                return false;
            }
        }

        //Close connection
        /// <summary>
        /// Attempts to close the current DB connection
        /// </summary>
        /// <returns>True if sucessfully closed, false otherwise</returns>
        public bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch(MySqlException e)
            {
                switch(e.Number)
                {
                    case 0:
                        Console.WriteLine("Unable to establish connection, Contact Admin/Check connection");
                        break;
                    case 1045:
                        Console.WriteLine("Invalid username/password!");
                        break;
                }
                return false;
            }//catch
            
        }//close connection

        //Insert statement
        /// <summary>
        /// Used to insert data into the database
        /// </summary>
        /// <param name="tablename">Name of the table data needs to be inserted into</param>
        /// <param name="columns">columns the data has to be inserted in</param>
        /// <param name="args">the actual data to be inserted. can contain orderby/limit parameters as well</param>
        /// <returns>true if sucessfully inserted, false otherwise</returns>
        public Boolean Insert(String tablename, String columns,List<String> args)
        {
            String query = "Insert into " + tablename + " (" + columns + ") values (";
            query += string.Join(",", args);
            /*foreach(String value in args)
            {
                //query += value + ",";
                query.Join(",", value);
            }*/
            query += ");";
            Console.WriteLine(query);
                    
            try
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch(MySqlException e)
            {
                Console.WriteLine(e.StackTrace);
                Console.WriteLine(e.Message);
                return false;
            }
            
        }

        //Update statement
        /// <summary>
        /// Updates current data in database
        /// </summary>
        /// <param name="tablename">table to be used in the mysql query</param>
        /// <param name="oldArg">the data to be replaced</param>
        /// <param name="newArg">the new data to be inserted</param>
        /// <param name="condition">condition that decides what column/row to look for</param>
        /// <returns>true if successful, false otherwise</returns>
        public Boolean Update(String tablename, String oldArg, String newArg, String condition)
        {
            String query = "";
            switch (tablename.ToLower())
            {
                case "user":
                    query = "Update user set " + oldArg + " = " + newArg + " where " + condition + ";";
                    break;
                case "delivery":
                    query = "Update delivery set " + oldArg + " = " + newArg + " where " + condition + ";";
                    break;
            }//switch
            try
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e.StackTrace);
                Console.WriteLine(e.Message);
                return false;
            }
        }


        //Delete statement
        /// <summary>
        /// Deletes data from the table
        /// </summary>
        /// <param name="tablename">table the data needs to be deleted from</param>
        /// <param name="condition">condition that decides what needs to be deleted</param>
        /// <returns>true if successful, false otherwise</returns>
        public Boolean Delete(String tablename, String condition)
        {
            String query = "";
            switch (tablename.ToLower())
            {
                case "user":
                    query = "Delete from user where "  + condition + ";";
                    break;
                case "delivery":
                    query = "Delete from delivery where "  + condition + ";";
                    break;
            }//switch
            try
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e.StackTrace);
                Console.WriteLine(e.Message);
                return false;
            }
        }


        /// <summary>
        /// Select query to fetch data from the database
        /// </summary>
        /// <param name="tablename">table to be used for the search</param>
        /// <param name="columns">columns that need to be returned. * for all columns</param>
        /// <param name="condition">condition that decides what gets fetched. * for everything</param>
        /// <returns>2D list where the outer list represents rows and the inner list represents columns</returns>
        public List<List<String>> SelectAll(String tablename,String columns, String condition)
        {
            String query = "";
            List<List<String>> returnData = new List<List<String>>();
            List<String> row = null;
            query = "Select " + columns + " from " + tablename + " where " + condition + ";";
            MySqlDataReader reader;
            try
            {
                //create and execute command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                reader = cmd.ExecuteReader();

                if (!reader.HasRows)
                {
                    reader.Close();
                    return null;    //no data found
                }
                while (reader.Read())
                {
                    row = new List<string>();
                    for (int i = 0; i <= 8; i++)
                    {
                        try
                        {
                            row.Add(reader[i].ToString());
                        }
                        catch(Exception e) { break; } //array out of bounds. when returned rows are less than 8
                    }//end for                           // nothing serioes and we can ignore
                    returnData.Add(row);
                }//end while

                reader.Close();
                return returnData;

            }
            catch (MySqlException e)
            {
                Console.WriteLine(e.StackTrace);
                Console.WriteLine(e.Message);
                return null;
            }
        }//end selectAll

        //Can be used to handle custom/complex queries
        public MySqlConnection getConnection()
        {
            return connection;
        }





    }//class
}//namespace
