using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Composite :  Behavior {
	
	public List<Behavior> m_Children = new List<Behavior>();
	
	private int m_CurrentIndex;
	
	protected void ResetChildPointer() {
		m_CurrentIndex = m_Children.Count - 1;
	}
	
	protected Behavior NextChild() {
		if (m_CurrentIndex < 0) {
			return null;
		}
		
		m_CurrentIndex--; // decrement our index so after we leave this function it points to the current child
		return m_Children[m_CurrentIndex + 1]; // add 1 to get the correct child
	}
	
}

public class Sequence : Composite {
	
	public Sequence(ref BehaviorTree bt) {
		m_BehaviorTree = bt;
	}
	
	public override void OnInitialize() {
		ResetChildPointer();
		m_Current = NextChild();
		BehaviorObserver observer = this.OnChildComplete;
		m_BehaviorTree.Start(m_Current, observer); //TODO do these need to be refs?
	}
		
	public void OnChildComplete(Status status) {
		Debug.Log("Sequence OnChildComplete");
		Behavior child = m_Current;
		
		if (child.m_Status == Status.BH_FAILURE) {
			m_BehaviorTree.Stop(this, Status.BH_FAILURE);
		}
		
		if (child.m_Status != Status.BH_SUCCESS) {
			Debug.LogError("Sequence - child status should be SUCCESS, but isn't!");
		}
		
		m_Current = NextChild();
		if (m_Current == null) {
			m_BehaviorTree.Stop(this, Status.BH_SUCCESS);
			
			//m_Status = Status.BH_INVALID;
		}
		else {
			BehaviorObserver observer = this.OnChildComplete;
			m_BehaviorTree.Start(m_Current, observer);
		}
	}
		
	public override Status Update(ref Blackboard bb) {
		return Status.BH_RUNNING;
	}
	
	public override void OnTerminate(Status status) {
		Debug.Log("Sequence Terminate");
	}
	
	protected BehaviorTree m_BehaviorTree;
	protected Behavior m_Current; // is this right? Are we starting from the correct end?
};

public class Decorator : Behavior {
	public Behavior m_Child;
	
	public Decorator (Behavior child) {
		m_Child = child;
	}
	
	public Decorator() {}
	
	public override Status Update(ref Blackboard bb) {
		return Status.BH_RUNNING;
	}
}

public class Repeat : Decorator {
	
	// A count of < 0 will signify to repeat until failure
	public Repeat(ref BehaviorTree bt, Behavior child, int count) : base(child) {
		m_Bt = bt;
		m_Child = child;
		m_TimesToRepeat = count;
	}
	
	public Repeat(ref BehaviorTree bt, int count) {
		m_Bt = bt;
		m_TimesToRepeat = count;
	}
	
	public override void OnInitialize() {
		if (m_Child == null) {
			Debug.LogError("Trying to init Repeat when m_Child isn't set");
		}
		m_RepeatCount = 0;
		BehaviorObserver observer = OnChildComplete;
		m_Bt.Start(m_Child, observer);
	}
	
	public void OnChildComplete(Status status) {
		
		if (++m_RepeatCount < m_TimesToRepeat || m_TimesToRepeat < 0) {
			// reinit the child
			// do this instead of "m_Bt.Start(m_Child), since the child still exists in the dequeue.
			// Alternatively, we could Bt.Stop then Bt.Start. Perhaps if Terminate functions need to get called,
			// that would be the proper way to handle it
			m_Child.m_Status = Status.BH_INVALID;
		}
		else if (m_RepeatCount >= m_TimesToRepeat) {
			m_Bt.Stop(m_Child, Status.BH_SUCCESS);
		}
	}
	public override Status Update(ref Blackboard bb) {
		return Status.BH_RUNNING;
	}
	
	private int m_TimesToRepeat; // set in constructor: number of times we're supposed to repeat the child. -1 for infinite
	private int m_RepeatCount; // number of times we've completed and restarted the child
	private BehaviorTree m_Bt;
}

public class MockBehavior : Behavior {
	public int m_InitializeCalled = 0;
	public int m_TerminateCalled = 0;
	public int m_UpdateCalled = 0;
	public Status m_ReturnStatus = Status.BH_RUNNING;
	public Status m_TerminateStatus = Status.BH_INVALID;
	
	public MockBehavior() {}
	
	public override void OnInitialize() {
		++m_InitializeCalled;
	}
	
	public override void OnTerminate(Status e) {
		++m_TerminateCalled;
		m_TerminateStatus = e;
	}
	
	public override Status Update(ref Blackboard bb) {
		++m_UpdateCalled;
		return m_ReturnStatus;
	}
};


public class MockComposite<T> : Composite {
	
	public MockComposite(BehaviorTree bt, int size) {
		for (int i = 0; i < size; ++i) {
			m_Children.Add(new MockBehavior());
		}
	}
	
	// can't overload [] in C#, just make an accessor I guess
	public MockBehavior Get(int index)
	{
		if (index >= m_Children.Count) {
			Debug.LogError("Accessing invalid index on MockBehavior! " + index + " out of " + m_Children.Count);
		}
		
		return (MockBehavior)m_Children[index];
	}
	
	public override Status Update(ref Blackboard bb) {
		return Status.BH_RUNNING;
	}
}