using UnityEngine;
using System.Collections;

public class HitEffectDestroy : MonoBehaviour {

	float duration = 0;
	ParticleSystem ps;
	void Start () {
		ps = GetComponent<ParticleSystem> ();
	}
	
	void Update () {
		duration += ps.duration * Time.deltaTime;
		if (duration >= ps.duration) {
			Destroy (gameObject);
		}
	}
}
