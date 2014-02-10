using UnityEngine;
using System.Collections;

public class Shooter : PlayerTool {
	public GameObject m_Bullet;
	
	private float m_CooldownTimer = 0f;
	private float m_FireRate = 0.75f;
	private Transform m_Player;
	
	public Transform PlayerTransform { set {m_Player = value; } }
	
	void Update() {
		// Pretty sure this doesn't get called every frame...
		m_CooldownTimer -= Time.deltaTime;
		
		if (m_CooldownTimer <= 0.0f) {
			m_CooldownTimer = 0.0f;
		}
	}
	
	public override bool TryUsingTool() {
		if (m_CooldownTimer > 0.0f) {
			return false;
		}
		
		// Fire shot
		GameObject bullet = GameObject.Instantiate(m_Bullet, m_Player.position + m_Player.forward, Quaternion.identity) as GameObject;
		
		Bullet bulletScript = bullet.GetComponent<Bullet>();
		bulletScript.m_Velocity = m_Player.forward;
		
		m_CooldownTimer = m_FireRate;
		
		return true;
	}
}
