// coded by Talha Mahmood 
using UnityEngine;
using System.Collections;

public class navigationScriptVector : MonoBehaviour {
	
	public Vector3 Target;
	private NavMeshHit hit;
	private bool blocked = false;
	
	// Use this for initialization
	void Start () {
		Target.x = 253;
		Target.y = 0.2980683f;
		Target.z = 181;
	}
	
	// Update is called once per frame
	void Update () {
		//Sets destination
		gameObject.GetComponent<NavMeshAgent>().SetDestination(Target);

		// to draw the ray to target 
		blocked = NavMesh.Raycast(transform.position, Target, out hit, NavMesh.AllAreas);
		Debug.DrawLine(transform.position, Target, blocked ? Color.red : Color.green);
		// to draw the ray to obstacle 
		if (blocked){
			Debug.DrawRay(hit.position, Vector3.up, Color.yellow);
			Debug.DrawRay(hit.position, Vector3.forward, Color.yellow);
		}
		/*
		if(transform.position == Target){
			gameObject.GetComponent<NavMeshAgent>().Stop;
		}
		*/

	}
}

