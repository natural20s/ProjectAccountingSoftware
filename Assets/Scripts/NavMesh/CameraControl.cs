using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {
	
	public int iMoveSpeed = 1;
	public int iZoomSpeed = 1;
	public int iRotateSpeed = 1;
	
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if ( Input.GetKey( KeyCode.E ) ) // move in -y direction
			transform.position += new Vector3(0, 1 * iZoomSpeed, 0);
		
		if ( Input.GetKey( KeyCode.R ) ) // move in positive y direction
			transform.position += new Vector3(0, -1 * iZoomSpeed, 0);
		
		if ( Input.GetKey(KeyCode.DownArrow) ) // rotate ccw about x axis (?)
			transform.RotateAround( transform.position, -transform.right, Time.deltaTime * iRotateSpeed );
		
		if ( Input.GetKey( KeyCode.UpArrow ) ) // rotate ccw about x axis (?)
			transform.RotateAround( transform.position, transform.right, Time.deltaTime * iRotateSpeed );
			
		if ( Input.GetKey( KeyCode.RightArrow ) ) // rotate ccw about y axis
			transform.RotateAround( transform.position, new Vector3(0, 1, 0), Time.deltaTime * iRotateSpeed );
		
		if ( Input.GetKey( KeyCode.LeftArrow ) ) // rotate ccw about y axis
			transform.RotateAround( transform.position, new Vector3(0, -1, 0), Time.deltaTime * iRotateSpeed );
		

		
		if(Input.GetKey(KeyCode.W))
		{
			Vector3 tempForward = transform.forward.normalized;
			tempForward.y = 0;
			transform.position += tempForward * iMoveSpeed;			
			
		}
		if(Input.GetKey(KeyCode.S))
		{
			Vector3 tempForward = transform.forward.normalized;
			tempForward.y = 0;
			transform.position -= tempForward * iMoveSpeed;		
		}
		if(Input.GetKey(KeyCode.A))
		{
			Vector3 tempRight = transform.right.normalized;
			tempRight.y = 0;
			transform.position -= tempRight * iMoveSpeed;
		}
		if(Input.GetKey(KeyCode.D))
		{
			Vector3 tempRight = transform.right.normalized;
			tempRight.y = 0;
			transform.position += tempRight * iMoveSpeed;
		}
	}
}
