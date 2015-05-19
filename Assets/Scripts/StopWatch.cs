using UnityEngine;
using System.Collections;

public class StopWatch : MonoBehaviour {
	//Fields
	HeaderScript headerText;
	HighScore scoreSystem;
	InfoScript info;
	float time;
	bool timerStarted = false;


	// Use this for initialization
	void Start () {
		scoreSystem = GameObject.Find("SuperParent").GetComponent<HighScore>();
		headerText = GameObject.Find("Canvas/Information/Header").GetComponent<HeaderScript>();
		info = GameObject.Find("Canvas/Information/Info").GetComponent<InfoScript>();
	}
	
	// Update is called once per frame
	void Update () {
		if(timerStarted){
			time = time + Time.deltaTime;
			headerText.displayTime((int)time);
			info.setInfoText("Timer started, get to the finish line as quickly as possible");

		}
	}

	/*
     * Starts the timer and sets timeStarted true.
	 */
	public void StartTimer(){
		time = 0f;
		timerStarted = true;

	}
	/*
     * Stops the timer, sets timerStarted false, sends the time to the HighScore script
	 */
	public void StopTimer(){
		timerStarted  = false;
		scoreSystem.setTime((int)time);

	}
	/*
     * Returns the time as an integer
	 */
	public int getTime(){
		return (int)time;
	}


	/*
     * Returns the boolean value timerStarted
	 */
	public bool getTimerStarted(){
		return timerStarted;
	}



}
