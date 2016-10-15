using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonHolder : MonoBehaviour {
	public Button attackButton;

	void Start () {
		attackButton = GameObject.Find ("AttackButton").GetComponent<Button> ();
		attackButton.gameObject.SetActive(false);
	}
}
