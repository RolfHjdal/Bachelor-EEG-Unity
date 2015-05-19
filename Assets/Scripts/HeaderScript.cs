using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HeaderScript : MonoBehaviour {


	private int areaCode;
	private string areaOne = "Area one";
	private string areaTwo = "Area two";
	private string areaThree = "Area three";
	private string areaFour = "Area four";
	private string areaFive = "Area five";
	StopWatch stopwatch;
	Text headerText;
	// Use this for initialization
	void Start () {
		headerText = GetComponent<Text>(); 
		stopwatch = GameObject.Find("SuperParent").GetComponent<StopWatch>();
	}



	/*
	 * Called on by StopWatch script, display the timer in the header
	 */
	public void displayTime(int t){
		headerText.text = t.ToString();

	}

	/*
	 * Called on by HighScore script when new highscore is made,
	 * this is displayed on the header.
	 */
	public void newBestTime(string b){
		headerText.text = b;
	}

	/*
	 * Sets a new area code if the player enters a new area,
	 * this is then displayed on the header.
	 * This method is called by the TriggerHandler script.
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
				headerText.text = areaOne;
				break; 
			case 2 : 
				print (areaTwo);
				headerText.text = areaTwo;
				break;
			case 3 : 
				headerText.text = areaThree;
				break;
			case 4 : 
				headerText.text = areaFour;
				break;
			case 5 : 
				headerText.text = areaFive;
				break;
			default:
				//print(out of bounds);
				headerText.text = " ";
				break;
			}
		}
	}
}

