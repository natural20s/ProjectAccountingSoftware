using UnityEngine;
using System.Collections;

public class DoorManager : MonoBehaviour 
{

	const string DOOR_OPEN_BOOL = "open";
	const string DOOR_OPEN_STATE = "DoorOpen";
	
	public Animator m_doorAnimator;
	public Collider2D m_colliderRef;

	public bool m_isOpen { get{ return m_doorAnimator.GetCurrentAnimatorStateInfo(0).IsName(DOOR_OPEN_STATE); } }

	public void Start()
	{
//		m_openStateHash = Animator.StringToHash("Base." + DOOR_OPEN_STATE); 
	}

	public void Update()
	{
		if(Input.GetKeyDown (KeyCode.Space))
		{
			OpenDoor();
		}
		else if(Input.GetKeyUp (KeyCode.Space))
		{
			CloseDoor();
		}

		if(m_doorAnimator.GetCurrentAnimatorStateInfo(0).IsName(DOOR_OPEN_STATE))
		{
			m_colliderRef.enabled = false;
		}
		else
		{
			m_colliderRef.enabled = true;
		}
	}

	public void OpenDoor()
	{
		m_doorAnimator.SetBool(DOOR_OPEN_BOOL, true);
	}

	public void CloseDoor()
	{
		m_doorAnimator.SetBool(DOOR_OPEN_BOOL, false);
	}
}
