using UnityEngine;
using System.Collections;

public enum ToolType { Magnet, Beacon };

public abstract class PlayerTool : MonoBehaviour {

	public virtual bool TryUsingTool() {
		return true;
	}
}