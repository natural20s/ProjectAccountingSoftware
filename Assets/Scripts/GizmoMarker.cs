using UnityEngine;
using System.Collections;

public class GizmoMarker : MonoBehaviour 
{

	public float m_size;

	public void OnDrawGizmos()
	{
		Gizmos.DrawWireCube(transform.position, new Vector3(m_size, m_size, m_size));
	}
}
