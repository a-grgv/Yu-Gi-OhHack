using UnityEngine;
using System.Collections;

public class Selectable : MonoBehaviour {

	public GameObject selectionCircle;
	public GameObject selectionCircle1;
	private GameObject circle;

	public float x = 0;
	public float y = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public bool isSelected() {
		return circle != null;
	}

	public void Selected(bool isEnemy = false) {
		if (circle == null) {
			Vector3 newPos = new Vector3(transform.position.x + x, transform.position.y, transform.position.z + y);
			circle = Instantiate(isEnemy ? selectionCircle1 : selectionCircle, newPos, transform.rotation, transform) as GameObject;
		}
	}

	public void Deselected() {
		if (circle != null) {
			Destroy(circle);
			circle = null;
		}
	}
}
