using UnityEngine;
using System.Collections;

public class PlayerMotor : MonoBehaviour {
	
	public float m_Speed = 50.0f;
	private float m_MaxForce = 10.0f;
	
	
	private Transform m_Trans;
	private Rigidbody m_Rigidbody;
	
	// Use this for initialization
	void Start () {
		m_Trans = transform;
		m_Rigidbody = m_Trans.rigidbody;
	}
	
	// Update is called once per frame
	void Update () {
	
		Vector3 desiredVelocity = Vector3.zero;
		bool ShouldMove = false;
		
		if (Input.GetKey(KeyCode.UpArrow)) {
			desiredVelocity += new Vector3(0, m_Speed, 0);
			ShouldMove = true;
		}
		if (Input.GetKey(KeyCode.LeftArrow)) {
			desiredVelocity += new Vector3(-m_Speed, 0, 0);
			ShouldMove = true;
		}
		if (Input.GetKey(KeyCode.DownArrow)) {
			desiredVelocity += new Vector3(0, -m_Speed,0);
			ShouldMove = true;
		}
		if (Input.GetKey(KeyCode.RightArrow)) {
			desiredVelocity += new Vector3(m_Speed, 0, 0);
			ShouldMove = true;
		}
		
		if (ShouldMove) {
			Rotate(desiredVelocity);
			
			m_Trans.Translate(m_Trans.forward * m_Speed * Time.deltaTime, Space.World);
			//m_Rigidbody.velocity = m_Trans.forward * m_Speed * Time.deltaTime;
			//m_Rigidbody.AddForce(m_Trans.forward * m_Speed * Time.deltaTime, ForceMode.VelocityChange);
		}
		else {
			//m_Rigidbody.velocity = Vector3.zero;
		}
	}
	
	
	private void Rotate(Vector3 Direction) {
		m_Trans.LookAt(m_Trans.position + Direction, m_Trans.up);
	}
}
