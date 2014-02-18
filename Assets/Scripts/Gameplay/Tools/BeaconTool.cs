using UnityEngine;
using System.Collections;

public class BeaconTool : PlayerTool {

	public GameObject m_BeaconPrefab;

	private GameObject m_BeaconObject;
	private Entity m_Enemy;
	private Transform m_Trans;

	public Entity Enemy { get { return m_Enemy; } set { m_Enemy = value; } }

	// Use this for initialization
	void Start () {
		m_Trans = transform;
		m_ToolType = ToolType.Beacon;
	}

	void Update() {
		// if beacon is active
		// probably destroy it after x seconds
	}

	public override bool TryUsingTool() {
		// right now, no restrictions on beacon usage
		if (m_BeaconObject != null) {
			DestroyBeacon();
		}

		// Deploy beacon
		m_BeaconObject = (GameObject)GameObject.Instantiate(m_BeaconPrefab, m_Trans.position + new Vector3(1, 0, 0), Quaternion.identity);
		m_BeaconObject.GetComponent<Beacon>().m_BeaconTool = this;
		m_Enemy.BeaconPositionChange(m_BeaconObject.transform.position);

		return true;
	}

	public void DestroyBeacon() {
		Debug.Log ("DestroyBeacon");
		Destroy(m_BeaconObject);
		m_Enemy.BeaconPositionChange(Vector3.zero);
	}
}
