using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour {
	
	private Transform m_Trans;
	private Shooter m_Gun;
	private BeaconTool m_Beacon;
	private Entity m_Enemy;
	
	// Use this for initialization
	void Start () {
		m_Trans = transform;

		m_Enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Entity>();

		m_Gun = m_Trans.GetComponent<Shooter>();
		m_Gun.PlayerTransform = m_Trans;

		m_Beacon = m_Trans.GetComponent<BeaconTool>();
		m_Beacon.Enemy = m_Enemy;
	}
	
	// Update is called once per frame
	void Update () {
	
		if (Input.GetKey(KeyCode.F)) {
			m_Gun.TryUsingTool();
		}

		if (Input.GetKey(KeyCode.Alpha2)) {
			m_Beacon.TryUsingTool();
		}
	}
}
