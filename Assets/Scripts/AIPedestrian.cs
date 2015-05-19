using UnityEngine;
using System.Collections;
using Pathfinding;

public class AIPedestrian : MonoBehaviour {

	public Transform curTarget;
	Seeker seeker;
	Path path;
	int currentWaypoint;
	CharacterController characterController;
	float maxWaypointDistance = 0.3f;
	float speed = 15;
	float lookSpeed = 5;
	Vector3 curLoc;
	Vector3 prevLoc;
	int rotMod = 1;
	public Transform[] targets;

	void Start(){
		seeker = GetComponent<Seeker>();
		characterController=GetComponent<CharacterController>();
		if(tag == "Samurai") rotMod *= -1; //turn 180deg
		targets = GameObject.Find("Targets").GetComponentsInChildren<Transform>();
		newTarget();
	}

	public void OnPathComplete(Path p){
		if(!p.error){
		path = p;
		currentWaypoint = 0;
		}else{
			Debug.Log(p.error);
		}
	}


	void FixedUpdate(){

		if(path == null){
			return;
		}
		if(currentWaypoint >= path.vectorPath.Count){
			return;
		}
		
		prevLoc = curLoc;
		curLoc = transform.position;
		Vector3 rotVec = prevLoc-curLoc;
		transform.rotation = Quaternion.Lerp (transform.rotation,  Quaternion.LookRotation(rotVec*rotMod), Time.fixedDeltaTime * lookSpeed); //rotate to heading direction

		Vector3 dir = (path.vectorPath[currentWaypoint]-transform.position).normalized * speed * Time.fixedDeltaTime;
		characterController.SimpleMove(dir);        
		if(Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]) < maxWaypointDistance){
			currentWaypoint++;
			print (currentWaypoint);
		}
		if(Vector3.Distance(transform.position, curTarget.transform.position) < maxWaypointDistance+0.7f){
			newTarget();
		}
	}

	void newTarget(){
		int targetSize = targets.Length;
		curTarget = targets[Random.Range (1,targetSize)];
		seeker.StartPath(transform.position, curTarget.position, OnPathComplete);
		print(curTarget.ToString());
	}
}
