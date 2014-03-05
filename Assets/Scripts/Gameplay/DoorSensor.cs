using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DoorSensor : MonoBehaviour 
{

	public DoorManager m_doorManager; //Door to open

	private List<Transform> m_trackedActors = new List<Transform>();

	public void OnTriggerEnter2D(Collider2D col)
	{


		if(col.tag == "Player" || col.tag == "Enemy")
		{
			if(!m_trackedActors.Contains(col.transform))
			{
			  	m_trackedActors.Add (col.transform);

				if(!m_doorManager.m_isOpen)
				{
					m_doorManager.OpenDoor();
				}
			}
		}

	}

	public void OnTriggerExit2D(Collider2D col)
	{
		Debug.Log (col.tag + " Has Exited Sensor");
		if(col.tag == "Player" || col.tag == "Enemy")
		{
			if(m_trackedActors.Contains(col.transform))
			{
				m_trackedActors.Remove(col.transform);

				if(m_trackedActors.Count == 0)
				{
					m_doorManager.CloseDoor();
				}
			}
		}
	}
}
