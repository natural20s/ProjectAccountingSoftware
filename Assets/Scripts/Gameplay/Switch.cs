using UnityEngine;
using System.Collections;

public class Switch : MonoBehaviour {
	
	public Activatable[] m_Activatable;
	
	// Use this for initialization
	void Start () {
	
	}
	
	
	// ToolType - The type of tool that is trying to activate this switch
	public void Trigger(ToolType type) {
		Debug.Log("Trigger called by tool type: " + type);
		
		if (m_Activatable.Length == 0) {
			Debug.LogWarning("Triggered a switch with no Activatables attached");
		}
		
		for (int idx = 0; idx < m_Activatable.Length; ++idx) {
			m_Activatable[idx].Activate();
		}
	}
}
