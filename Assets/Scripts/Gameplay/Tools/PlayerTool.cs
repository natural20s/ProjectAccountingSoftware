using UnityEngine;
using System.Collections;

public enum ToolType { Magnet, Beacon, Stunner };

public abstract class PlayerTool : MonoBehaviour {

	public virtual bool TryUsingTool() {
		return true;
	}
}