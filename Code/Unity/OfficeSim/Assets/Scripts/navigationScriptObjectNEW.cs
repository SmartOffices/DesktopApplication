// Coded by Talha Mahmood 
using UnityEngine;
using System.Collections.Generic;
using SmartOfficeMetro;
using BMove;
using System;

namespace BMove{



public class navigationScriptObjectNEW: MonoBehaviour {
	public int index;
	public GameObject LightR;
	public GameObject LightG;
	public GameObject TextObjectDetected;
	public GameObject TextRRotCW;
	public GameObject TextRRotCCW;
	public GameObject TextRobotMovingF;
	public GameObject TextRobotStopped;
	private int angle;
	private int prevangle;
	private float pos;
	private float prevpos;
	


	//public GameObject Text;
	private NavMeshHit hit;
	private bool blocked = false;
	public int battery_life = 100;
	public int frame = 120;
	private bool delivery;
    private float reach_distance = 1.0f;
	public Transform[] empPoints;
    private NavMeshAgent agent;
    private float agent_remaining_distance;
    public SmartOfficeClient soc;



	public void moveBot(int indx)

	{
		this.index = indx -1 ;
		Start();
		Update();
		System.Threading.Thread.Sleep(5000);
		
	}
        public void waypoint(List<int> waypoints)
        {
            foreach(int point in waypoints)
            {
                this.index = point;
                Debug.LogError("going for " + point);
                for (;;)
                {
                    if(agent_remaining_distance >= 0.0)
                    {
                        Debug.LogError("reached goal 1");
                     //   soc.TaskManager().task.Status = true;
                        break;
                    }
                }
            }
        }
	// Use this for initialization
	void Start () {
            //transform.position = empPoints[0].position;
         //   transform.position = soc.task.Destination;
  	     	agent = gameObject.GetComponent<NavMeshAgent>();
			TextRRotCW.SetActive(false);
			TextRRotCCW.SetActive(false);
			TextRobotMovingF.SetActive(false);
			TextRobotStopped.SetActive(false);

    }


        public void TaskManager()
        {

            try
            {
                getNewTask:
                LinkedList<Task> task = SmartOfficeClient.tasks.Dequeue();   //temporarily stores task to be executed
                LinkedListNode<Task> node = task.First;
                ExecuteTaskChain:
                while (!(node.Value.Status))
                {
                    index = node.Value.Destination;
                    if (agent_remaining_distance <= 0.0)
                    {
                        node.Value.Status = true;
                        break;
                    }
                }

                node = node.Next;
                while (node != null)
                {
                    goto ExecuteTaskChain;
                }
                goto getNewTask;
            }
            catch(Exception e)
            {

            }
        }

        // Update is called once per frame
        void Update () {

            TaskManager();

        // Navmesh sets destination
        gameObject.GetComponent<NavMeshAgent>().SetDestination(empPoints[this.index].position);
           // agent.
            agent_remaining_distance = agent.remainingDistance;
		/*
		if (transform.position == empPoints[this.index]){
			delivery = true;
		}
		
		if (delivery)
		{
			delivery = false;
			battery_life--;
		}
		*/

		// Casting a ray how far the robot can see 
		Vector3 fwd = transform.TransformDirection(Vector3.forward);
		if (Physics.Raycast(transform.position, fwd ,7) == true)
		{
			Debug.DrawRay(transform.position, fwd * 5,Color.red);
			//set 3dtext active 
				TextObjectDetected.SetActive(true);
				LightR.SetActive(true);
				LightG.SetActive(false);
		}
		else 
		{
			Debug.DrawRay(transform.position, fwd * 5,Color.green);
			// set 3dtext inactive
				TextObjectDetected.SetActive(false);
				LightG.SetActive(true);
				LightR.SetActive(false);
		}

		// Logging Rotation
			prevangle = angle;
			angle = (int) transform.rotation.eulerAngles.y; 
		if ( angle - prevangle > 0){
				TextRRotCW.SetActive(true);
				TextRRotCCW.SetActive(false);
			}
		else if (angle - prevangle < 0) { 
				TextRRotCW.SetActive(false);
				TextRRotCCW.SetActive(true);
			 }
		else { 
				TextRRotCW.SetActive(false);
				TextRRotCCW.SetActive(false);
			}

		// Logging Movement
			prevpos = pos;
			pos =  transform.position.sqrMagnitude;
		if(pos - prevpos == 0.0f) {
				TextRobotMovingF.SetActive(false);
				TextRobotStopped.SetActive(true);
				LightG.SetActive(true);
				LightR.SetActive(true);
			}
		else {
				TextRobotMovingF.SetActive(true);
				TextRobotStopped.SetActive(false);
				LightG.SetActive(true);
				LightR.SetActive(false);
			}

		
		// to draw the ray to target 
		blocked = NavMesh.Raycast(transform.position, empPoints[this.index].transform.position, out hit, NavMesh.AllAreas);
		Debug.DrawLine(transform.position, empPoints[this.index].transform.position, blocked ? Color.red : Color.green);
		// to draw the ray to obstacle 
		if (blocked){
			Debug.DrawRay(hit.position, Vector3.up, Color.yellow);
			Debug.DrawRay(hit.position, Vector3.forward, Color.yellow);
		}
	}
}

}


