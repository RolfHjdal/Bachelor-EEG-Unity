using UnityEngine;
using System.Collections;

public class TrueFalse : MonoBehaviour {


	bool triggered = false;

	void OnTriggerStay(Collider other){
		if (other.gameObject.tag == "Player") {

			triggered = true;
			
		}


	}
	void OnTriggerExit(Collider other){
		if (other.gameObject.tag == "Player") {
				
			triggered = false;
		}
	
	}

	public bool returnTrigger(){
		return triggered;
	}
}
