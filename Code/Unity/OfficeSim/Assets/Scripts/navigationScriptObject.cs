using UnityEngine;
using System.Collections;

public class navigationScriptObject : MonoBehaviour {
	
	public GameObject Target;
	//public GameObject Text;
	private NavMeshHit hit;
	private bool blocked = false;
	// Use this for initialization
	void Start () {
	}
	// ADD LABEL WHEN COLLISION DETECTED  
	// Update is called once per frame
	void Update () {
		
		// Navmesh sets destination
		gameObject.GetComponent<NavMeshAgent>().SetDestination(Target.transform.position);
		
		// Casting a ray how far the robot can see 
		Vector3 fwd = transform.TransformDirection(Vector3.forward);
		if (Physics.Raycast(transform.position, fwd ,7) == true)
		{
			Debug.DrawRay(transform.position, fwd * 5,Color.red);
			//set 3dtext active 
			//Text.SetActive(true);
		}
		else 
		{
			Debug.DrawRay(transform.position, fwd * 5,Color.green);
			// set 3dtext inactive
			//Text.SetActive(false);
		}
		
		// to draw the ray to target 
		blocked = NavMesh.Raycast(transform.position, Target.transform.position, out hit, NavMesh.AllAreas);
		Debug.DrawLine(transform.position, Target.transform.position, blocked ? Color.red : Color.green);
		// to draw the ray to obstacle 
		if (blocked){
			Debug.DrawRay(hit.position, Vector3.up, Color.yellow);
			Debug.DrawRay(hit.position, Vector3.forward, Color.yellow);
		}
	}
}


