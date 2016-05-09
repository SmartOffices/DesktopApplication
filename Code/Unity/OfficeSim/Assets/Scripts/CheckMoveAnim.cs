// Coded by Thinh Nguyen

using UnityEngine;
using System.Collections;

public class CheckMoveAnim : MonoBehaviour {
	public GameObject zombie;
	static Animator anim;
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		if(zombie.transform.hasChanged == false){
			anim.SetBool("isMoving",false);
		}
		else {
			anim.SetBool("isMoving",true);
		}
	}
}
