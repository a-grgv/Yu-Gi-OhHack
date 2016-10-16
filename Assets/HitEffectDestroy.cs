using UnityEngine;
using System.Collections;

public class HitEffectDestroy : MonoBehaviour {

	float duration = 0;
	ParticleSystem ps;
	Selectable parentScript;
	void Start () {
		ps = GetComponent<ParticleSystem> ();
		parentScript = transform.parent.GetComponent<Selectable> ();
		var s = GameObject.FindGameObjectWithTag ("ActualPlayer").GetComponent<Selector>();
		parentScript.Selected (s.red);
	}
	
	void Update () {
		duration += ps.duration * Time.deltaTime;
		if (duration >= ps.duration + 2) {
			Destroy (gameObject);
			parentScript.Deselected ();
		}
	}
}
