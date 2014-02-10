using UnityEngine;
using System.Collections;

public class SHMUPWeaponManager : MonoBehaviour {
	
	public float m_FireRate = 0.5f;
	
	public GameObject m_BulletPrefab;
	
	private bool m_FireUp = false;
	private bool m_FireDown = false;
	private bool m_FireLeft = false;
	private bool m_FireRight = false;
		
	private float m_CooldownTimer = 0.0f;
	private Transform m_Trans;
	
	// Use this for initialization
	void Start () {
		m_Trans = transform;
	}
	
	// Update is called once per frame
	void Update () {
		
		m_CooldownTimer -= Time.deltaTime;
		
		if (m_CooldownTimer <= 0.0f) {
			// reset the cooldown timer if we actually shot
			m_CooldownTimer = Fire() ? m_FireRate : 0.0f;
		}
		
	}
	
	// return whether we actually fired a shot or not
	private bool Fire() {
		bool didFire = false;
		
		if (m_FireUp) {
			InstantiateBullet(new Vector3(0, 1, 0));
			didFire = true;
		}
		
		if (m_FireLeft) {
			InstantiateBullet(new Vector3(-1, 0, 0));
			didFire = true;
		}
		
		if (m_FireDown) {
			InstantiateBullet(new Vector3(0, -1, 0));
			didFire = true;
		}
		
		if (m_FireRight) {
			InstantiateBullet(new Vector3(1, 0, 0));
			didFire = true;
		}
		
		return didFire;
	}
	
	private void InstantiateBullet(Vector3 direction) {
		GameObject bullet = GameObject.Instantiate(m_BulletPrefab, m_Trans.position, Quaternion.identity) as GameObject;
		bullet.GetComponent<SHMUPBullet>().Velocity = direction;
	}
	
	public void ToggleFireDirection(KeyCode direction) {
		switch(direction) {
		case KeyCode.UpArrow:
			m_FireUp = !m_FireUp;
			break;
			
		case KeyCode.LeftArrow:
			m_FireLeft = !m_FireLeft;
			break;
			
		case KeyCode.RightArrow:
			m_FireRight = !m_FireRight;
			break;
			
		case KeyCode.DownArrow:
			m_FireDown = !m_FireDown;
			break;
			
		default:
			Debug.LogWarning("Passed a non-direction \"" + direction + "\" to ToggleFireDirection");
			break;
		}
	}
}
