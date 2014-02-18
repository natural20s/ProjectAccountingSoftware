using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Entity : MonoBehaviour {
	
	public Blackboard m_Blackboard;
	private Root m_Root = new Root();
	/*private BehaviorTree m_Bt;
	private Sequence randomMove;
	
	private Flee m_Flee;
	private Chase m_Chase;
	*/
	private bool m_IsChasing = true;
	
	// Use this for initialization
	void Start () {
		Restart();
	}
	
	void Update () {

		if (Input.GetKeyDown(KeyCode.P))
		{
			//ToggleAggro();
		}
		m_Root.Tick(ref m_Blackboard);
//		if (m_Bt.IsEmpty()) {
//			Restart();
//		}
	}
	
				
	public void Restart() {

		// Creating and setting some basic values for the blackboard
		m_Blackboard = new Blackboard();
		m_Blackboard.Trans = transform;
		m_Blackboard.Player = GameObject.FindGameObjectWithTag("Player").transform;
		m_Blackboard.Destination = transform.position + new Vector3(10, 0, 5);


		//-------------------------------------------------------------------------
		// Higher level sequence/selectors which we'll add leaf behaviors to later
		//-------------------------------------------------------------------------
		Sequence randomMove = new Sequence();
		Sequence moveToBeacon = new Sequence();

		//----------------------------------------------------------------------------------
		// Create leaf behaviors. Should only need one of each
		// Some of these get used often (MoveToPoint), others are specific (CheckForBeacon)
		//----------------------------------------------------------------------------------
		MoveToPoint moveToPoint = new MoveToPoint(); 
		PickRandomTarget pickRandomTarget = new PickRandomTarget();
		CheckForBeacon checkForBeacon = new CheckForBeacon();

		//---------------------------------------------------------------------------------------
		// Building the subtrees.
		// Add children to subtrees in left to right order, since each AddChild is doing a push_back
		//----------------------------------------------------------------------------------------
		moveToBeacon.AddChild(checkForBeacon);
		moveToBeacon.AddChild(moveToPoint);

		randomMove.AddChild(pickRandomTarget);
		randomMove.AddChild(moveToPoint);

		//--------------------------------------------------
		// Add subtrees to the root.
		// Like before, add behaviors in left to right order
		//--------------------------------------------------
		m_Root.AddChild(moveToBeacon);
		m_Root.AddChild(randomMove);


		//m_Blackboard.MovementPath = NavGraphConstructor.Instance.FindPathToLocation(m_Blackboard.Trans.position, m_Blackboard.Destination);
		//m_Blackboard.PathCurrentIdx = 0;

//		repeat.m_Child = randomMove;
//
//		// Try out Chase behavior
//		m_Chase = new Chase(moveBehavior, m_Bt);
//		m_Flee = new Flee(moveBehavior, m_Bt);
//		
//		List<Behavior> tree = new List<Behavior>();
//		tree.Add(repeat);
//		tree.Add(m_Chase);
//
//		root.m_Children = tree;
//
//		m_Bt.Start(root, this.SequenceComplete);
	}
	
//	public void ToggleAggro() {
//		m_Bt.Reset();
//		
//		if (m_IsChasing) {
//			m_Bt.Start(m_Flee, this.SequenceComplete);
//			m_IsChasing = false;
//		}
//		else {
//			m_Bt.Start(m_Chase, this.SequenceComplete);
//			m_IsChasing = true;
//		}
//	}

	// Passing a value of Vector3.zero will tell the Enemy that no beacon exists
	public void BeaconPositionChange(Vector3 beaconLocation) {
		m_Blackboard.Beacon = beaconLocation;
	}
}
