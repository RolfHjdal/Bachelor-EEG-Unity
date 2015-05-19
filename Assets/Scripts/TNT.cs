using UnityEngine;
using System.Collections;

public class TNT : MonoBehaviour {
	private float force = 500;
	private float radius = 30;
	public bool hasExploded = false;
	public GameObject explosion;

public void Explode(){

		Collider[] colliders = Physics.OverlapSphere (transform.position, radius);

		foreach (Collider c in colliders) {

			if(c.rigidbody == null) continue;
			c.rigidbody.AddExplosionForce(force, transform.position, radius, 5f, ForceMode.Impulse);
			print ("i should be about to explod");

		}

		Instantiate (explosion, transform.position, Quaternion.identity);
		Destroy (this.gameObject);
		hasExploded = true;
	}
	public bool getHasExploded(){
		return hasExploded;
	}
	   

}
