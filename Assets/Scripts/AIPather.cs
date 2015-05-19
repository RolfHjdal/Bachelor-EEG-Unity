 using UnityEngine;
using System.Collections;
using Pathfinding;

public class AIPather : MonoBehaviour {

	public Transform target;
	Seeker seeker;
	Path path;
	int currentWaypoint;
	CharacterController characterController;
	float maxWaypointDistance = 0.3f;
	float speed = 15;

	public float lookSpeed = 5;

	public Vector3 curLoc;
	public Vector3 prevLoc;
	public int rotMod = 1;

	void Start(){
		seeker = GetComponent<Seeker>();
		seeker.StartPath(transform.position, target.position, OnPathComplete);
		characterController=GetComponent<CharacterController>();
		if(tag == "Samurai") rotMod *= -1;
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
	}
}
