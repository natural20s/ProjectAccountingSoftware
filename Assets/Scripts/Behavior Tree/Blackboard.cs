using UnityEngine;
using System.Collections;

/*
 * Blackboard:
 * Way to store information and share across nodes in the tree
 * 
 */ 
public class Blackboard {

	#region random stuff used for testing
	public Vector3 StartPoint;
	#endregion

	// Need to assign these manually. Perhaps make them required of a constructor?
	public Transform Trans;
	public Transform Player;
	public Looker LookAtObject;


	public Vector3 Destination;
	public float MoveSpeed = 5.0f;
	public Vector3 Beacon = Vector3.zero;
	public float TimeSincePlayerLOS = 0.0f;
	public Vector3 LastKnownPlayerPosition = Vector3.zero;

	public float LOSDistance = 80.0f;


	public Vector3[] MovementPath;
	public int PathCurrentIdx;

	public float TimeSinceLastUpdate;

	// holding state for whether we're stunned cause I don't have a clean to to remove behaviors dynamically
	public bool IsStunActive = false; 
	public float StunTimeRemaining = 0.0f;

	#region Helper Functions & Properties
	public Vector2 ToPlayer2D { 
		get { Vector3 ToPlayer = Player.position - Trans.position; 
			return new Vector2(ToPlayer.x, ToPlayer.y);
			} 
	}

	public Vector3 ToPlayer { 
		get { return Player.position - Trans.position; } 
	}

	// Calling this will get the Entity ready to call the MoveToPoint behavior with no additional work 
	public void SetDestinationAndPath(Vector3 target) {
		Destination = target;
		MovementPath = NavGraphConstructor.Instance.FindPathToLocation(Trans.position, target);
		PathCurrentIdx = 0;
	}

	// Returns the point we're currently traveling to on our nav graph path
	public Vector3 NextPointOnPath() {
		if (PathCurrentIdx >= MovementPath.Length) {
			return MovementPath[MovementPath.Length-1];
		}
		
		return MovementPath[PathCurrentIdx];
	}
	#endregion

	#region Debug variables
	public bool ShowCurPath = true;
	public bool ShowLOSCheck = true;

	public void LOSCheck(bool canSeePlayer, Vector3 closestPointToPlayer) {
		if (ShowLOSCheck) {
			Color debugColor = Color.yellow;
			if (canSeePlayer)
				debugColor = Color.green;

			Debug.DrawLine(Trans.position, closestPointToPlayer, debugColor);
		}
	}
	#endregion
}
