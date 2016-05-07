using System;

using System.Windows.Input;

using MahApps.Metro.Controls;

using System.Windows;
using SmartOfficeMetro.Model;

using System.Threading;
using System.ComponentModel;

namespace SmartOfficeMetro
{

        /// <summary>
        /// Interaction logic for Login.xaml
        /// </summary>
        /// 
        
        public partial class Login : MetroWindow, INotifyPropertyChanged
        {

            SmartOfficeClient client;
            private SynchronizationContext _uiContext = SynchronizationContext.Current;
           // public static User userLogin;
            private bool isvisible; //needed to display the spinner while logging in
            private static Boolean allowLogin;
            MainWindow mainDisplay = null;
            /// <summary>
            /// Needed to start and stop the spinner motion when data is being fetched
            /// </summary>
            public bool isVisible
            {
                get { return isvisible; }
                set
                {
                    if (isvisible != value)
                    {
                        isvisible = value;
                        OnPropertyChanged("isVisible");  // To notify when the property is changed
                    }
                }
            }

            public Login()
            {
                InitializeComponent();
               
                isVisible = false;
                DataContext = this;
                textBoxUsername.Focus();
            }


        /// <summary>
        /// Notifies the UI that a UI element property has changed and it needs to refresh
        /// </summary>
        /// <param name="propertyName"></param>
            public event PropertyChangedEventHandler PropertyChanged;
            protected virtual void OnPropertyChanged(string propertyName)
            {
                PropertyChangedEventHandler handler = PropertyChanged;
                if (handler != null)
                    handler(this, new PropertyChangedEventArgs(propertyName));
            }

            /// <summary>
            /// Start the spinner motion(isVisible=true) and start the login task as another thread
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void buttonLogin_Click(object sender, RoutedEventArgs e)
            {
                //_uiContext = SynchronizationContext.Current;
             //   UserManager.Instance.username = textBoxUsername.Text;
             //   UserManager.Instance.password = passwordBox.Password;
                //LoginDialogData details = await this.ShowLoginAsync("login please", "loginnnn");

               // UserManager.Instance.Name = "";
               // UserManager.Instance.image = "kitten-2.jpg";
                //isVisible = true;
           //     UserManager.Instance.username = textBoxUsername.Text;
          //      UserManager.Instance.password = passwordBox.Password;
         //       UserManager.Instance.Name = textBoxUsername.Text;

//                Application.Current.MainWindow = mainDisplay;
              //  this.Close();
  //              mainDisplay.Show();
                // mainDisplay.UpdateUI();
              //  this.Close();
               // mainDisplay.Show();

                // UserManager.Instance.image = "kitten-2.jpg";

                // MainWindow createDisplay = new MainWindow(UserManager.Instance);
                // Application.Current.MainWindow = createDisplay;
                // this.Close();
                //  createDisplay.Show();
                // this.Hide();
                // var form2 = new MainWindow(UserManager.Instance);
                // form2.Closed += (s, args) => this.Close();
                // form2.Show();

                isVisible = true;
            UserManager.Instance.username = textBoxUsername.Text;
            UserManager.Instance.password = passwordBox.Password;
            UserManager.Instance.image = "Contacts-96.png";
            /*      Thread startLogin = new Thread(new ThreadStart(loginUser));
                          startLogin.SetApartmentState(ApartmentState.STA);
                          startLogin.IsBackground = true;
                          startLogin.Start();
      */
           
            //Create initial UI of the main window while background worker authenticates
            mainDisplay = new MainWindow();
            Application.Current.MainWindow = mainDisplay;

            //mainDisplay.UpdateUI();
            mainDisplay.Show();
            // Task<Boolean> login_result = new Task<Boolean>(() => loginUser());
            /*
                        if(false)
                        {
                            createDisplay = new MainWindow(UserManager.Instance);
                            Application.Current.MainWindow = createDisplay;
                            createDisplay.Show();
                            this.Close();
                        }
                        else
                        {
                            this.ShowMessageAsync("ad", "dasd");
                        }
             */
            //Application.Current.MainWindow.
            //createDisplay.Show();
            //this.Close();
            // loginUser();
            //Thread.Sleep(5000);


        }//button click
     /*
             private void toggleSpinner()
             {
                 isVisible = !isVisible;
             }
             private Image resizeImage(Image image, System.Drawing.Size size)
             {
                 return (Image)(new Bitmap(image, size));
             }

             private void clickBox(object sender, RoutedEventArgs e)
             {
                 isVisible = !isVisible;
             }
          */
     /// <summary>
     /// Login method for the window. Fetches the username and password and requests login via the client
     /// </summary>
        private void loginUser()
        {
           
                
            
            //Make initial connection with the server
            client = new SmartOfficeClient(UserManager.Instance);

            //Give client a small time to connect
            System.Threading.Thread.Sleep(10);
            //allowLogin = (client.requestLogin(UserManager.Instance, _uiContext));


            if ((client.requestLogin(UserManager.Instance, _uiContext)))
            {
                allowLogin = true;
                _uiContext.Post(new SendOrPostCallback(new Action<object>(o =>
                {
                    mainDisplay.UpdateUI();
                    mainDisplay.Show();
                    this.Close();
                })), null);
            }
            else
            {
                allowLogin = false;
            }
            isVisible = false;
            //Once data is done loading, stop the spinner
            //     isVisible = false;
            // System.Windows.Threading.Dispatcher.Run();
            // return result;
            /*
            //NEED TO REPLACE BY UserLogin OBJECT SOON!!
            UserLogin userLogin = new UserLogin(username, password);
            User loginUser = new User(username, password, Name, image);

            //Create Client of user
            client = new SmartOfficeClient(userLogin);
            System.Diagnostics.Debug.WriteLine("IM PAST THE CONNECTION PHASE");
            */
            /*
            //Give client a small time to connect
            System.Threading.Thread.Sleep(10);

            //request login
            if((client.requestLogin(userLogin, _uiContext)))
            {
                MainWindow createDisplay = new MainWindow(loginUser);
                Application.Current.MainWindow = createDisplay;
                this.Close();
                createDisplay.Show();
            }
            */

            //setSpinner(false);
            /*
            //try establishing connection with the server
            while (!SmartOfficeClient.dataReceived || counter < connectionCounter)
            {
                System.Threading.Thread.Sleep(500);
                counter++;
            }
            //if still can't establish connection, display error else continue to login check;
            if (!SmartOfficeClient.dataReceived)
            {
                this.ShowMessageAsync("Oops", "Unable to establish connection with server. Please try again later", MessageDialogStyle.Affirmative);
                this.BringIntoView();
            }
            else
            {
                if (SmartOfficeClient.allow)
                {
                    
                }
                else
                {
                    this.ShowMessageAsync("Oops", "Your Login information seems to be incorrect! Please try again");
                    client.disconnect();
                }
            //}*/
        }//login

       

        private void MetroWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                buttonLogin_Click(sender, e);
            }
        }

        /// <summary>
        /// Had to override the default closing method to make sure threads are closed properly as well
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 


    }//end class
    
}//end namespace
