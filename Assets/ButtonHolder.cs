using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonHolder : MonoBehaviour {
	public Button attackButton;
	public Button cancelButton;
	public Text turnDisplay;

	private GameObject localPlayer;

	void Start () {
		attackButton = GameObject.Find ("AttackButton").GetComponent<Button> ();
		cancelButton = GameObject.Find ("ClearButton").GetComponent<Button> ();
		turnDisplay = GameObject.Find ("TurnText").GetComponent<Text> ();
		attackButton.gameObject.SetActive(false);
		cancelButton.gameObject.SetActive(false);
		turnDisplay.text = "";
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

	public void Recreate(GameObject o) {
		StartCoroutine(RecreateAfter(o));
	}

	IEnumerator RecreateAfter(GameObject o) {
		yield return new WaitForSeconds(10);
		o.SetActive (true);
	}
}
