using UnityEngine;
using System.Collections;
using System.Linq;

public class AutogenerateMarkers : MonoBehaviour {

	public GameObject SceneRoot;
	public GameObject[] Monsters;

	void Start () {
		TextAsset[] PatternAssets = Resources.LoadAll("ardata/markers", typeof(TextAsset)).Cast<TextAsset>().ToArray();

		for (var i = 0; i < PatternAssets.Length; i++) {
			var m = Monsters [i % Monsters.Length];
			var arm = gameObject.AddComponent<ARMarker> () as ARMarker;
			arm.MarkerType = MarkerType.Square;
			arm.Tag = "Marker " + i;
			arm.PatternFilename = PatternAssets[i].name;
			arm.PatternContents = PatternAssets[i].text;
			arm.Load ();

			var marker = new GameObject ();
			marker.name = "Marker " + i;
			marker.transform.parent = SceneRoot.transform;
			marker.layer = 8;
			var obj = Instantiate (m, marker.transform) as GameObject;
			obj.SetActive (true);

			var arto = marker.AddComponent<ARTrackedObject> () as ARTrackedObject;
			arto.MarkerTag = "Marker " + i;
			arto.secondsToRemainVisible = 0.25f;
		}

		var originScr = SceneRoot.GetComponent<AROrigin> ();
		originScr.findMarkerMode = AROrigin.FindMode.AutoByTags;
		originScr.findMarkerMode = AROrigin.FindMode.AutoAll;
	}
}
