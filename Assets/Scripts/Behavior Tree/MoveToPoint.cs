using UnityEngine;
using System.Collections;

public class MoveToPoint : Behavior {

	private const float m_DistanceThreshold = 1.5f;

	public override void OnInitialize(ref Blackboard bb) {
		//Debug.Log("MoveToPoint Init " + bb.Destination);
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

		if (toDestination.sqrMagnitude <= m_DistanceThreshold * m_DistanceThreshold || 
		    	bb.PathCurrentIdx >= bb.MovementPath.Length) {
			// If we've reached our destination, we're done
			Debug.Log ("MoveToPoint finish");
			return Status.BH_SUCCESS;
		}

		//else
		// Move normally
		Debug.DrawLine(bb.Trans.position, bb.MovementPath[bb.PathCurrentIdx], Color.green);
		Vector3 toNextPoint = bb.MovementPath[bb.PathCurrentIdx] - bb.Trans.position;
		toNextPoint.z = 0;

		if (toNextPoint.sqrMagnitude < 3) { ++bb.PathCurrentIdx; }

		toNextPoint.z = 0;
		toNextPoint.Normalize();
		bb.Trans.Translate(toNextPoint * bb.MoveSpeed * Time.deltaTime);

		m_Status = Status.BH_RUNNING;
		return Status.BH_RUNNING;
	}
};

// Simply boolean-esque check for whether there is a beacon in the world that should travel to
public class CheckForBeacon : Behavior {

	public CheckForBeacon() {

	}

	public override void OnInitialize(ref Blackboard bb) {
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

public class CanSeePlayer : Behavior {

	private const float m_TimeBeforeBailingOut = 1.0f;

	public CanSeePlayer() {}

	public override void OnInitialize(ref Blackboard bb) {
		Debug.Log ("CanSeePlayer Initialize");
		bb.TimeSincePlayerLOS = m_TimeBeforeBailingOut;
	}

	public override Status Update(ref Blackboard bb) {
		RaycastHit2D hitInfo = Physics2D.Raycast(bb.Trans.position, bb.ToPlayer2D.normalized, 150);

		if (hitInfo && hitInfo.transform.tag == "Player") {
			Debug.Log("AI can see player");
			bb.TimeSincePlayerLOS = 0.0f;
			bb.SetDestinationAndPath(bb.Player.position);

			return Status.BH_SUCCESS; // is returning Success here mean we're goin to OnIntialize next frame?
		}
		else if (bb.TimeSincePlayerLOS >= m_TimeBeforeBailingOut) {
			// We don't currently have sight of the player, and we don't have a history
			// of viewing the player, so we fail
			return Status.BH_FAILURE;
		}
		// else
		// We must have some history of viewing the player (aka TimeSincePlayerLOS < m_TimmeBeforeBailingOut)
		// so we'll increment the timer
		bb.TimeSincePlayerLOS += Time.deltaTime;

		return Status.BH_RUNNING;
	}
}

public class ChasePlayer : Behavior {
	// we wait until the player has moved this far from her last known position (sqrMagnitude distance) before starting raycasts again
	private const float m_RefindPlayerDist = 5.0f; 
	private const float m_DistanceThreshold = 1.5f; // sqr distance we need to be to our endpoint

	public ChasePlayer() {}

	public override void OnInitialize(ref Blackboard bb) {
		//Debug.Log ("ChasePlayer Initialize");
		bb.LastKnownPlayerPosition = Vector3.zero;
	}

	// Our exit conditions are
	// a) we reach the player
	// b) we reach the  players last known position and we can't find the player after x seconds (second half may be a new behavior)
	public override Status Update(ref Blackboard bb) {

		if ((bb.LastKnownPlayerPosition - bb.Player.position).sqrMagnitude >= m_RefindPlayerDist) {
			RaycastHit2D hitInfo = Physics2D.Raycast(bb.Trans.position, bb.ToPlayer2D.normalized, 150);
			
			if (hitInfo && hitInfo.transform.tag == "Player") {
				Debug.Log("AI can see player");
				bb.LastKnownPlayerPosition = bb.Player.position;
				bb.SetDestinationAndPath(bb.Player.position);
			}
		}

		if (bb.LastKnownPlayerPosition == Vector3.zero) {
			// We don't have a destination, return failure
			//Debug.Log("ChasePlayer failed");
			return Status.BH_FAILURE;
		}
			
		Debug.DrawLine(bb.Trans.position, bb.Destination, Color.red);
		
		Vector3 toDestination = bb.Destination - bb.Trans.position;
		
		if (toDestination.sqrMagnitude <= m_DistanceThreshold * m_DistanceThreshold || 
		    bb.PathCurrentIdx >= bb.MovementPath.Length) {
			// If we've reached our destination, we're done
			Debug.Log ("ChasePlayer finish");
			return Status.BH_SUCCESS;
		}
		
		//else
		// Move normally
		Debug.DrawLine(bb.Trans.position, bb.MovementPath[bb.PathCurrentIdx], Color.green);
		Vector3 toNextPoint = bb.MovementPath[bb.PathCurrentIdx] - bb.Trans.position;
		toNextPoint.z = 0;
		
		if (toNextPoint.sqrMagnitude < 3) { ++bb.PathCurrentIdx; }
		
		toNextPoint.z = 0;
		toNextPoint.Normalize();
		bb.Trans.Translate(toNextPoint * bb.MoveSpeed * Time.deltaTime);
		
		m_Status = Status.BH_RUNNING;
		return Status.BH_RUNNING;
	}
}

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