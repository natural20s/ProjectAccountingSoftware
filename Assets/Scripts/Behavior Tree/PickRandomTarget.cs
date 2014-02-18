using UnityEngine;
using System.Collections;

public class PickRandomTarget : Behavior {
	
	public override void OnInitialize() {
		//Debug.Log("PickRandomTarget init");
	}
	
	public override Status Update(ref Blackboard bb) {
		int xDist = Random.Range(-7, 7);
		int yDist = Random.Range(-7, 7);
		
		Vector3 newLocation = new Vector3(xDist + 10, yDist + 10, bb.Trans.position.z);
		
		if ((newLocation - bb.Trans.position).sqrMagnitude >= 5) {
			//Debug.Log("PickRandomPoint found new point");

			bb.SetDestinationAndPath(newLocation);
			return Status.BH_SUCCESS;
		}
		
		//Debug.Log("PickRandomPoint failed to find point");
		return Status.BH_RUNNING;
	}
}
