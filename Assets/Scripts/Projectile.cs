using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

	public GameObject tracerPrefab;
	GameObject explosionSystem;

	int tick;

	bool collided;

	public bool explosion;
	public float bombRadius;
	public float power;

	public bool getStuck;

	//GameObject trajectory;

	public bool thrown;
	public bool stuck;

	public GameObject brokenPrefab;
	public int health = 1;

	public AudioClip thud;
	public AudioClip thud2;
	public AudioClip explodeSFX;
	AudioSource audio;

	// Use this for initialization
	void Start () {
		thrown = false;
		//trajectory = GameObject.Find ("Trajectory");
		explosionSystem = GameObject.Find ("Explosion System");
		stuck = false;

		audio = gameObject.AddComponent<AudioSource> ();
		audio.spatialBlend = 1.0f;
		audio.rolloffMode = AudioRolloffMode.Linear;
		audio.maxDistance = 50.0f;

	}
	
	// Update is called once per frame
	void Update () {
		Vector3 pos = transform.position;
		pos.x = Mathf.Clamp (pos.x, -6f, 6f);
		transform.position = pos;


		if (pos.y < -20.0f) {
			Destroy (gameObject);
		}

		if(stuck) {
			FixedJoint fj = gameObject.GetComponent<FixedJoint> ();
			if (!fj.connectedBody) {
				stuck = false;
				Destroy (fj);
			}
		}

		/*tick++;
		if (thrown) {
			if (tick % 10 == 0) {
				GameObject obj = Instantiate (tracerPrefab, transform.position, Quaternion.identity);
				obj.transform.parent = trajectory.transform;
			}
		}*/
	}

	void OnCollisionEnter(Collision other) {
		if (thrown && other.relativeVelocity.magnitude > 3.0f) {
			if (thud && Random.Range (0, 2) == 0) {
				audio.PlayOneShot (thud);
			} else if(thud2) {
				audio.PlayOneShot (thud2);
			}
		}

		if (other.gameObject.tag == "Floor" || other.gameObject.tag == "Block" || other.gameObject.tag == "Projectile") {
			if (thrown && explosion && other.relativeVelocity.magnitude > 6.0f) {

				GameObject explObj = new GameObject ();
				AudioSource explAudio = explObj.AddComponent<AudioSource> ();
				explAudio.transform.position = transform.position;
				explAudio.rolloffMode = AudioRolloffMode.Linear;
				explAudio.spatialBlend = 1.0f;
				explAudio.maxDistance = 20.0f;
				explAudio.PlayOneShot(explodeSFX);
				Destroy (explObj, 3.0f);

				GameObject[] objs = GameObject.FindGameObjectsWithTag ("Block");
				Vector3 position = transform.position;

				foreach (GameObject obj in objs) {
					float d = (obj.transform.position - position).magnitude;
					if (d < bombRadius * 0.5) {
						obj.GetComponent<Break> ().breakBlock ();
					} else if (d <= bombRadius) {
						Rigidbody rb = obj.GetComponent<Rigidbody> ();
						rb.AddExplosionForce (power, position, bombRadius, 3.0f);
					}
				}

				objs = GameObject.FindGameObjectsWithTag ("Animal");

				foreach (GameObject obj in objs) {
					float d = (obj.transform.position - position).magnitude;
					if (d < bombRadius * 0.5) {
						obj.GetComponent<Animal> ().killAnimal ();
					} else if (d <= bombRadius) {
						Rigidbody rb = obj.GetComponent<Rigidbody> ();
						rb.AddExplosionForce (power, position, bombRadius, 3.0f);
					}
				}

				objs = GameObject.FindGameObjectsWithTag ("Scenary");

				foreach (GameObject obj in objs) {
					float d = (obj.transform.position - position).magnitude;
					if (d <= bombRadius) {
						Rigidbody rb = obj.GetComponent<Rigidbody> ();
						rb.AddExplosionForce (power * 10, position, bombRadius, 3.0f);
					}
				}

				objs = GameObject.FindGameObjectsWithTag ("Projectile");

				foreach (GameObject obj in objs) {
					float d = (obj.transform.position - position).magnitude;
					if (d <= bombRadius) {
						Rigidbody rb = obj.GetComponent<Rigidbody> ();
						rb.AddExplosionForce (power, position, bombRadius, 3.0f);
					}
				}

				if (explosionSystem) {
					ParticleSystem s = explosionSystem.GetComponent<ParticleSystem> ();
					ParticleSystem.EmitParams e = new ParticleSystem.EmitParams ();
					e.position = transform.position;
					e.applyShapeToPosition = true;
					s.Emit (e, 200);
					s.Play ();
				}
				Destroy (gameObject);
			}

			if (other.gameObject.tag != "Projectile" && getStuck && other.relativeVelocity.magnitude > 6.0f) {
				if (!gameObject.GetComponent<FixedJoint> ()) {
					if (getStuck && other.gameObject.tag == "Block") {
						FixedJoint fj = gameObject.AddComponent<FixedJoint> ();
						fj.connectedBody = other.gameObject.GetComponent<Rigidbody> ();
						gameObject.GetComponent<Rigidbody> ().velocity = Vector3.zero;

						stuck = true;
					} else if (getStuck && other.gameObject.tag == "Floor") {
						Rigidbody rb = gameObject.GetComponent<Rigidbody> ();
						Vector3 vel = rb.velocity;
						vel.z *= 0.77f;
						rb.velocity = vel;
					}
				}
			}

			if (stuck && ((other.gameObject.tag == "Projectile" && other.relativeVelocity.magnitude > 5.0f) || (other.gameObject.tag == "Catapult" && other.relativeVelocity.magnitude >= 7.5f))) {
				FixedJoint fj = gameObject.GetComponent<FixedJoint> ();
				fj.connectedBody.GetComponent<Break> ().breakBlock ();
			}

			if (brokenPrefab && other.relativeVelocity.magnitude > 8.0f) {
				health--;
				if (health <= 0) {
					Instantiate (brokenPrefab, transform.position, transform.rotation);
					Destroy (gameObject);
				}
			}

			thrown = false;
		}
	}
}
