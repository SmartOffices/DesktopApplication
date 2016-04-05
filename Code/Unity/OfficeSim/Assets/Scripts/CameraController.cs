using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	public Camera cam1; 
	public Camera cam2; 

	void Start() { 
		cam1.enabled = true;
		cam2.enabled = false;
	}
	void Update() {
		if(Input.GetKeyUp(KeyCode.C)){
			cam1.enabled = !cam1.enabled;
			cam2.enabled = !cam2.enabled;
		}

	}

}
