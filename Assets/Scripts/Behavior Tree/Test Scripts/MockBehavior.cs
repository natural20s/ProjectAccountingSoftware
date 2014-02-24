using UnityEngine;
using System.Collections;

public class MockBehavior : Behavior {

	public int m_InitializeCalled = 0;
	public int m_TerminateCalled = 0;
	public int m_UpdateCalled = 0;
	public Status m_ReturnStatus = Status.BH_RUNNING;
	public Status m_TerminateStatus = Status.BH_INVALID;

	public override void OnInitialize(ref Blackboard bb) {
		++m_InitializeCalled;
	}

	public override void OnTerminate(Status s) {
		Debug.Log ("Status passed to termiante " + s);
		++m_TerminateCalled;
		m_TerminateStatus = s;
	}

	public override Status Update(ref Blackboard bb) {
		++m_UpdateCalled;
		return m_ReturnStatus;
	}
}

public class MockSequence : Sequence {
	public MockSequence(int size) {
		for (int idx = 0; idx < size; ++idx) {
			m_Children.Add(new MockBehavior());
		}
	}

	public MockBehavior AtIndex(int idx) {
		if (idx >= m_Children.Count) {
			Debug.LogError ("MockComposite AtIndex out of range");
			return new MockBehavior();
		}

		return (MockBehavior)m_Children[idx];
	}
}