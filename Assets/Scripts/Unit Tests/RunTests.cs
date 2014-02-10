using UnityEngine;
using System.Collections;

public class RunTests : MonoBehaviour {

	// Use this for initialization
	void Start () {
		SequenceOnePassThrough();	
	}
	
	public void RunTest1() {
		MockBehavior mockBehavior = new MockBehavior();
		BehaviorTree bt = new BehaviorTree();

		bt.Start(mockBehavior);
		CheckIntEqual(0, mockBehavior.m_InitializeCalled, "Init test");
		
		bt.Tick();
		CheckIntEqual(1, mockBehavior.m_InitializeCalled, "Single tick");
	}
	
	public void RunTest2() {
		MockBehavior t = new MockBehavior();
		BehaviorTree bt = new BehaviorTree();
		BehaviorObserver del = DelegateHandler;
		
		bt.Start(t, del);
		bt.Tick();
		CheckIntEqual(1, t.m_UpdateCalled, "Single tick");
		
		t.m_ReturnStatus = Status.BH_SUCCESS;
		bt.Tick();
		CheckIntEqual(2, t.m_UpdateCalled, "2nd tick return success");
	}
	
	public void RunTest3() {
		MockBehavior t = new MockBehavior();
		BehaviorTree bt = new BehaviorTree();
		BehaviorObserver del = DelegateHandler;
		
		bt.Start(t, del);
		bt.Tick();
		CheckIntEqual(0, t.m_TerminateCalled, "1st terminate called");
		
		t.m_ReturnStatus = Status.BH_SUCCESS;
		bt.Tick();
		CheckIntEqual(1, t.m_TerminateCalled, "2nd terminate called");
	}
	
	public void SequenceOnePassThrough () {
		Status[] status = { Status.BH_SUCCESS, Status.BH_FAILURE };
		for (int i = 0; i < 2; ++i) {
			BehaviorTree bt = new BehaviorTree();
			MockComposite<Sequence> seq = new MockComposite<Sequence>(bt, 1);
			
			bt.Start(seq);
			bt.Tick();
			CheckIntEqual((int)seq.m_Status, (int)Status.BH_RUNNING, "Check status " + i);
			CheckIntEqual(0, seq.Get(0).m_TerminateCalled, "Termiante called " + i);
			
			seq.Get(0).m_ReturnStatus = status[i];
			bt.Tick();
			CheckIntEqual((int)seq.m_Status, (int)status[i], "Check status " + i);
			CheckIntEqual(1, seq.Get(0).m_TerminateCalled, "Terminate called " + i);
		}
	}
	
	public void CheckIntEqual(int a, int b, string testName) {
		if (a == b) {
			Debug.Log(testName + " passed.");
		}
		else {
			Debug.LogError(testName + " test failed. " + a + " != " + b);
		}
	}	
	
	public static void DelegateHandler(Status status) {
		Debug.Log("DelegateHandler called: " + status);
	}
}