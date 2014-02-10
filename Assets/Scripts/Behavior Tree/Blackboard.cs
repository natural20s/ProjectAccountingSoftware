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
	public Transform Trans;
	public Transform Player;
}
