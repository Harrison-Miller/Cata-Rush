using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatapultController : MonoBehaviour {

	public float movementForce;

	WheelCollider flCol;
	WheelCollider frCol;
	WheelCollider blCol;
	WheelCollider brCol;

	HingeJoint arm;

	public float throwForce;
	public float pickupRadius;
	public float topSpeed;

	GameObject attachPoint;
	public GameObject projectile;
	int braking;

	[HideInInspector] public bool lockControls;

	Rigidbody body;

	public ProgressBar pb;
	float power;
	bool powerDir;
	bool throwing;
	float throwTime;
	bool pullStarted;

	public Material ropeMaterial;
	LineRenderer lr;

	public AudioClip pullSFX;
	public AudioClip throwSFX;
	AudioSource source;

	void Start () {
		source = gameObject.GetComponent<AudioSource> ();

		body = gameObject.GetComponent<Rigidbody> ();

		flCol = transform.Find ("Wheel Colliders/Front Left").gameObject.GetComponent<WheelCollider> ();
		frCol = transform.Find ("Wheel Colliders/Front Right").gameObject.GetComponent<WheelCollider> ();
		blCol = transform.Find ("Wheel Colliders/Back Left").gameObject.GetComponent<WheelCollider> ();
		brCol = transform.Find ("Wheel Colliders/Back Right").gameObject.GetComponent<WheelCollider> ();

		arm = transform.Find ("Arm").gameObject.GetComponent<HingeJoint> ();
		attachPoint = transform.Find ("Arm/Attach Point").gameObject;

		JointMotor motor = arm.motor;
		motor.targetVelocity = 10000.0f;
		motor.force = throwForce;
		arm.motor = motor;

		powerDir = true;
		pullStarted = false;


		lr = gameObject.AddComponent<LineRenderer> ();
		//lr.useWorldSpace = false;
		lr.startWidth = 0.25f;
		lr.endWidth = 0.25f;
		lr.material = ropeMaterial;
		lr.receiveShadows = false;
		//lr.transform.parent = transform;
		//lr.SetPosition (0, new Vector3(0, 0, -2.5f));

		lockControls = false;

	}

	public void finishLevel() {
		flCol.motorTorque = movementForce;
		frCol.motorTorque = movementForce;
		blCol.motorTorque = movementForce;
		brCol.motorTorque = movementForce;
		flCol.brakeTorque = 0.0f;
		frCol.brakeTorque = 0.0f;
		blCol.brakeTorque = 0.0f;
		brCol.brakeTorque = 0.0f;
		lockControls = true;
		pb.enabled = false;

	}

	void Update () {

		Vector3 basePos = transform.position + new Vector3(0, -0.5f, -2.5f);
		lr.SetPosition (0, basePos);

		Vector3 attachPos = attachPoint.transform.position + new Vector3 (0, -0.5f, -0.5f);
		lr.SetPosition (1, attachPos);

		float ropeLen = (attachPos - basePos).magnitude;
		lr.material.SetTextureScale ("_MainTex", new Vector2 (ropeLen, 1));

		float mag = body.velocity.z;
		float dir = Input.GetAxis ("Horizontal");

		if (lockControls) {
			dir = 0.0f;
		}
		float v =  dir * movementForce;


		if (Mathf.Sign (mag) != Mathf.Sign (v)) {
			v = v * 4.0f;
		}

		if (Mathf.Abs (mag) > 2.0f && !source.isPlaying) {
			source.Play ();
		} else if(Mathf.Abs(mag) <= 3.0f) {
			source.Pause ();
		}

		if (Mathf.Abs (mag) < topSpeed || Mathf.Sign (mag) != Mathf.Sign (v)) {
			flCol.motorTorque = v;
			frCol.motorTorque = v;
			blCol.motorTorque = v;
			brCol.motorTorque = v;

		} else {
			//print ("to fast: " + mag);
			flCol.motorTorque = 0;
			frCol.motorTorque = 0;
			blCol.motorTorque = 0;
			brCol.motorTorque = 0;
		}

		if (Mathf.Abs (mag) > 0.5f && dir == 0.0f) {
			float t = -movementForce * 0.5f * mag;
			flCol.motorTorque = t;
			frCol.motorTorque = t;
			blCol.motorTorque = t;
			brCol.motorTorque = t;
		} else if (Mathf.Abs (mag) <= 0.5f && dir == 0.0f) {
			flCol.brakeTorque = 0.1f;
			frCol.brakeTorque = 0.1f;
			blCol.brakeTorque = 0.1f;
			brCol.brakeTorque = 0.1f;
		} else {
			flCol.brakeTorque = 0.0f;
			frCol.brakeTorque = 0.0f;
			blCol.brakeTorque = 0.0f;
			brCol.brakeTorque = 0.0f;
		}

		if (projectile) {
			projectile.transform.position = attachPoint.transform.position;
			//projectile.transform.rotation = Quaternion.identity;
		} 

		if (Input.GetKey ("space")) {
			if (!pullStarted) {
				AudioSource.PlayClipAtPoint (pullSFX, transform.position, 0.5f);
				pullStarted = true;
			}
			if (powerDir) {
				power += 0.01f;
				if (power > 1.0f) {
					powerDir = false;
				}
			} else {
				power -= 0.01f;
				if (power < 0.0f) {
					powerDir = true;
				}
			}
				
			if (pb) {
				pb.percent = power;
			}
				
		} else if (Input.GetKeyUp ("space")) {
			pullStarted = false;
			
			AudioSource.PlayClipAtPoint (throwSFX, transform.position);

			arm.useMotor = true;

			if (projectile) {
				projectile.GetComponent<Projectile> ().thrown = true;

			}

			projectile = null;

			JointMotor motor = arm.motor;
			motor.force = throwForce * power;
			arm.motor = motor;

			powerDir = true;
			throwing = true;
			throwTime = Time.time;


			if (pb) {
				pb.lastSpot = power;
			}

		} else if (throwing && arm.velocity < 1.0f) {
			float dt = Time.time - throwTime;
			if (dt > 1.0f) {
				power = 0.0f;
				if (pb) {
					pb.percent = power;
				}

				arm.useMotor = false;
				throwing = false;
			}
		} else if (arm.angle <= 0 && !projectile && !throwing) {
			GameObject[] objs = GameObject.FindGameObjectsWithTag ("Projectile");
			Vector3 position = transform.position;

			GameObject pickMe = null;
			float dist = 10000.0f;
			foreach(GameObject obj in objs) {
				float d = (obj.transform.position - position).magnitude;
				bool stuck = obj.GetComponent<Projectile> ().stuck;
				if (!stuck && d < pickupRadius && d < dist) {
					dist = d;
					pickMe = obj;
				}
			}

			if (pickMe) {
				projectile = pickMe;
				//print ("pick object: " + projectile.ToString ());
			}
		}
	}
		
}
