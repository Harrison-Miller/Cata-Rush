using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour {

	float time;
	float start;

	void Start () {
		GameObject debris = GameObject.Find ("Debris");
		gameObject.transform.parent = debris.transform;
		time = 8.0f + Random.Range (0, 3);
		Destroy (gameObject,time);
		start = Time.time;
	}

	void Update () {
		if (transform.position.y < -20.0f) {
			Destroy (gameObject);
		}
			
		MeshRenderer mr = gameObject.GetComponent<MeshRenderer> ();
		Color c = mr.material.color;

		float dt = Time.time - start;

		if (dt > 3) {
			float r = (dt - 3) / (time - 3);
			c.a = (255 - (r * 255.0f)) / 255.0f;

			mr.material.color = c;

		}
	}
}
