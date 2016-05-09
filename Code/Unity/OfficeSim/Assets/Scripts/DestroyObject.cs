// Coded by Talha Mahmood
using UnityEngine;
using System.Collections;

public class DestroyObject : MonoBehaviour {

	void OnCollisionEnter (Collision col)
	{
		if(col.gameObject.name == "Yaku_J_Ignite")
		{
			Destroy(col.gameObject);
		}
	}
}
