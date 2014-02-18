using UnityEngine;
using System.Collections;

public enum ToolType { Magnet, Beacon };

public abstract class PlayerTool : MonoBehaviour {

	protected ToolType m_ToolType;

	public virtual bool TryUsingTool() {
		return true;
	}
}