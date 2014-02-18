using UnityEngine;
using System.Collections;

public class MoveToPoint : Behavior {

	private float m_DistanceThreshold = 1.5f;

	public override void OnInitialize() {
		//Debug.Log("MoveToPoint Init");
	}

	public override void OnTerminate(Status status) {
		//Debug.Log("MoveToPoint Terminate");
	}

	public override Status Update(ref Blackboard bb) {
		if (bb.Destination == Vector3.zero) {
			// We don't have a destination, return failure
			Debug.Log("MoveToPoint failed");
			return Status.BH_FAILURE;
		}

		Debug.DrawLine(bb.Trans.position, bb.Destination, Color.red);
		
		Vector3 toDestination = bb.Destination - bb.Trans.position;

		if (toDestination.sqrMagnitude <= m_DistanceThreshold * m_DistanceThreshold) {
			// If we've reached our destination, we're done
			return Status.BH_SUCCESS;
		}
		else if (bb.PathCurrentIdx >= bb.MovementPath.Length) {
			// if our path index is out of range of our path nodes, return failure
			return Status.BH_SUCCESS;
		}
		//else
		// Move normally
		Debug.DrawLine(bb.Trans.position, bb.MovementPath[bb.PathCurrentIdx], Color.green);
		Vector3 toNextPoint = bb.MovementPath[bb.PathCurrentIdx] - bb.Trans.position;

		if (toNextPoint.sqrMagnitude < 3) { ++bb.PathCurrentIdx; }

		toNextPoint.z = 0;
		toNextPoint.Normalize();
		bb.Trans.Translate(toNextPoint * bb.MoveSpeed * Time.deltaTime);

		return Status.BH_RUNNING;
	}
};

// Simply boolean-esque check for whether there is a beacon in the world that should travel to
public class CheckForBeacon : Behavior {

	public CheckForBeacon() {

	}

	public override void OnInitialize() {
		//Debug.Log("CheckForBeacon Initialize");
	}

	public override Status Update(ref Blackboard bb) {
		if (bb.Beacon == Vector3.zero) {
			// no beacon exists for us to travel to, return failure
			//Debug.Log("Failed to find beacon");
			return Status.BH_FAILURE;
		}

		// else a beacon exists, we are attracted to beacons, let's go to it!
		if (bb.Beacon != bb.Destination) {
			bb.SetDestinationAndPath(bb.Beacon);
		}

		return Status.BH_SUCCESS;
	}
}

// Chase is basically going to be just calling MoveToPoint, except we'll update our destination
//public class Chase : Decorator {
//	
//	private BehaviorTree m_Bt;
//	
//	public Chase (Behavior child, BehaviorTree bt) : base(child) {
//		m_Bt = bt;
//	}
//	
//	public override void OnInitialize() {
//		Debug.Log("Chase::Init");
//		BehaviorObserver observer = OnChildComplete;
//		m_Bt.Start(m_Child, observer);
//	}
//	
//	public override void OnTerminate(Status status) {
//		Debug.Log("Chase::Terminate");
//	}
//	
//	public override Status Update(ref Blackboard bb) {
//
//		RaycastHit2D hitInfo = Physics2D.Raycast(bb.Trans.position, bb.ToPlayer2D.normalized, 150);
//
//		// logic for determining what to chase (beacon, player, etc.)
//
//
//
//		if (hitInfo != null && hitInfo.transform.tag == "Player") {
//			Debug.Log("AI can see player");
//			bb.MovementPath = NavGraphConstructor.Instance.FindPathToLocation(bb.Trans.position, bb.Player.position);
//			bb.Destination = bb.Player.position;
//		}
//
//
//		return Status.BH_RUNNING;
//	}
//	
//	public void OnChildComplete (Status status) {
//		m_Bt.Stop(m_Child, status);
//	}
//};
//
//public class Flee : Decorator {
//	private BehaviorTree m_Bt;
//	
//	public Flee (Behavior child, BehaviorTree bt) : base(child) {
//		m_Bt = bt;
//	}
//	
//	public override void OnInitialize() {
//		m_Bt.Start(m_Child, OnChildComplete);
//	}
//	
//	public void OnChildComplete (Status status) {
//		Debug.LogError("Flee::OnChildComplete this shouldn't get called...");
//	}
//	
//	public override Status Update(ref Blackboard bb) {
//		Vector3 toMe = bb.Trans.position - bb.Player.position;
//		toMe.z = 0;
//		
//		if (toMe.sqrMagnitude >= 70.0f)
//		{
//			// don't actually return, just stop running
//			return Status.BH_RUNNING;
//			//return Status.BH_SUCCESS;	
//		}
//		
//		toMe.Normalize();
//		toMe *= 2.5f;
//		bb.Destination = toMe + bb.Trans.position;
//		
//		return Status.BH_RUNNING;
//	}
//};