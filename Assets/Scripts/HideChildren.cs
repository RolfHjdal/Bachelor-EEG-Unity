using UnityEngine;
using System.Collections;

public class HideChildren : MonoBehaviour {

	// Get all renderers from children, then disable each of them
	void Awake () {
			MeshRenderer[] mr = GetComponentsInChildren<MeshRenderer>();
			foreach (MeshRenderer m in mr)
			{
			m.enabled=false;
			}
	}
}
