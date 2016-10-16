using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonHolder : MonoBehaviour {
	public Button attackButton;
	public Button cancelButton;

	private GameObject localPlayer;

	void Start () {
		attackButton = GameObject.Find ("AttackButton").GetComponent<Button> ();
		cancelButton = GameObject.Find ("ClearButton").GetComponent<Button> ();
		attackButton.gameObject.SetActive(false);
		cancelButton.gameObject.SetActive(false);
	}

	GameObject GetLocalPlayer() {
		if (localPlayer == null) {
			foreach (var p in GameObject.FindGameObjectsWithTag ("ActualPlayer")) {
				if (p.GetComponent<Selector> ().isLocal) {
					localPlayer = p;
					break;
				}
			}
		}

		return localPlayer;
	}

	public void Attack() {
		var p = GetLocalPlayer ();

		if (p != null) {
			p.GetComponent<Selector> ().Attack ();
		}
	}

	public void Clear() {
		var p = GetLocalPlayer ();

		if (p != null) {
			p.GetComponent<Selector> ().DeselectAll ();
		}
	}
}
