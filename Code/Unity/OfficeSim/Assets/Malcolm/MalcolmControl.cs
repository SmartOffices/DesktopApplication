
using UnityEngine;
using System.Collections;

public class MalcolmControl : MonoBehaviour {

	static Animator anim;
	public float speed = 0.1F;
	public float rotationSpeed = 0.1F;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {

	float translation = Input.GetAxis("Vertical") * speed;
	float rotation = Input.GetAxis("Horizontal") * rotationSpeed;
	translation *= Time.deltaTime;
		translation *=  0.3F;
	rotation *= Time.deltaTime;
		rotation *= 0.3F; 
	transform.Translate(0, 0, translation);
	transform.Rotate(0, rotation, 0);

		if (Input.GetKeyDown("space")){
			anim.SetBool ("isJumping",true);
		}
		else {
			anim.SetBool ("isJumping",false);
		}
		if (translation != 0) {
			anim.SetBool ("isWalking", true);
		} 
		else {
			anim.SetBool ("isWalking", false);
		}
	}


}
