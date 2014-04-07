using UnityEngine;
using System.Collections;

public class StunGrenadeTool : PlayerTool {

	public GameObject m_StunGrenade;

	// We'll only allow one activate stun grenade at a time
	private GameObject m_ActiveGrenade;
	private Transform m_Player;

	public Transform PlayerTransform { set {m_Player = value; } }

	// Use this for initialization
	void Start () {
	
	}

	public override bool TryUsingTool() {
		if (m_ActiveGrenade != null) {
			return false;
		}

		m_ActiveGrenade = GameObject.Instantiate(m_StunGrenade, m_Player.position + m_Player.forward + new Vector3(0, 0, -2f), Quaternion.identity) as GameObject;
		return true;
	}
}
