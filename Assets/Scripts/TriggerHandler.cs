using UnityEngine;
using System.Collections;
using System;

public class TriggerHandler : MonoBehaviour {
	private int myArea;
	private InfoScript info;
	private HeaderScript header;


	void Start(){

		 findMyAreaCode();
		info = GameObject.Find("Canvas/Information/Info").GetComponent<InfoScript>();
		header = GameObject.Find ("Canvas/Information/Header").GetComponent<HeaderScript>();

	}
	//stay
	void OnTriggerEnter(Collider other){
		if(other.gameObject.tag == "Player"){
			info.setAreaCode(myArea);
			header.setAreaCode(myArea);
		}
	}
	void OnTriggerExit(Collider other){
		if(other.gameObject.tag == "Player"){
		//	info.setAreaCode(0);
		//	header.setAreaCode(0);
		}
	}
	private void findMyAreaCode(){
		
		string[] splitTag;
		string myTag = this.gameObject.tag;
		splitTag=myTag.Split(' ');
		myArea = Convert.ToInt32(splitTag[1]);
	}
	public int getAreaCode(){
		findMyAreaCode ();
		return myArea;
	}

}
