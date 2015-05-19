using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InfoScript : MonoBehaviour {
	//fields
	private int areaCode;
	private string areaOne = "This is level 1, time for a dragrace, once you cross the start line, a timer will start";
	private string areaTwo = "Welcome to level 2, here you will test your abilities to navigate through narrow corridors, cross the start line to get started";
	private string areaThree = "You are now in level 3, test your abilities to focus by crossing these ramps you see ahead";
	private string areaFour = "Level 4, get through the track without touching any of the cones, they will explode if you do";
	private string areaFive = "Last level, this is level 5, navigate the busy street without bumping into anyone, watch out for the samurais!";
	public Text infoText;
	StopWatch stopwatch;
	// Use this for initialization
	void Start () {
		infoText = GetComponent<Text>(); 
		stopwatch = GameObject.Find("SuperParent").GetComponent<StopWatch>();
	}


	/*
	 * Called by TriggerHandler, changes the areaCode if the
	 * player enters a new area.
	 * Changes the info text to the corresponding area
	 */
	public void setAreaCode(int n){
		if(n == areaCode){
			return;
		}
		areaCode = n;
	   if(!stopwatch.getTimerStarted()){
		switch(areaCode){
		case 1 :
			print (areaOne);
			infoText.text = areaOne;
			break; 
		case 2 : 
			print (areaTwo);
			infoText.text = areaTwo;
			break;
		case 3 : 
			infoText.text = areaThree;
			break;
		case 4 : 
			infoText.text = areaFour;
			break;
		case 5 : 
			infoText.text = areaFive;
			break;
		default:
			//print(out of bounds);
			infoText.text = " ";
			break;
		}

	}
}
	/*
	 * This method is called by StopWatch script when timer is started,
	 * gives information in the info section of the GUI when the timer is started.
	 */
	public void setInfoText(string info){
		infoText.text = info;
	}

	/*
	 * Returns the current areaCode.
	 */
	public int getAreaCode(){
		return areaCode;

	}
}
