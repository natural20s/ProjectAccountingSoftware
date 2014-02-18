using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Composite :  Behavior {
	
	public List<Behavior> m_Children = new List<Behavior>();
	
	private int m_CurrentIndex;

	public void AddChild(Behavior child) { m_Children.Add(child); }

	public void RemoveChild(Behavior child) {}
	public void CleaerChildren() {}

	public override void OnInitialize(ref Blackboard bb) {
		//Debug.Log("Composite::OnInitialize");
		m_CurrentChild = 0;
	}

	public override void OnTerminate(Status status) {
		//Debug.Log("Sequence Terminate");
		if (status != Status.BH_SUCCESS) {
			Reset();
		}
	}
	
	public override void Reset() {
		base.Reset();
		m_CurrentChild = 0;
		
		for (int idx = 0; idx < m_Children.Count; ++idx) {
			m_Children[idx].Reset();
		}
	}

	protected int m_CurrentChild;
}

public class Sequence : Composite {
	
	public Sequence() {
	}	
		
	public override Status Update(ref Blackboard bb) {
		for(;;) {
			Status retStatus = m_Children[m_CurrentChild].Tick(ref bb);

			if (retStatus != Status.BH_SUCCESS) {
				return retStatus;
			}

			// Hit the end of the array, we're done!
			if (++m_CurrentChild == m_Children.Count) {
				return Status.BH_SUCCESS;
			}
		}

	}
};

public class Root : Composite {

	public Root() {}

	public override Status Update(ref Blackboard bb) {
		m_CurrentChild = 0; // each tick we want to restart
		for(;;) {
			Status retStatus = m_Children[m_CurrentChild].Tick(ref bb);
			
			if (retStatus == Status.BH_RUNNING) {
				return retStatus;
			}
			
			// We're the root and we've completed all of our children, let's reset
			if (++m_CurrentChild == m_Children.Count) {
				//Debug.Log("Root - reset everything");
				Reset(); // this will reset every node in the tree
				return Status.BH_SUCCESS;
			}
		}
	}
}

public abstract class Decorator : Behavior {
	public Behavior m_Child;
	
	public Decorator (Behavior child) {
		m_Child = child;
	}
	
	public Decorator() {}
}


//public class Repeat : Decorator {
//	
//	// A count of < 0 will signify to repeat until failure
//	public Repeat(ref BehaviorTree bt, Behavior child, int count) : base(child) {
//		m_Bt = bt;
//		m_Child = child;
//		m_TimesToRepeat = count;
//	}
//	
//	public Repeat(ref BehaviorTree bt, int count) {
//		m_Bt = bt;
//		m_TimesToRepeat = count;
//	}
//	
//	public override void OnInitialize() {
//		if (m_Child == null) {
//			Debug.LogError("Trying to init Repeat when m_Child isn't set");
//		}
//		m_RepeatCount = 0;
//		BehaviorObserver observer = OnChildComplete;
//		m_Bt.Start(m_Child, observer);
//	}
//	
//	public void OnChildComplete(Status status) {
//		
//		if (++m_RepeatCount < m_TimesToRepeat || m_TimesToRepeat < 0) {
//			// reinit the child
//			// do this instead of "m_Bt.Start(m_Child), since the child still exists in the dequeue.
//			// Alternatively, we could Bt.Stop then Bt.Start. Perhaps if Terminate functions need to get called,
//			// that would be the proper way to handle it
//			m_Child.m_Status = Status.BH_INVALID;
//		}
//		else if (m_RepeatCount >= m_TimesToRepeat) {
//			m_Bt.Stop(m_Child, Status.BH_SUCCESS);
//		}
//	}
//	public override Status Update(ref Blackboard bb) {
//		return Status.BH_RUNNING;
//	}
//	
//	private int m_TimesToRepeat; // set in constructor: number of times we're supposed to repeat the child. -1 for infinite
//	private int m_RepeatCount; // number of times we've completed and restarted the child
//	private BehaviorTree m_Bt;
//}

//public class MockBehavior : Behavior {
//	public int m_InitializeCalled = 0;
//	public int m_TerminateCalled = 0;
//	public int m_UpdateCalled = 0;
//	public Status m_ReturnStatus = Status.BH_RUNNING;
//	public Status m_TerminateStatus = Status.BH_INVALID;
//	
//	public MockBehavior() {}
//	
//	public override void OnInitialize() {
//		++m_InitializeCalled;
//	}
//	
//	public override void OnTerminate(Status e) {
//		++m_TerminateCalled;
//		m_TerminateStatus = e;
//	}
//	
//	public override Status Update(ref Blackboard bb) {
//		++m_UpdateCalled;
//		return m_ReturnStatus;
//	}
//};
//
//
//public class MockComposite<T> : Composite {
//	
//	public MockComposite(BehaviorTree bt, int size) {
//		for (int i = 0; i < size; ++i) {
//			m_Children.Add(new MockBehavior());
//		}
//	}
//	
//	// can't overload [] in C#, just make an accessor I guess
//	public MockBehavior Get(int index)
//	{
//		if (index >= m_Children.Count) {
//			Debug.LogError("Accessing invalid index on MockBehavior! " + index + " out of " + m_Children.Count);
//		}
//		
//		return (MockBehavior)m_Children[index];
//	}
//	
//	public override Status Update(ref Blackboard bb) {
//		return Status.BH_RUNNING;
//	}
//}