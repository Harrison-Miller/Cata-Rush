using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scenary : MonoBehaviour {


	public float anchorY;
	public float connectedAnchorY;
	public float breakForce;

	HingeJoint hj;
	Rigidbody rb;
	float dir;

	public bool goaway = false;

	public AudioClip breakSound;
	AudioSource audio;

	bool knockedOver;

	bool lighten;

	float startTime;


	// Use this for initialization
	void Start () {
		rb = gameObject.GetComponent<Rigidbody> ();
		hj = gameObject.AddComponent<HingeJoint> ();

		Vector3 a = transform.eulerAngles;
		if (a.x == 0 && a.y == 0 && a.z == 0) {
			hj.axis = Vector3.right;
		} else {
			hj.axis = Vector3.Cross(Vector3.right, a.normalized);
		}

		hj.autoConfigureConnectedAnchor = false;

		Vector3 anchor = transform.position;
		//anchor.y = 0.0f;
		anchor.z = 0.0f;
		anchor.x = 0.0f;
		anchor.y = anchorY; // -1.8f

		Vector3 connectedAnchor = transform.position;
		connectedAnchor.y = connectedAnchorY; //0.2f

		hj.anchor = anchor;
		hj.connectedAnchor = connectedAnchor;

		hj.breakForce = breakForce;
		hj.breakTorque = breakForce;

		rb = gameObject.GetComponent<Rigidbody> ();

		audio = gameObject.AddComponent<AudioSource> ();
		audio.maxDistance = 15.0f;
		audio.rolloffMode = AudioRolloffMode.Linear;
		audio.spatialBlend = 1.0f;

		knockedOver = false;

		lighten = false;

	}
	
	// Update is called once per frame
	void Update () {
		if (lighten) {
			MeshRenderer mr = gameObject.GetComponent<MeshRenderer> ();
			Color c = mr.material.color;

			float dt = Time.time - startTime;
			float r = dt / 4.0f;
			c.a = (255 - (r * 255.0f)) / 255.0f;
			mr.material.color = c;

			if (c.a <= 0.5f) {
				lighten = false;
			}
		}

		if (transform.position.y < -20.0f) {
			Destroy (gameObject);
		}

		if (!hj) {
			if (!knockedOver) {
				knockedOver = true;
				audio.PlayOneShot (breakSound);
				if (goaway) {
					Invoke ("Lighten", 4.0f);
					rb.velocity = new Vector3 ((Random.Range (0, 2) - 1) * 5, 2, 0);

				}
			}

			if (goaway) {
				dir = Mathf.Sign (transform.position.x);
				rb.AddForce (new Vector3 (0, 50 * dir, 0));

			}
		}
	}

	void Lighten() {
		lighten = true;
		gameObject.layer = LayerMask.NameToLayer ("GoneScenary");
		startTime = Time.time;

	}
}
