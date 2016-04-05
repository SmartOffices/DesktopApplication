using UnityEngine;
using System.Collections.Generic;

namespace BMove{



public class navigationScriptObjectNEW: MonoBehaviour {
	public int index;
	public GameObject LightR;
	public GameObject LightG;

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
                        break;
                    }
                }
            }
        }
	// Use this for initialization
	void Start () {
		transform.position = empPoints[0].position;
            agent = gameObject.GetComponent<NavMeshAgent>();

    }
	// ADD LABEL WHEN COLLISION DETECTED  
	// Update is called once per frame
	void Update () {
		

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
			//Text.SetActive(true);
				LightR.SetActive(true);
				LightG.SetActive(false);
		}
		else 
		{
			Debug.DrawRay(transform.position, fwd * 5,Color.green);
			// set 3dtext inactive
			//Text.SetActive(false);
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

