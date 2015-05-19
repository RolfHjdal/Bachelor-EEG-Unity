using UnityEngine;
using System.Collections;

public class Explosive : MonoBehaviour {
	TNT bomb;
	Rigidbody player;
	bool detonated = false;
	Camera camera1;
	Camera camera2;
	Camera camera3;
	TrueFalse trigger;
	float time;
	bool triggered;
	bool countDown;


	void Start(){ 
		 bomb = GameObject.Find("Bomb").GetComponent<TNT>();
		 player = GameObject.Find ("Player").GetComponent<Rigidbody> ();
		 trigger = GameObject.Find ("TrueFalseTrigger").GetComponent<TrueFalse> ();
		 camera1 = GameObject.Find("Character/PlayerCam").GetComponent<Camera>();
		 camera2 = GameObject.Find ("MainCamera1").GetComponent<Camera>();
		 camera3 = GameObject.Find ("MainCamera2").GetComponent<Camera>();

		 camera2.enabled = false;
		 camera3.enabled = false;
	}

	void Update(){
		if(countDown){
			time = time + Time.deltaTime;
			if(time>2){
				Time.timeScale = 1F;
				Time.fixedDeltaTime = 0.02F;
				Application.LoadLevel(Application.loadedLevel);			
			}
		}
	}

	     void OnTriggerEnter(Collider other){
		if (other.gameObject.tag == "Player") { 

			if(!detonated){

				triggered = trigger.returnTrigger();

				if(!triggered){

					camera3.enabled = false;
					camera1.enabled = false;
					camera2.enabled = true;
					print ("not triggered");

				} 
				if(triggered)
				{   
					camera3.enabled = true;
					camera1.enabled = false;
					camera2.enabled = false;
					print("triggered");
				}
			
				bomb.gameObject.GetComponent<AudioSource>().Play();
				//yield return WaitForSeconds(1);
				Time.timeScale = 0.5F;
				Time.fixedDeltaTime = 0.5F * 0.02F;
				bomb.Explode();
				detonated = true;
				countDown = true; 

			}
					
	        
				}
		}


}