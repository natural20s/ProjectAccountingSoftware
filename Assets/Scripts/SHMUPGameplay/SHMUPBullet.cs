using UnityEngine;
using System.Collections;

public class SHMUPBullet : MonoBehaviour {
	
	private Vector3 m_Velocity = Vector3.zero;
	private float m_Speed = 5.5f;
	private Transform m_Trans;
	
	public Vector3 Velocity { get { return m_Velocity; } set { m_Velocity = value; } }
	
	private float m_TempKillAfterSeconds = 3.0f;
	
	// Use this for initialization
	void Start () {
		m_Trans = transform;
	}
	
	// Update is called once per frame
	void Update () {
		
		UpdatePosition();
		
		
		if (m_TempKillAfterSeconds <= 0.0f) {
			Destroy(this.gameObject);
		}
		
		m_TempKillAfterSeconds -= Time.deltaTime;
	}
	
	private void UpdatePosition() {
		m_Trans.Translate(m_Velocity * Time.deltaTime * m_Speed);
	}
}
