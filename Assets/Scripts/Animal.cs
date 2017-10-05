using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour {

	public GameObject bloodPrefab;

	Rigidbody rb;
	bool grounded;

	public AudioClip sound1;
	public AudioClip sound2;
	public AudioClip deathSound;
	public float audioChance;

	AudioSource audio;

	// Use this for initialization
	void Start () {
		rb = gameObject.GetComponent<Rigidbody> ();
		transform.Rotate (Vector3.up * Random.value * 360.0f);

		audio = gameObject.AddComponent<AudioSource> ();
		audio.spatialBlend = 1.0f;
		audio.rolloffMode = AudioRolloffMode.Linear;
		audio.maxDistance = 25.0f;

	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (transform.position.y < -20.0f) {
			Destroy (gameObject);
		}

		if (grounded && Random.value < 0.05f) {
			grounded = false;
			//print ("jump" + Time.time);
			rb.AddForce (Vector3.up * 2.0f, ForceMode.Impulse);

			transform.Rotate (Vector3.up * Random.Range (-30.0f, 30.0f));

			if (Random.value < audioChance) {
				if (sound1 && Random.Range (0, 2) == 0) {
					audio.PlayOneShot (sound1);
				} else if (sound2) {
					audio.PlayOneShot (sound2);
				}

			}

			if (Random.value < 0.5f) {
				rb.AddForce (-transform.forward * 20.0f);
			}
		}
	}

	public void killAnimal() {
		Destroy (gameObject);
		if (bloodPrefab) {
			GameObject explObj = new GameObject ();
			AudioSource explAudio = explObj.AddComponent<AudioSource> ();
			explAudio.transform.position = transform.position;
			explAudio.rolloffMode = AudioRolloffMode.Linear;
			explAudio.spatialBlend = 1.0f;
			explAudio.maxDistance = 15.0f;
			explAudio.PlayOneShot(deathSound);
			Destroy (explObj, 3.0f);

			Vector3 pos = transform.position;
			pos.y = 0.01f;

			GameObject blood = Instantiate (bloodPrefab, pos, Quaternion.identity);
			GameObject scenary = GameObject.Find ("Scenary");
			blood.transform.parent = scenary.transform;


			blood.transform.eulerAngles = Vector3.up * Random.value * 360.0f;
			blood.transform.localScale = new Vector3 (1, 1, 1) * Random.Range (0.1f, 0.3f);
		}
	}

	void OnCollisionEnter(Collision col) {
		if (col.gameObject.tag == "Floor" || col.gameObject.tag == "Block") {
			grounded = true;
			if (col.relativeVelocity.magnitude > 5.0f) {
				killAnimal ();
			}
		} else if (col.gameObject.tag == "Catapult" || col.gameObject.tag == "Projectile") {
			if (col.relativeVelocity.magnitude > 3.0f) {
				killAnimal ();

			}
		}
	}
}
