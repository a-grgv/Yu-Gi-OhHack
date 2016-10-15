using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Selector : MonoBehaviour {

	GameObject selected;
	GameObject enemySelected;

	public Button attackButton;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		foreach (var touch in Input.touches) {
			if (touch.phase == TouchPhase.Began) {
				// Construct a ray from the current touch coordinates
				var ray = Camera.main.ScreenPointToRay (touch.position);
				if (Physics.Raycast (ray)) {
					// Create a particle if hit
					//Instantiate (particle, transform.position, transform.rotation);
				}
			}
		}

		if (Input.GetMouseButtonUp(0)) {
			RaycastHit hit;
			var ray = Camera.main.ScreenPointToRay (Input.mousePosition);
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
							a.Selected ();
						} else if (enemySelected == null) {
							enemySelected = hit.transform.gameObject;
							a.Selected (true);
						}
					}
				}
			} else {
				DeselectAll ();
			}
		}

		if (selected != null && enemySelected != null) {
			attackButton.enabled = true;
		} else {
			attackButton.enabled = false;
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
		
	}
}
