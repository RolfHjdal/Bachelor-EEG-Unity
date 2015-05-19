/*
 * Handles automatic braking when the incline is over a certain treshold or if player is near collision
 * */

using UnityEngine;
using System.Collections;

public class EmergencyBrake : MonoBehaviour {

	WheelCollider wheel1, wheel2, wheel3, wheel4;
	GameObject character; 

	void Start(){
		wheel1 = GameObject.Find("Player/Character/BoxCollider/WheelCollidersLR").GetComponent<WheelCollider>();
		wheel2 = GameObject.Find("Player/Character/BoxCollider/WheelCollidersRR").GetComponent<WheelCollider>();
		wheel3 = GameObject.Find("Player/Character/BoxCollider/WheelCollidersRF").GetComponent<WheelCollider>();
		wheel4 = GameObject.Find("Player/Character/BoxCollider/WheelCollidersLF").GetComponent<WheelCollider>();
		character = GameObject.Find("Player/Character");
	}

	void Update(){
		float incline = character.transform.eulerAngles.x; //angle of wheelchair
		if((incline > 274.0f && Input.GetAxis("Vertical") == 0.0f)) {
			LockWheels();
		}
		
		if(Input.GetAxis("Vertical") > 0.0f){
			UnlockWheels();
		}
	}
	//stops the player from colliding with other objects. player will need to back out of a collision
	void OnTriggerStay(Collider other){
		if (other.tag == "Collidable" && Input.GetAxis("Vertical") >-0.3f) {
					
			LockWheels();

			if(Input.GetAxis("Vertical") < 0.0f){
				UnlockWheels();
			}
				} else {
			return;		
		}
	}

	void OnTriggerExit(Collider other){
		UnlockWheels ();
	}

	void LockWheels(){
		wheel1.brakeTorque = 100f;
		wheel2.brakeTorque = 100f;
		wheel3.brakeTorque = 100f;
		wheel4.brakeTorque = 100f;

	}
	void UnlockWheels(){
		wheel1.brakeTorque = 0f;
		wheel2.brakeTorque = 0f;
		wheel3.brakeTorque = 0f;
		wheel4.brakeTorque = 0f;
	}
}