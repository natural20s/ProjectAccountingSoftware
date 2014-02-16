using UnityEngine;
using System.Collections;

public class NavMeshTest : MonoBehaviour {

	private NavMeshAgent m_Agent;
	private Transform m_Trans;

	// Use this for initialization
	void Start () {
		m_Trans = transform;
		m_Agent = m_Trans.GetComponent<NavMeshAgent>();

		m_Agent.SetDestination(new Vector3(0, 0, 0));
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
