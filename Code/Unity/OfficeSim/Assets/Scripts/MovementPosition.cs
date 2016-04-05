using UnityEngine;
using System.IO; 

public class MovementPosition : MonoBehaviour {
	// Vector to hold position
	Vector3 Location;
	string fileName = @"/Users/talhamahmood/Desktop/position.txt";
	StreamWriter fs = null; 


	// Use this for initialization
	void Start () {
		fs = File.CreateText(fileName);
		fs.WriteLine("Position of Robot");
	}
	
	// Update is called once per frame
	void Update () {
		Location = transform.position;
		fs.WriteLine(Location);
	}

	/*
	private float mySlider = 1.0f;
	void OnGUI(){
		mySlider = LabelSlider (new Rect (10, 100, 100, 20), mySlider, 5.0f, "Distance");
		// GUI.Box (new Rect (Screen.width - 100,0,100,50), "Top-right");
		//GUI.Box (new Rect (0,0,100,50), "Top-left");
		//GUI.Box (new Rect (0,Screen.height - 50,100,50), "Bottom-left");
		//GUI.Box (new Rect (Screen.width - 100,Screen.height - 50,100,50), "Bottom-right");
	}

	float LabelSlider (Rect screenRect, float sliderValue, float sliderMaxValue, string labelText) {
		GUI.Label (screenRect, labelText);
		
		// <- Push the Slider to the end of the Label
		screenRect.x += screenRect.width; 
		
		sliderValue = GUI.HorizontalSlider (screenRect, sliderValue, 0.0f, sliderMaxValue);
		return sliderValue;
	}
	*/
}
