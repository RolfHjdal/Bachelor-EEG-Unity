///*
// * Handles movement of the playable character. With wasd/keyboard movement, manual emotiv movement, or with autopilot.
// * */
using UnityEngine;
using System.Collections;
using Pathfinding;

public class InputHandler : MonoBehaviour
{
	Seeker seeker;
	Path path;
	Transform target;
	Transform[] taggedTargets;
	WheelCollider WheelCollidersRR, WheelCollidersLR, WheelCollidersRF, WheelCollidersLF;
	public string CurrentCognitivAction;
	public float CurrentCognitivPower;
	public float h, v, treshold, forwardGain;	
	float MaxWaypointDistance = 0.3f;
	float lookSpeed = 2;
	bool IsGUIActive, IsAutopilot;
	int CurrentWaypoint;
	string tag;
	
	void Start ()
	{
		WheelCollidersRR = GameObject.Find ("Player/Character/BoxCollider/WheelCollidersRR").GetComponent<WheelCollider> ();
		WheelCollidersLR = GameObject.Find ("Player/Character/BoxCollider/WheelCollidersLR").GetComponent<WheelCollider> ();
		WheelCollidersRF = GameObject.Find ("Player/Character/BoxCollider/WheelCollidersRF").GetComponent<WheelCollider> ();
		WheelCollidersLF = GameObject.Find ("PlayerCharacter/BoxCollider/WheelCollidersLF").GetComponent<WheelCollider> ();
		taggedTargets = GameObject.Find ("TaggedTargets").GetComponentsInChildren<Transform> ();
		seeker = GetComponent<Seeker> ();
		treshold = 0.05f;
		forwardGain = 50;
	}
	 
	void Update ()
	{
		EmotivInput ();
		KeyboardInput ();
		if (!IsAutopilot) {
			WheelchairRotation ();		//steering
			WheelchairForward ();	//apply torque
		}
	}
	
	void Autopilot (){	
		if (path == null) {
			return;		
		}
		if (CurrentWaypoint >= path.vectorPath.Count) {
			return;
		}

			Vector3 HeadingDirection = (path.vectorPath [CurrentWaypoint] - transform.position).normalized; //direction to next waypoint
			
			rigidbody.velocity = HeadingDirection * 0.6f; //move towards next waypoint
			//rotate wheelchair smoothly towards heading direction
			transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.LookRotation(HeadingDirection),Time.fixedDeltaTime*lookSpeed);
			
			if (Vector3.Distance (transform.position, path.vectorPath [CurrentWaypoint]) < MaxWaypointDistance) 
			{
				CurrentWaypoint++; //head to next waypoint if close to current
			}
			
			if(Vector3.Distance(transform.position, target.transform.position) < MaxWaypointDistance+0.3f)
			{
			IsAutopilot = false;
			rigidbody.velocity = HeadingDirection * 0.2f;
			}
	}

	void FixedUpdate ()
	{
		if (IsAutopilot)
			Autopilot ();
		CognitivActionUpdate();
	}


	//sets power and name of active action
	void CognitivActionUpdate(){
		int count=0;
		foreach (float f in EmoCognitiv.CognitivActionPower)
		{
			//only the active action can be over 0
			if(f>0f)
			{ 
				CurrentCognitivPower = f;
				CurrentCognitivAction = EmoCognitiv.cognitivActionList[count].ToString();
			}
			else 
			{
				count+=1;
			}
		}
	}

	//finds target for the pathfinding algorithm
	public void findArea (string a)
	{
		if (a != null) {
			foreach (Transform t in taggedTargets) {
				if (t.tag.Equals (a)) {
					target = t;
					seeker.StartPath (transform.position, target.position, OnPathComplete); //calculate path
				}
			}
		}
	}

	public void OnPathComplete (Path p)
	{
		if (!p.error) {
			print ("pathcomplete");
			path = p;
			CurrentWaypoint = 0;
			IsAutopilot=true;
		} else {
			IsAutopilot=false;
			Debug.Log (p.error);
			print ("some error with path completion");
		}
	}

	void KeyboardInput ()
	{
		//wasd-input
		if ((Input.GetAxisRaw ("Horizontal") != 0 || Input.GetAxisRaw ("Vertical") != 0) && !IsGUIActive) {
			h = Input.GetAxis ("Horizontal");
			v = Input.GetAxis ("Vertical");
		}

		//buttons 1-5 on keyboard teleports the player
		if (Input.GetKey (KeyCode.Alpha1)) {
			transform.position = new Vector3 (-15f, 0.5f, -9f);
			transform.rotation = Quaternion.Euler (340, 80, 360);
		}
		if (Input.GetKey (KeyCode.Alpha2)) {
			transform.position = new Vector3 (-5.06f, 0.5f, -5.49f);
			transform.rotation = Quaternion.Euler (0, 0, 0);
		}
		if (Input.GetKey (KeyCode.Alpha3)) {
			transform.position = new Vector3 (-1.4f, 0.36f, 4.46f);
			transform.rotation = Quaternion.Euler (0, 0, 0);
		}
		if (Input.GetKey (KeyCode.Alpha4)) {
			transform.position = new Vector3 (2.73f, 0.3f, 9.71f);
			transform.rotation = Quaternion.Euler (360, 170, 0);
		}
		if (Input.GetKey (KeyCode.Alpha5)) {
			transform.position = new Vector3 (8.13f, 0.5f, 9.47f);
			transform.rotation = Quaternion.Euler (360, 170, 0);
		}
	}


	//read input from Emotiv headset
	void EmotivInput ()
	{
		v = h = 0;

		float CognitivPush = EmoCognitiv.CognitivActionPower [1]; 	//push
		float CognitivLeft = EmoCognitiv.CognitivActionPower [5]; 	//left
		float CognitivRight = EmoCognitiv.CognitivActionPower [6];	 //right

		if (CognitivPush > treshold) { //push
			v = CognitivPush * 10;
		}
		
		if (CognitivLeft > treshold) { // left
			h = -CognitivLeft * 10;
		}
		
		if (CognitivRight > treshold) { // right
			h = CognitivRight * 10;
		}
	}

	//rotates player on input h
	void WheelchairRotation ()
	{	
		transform.Rotate (Vector3.up, h * Time.deltaTime * 100);
	}


	void WheelchairForward ()
	{
		WheelCollidersRR.motorTorque = v * forwardGain; 
		WheelCollidersLR.motorTorque = v * forwardGain;
		WheelCollidersRF.motorTorque = v * forwardGain; 
		WheelCollidersLF.motorTorque = v * forwardGain;
	}

	public string GetCurrentCognitivAction ()
	{
		return CurrentCognitivAction;
	}

	public float GetCurrentCognitivPower()
	{
		return CurrentCognitivPower;
	}

	//usually called from ButtonHandler script
	public void setGUIMode (bool gui)
	{
		IsGUIActive = gui;
	}
}


