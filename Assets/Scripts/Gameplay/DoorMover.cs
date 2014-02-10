using UnityEngine;
using System.Collections;

public class DoorMover : Activatable {
	
	public Vector3 m_DoorMoveDistance;
	
	private Transform m_Trans;
	private bool m_HasActivated = false;
	
	void Start() {
		m_Trans = transform;
	}
	
	public override void Activate() {
		if (!m_HasActivated) {
			m_HasActivated = true;
			StartCoroutine("MoveDoor");
		}
	}
	
	IEnumerator MoveDoor() {
		float TotalTime = 0.75f;
		int Ticks = 10; // increase this number for a smoother transition
		Vector3 Initial = m_Trans.position;
		Vector3 Destination = m_DoorMoveDistance + m_Trans.position;
		
		for (int i = 0; i < Ticks; ++i ) {
			Vector3 NewPosition = Initial * ((Ticks - i) / (float)Ticks) + Destination * (i / (float)Ticks);
			m_Trans.Translate(NewPosition - m_Trans.position);

			yield return new WaitForSeconds(TotalTime / Ticks); 
		}
	}
}