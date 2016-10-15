using UnityEngine;
using System.Collections;

public class HackTheSpawn : MonoBehaviour {

	public GameObject[] stuff;

	void Start () {
		// So hackity hack hack... I hate myself for writing this
		foreach (var t in stuff) {
			Instantiate(t, transform.position, transform.rotation, transform);
		}
	}
	
	void Update () {
	
	}
}
