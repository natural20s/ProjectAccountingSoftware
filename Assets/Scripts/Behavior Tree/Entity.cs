using UnityEngine;
using System.Collections;

public class Entity : MonoBehaviour {
	
	public Blackboard m_Blackboard;
	private BehaviorTree m_Bt;
	private Sequence randomMove;
	
	private Flee m_Flee;
	private Chase m_Chase;
	
	private bool m_IsChasing = true;
	
	// Use this for initialization
	void Start () {
		Restart();
	}
	
	void Update () {
		m_Bt.Tick();

		if (Input.GetKeyDown(KeyCode.P))
		{
			ToggleAggro();
		}
		
		if (m_Bt.IsEmpty()) {
			Restart();
		}
	}
	
				
	public void Restart() {
		// Init BB
		
		m_Blackboard = new Blackboard();
		m_Blackboard.Trans = transform;
		m_Blackboard.Player = GameObject.FindGameObjectWithTag("Player").transform;
		m_Blackboard.Destination = transform.position + new Vector3(10, 0, 5);
		
		m_Bt = new BehaviorTree();
		m_Bt.m_Blackboard = m_Blackboard;
		
		// Init tree
		Repeat repeat = new Repeat(ref m_Bt, -1);
		Sequence randomMove = new Sequence(ref m_Bt);
		PickRandomTarget pickTarget = new PickRandomTarget();
		MoveToPoint moveBehavior = new MoveToPoint();
		
		
		randomMove.m_Children.Add(moveBehavior);
		randomMove.m_Children.Add(pickTarget);
		
		// Try out Chase behavior
		m_Chase = new Chase(moveBehavior, m_Bt);
		m_Flee = new Flee(moveBehavior, m_Bt);
		
		repeat.m_Child = randomMove;
		
		m_Bt.Start(repeat, this.SequenceComplete);
	}
	
	public void SequenceComplete(Status status) {
		PickRandomTarget pickTarget = new PickRandomTarget();
		MoveToPoint moveBehavior = new MoveToPoint();
		
		
		randomMove.m_Children.Add(moveBehavior);
		randomMove.m_Children.Add(pickTarget);
	}
	
	public void ToggleAggro() {
		m_Bt.Reset();
		
		if (m_IsChasing) {
			m_Bt.Start(m_Flee, this.SequenceComplete);
			m_IsChasing = false;
		}
		else {
			m_Bt.Start(m_Chase, this.SequenceComplete);
			m_IsChasing = true;
		}
	}
		
}
