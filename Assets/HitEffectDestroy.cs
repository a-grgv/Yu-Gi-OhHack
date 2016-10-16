using UnityEngine;
using System.Collections;

public class HitEffectDestroy : MonoBehaviour {

	float duration = 0;
	ParticleSystem ps;
	Selectable parentScript;
	void Start () {
		ps = GetComponent<ParticleSystem> ();
		parentScript = transform.parent.GetComponent<Selectable> ();
		parentScript.Selected ("red");
	}
	
	void Update () {
		duration += ps.duration * Time.deltaTime;
		if (duration >= ps.duration + 1) {
			transform.parent.gameObject.SetActive(false);
			GameObject.FindGameObjectWithTag ("GameController").GetComponent<ButtonHolder>().Recreate(transform.parent.gameObject);
			parentScript.Deselected ();
			Destroy (gameObject);
		}
	}
}
