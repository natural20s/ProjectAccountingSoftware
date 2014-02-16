using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
	
	public Vector2 m_Velocity = Vector2.zero;
	public float m_Speed = 4.5f;
	
	private float m_Timer = 3.5f;
	private Transform m_Trans;
	private Rigidbody m_Rigidbody;
	private const ToolType m_ToolType = ToolType.Magnet;
	
	// Use this for initialization
	void Start () {
		m_Trans = transform;
		//m_Rigidbody = m_Trans.rigidbody;
	}
	
	// Update is called once per frame
	void Update () {
		if (m_Velocity != Vector2.zero) {
			//m_Rigidbody.AddForce(m_Velocity * Time.deltaTime * m_Speed, ForceMode.VelocityChange);
			m_Trans.Translate(m_Velocity * Time.deltaTime * m_Speed);
		}
		
		m_Timer -= Time.deltaTime;
		if (m_Timer <= 0.0f) {
			Destroy(this.gameObject);
		}
	}
	
	public void OnCollisionEnter(Collision collision) {
		Transform CollisionTrans = collision.transform;
		
		if (CollisionTrans.tag == "Switch") {
			CollisionTrans.GetComponent<Switch>().Trigger(m_ToolType);
		}
	}
}
