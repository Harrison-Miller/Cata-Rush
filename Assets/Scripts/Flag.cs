using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour {

	Game game;
	GameObject cata;

	// Use this for initialization
	void Start () {
		game = GameObject.Find ("Game").GetComponent<Game> ();
		cata = GameObject.Find ("Catapult");
		transform.eulerAngles = Vector3.up * Random.Range (0, 360);
	}
	
	// Update is called once per frame
	void Update () {
		//transform.eulerAngles = Vector3.up * game.wind;

		float d = (transform.position - cata.transform.position).magnitude;
		if (d < 5) {
			game.finishedLevel ();
		}
	}
}
