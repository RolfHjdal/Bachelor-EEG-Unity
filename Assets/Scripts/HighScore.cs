using UnityEngine;
using System.Collections;



public class HighScore : MonoBehaviour {
	StopWatch watch;
	int bestTime1 = 500; int bestTime2 = 500; int bestTime3 = 500; int bestTime4 = 500; 
	HeaderScript headerText; 
	InfoScript info;
	int time;
	int level;

	void Start(){
	watch = GameObject.Find("SuperParent").GetComponent<StopWatch>();
	headerText = GameObject.Find("Canvas/Information/Header").GetComponent<HeaderScript>();
		info = GameObject.Find("Canvas/Information/Info").GetComponent<InfoScript>();
	}

	/*
	 * Finds out if the time it has recieved is better than the previous best time,
	 * it also figures out which level the time belongs to.
	 */
	public void setTime(int t){
		time = t;
		switch(level){
		case 1:
			if(time<bestTime1){
				bestTime1 = time;
				headerText.newBestTime("Best! :  " + bestTime1.ToString());
				info.setInfoText("New best time on level " + level);
			}
			break;
		case 2:
			if(time<bestTime2){
				bestTime2 = time;
				headerText.newBestTime("Best! :  " + bestTime2.ToString());
				info.setInfoText("New best time on level " + level);

			}
			break;
		case 3:
			if(time<bestTime3){
				bestTime3 = time;
				headerText.newBestTime("Best! :  " + bestTime3.ToString());
				info.setInfoText("New best time on level " + level);
			}
			break;
		case 4:
			if(time<bestTime4){
				bestTime4 = time;
				headerText.newBestTime("Best! :  " + bestTime4.ToString());
				info.setInfoText("New best time on level " + level);

				GameObject player = GameObject.Find ("Player");
				
				player.transform.position = new Vector3 (2.73f, 0.3f, 9.71f);
				player.transform.rotation = Quaternion.Euler (360, 170, 0);
			}
			break;
		default:
			break;
		}
	}

	/*
	 * This method is called upon by the Time script, it gives
	 * this info about which level the player is in.
	 */
	public void setLevel(int l){
		level = l;
	}





}
