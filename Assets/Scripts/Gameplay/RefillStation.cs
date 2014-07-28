using UnityEngine;
using System.Collections;

public class RefillStation : MonoBehaviour {
	public ToolType m_RefillType;
	public int m_RefillQuantity = 2;
	public int m_NumRefillsAvailable = -1; // -1 signifies unlimited refills

	private Transform m_Player;
	private Transform m_Transform;

	private float m_ProximitySqr = 25.0f;

	// Use this for initialization
	void Start () {
		m_Player = GameObject.FindGameObjectWithTag("Player").transform;
		m_Transform = transform;
	}
	
	// Update is called once per frame
	void Update () {
		if ((m_Player.position - m_Transform.position).sqrMagnitude <= m_ProximitySqr) 
		{
			if (Input.GetKeyDown(KeyCode.R) && m_NumRefillsAvailable != 0)
			{
				m_Player.GetComponent<PlayerInput>().Refill(m_RefillType, m_RefillQuantity);

				if (m_NumRefillsAvailable > 0)
					m_NumRefillsAvailable--;
			}
		}

	}

	void OnDrawGizmos()
	{
		float proximity = Mathf.Sqrt (m_ProximitySqr);
		Gizmos.DrawCube(transform.position, new Vector3(proximity, proximity, 1));
	}
}
		