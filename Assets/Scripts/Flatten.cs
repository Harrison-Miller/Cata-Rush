using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flatten : MonoBehaviour {

	bool flattened;

	AudioSource audio;
	public AudioClip flattenSound;

	// Use this for initialization
	void Start () {
		audio = gameObject.AddComponent<AudioSource> ();
		audio.maxDistance = 15.0f;
		audio.rolloffMode = AudioRolloffMode.Linear;
		audio.spatialBlend = 1.0f;
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.position.y < -20.0f) {
			Destroy (gameObject);
		}
	}

	void OnCollisionEnter(Collision col) {
		if (!flattened && col.gameObject.tag == "Catapult") {
			Vector3 scale = transform.localScale;
			scale.y = 0.01f;
			scale.x *= 1.5f;
			scale.z *= 1.5f;
			transform.localScale = scale;

			Vector3 rot = transform.eulerAngles;
			rot.x = 0.0f;
			rot.z = 0.0f;

			transform.eulerAngles = rot;

			Vector3 pos = transform.position;
			pos.y = 0.01f;
			transform.position = pos;

			Destroy (gameObject.GetComponent<Rigidbody> ());
			Destroy (gameObject.GetComponent<BoxCollider> ());

			flattened = true;

			audio.PlayOneShot (flattenSound);

		}
	}

	void OnTriggerEnter(Collider col) {
		if (!flattened && col.gameObject.tag == "Catapult") {
			Vector3 scale = transform.localScale;
			scale.y = 0.01f;
			scale.x *= 1.5f;
			scale.z *= 1.5f;
			transform.localScale = scale;

			Vector3 rot = transform.eulerAngles;
			rot.x = 0.0f;
			rot.z = 0.0f;

			transform.eulerAngles = rot;

			Vector3 pos = transform.position;
			pos.y = 0.01f;
			transform.position = pos;

			Destroy (gameObject.GetComponent<Rigidbody> ());
			Destroy (gameObject.GetComponent<BoxCollider> ());

			audio.PlayOneShot (flattenSound);

			flattened = true;
		}
	}
}
