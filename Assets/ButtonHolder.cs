using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonHolder : MonoBehaviour {
	public Button attackButton;

	private GameObject localPlayer;

	void Start () {
		attackButton = GameObject.Find ("AttackButton").GetComponent<Button> ();
		attackButton.gameObject.SetActive(false);
	}

	public void Attack() {
		if (localPlayer == null) {
			foreach (var p in GameObject.FindGameObjectsWithTag ("ActualPlayer")) {
				if (p.GetComponent<Selector> ().isLocal) {
					localPlayer = p;
					break;
				}
			}
		}

		if (localPlayer != null) {
			localPlayer.GetComponent<Selector> ().Attack ();
		}
	}
}
