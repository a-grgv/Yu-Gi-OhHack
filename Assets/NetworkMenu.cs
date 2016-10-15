using UnityEngine;
using System.Collections;

public class NetworkMeny : MonoBehaviour {

	public string connectionIp = "127.0.0.1";
	public string port = "8632";

	private void OnGUI() {
		connectionIp = GUILayout.TextField (connectionIp);
		port = GUILayout.TextField (port);
	}

	
	// Update is called once per frame
	void Update () {
	
	}
}
