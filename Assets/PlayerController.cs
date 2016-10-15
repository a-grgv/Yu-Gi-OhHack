using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerController : NetworkBehaviour {

	void Start () {
		NetworkManager.singleton.networkAddress = Network.player.ipAddress;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.W)) {
			Print ();
		}
	}

	void Print() {
		PrintLogic ();
		if (isServer) {
			RpcPrint ();
		} else {
			CmdPrint ();
		}
	}

	[Command]
	void CmdPrint() {
		Debug.Log ("cmd");
	}

	[ClientRpc]
	void RpcPrint() {
		Debug.Log ("rpc");
		PrintLogic ();
	}

	void PrintLogic() {
		Debug.Log ("logic");
	}
}
