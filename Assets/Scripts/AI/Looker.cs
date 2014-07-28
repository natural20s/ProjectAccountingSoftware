using UnityEngine;
using System.Collections;

public class Looker : MonoBehaviour {

	public Target m_Target = new Target();
	private Transform m_Trans;

	private float m_MaxRotationPerTick = 7.0f; // in degrees

	// Use this for initialization
	void Start () {
		m_Trans = transform;
	}
	
	// Update is called once per frame
	void LateUpdate () {
	
		// Not currently smoothing the looking
		// In Unity2D, Up acts like forward, so LookAt doesn't work. We're going to use Up instead of Forward for rotating our object
		if (m_Target.GetPosition() != Vector3.zero) {
			Vector3 targetUnitVector = (m_Target.GetPosition() - Position2D()).normalized;
			float dotResult = Vector3.Dot(m_Trans.up, targetUnitVector);

			if (dotResult <= 0.99f) {
				dotResult = Mathf.Acos(dotResult) * 180.0f / Mathf.PI; // convert to degrees
				//Debug.Log ("Dot difference: " + dotResult + " between " + m_Trans.up + " and " + targetUnitVector);

				// Do another dot to determine whether we multiple by -1 or not
				float secondDotResult = Vector3.Dot (m_Trans.right, targetUnitVector);
				if (secondDotResult >= 0f)
					dotResult *= -1;

				dotResult = Mathf.Clamp(dotResult, -m_MaxRotationPerTick, m_MaxRotationPerTick);

				m_Trans.Rotate (new Vector3(0, 0, dotResult));
			}
		}
	}

	private Vector3 Position2D() {
		return new Vector3 (m_Trans.position.x, m_Trans.position.y, 0);
	}
}

public class Target {

	private Transform m_ObjTrans = null;
	private Vector3? m_InterestPoint = null;

	private Vector3 m_Offset = Vector3.zero;

	public Target() {
	}

	public Vector3 GetPosition() {
		if (m_ObjTrans) 
			return m_ObjTrans.position + m_Offset;

		if (m_InterestPoint != null)
			return (Vector3)m_InterestPoint + m_Offset;

		Debug.LogWarning("Caution: Called GetPosition on Target without setting Target: " + m_ObjTrans + " " + m_InterestPoint + " " + m_Offset);
		return Vector3.zero;
	}

	public void SetTarget(Transform desiredTransform, Vector3 offset = default(Vector3)) {
		m_ObjTrans = desiredTransform;
		m_InterestPoint = null;
		m_Offset = offset;
	}

	public void SetTarget(Vector3 point, Vector3 offset = default(Vector3)) {
		m_ObjTrans = null;
		m_InterestPoint = new Vector3(point.x, point.y, 0);
		m_Offset = offset;
	}
}