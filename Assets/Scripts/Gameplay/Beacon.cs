using UnityEngine;
using System.Collections;

public class Beacon : MonoBehaviour {

	public BeaconTool m_BeaconTool;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D invader) {
		if (invader.tag == "Enemy") {
			Debug.Log ("Enemy is about to destroy beacon");
			m_BeaconTool.DestroyBeacon();
		}
	}
}
