using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Break : MonoBehaviour {

	public float staticForce = 1300.0f;
	public float dynamicForce = 1300.0f;
	public GameObject brokenPrefab;
	public Mesh hitMesh;
	bool hit;

	public AudioClip breakSFX;

	// Use this for initialization
	void Start () {
		Rigidbody rb = gameObject.GetComponent<Rigidbody> ();
		rb.mass = rb.mass * transform.localScale.x;
		hit = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.position.y < -20.0f) {
			Destroy (gameObject);
		}
	}

	public void breakBlock() {
		if (!hit && hitMesh) {
			MeshFilter mf = gameObject.GetComponent<MeshFilter> ();
			mf.mesh = hitMesh;
			hit = true;

			Camera.main.GetComponent<AudioSource> ().PlayOneShot (breakSFX);
		}
		else if (brokenPrefab) {
			chopUp (transform.localScale, transform);
			//GameObject broken = Instantiate (brokenPrefab, transform.position, transform.rotation);
			//broken.transform.localScale = transform.localScale;
			Destroy (gameObject);

			if (breakSFX) {
				Camera.main.GetComponent<AudioSource> ().PlayOneShot (breakSFX);
			}

		}
	}

	public void chopUp(Vector3 size, Transform t) {
		int chunkSize = 4;

		if (size.x > chunkSize) {
			GameObject a = new GameObject ();
			a.transform.position = t.position;
			a.transform.rotation = t.rotation;
			a.transform.Translate (new Vector3 (-size.x / 2, 0, 0));

			chopUp (new Vector3 (size.x / 2, size.y, size.z), a.transform);

			a.transform.position = t.position;
			a.transform.rotation = t.rotation;
			a.transform.Translate (new Vector3 (size.x / 2, 0, 0));
			
			chopUp (new Vector3 (size.x / 2, size.y, size.z), a.transform);
			Destroy (a);
		} else if (size.y > chunkSize) {
			GameObject a = new GameObject ();
			a.transform.position = t.position;
			a.transform.rotation = t.rotation;
			a.transform.Translate (new Vector3 (0, -size.y/2, 0));

			chopUp (new Vector3 (size.x, size.y / 2, size.z), a.transform);

			a.transform.position = t.position;
			a.transform.rotation = t.rotation;
			a.transform.Translate (new Vector3 (0, size.y/2, 0));

			chopUp (new Vector3 (size.x, size.y / 2, size.z), a.transform);
			Destroy (a);
		} else if (size.z > chunkSize) {
			GameObject a = new GameObject ();
			a.transform.position = t.position;
			a.transform.rotation = t.rotation;
			a.transform.Translate (new Vector3 (0, 0, -size.z/2));

			chopUp (new Vector3 (size.x, size.y, size.z / 2), a.transform);

			a.transform.position = t.position;
			a.transform.rotation = t.rotation;
			a.transform.Translate (new Vector3 (0, 0, size.z/2));

			chopUp (new Vector3 (size.x, size.y, size.z / 2), a.transform);
			Destroy (a);
		} else {
			GameObject broken = Instantiate (brokenPrefab, t.position, t.rotation);
			broken.transform.localScale = size;
		}
	}

	void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.tag != "Catapult" && col.gameObject.tag != "Block") {
			float breakForce = dynamicForce;
			float mass;
			if (col.rigidbody) {
				mass = col.rigidbody.mass;
			} else {
				mass = 10;
				breakForce = staticForce;
			}

			float force = col.relativeVelocity.magnitude * mass;
			//print ("force = " + force.ToString ());
			if (force > breakForce) {
				breakBlock ();
			}

		}
			
	}
}
