using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Selectable : MonoBehaviour {
	private GameObject circle;

	public SpriteRenderer s;
	public Canvas c;

	public float x = 0;
	public float y = 0;

	void Start() {
		c.gameObject.SetActive (false);
	}

	public bool isSelected() {
		return circle != null;
	}

	public void Selected(string color) {
		c.gameObject.SetActive (true);

		if (color == "red") {
			s.color = new Color (1, 0, 0, 0.5f);
		} else {
			s.color = new Color (0, 1, 0, 0.5f);
		}
	}

	public void Deselected() {
		c.gameObject.SetActive (false);
	}
}
