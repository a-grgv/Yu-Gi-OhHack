using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {
	public Transform target;
	public GameObject hitEffect;
	public float speed = 2f;

	void Update(){
		if(target == null)  return;

		transform.position = Vector3.MoveTowards(transform.position, target.position, Time.deltaTime * speed);

		if (transform.position == target.position) {
			if (hitEffect != null) {
				(Instantiate (hitEffect, target.position, target.rotation) as GameObject).transform.parent = target.transform;
			}
			Destroy (gameObject);
		}
	}
}
