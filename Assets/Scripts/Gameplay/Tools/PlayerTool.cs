using UnityEngine;
using System.Collections;

public enum ToolType { Magnet };

public abstract class PlayerTool : MonoBehaviour {

	public virtual bool TryUsingTool() {
		return true;
	}
}