using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Sanford.Collections;
using Sanford.Collections.Generic;

public enum Status {
	BH_INVALID, BH_SUCCESS, BH_FAILURE, BH_RUNNING, BH_SUSPENDED
};

public delegate void BehaviorObserver (Status status);

public abstract class Behavior {
	
	public Status m_Status;
	public BehaviorObserver m_Observer;
	
	public virtual void OnInitialize() {}
	public virtual void OnTerminate(Status status) {}
	public abstract Status Update(ref Blackboard bb);
	
	public Status Tick(ref Blackboard bb) {
		if (m_Status == Status.BH_INVALID) {
			OnInitialize();
		}
		
		m_Status = Update(ref bb);
		
		if (m_Status != Status.BH_RUNNING)
		{
			OnTerminate(m_Status);
		}
		
		return m_Status;
	}
	
	public Behavior() {
		m_Status = Status.BH_INVALID;
	}
}

public class BehaviorTree {
	
	
	protected Deque<Behavior> m_Behaviors = new Deque<Behavior>(); 
	
	public Blackboard m_Blackboard = new Blackboard(); // we assume this will be assigned by the tree controller...for now
	
	public void Start(Behavior bh, BehaviorObserver observer = null) {
		
		if (observer != null) {
			bh.m_Observer = observer;
		}
		
		m_Behaviors.PushFront(bh); // is/should/can this be passed by reference?
	}
	
	public void Stop(Behavior bh, Status result) {
		if (result == Status.BH_RUNNING) {
			Debug.LogError("Trying to stop something that was still running!");
		}
		bh.m_Status = result;
		if (bh.m_Observer != null) {
			bh.m_Observer(result);
		}
	}
	public void Tick() {
		// Insert an end-of-update marker into the list of tasks
		m_Behaviors.PushBack(null); // insert check for an empty list
		
		// Keep going updating tasks until we encounter the marker
		while (Step()) {
			continue;
		}
	}
	
	public bool Step() {
		Behavior current = m_Behaviors.PopFront();
		
		if (current == null) {
			return false;
		}
		
		// Perform the update on this individual task
		current.Tick(ref m_Blackboard);
		
		// Process the observer if the task terminated
		if (current.m_Status != Status.BH_RUNNING) {
			if (current.m_Observer != null) {
				current.m_Observer(current.m_Status);
			}
			//current.OnTerminate(current.m_Status);
		}	
		else {
			// in the cpp version, we would push the current behavior to the back of the dequeue
			// this will eventually push the NULL behavior forward, which terminates the loop
			m_Behaviors.PushBack(current);
		}
		return true;
	}
	
	public bool IsEmpty() {
		return m_Behaviors.Count == 0;
	}
	
	// Only using this temporarly for testing
	public void Reset() {
		while (m_Behaviors.Count > 0) {
			Behavior temp = m_Behaviors.PopFront();
			if (temp != null) {
				temp.OnTerminate(Status.BH_FAILURE);
			}
		}
			
	}
}