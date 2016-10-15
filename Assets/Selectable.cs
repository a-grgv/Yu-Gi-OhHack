using UnityEngine;
using System.Collections;

public class Selectable : MonoBehaviour {
	private GameObject circle;

	public float x = 0;
	public float y = 0;

	public bool isSelected() {
		return circle != null;
	}

	public void Selected(GameObject c) {
		if (circle == null) {
			Vector3 newPos = new Vector3(transform.position.x + x, transform.position.y, transform.position.z + y);
			circle = Instantiate(c, newPos, transform.rotation) as GameObject;
			circle.transform.parent = transform;
		}
	}

	public void Deselected() {
		if (circle != null) {
			Destroy(circle);
			circle = null;
		}
	}
}
