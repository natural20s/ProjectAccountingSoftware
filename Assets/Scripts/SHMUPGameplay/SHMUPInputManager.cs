using UnityEngine;
using System.Collections;

public class SHMUPInputManager : MonoBehaviour {
	
	public SHMUPWeaponManager m_WeaponManager;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		if (Input.GetKeyDown(KeyCode.UpArrow)) {
			m_WeaponManager.ToggleFireDirection(KeyCode.UpArrow);
		}
		if (Input.GetKeyDown(KeyCode.LeftArrow)) {
			m_WeaponManager.ToggleFireDirection(KeyCode.LeftArrow);
		}
		if (Input.GetKeyDown(KeyCode.DownArrow)) {
			m_WeaponManager.ToggleFireDirection(KeyCode.DownArrow);
		}
		if (Input.GetKeyDown(KeyCode.RightArrow)) {
			m_WeaponManager.ToggleFireDirection(KeyCode.RightArrow);
		}
	}
	
	
}
