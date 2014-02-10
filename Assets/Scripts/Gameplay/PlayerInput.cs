using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour {
	
	private Transform m_Trans;
	private Shooter m_Gun;
	
	// Use this for initialization
	void Start () {
		m_Trans = transform;
		m_Gun = m_Trans.GetComponent<Shooter>();
		m_Gun.PlayerTransform = m_Trans;
	}
	
	// Update is called once per frame
	void Update () {
	
		if (Input.GetKey(KeyCode.F)) {
			m_Gun.TryUsingTool();
		}
	}
}
