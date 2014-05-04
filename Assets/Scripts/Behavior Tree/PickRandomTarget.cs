using UnityEngine;
using System.Collections;

public class PickRandomTarget : Behavior {
	
	public override void OnInitialize(ref Blackboard bb) {
		//Debug.Log("PickRandomTarget init");
	}
	
	public override Status Update(ref Blackboard bb) {
		int xDist = Random.Range(-12, 12);
		int yDist = Random.Range(-12, 12);

		Vector3 newLocation = new Vector3(xDist + bb.StartPoint.x, yDist + bb.StartPoint.y, bb.Trans.position.z);
		
		if ((newLocation - bb.Trans.position).sqrMagnitude >= 5) {
			//Debug.Log("PickRandomPoint found new point");

			bb.SetDestinationAndPath(newLocation);
			return Status.BH_SUCCESS;
		}
		
		//Debug.Log("PickRandomPoint failed to find point");
		return Status.BH_RUNNING;
	}
}
