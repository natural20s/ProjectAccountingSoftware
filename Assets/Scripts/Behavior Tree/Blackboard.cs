using UnityEngine;
using System.Collections;

/*
 * Blackboard:
 * Way to store information and share across nodes in the tree
 * 
 */ 
public class Blackboard {

	public Vector3 Destination;
	public float MoveSpeed = 2.0f;
	public Vector3 Beacon = Vector3.zero;
	public float TimeSincePlayerLOS = 0.0f;

	public Transform Trans;
	public Transform Player;

	public Vector3[] MovementPath;
	public int PathCurrentIdx;

	public float TimeSinceLastUpdate;

	#region Helper Functions & Properties
	public Vector2 ToPlayer2D { get { 
			Vector3 ToPlayer = Player.position - Trans.position; 
			return new Vector2(ToPlayer.x, ToPlayer.y);
		} }

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
}
