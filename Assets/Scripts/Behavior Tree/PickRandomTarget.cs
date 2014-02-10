using UnityEngine;
using System.Collections;

public class PickRandomTarget : Behavior {
	
	public override void OnInitialize() {
		Debug.Log("PickRandomTarget init");
	}
	
	public override Status Update(ref Blackboard bb) {
		int xDist = Random.Range(-15, 15);
		int zDist = Random.Range(-15, 15);
		
		Vector3 newLocation = new Vector3(xDist, bb.Trans.position.y, zDist);
		
		if ((newLocation - bb.Trans.position).sqrMagnitude >= 8) {
			bb.Destination = newLocation;
			Debug.Log("PickRandomPoint found new point");
			return Status.BH_SUCCESS;
		}
		
		Debug.Log("PickRandomPoint failed to find point");
		return Status.BH_RUNNING;
	}
}
