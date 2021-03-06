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

		//Debug.DrawLine(bb.Trans.position, bb.Destination, Color.red);

		Vector3 toDestination = bb.Destination - bb.Trans.position;

		if (toDestination.sqrMagnitude <= m_DistanceThreshold * m_DistanceThreshold || 
		    	bb.PathCurrentIdx >= bb.MovementPath.Length) {
			// If we've reached our destination, we're done
			Debug.Log ("MoveToPoint finish");
			return Status.BH_SUCCESS;
		}

		if (bb.LookAtObject)
			bb.LookAtObject.m_Target.SetTarget(bb.MovementPath[bb.PathCurrentIdx]);

		// Otherwise move normally
		Vector3 toNextPoint = bb.MovementPath[bb.PathCurrentIdx] - bb.Trans.position;
		toNextPoint.z = 0;

		if (toNextPoint.sqrMagnitude < 3) { 
			++bb.PathCurrentIdx; 
		}

		toNextPoint.z = 0;
		toNextPoint.Normalize();
		bb.Trans.Translate(toNextPoint * bb.MoveSpeed * Time.deltaTime, Space.World);

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
	private const float m_AngleOfLOS = 160; // in degrees

	public class LOSInfo {
		public Vector3 HitPoint;
		public bool CanSeePlayer;

		public LOSInfo() {
			HitPoint = Vector3.zero;
			CanSeePlayer = false;
		}
	}

	public ChasePlayer() {}

	public override void OnInitialize(ref Blackboard bb) {
		//Debug.Log ("ChasePlayer Initialize");
		bb.LastKnownPlayerPosition = Vector3.zero;
	}

	// Our exit conditions are
	// a) we reach the player
	// b) we reach the  players last known position and we can't find the player after x seconds (second half may be a new behavior)
	public override Status Update(ref Blackboard bb) {
		LOSInfo losResults = new LOSInfo();

		if ((bb.LastKnownPlayerPosition - bb.Player.position).sqrMagnitude >= m_RefindPlayerDist) {
			losResults = CanSeePlayer(ref bb);

			if (losResults.CanSeePlayer) {
				bb.LastKnownPlayerPosition = bb.Player.position;
				bb.SetDestinationAndPath(bb.Player.position);
			}

		} 
		else if (bb.ShowLOSCheck) {
			losResults = CanSeePlayer(ref bb);
		}

		bb.LOSCheck(losResults.CanSeePlayer, losResults.HitPoint); 

		if (bb.LastKnownPlayerPosition == Vector3.zero) {
			// We don't have a destination, return failure
			//Debug.Log("ChasePlayer failed");
			return Status.BH_FAILURE;
		}
		
		Vector3 toDestination = bb.Destination - bb.Trans.position;
		if (toDestination.sqrMagnitude <= m_DistanceThreshold * m_DistanceThreshold || 
		    bb.PathCurrentIdx >= bb.MovementPath.Length) {
			// If we've reached our destination, we're done
			Debug.Log ("ChasePlayer finish");
			return Status.BH_SUCCESS;
		}

		// Move normally
		//Debug.DrawLine(bb.Trans.position, bb.MovementPath[bb.PathCurrentIdx], Color.green);
		Vector3 toNextPoint = bb.MovementPath[bb.PathCurrentIdx] - bb.Trans.position;
		toNextPoint.z = 0;
		
		if (toNextPoint.sqrMagnitude < 3) { ++bb.PathCurrentIdx; }
		
		toNextPoint.z = 0;
		toNextPoint.Normalize();
		bb.Trans.Translate(toNextPoint * bb.MoveSpeed * Time.deltaTime);
		
		m_Status = Status.BH_RUNNING;
		return Status.BH_RUNNING;
	}

	protected LOSInfo CanSeePlayer(ref Blackboard bb) {
		LOSInfo results = new LOSInfo();
		results.CanSeePlayer = false;

		RaycastHit2D hitInfo = Physics2D.Raycast(bb.Trans.position, bb.ToPlayer2D.normalized, bb.LOSDistance);
		if (hitInfo) {
			results.HitPoint = hitInfo.point;

			// dividing in half so I can add/subtract the half angle to find my field of view vectors
			float angleRads = (m_AngleOfLOS / 2.0f) * Mathf.PI / 180.0f;

			float unitCircleX = bb.Trans.up.x;
			float unitCircleY = bb.Trans.up.y;

			float leftUnitCircleX = unitCircleX * Mathf.Cos(-angleRads) - unitCircleY * Mathf.Sin(-angleRads);
			float leftUnitCircleY = unitCircleX * Mathf.Sin(-angleRads) + unitCircleY * Mathf.Cos(-angleRads);
			Vector3 leftVectorFOV = new Vector3(leftUnitCircleX, leftUnitCircleY, 0);
			leftVectorFOV = leftVectorFOV * bb.LOSDistance + bb.Trans.position;

			float rightUnitCircleX = unitCircleX * Mathf.Cos(angleRads) - unitCircleY * Mathf.Sin(angleRads);
			float rightUnitCircleY = unitCircleX * Mathf.Sin(angleRads) + unitCircleY * Mathf.Cos(angleRads);
			Vector3 rightVectorFOV = new Vector3(rightUnitCircleX, rightUnitCircleY, 0);
			rightVectorFOV = rightVectorFOV * bb.LOSDistance + bb.Trans.position;

			Debug.DrawLine(bb.Trans.position, rightVectorFOV, Color.white);
			Debug.DrawLine(bb.Trans.position, leftVectorFOV, Color.white);

			Debug.DrawLine (bb.Trans.position, bb.Trans.up*4 + bb.Trans.position, Color.red);

			if (hitInfo.transform.tag == "Player") {
				// we can see the player, are we within the field of view?
				//float dotResult = Vector3.Dot(bb.Trans.forward, hitInfo.point);
				results.CanSeePlayer = true;
			} 
		} 
		else {
			results.HitPoint = bb.Trans.position + (bb.ToPlayer.normalized * bb.LOSDistance);
		}

		return results;
	}
}

public class Stunned : Behavior {


	public Stunned() {}

	public override void OnInitialize(ref Blackboard bb) {
		if (bb.IsStunActive && bb.StunTimeRemaining <= 0f) {
			Debug.LogError("StunTimeRemaining less than 0.f when initializing Stunned behavior");
		}
	}

	public override Status Update(ref Blackboard bb) {
		if (bb.IsStunActive == false) {
			return Status.BH_FAILURE;
		}

		bb.StunTimeRemaining -= Time.deltaTime;

		if (bb.StunTimeRemaining <= 0.0f) {
			bb.IsStunActive = false; // Not sure where else I could set this...man I feel dirty for using state like this
			return Status.BH_SUCCESS;
		}

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