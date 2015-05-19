using UnityEngine;
using System.Collections;
using System;


public class Timer : MonoBehaviour {
	
	//Fields
	private HeaderScript header;
	StopWatch watch;
	HighScore scoreSystem;
	new int tag;
	String type;

	// Use this for initialization
	void Start () {
		header = GameObject.Find ("Canvas/Information/Header").GetComponent<HeaderScript>();
		watch = GameObject.Find ("SuperParent").GetComponent<StopWatch>();
		scoreSystem = GameObject.Find ("SuperParent").GetComponent<HighScore>();
	}

	/*
     * Finds and splits the tag attched to the timer trigger.
     * Type can either be on or off.
     * Tag stores the level.
	 */
	private void findMyAreaTag(){
		
		string[] splitTag;
		string myTag = this.gameObject.tag;
		splitTag = myTag.Split(' ');
		tag = Convert.ToInt32(splitTag[1]);
		type = splitTag[2];
	}
	
	
	/*
     * Detects if the player has entered the timer trigger
     * and wheter that trigger should turn the timer on and off.
     * Turns on or off the timer accordingly, the level tag is sent 
     * to the highscore system
	 */
	void OnTriggerEnter(Collider other){
	
		if ((other.gameObject.tag == "Player")) {
			findMyAreaTag();
			if(type.Equals("On")){
				scoreSystem.setLevel(tag);
				watch.StartTimer();

			}	

			if(type.Equals("Off")){
				scoreSystem.setLevel(tag);	
				watch.StopTimer();
					
				}
			
			
		}
	}
	
	
	
}