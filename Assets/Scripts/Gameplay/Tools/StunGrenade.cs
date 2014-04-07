using UnityEngine;
using System.Collections;

public class StunGrenade : MonoBehaviour {

	public float m_StunDuration = 2.0f;

	public float m_TimeTillDetonation = 1.0f;
	public float m_DetonateDistance = 8.0f;

	private Transform m_Trans;


	// Use this for initialization
	void Start () {
		m_Trans = transform;
	}
	
	// Update is called once per frame
	void Update () {
		m_TimeTillDetonation -= Time.deltaTime;

		if (m_TimeTillDetonation <= 0.0f) {
			Entity enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Entity>();

			if ( (enemy.transform.position - m_Trans.position).sqrMagnitude < m_DetonateDistance * m_DetonateDistance) {
				enemy.StunPlayer(m_StunDuration);
			}

			Destroy(this.gameObject);
		}
	}


}
