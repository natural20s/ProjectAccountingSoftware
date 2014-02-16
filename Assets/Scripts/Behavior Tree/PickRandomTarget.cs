using UnityEngine;
using System.Collections;

public class PickRandomTarget : Behavior {
	
	public override void OnInitialize() {
		Debug.Log("PickRandomTarget init");
	}
	
	public override Status Update(ref Blackboard bb) {
		int xDist = Random.Range(-10, 10);
		int zDist = Random.Range(-10, 10);
		
		Vector3 newLocation = new Vector3(xDist, bb.Trans.position.y, zDist);
		
		if ((newLocation - bb.Trans.position).sqrMagnitude >= 8) {
			Debug.Log("PickRandomPoint found new point");

			bb.Destination = newLocation;
			bb.MovementPath = NavGraphConstructor.Instance.FindPathToLocation(bb.Trans.position, newLocation);
			bb.PathCurrentIdx = 0;
			return Status.BH_SUCCESS;
		}
		
		Debug.Log("PickRandomPoint failed to find point");
		return Status.BH_RUNNING;
	}
}
