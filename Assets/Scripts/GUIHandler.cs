using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GUIHandler : MonoBehaviour {

	private int areaCode;
	private string areaOne = "Wheelchair is in area one";
	private string areaTwo = "Wheelchair is in area two";
	private string areaThree = "Wheelchair is in area three";
	private string areaFour = "Wheelchair is in area four";
	private string areaFive = "Wheelchair is in area five";

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		switch(areaCode){
			case 1 :
				print (areaOne);
				break; 
			case 2 : 
				print (areaTwo);
				break;
			case 3 : 
				print (areaThree);
				break;
			case 4 : 
				print (areaFour);
				break;
			case 5 : 
				print (areaFive);
				break;
			default:
			//print(out of bounds);
			break;
		}
	}

	public void setArea(int area){
	areaCode=area;
	}

}
