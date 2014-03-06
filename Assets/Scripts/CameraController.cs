using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour 
{
	public Transform m_playerTransform;
	public Texture2D m_fogOfWar;
	public Texture2D m_black;


	public void Update()
	{
		Vector3 tempPosition = m_playerTransform.position;
		tempPosition.z = transform.position.z;
		transform.position = tempPosition;
	}

	public void OnGUI()
	{
		GUI.DrawTexture(new Rect(0,-(Screen.width - Screen.height) / 2,Screen.width, Screen.width), m_fogOfWar);
	}
}
