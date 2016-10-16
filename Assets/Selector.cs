using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class Selector : NetworkBehaviour {

	GameObject selected;
	GameObject enemySelected;

	public GameObject defaultAttackEffect;

	public GameObject green;
	public GameObject red;

	public GameObject[] hits;

	Button attackButton;

	public bool isLocal = false;

	void Start() {
		NetworkManager.singleton.networkAddress = Network.player.ipAddress;

		attackButton = GameObject.FindGameObjectWithTag ("GameController").GetComponent<ButtonHolder>().attackButton;
	}

	public override void OnStartLocalPlayer()
	{
		isLocal = true;
	}

	void FixedUpdate () {
		foreach (var touch in Input.touches) {
			if (touch.phase == TouchPhase.Began) {
				DoSelectMagic (touch.position);
			}
		}

		if (Input.GetMouseButtonUp(0)) {
			DoSelectMagic (Input.mousePosition);
		}
	}

	void DoSelectMagic(Vector3 pos) {
		if (isServer) {
			RpcDoSelectMagic (pos);
		} else {
			DoSelectMagicLogic (pos);
			CmdDoSelectMagic (pos);
		}
	}

	[Command]
	void CmdDoSelectMagic(Vector3 pos) {
		DoSelectMagicLogic (pos);
	}

	[ClientRpc]
	void RpcDoSelectMagic(Vector3 pos) {
		DoSelectMagicLogic (pos);
	}

	void DoSelectMagicLogic(Vector3 pos) {
		Debug.Log ("123");
		if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject ()) {
			return;
		}

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
						a.Selected (green);
					} else if (enemySelected == null) {
						enemySelected = hit.transform.gameObject;
						a.Selected (red);
					}
				}
			}
		} else {
			DeselectAll ();
		}

		if (selected != null && enemySelected != null) {
			attackButton.gameObject.SetActive (true);
		} else {
			attackButton.gameObject.SetActive (false);
		}
	}

	void DeselectAll() {
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
}
