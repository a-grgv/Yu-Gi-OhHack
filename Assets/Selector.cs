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

	Button attackButton;

	void Start() {
		NetworkManager.singleton.networkAddress = Network.player.ipAddress;

		attackButton = GameObject.FindGameObjectWithTag ("GameController").GetComponent<ButtonHolder>().attackButton;
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
		DoSelectMagicLogic (pos);
		if (isServer) {
			RpcDoSelectMagic (pos);
		} else {
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
	}

	public void Attack() {
		AttackLogic ();
		if (isServer) {
			RpcAttack ();
		} else {
			CmdAttack ();
		}
	}

	[Command]
	void CmdAttack() {
		AttackLogic ();
	}

	[ClientRpc]
	void RpcAttack() {
		AttackLogic ();
	}

	void AttackLogic() {
		if (selected == null || enemySelected == null) {
			return;
		}

		GameObject effect;
		AttackEffect effectScript = selected.GetComponent<AttackEffect> ();
		if (effectScript) {
			effect = effectScript.attack ?? defaultAttackEffect;
		} else {
			effect = defaultAttackEffect;
		}

		var ef = Instantiate (effect, enemySelected.transform.position, enemySelected.transform.rotation) as GameObject;
		ef.transform.parent = enemySelected.transform;
	}
}
