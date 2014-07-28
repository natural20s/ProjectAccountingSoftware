using UnityEngine;
using System.Collections;

public class PlayerMotor : MonoBehaviour {
	
	public float m_Speed = 50.0f;

	private Transform m_Trans;
	
	// Use this for initialization
	void Start () {
		m_Trans = transform;
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
			
			m_Trans.Translate(m_Trans.up * m_Speed * Time.deltaTime, Space.World);
			//m_Rigidbody.velocity = m_Trans.forward * m_Speed * Time.deltaTime;
			//m_Rigidbody.AddForce(m_Trans.forward * m_Speed * Time.deltaTime, ForceMode.VelocityChange);
		}
		else {
			//m_Rigidbody.velocity = Vector3.zero;
		}
	}
	
	
	private void Rotate(Vector3 Direction) {
		// DON'T LOOK AT ME LIKE THAT
		if (Direction.x == 0 && Direction.y == m_Speed) { 
			m_Trans.rotation = Quaternion.Euler(0, 0, 0); 
		} 
		else if (Direction.x == -m_Speed && Direction.y == m_Speed) {
			m_Trans.rotation = Quaternion.Euler(0, 0, 45); 
		}
		else if (Direction.x == -m_Speed && Direction.y == 0) {
			m_Trans.rotation = Quaternion.Euler(0, 0, 90); 
		}
		else if (Direction.x == -m_Speed && Direction.y == -m_Speed) {
			m_Trans.rotation = Quaternion.Euler(0, 0, 135); 
		}
		else if (Direction.x == 0 && Direction.y == -m_Speed) {
			m_Trans.rotation = Quaternion.Euler(0, 0, 180); 
		}
		else if (Direction.x == m_Speed && Direction.y == -m_Speed) {
			m_Trans.rotation = Quaternion.Euler(0, 0,225); 
		}
		else if (Direction.x == m_Speed && Direction.y == 0) {
			m_Trans.rotation = Quaternion.Euler(0, 0,270); 
		}
		else if (Direction.x == m_Speed && Direction.y == m_Speed) {
			m_Trans.rotation = Quaternion.Euler(0, 0, 315); 
		}

		// Used the following for 3D
		//m_Trans.LookAt(m_Trans.position + Direction, m_Trans.up);
	}
}
