using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RemoveOnStart : MonoBehaviour 
{
	public List<Collider> m_collidersToRemove = new List<Collider>();

	// Use this for initialization
	void Start () 
	{
		foreach(Collider col in m_collidersToRemove)
		{
			Destroy(col);
		}
	}

}
