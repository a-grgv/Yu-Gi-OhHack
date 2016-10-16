using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class Selector : NetworkBehaviour {

	GameObject selected;
	GameObject enemySelected;

	public GameObject defaultAttackEffect;

	public GameObject[] hits;

	Button attackButton;
	Button cancelButton;
	Text turnDisplay;

	public bool isLocal = false;
	public bool isMyTurn = true;

	void Start() {
		NetworkManager.singleton.networkAddress = Network.player.ipAddress;

		attackButton = GameObject.FindGameObjectWithTag ("GameController").GetComponent<ButtonHolder>().attackButton;
		cancelButton = GameObject.FindGameObjectWithTag ("GameController").GetComponent<ButtonHolder>().cancelButton;
		turnDisplay = GameObject.FindGameObjectWithTag ("GameController").GetComponent<ButtonHolder>().turnDisplay;
		SetTurnText ();
	}

	public override void OnStartLocalPlayer()
	{
		isLocal = true;
		Debug.Log ("123");
		isMyTurn = isServer;
	}

	void SetTurnText() {
		Debug.Log (isMyTurn);
		if (isMyTurn) {
			turnDisplay.text = "Your turn";
		} else {
			turnDisplay.text = "Opponent's turn";
		}
	}

	void FixedUpdate () {
		if (Input.GetMouseButtonUp(0)) {
			DoSelectMagic (Input.mousePosition);
		}
	}

	void DoSelectMagic(Vector3 pos) {
		DoSelectMagicLogic (pos);
	}

	void DoSelectMagicLogic(Vector3 pos) {
		if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject ()) {
			return;
		}

		var bothSelected = selected != null && enemySelected != null;

		if (!bothSelected && isMyTurn) {
			RaycastHit hit;
			var ray = Camera.main.ScreenPointToRay (pos);
			if (Physics.Raycast (ray, out hit)) {
				if (hit.transform.gameObject.CompareTag ("Player")) {
					Selectable a = hit.transform.GetComponent<Selectable> ();
					if (a.isSelected ()) {
						a.Deselected ();
						if (selected == hit.transform.gameObject) {
							selected = null;
						} else if (enemySelected == hit.transform.gameObject) {
							enemySelected = null;
						}
					} else {
						if (selected == null) {
							selected = hit.transform.gameObject;
							a.Selected ("green");
						} else if (enemySelected == null) {
							enemySelected = hit.transform.gameObject;
							a.Selected ("red");
						}
					}
				}
			} else {
				DeselectAll ();
			}
		}

		bothSelected = selected != null && enemySelected != null;
		var eitherSelected = selected != null || enemySelected != null;

		if (eitherSelected) {
			cancelButton.gameObject.SetActive (true);
		} else {
			cancelButton.gameObject.SetActive (false);
		}

		if (bothSelected) {
			attackButton.gameObject.SetActive (true);
		} else {
			attackButton.gameObject.SetActive (false);
		}
	}

	public void DeselectAll() {
		if (selected != null) {
			Selectable s = selected.GetComponent<Selectable> ();
			s.Deselected ();
			selected = null;
		}

		if (enemySelected != null) {
			Selectable s = enemySelected.GetComponent<Selectable> ();
			s.Deselected ();
			enemySelected = null;
		}

		attackButton.gameObject.SetActive (false);
		cancelButton.gameObject.SetActive (false);
	}

	public void Attack() {
		if (selected == null || enemySelected == null) {
			return;
		}

		string name = enemySelected.name;
		string hit = selected.GetComponent<AttackEffect> ().attack.name;
		if (isServer) {
			RpcAttack (name, hit);
		} else {
			AttackLogic (name, hit);
			CmdAttack (name, hit);
		}
	}

	[Command]
	void CmdAttack(string name, string hit) {
		AttackLogic (name, hit);
	}

	[ClientRpc]
	void RpcAttack(string name, string hit) {
		AttackLogic (name, hit);
	}

	void AttackLogic(string enemyName, string hitName) {
		if (!isLocalPlayer) {
			var actualLocalPlayer = GetLocalPlayer ();
			if (actualLocalPlayer != null) {
				actualLocalPlayer.GetComponent<Selector> ().ToggleTurn ();
			}
		} else {
			ToggleTurn ();
		}

		if (string.IsNullOrEmpty(enemyName) || string.IsNullOrEmpty(hitName)) {
			return;
		}

		DeselectAll ();

		var enemy = GameObject.Find (enemyName);
		if (!enemy) {
			return;
		}

		GameObject go = defaultAttackEffect;
		foreach (var g in hits) {
			if (g.name == hitName) {
				go = g;
				break;
			}
		}

		var ef = Instantiate (go, enemy.transform.position, enemy.transform.rotation) as GameObject;
		ef.transform.parent = enemy.transform;
	}

	public void ToggleTurn() {
		isMyTurn = !isMyTurn;
		SetTurnText ();
	}

	GameObject GetLocalPlayer() {
		foreach (var p in GameObject.FindGameObjectsWithTag ("ActualPlayer")) {
			if (p.GetComponent<Selector> ().isLocal) {
				return p;
			}
		}
		return null;
	}
}
