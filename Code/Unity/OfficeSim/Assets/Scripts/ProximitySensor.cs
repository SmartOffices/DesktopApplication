using UnityEngine;
using System.Collections;

public class ProximitySensor : MonoBehaviour
{
	public float Distance = 20.0f;
	
	void OnDrawGizmos()
	{
		Gizmos.DrawIcon(transform.position, "eye.png");
	}
	
	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawRay(transform.position, transform.forward * Distance);
		Gizmos.color = Color.white;
	}
	
	public bool Detect(out RaycastHit hit)
	{
		if (Physics.Raycast(transform.position, transform.forward, out hit, Distance))
		{
			if (hit.transform != transform)
			{
				Debug.DrawLine(transform.position, hit.point, Color.red);
				return true;
			}
		}        return false;
	}
}