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
	//private bool m_IsChasing = true;
	
	// Use this for initialization
	void Start () {
		//TestSuite();
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
		m_Blackboard.StartPoint = transform.position;
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		if (player) {
			m_Blackboard.Player = player.transform;
		}
		m_Blackboard.Destination = transform.position + new Vector3(10, 0, 5);


		//-------------------------------------------------------------------------
		// Higher level sequence/selectors which we'll add leaf behaviors to later
		//-------------------------------------------------------------------------
		Sequence randomMove = new Sequence();
		Sequence moveToBeacon = new Sequence();

		//----------------------------------------------------------------------------------
		// Create leaf behaviors. Should only need one of each.
		// Some of these get used often (MoveToPoint), others are specific (CheckForBeacon)
		//----------------------------------------------------------------------------------
		MoveToPoint moveToPoint = new MoveToPoint(); 
		PickRandomTarget pickRandomTarget = new PickRandomTarget();
		CheckForBeacon checkForBeacon = new CheckForBeacon();
		ChasePlayer chasePlayer = new ChasePlayer();
		Stunned stunned = new Stunned();

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
		m_Root.AddChild(stunned);
		m_Root.AddChild(moveToBeacon);
		m_Root.AddChild(chasePlayer);
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

	public void StunPlayer(float stunTime) {
		if (stunTime <= 0.0f) {
			Debug.LogWarning("Passed in a stun time less than or equal to 0.0f, that's probably bad an unintentional");
		}

		if (m_Blackboard.StunTimeRemaining > 0.0f || m_Blackboard.IsStunActive) {
			Debug.LogWarning("Trying to stun enemy when it should already be stunned, not adding new state");
			return;
		}

		m_Blackboard.IsStunActive = true;
		m_Blackboard.StunTimeRemaining = stunTime;
	}

	public void TestSuite() {
//		MockBehavior t = new MockBehavior();
//		CheckEqual(0, t.m_InitializeCalled, "Initialize sanity check");
//
//		t.Tick(ref m_Blackboard);
//		CheckEqual(1, t.m_InitializeCalled, "Initialize Tick check");


		MockSequence seq = new MockSequence(2);

		CheckEqual(seq.Tick(ref m_Blackboard), Status.BH_RUNNING, "Sequence first Tick");
		CheckEqual(0, seq.AtIndex(0).m_TerminateCalled, "Sequence Terminate check");

		seq.AtIndex(0).m_ReturnStatus = Status.BH_FAILURE;
		CheckEqual(seq.Tick(ref m_Blackboard), Status.BH_FAILURE, "Seq Tick fail check");
		CheckEqual(1, seq.AtIndex(0).m_TerminateCalled, "Seq Terminate");
		CheckEqual(0, seq.AtIndex(1).m_InitializeCalled, "Seq Bh2 Init check");
	}

	private void CheckEqual(int expected, int actual, string testName) {
		string succeedOrFail = "failed";
		if (expected == actual) {
			succeedOrFail = "succeeded";
		}

		Debug.LogWarning (testName + " " + succeedOrFail + " " + expected + " <-> " + actual);
	}

	private void CheckEqual(Status expected, Status actual, string testName) {
		string succeedOrFail = "failed";
		if (expected == actual) {
			succeedOrFail = "succeeded";
		}
		
		Debug.LogWarning (testName + " " + succeedOrFail + " " + expected + " <-> " + actual);
	}
}
