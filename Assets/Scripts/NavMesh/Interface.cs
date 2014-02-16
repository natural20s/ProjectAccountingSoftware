using UnityEngine;
using System.Collections;

public class Interface : MonoBehaviour 
{

	// A quick reference to help with writing GUIs
	//Rect(x, y, width, height)
	
	public GameObject NavGraphOwner;
	
	public int iXOffset = 10;
	public int iYOffset = 30;
	
	public int iBoxWidth = 225;
	public int iBoxHeight = 330;
	
	public int iPropertyTextWidth = 125;
	public int iPropertyTextHeight = 25;
	
	private string s_iStartingX = "0";
	private string s_iStartingZ = "0";
	private string s_iEndX = "16";
	private string s_iEndZ = "1";
	private string s_iIncDist = "1";
	private string s_iXCastDist = "20";
	private string s_iZCastDist = "20";
	
	private int iStartX = 0;
	private bool displayCameraControls = true;
	
	// Use this for initialization
	void Start () 
	{
		
	}
	
	
	void OnGUI()
	{
		// Create the main box that all the buttons will be in
		GUI.Box( new Rect(0, 0, iBoxWidth, iBoxHeight), "Options Panel");
		
		
		// Make box for x and y starting coordinates
		// ******** isn't there a way to optimize divide by two?
		GUI.Box( new Rect(iXOffset, iYOffset, iPropertyTextWidth, iPropertyTextHeight), "Starting X value" ); 
		s_iStartingX = GUI.TextField(new Rect( (iBoxWidth * 0.8f), iYOffset, 40, 25), s_iStartingX, 4 ); 
		
		GUI.Box( new Rect(iXOffset, iYOffset*2, iPropertyTextWidth, iPropertyTextHeight), "Starting Z value" );
		s_iStartingZ = GUI.TextField(new Rect( (iBoxWidth * 0.8f), iYOffset*2, 40, 25), s_iStartingZ, 4);
		
		GUI.Box( new Rect(iXOffset, iYOffset*3, iPropertyTextWidth, iPropertyTextHeight), "End X Value" );
		s_iEndX = GUI.TextField(new Rect( (iBoxWidth * 0.8f), iYOffset*3, 40, 25), s_iEndX, 4);
			
		GUI.Box( new Rect(iXOffset, iYOffset*4, iPropertyTextWidth, iPropertyTextHeight), "End Z Value" );
		s_iEndZ = GUI.TextField(new Rect( (iBoxWidth * 0.8f), iYOffset*4, 40, 25), s_iEndZ, 4);
		
		if ( GUI.Button( new Rect(iBoxWidth/4, iYOffset*5.25f, 100, 25), "Find path!") )
		{
			Debug.Log("Int: " + iStartX);
			//SendMessage("SetParametersAndFindPath", s_iStartingX, s_iStartingY, s_iEndX, s_iEndY, s_iIncDist);
			//SendMessage("SetParametersAndFindPath");
			GetComponent<NavGraphConstructor>().SetParametersAndFindPath(s_iStartingX, s_iStartingZ, s_iEndX, s_iEndZ, s_iIncDist);

		}
		
		
		GUI.Box( new Rect(iXOffset, iYOffset*7, iPropertyTextWidth, iPropertyTextHeight), "Increment Distance" );
		s_iIncDist = GUI.TextField( new Rect( (iBoxWidth * 0.8f), iYOffset*7, 40, 25), s_iIncDist, 4);
		
		GUI.Box( new Rect(iXOffset, iYOffset*8, iPropertyTextWidth, iPropertyTextHeight), "X cast distance" );
		s_iXCastDist = GUI.TextField(new Rect( (iBoxWidth * 0.8f), iYOffset*8, 40, 25), s_iXCastDist, 4);
		
		GUI.Box( new Rect(iXOffset, iYOffset*9, iPropertyTextWidth, iPropertyTextHeight), "Z cast distance" );
		s_iZCastDist = GUI.TextField(new Rect( (iBoxWidth * 0.8f), iYOffset*9, 40, 25), s_iZCastDist, 4);
		
		if ( GUI.Button( new Rect(iBoxWidth/4, iBoxHeight * 0.9f, 100, 25), "Make graph") )
		{
			GetComponent<NavGraphConstructor>().SetParametersAndCreateGraph(s_iIncDist, s_iXCastDist, s_iZCastDist);
		}
		
		
		if ( displayCameraControls )
		{
			GUI.Box( new Rect(0, iYOffset * 11, iBoxWidth + 50, 75), "Camera controls \n" +
				"Lateral movement is controlled by WASD \n" +
				"Tilt the camera with the arrow keys \n" +
				"Zoom in and out with R and E, respectively");
			       
		}
		
	}
}
