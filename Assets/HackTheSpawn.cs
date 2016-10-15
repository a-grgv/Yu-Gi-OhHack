using UnityEngine;
using System.Collections;

public class HackTheSpawn : MonoBehaviour {

	public GameObject[] stuff;

	// Use this for initialization
	void Start () {
		// So hackity hack hack... I hate myself for writing this
		foreach (var t in stuff) {
			Instantiate(t, transform);
		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
