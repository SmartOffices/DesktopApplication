//Code Contributers: Poornima Suresh

using UnityEngine;
using System.Collections;



namespace BMove
{

    public class A                                  //dummy class to hold index
    {
        public int ind;

    }

    public class BotMovement : MonoBehaviour
    {
        public A a;
        public void moveBot(int indx)               //function to be called by client file to move robot
        {
            a.ind = indx;                           //index of position in empPoints array
            Start();
            Update();
            System.Threading.Thread.Sleep(5000);    //wait 5 seconds to process the next order

        }

        //speed and movement variables:
        public float moveSpeed;
        private float maxSpeed = 5f;
        private Vector3 input;

        
        public Transform[] empPoints;               //array containing positions, defined in the Unity GUI
        public int battery_life = 100;              //Robot's battery percentage
        private bool delivery;                      //whether or not the delivery has taken place

        void Start()
        {
            //initialize robot's position at the lobby
            transform.position = empPoints[0].position;
        }

        // Update is called once per frame
        void Update()
        {
            //moves robot to location specified by index
            transform.position = Vector3.MoveTowards(transform.position, empPoints[a.ind].position, moveSpeed * Time.deltaTime);

            //if the robot has reached its destination
            if (transform.position == a.ind)
            {
                delivery = true;    // the delivery has occurred
            }

            if (delivery)           //if the delivery has occurred
            {
                delivery = false;   //set delivery to false until next delivery occurs
                battery_life--;     //decrement battery life
            }

            //Controlled movement: robot moves left, right, up, and down when respective keys are pressed
            /*
            input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
            if (GetComponent<Rigidbody>().velocity.magnitude < maxSpeed)
            {
                GetComponent<Rigidbody>().AddForce(input*moveSpeed);
            }

            // deprecated rigidbody.AddForce(input);

            //print(input);
            */
        }
    }

}
