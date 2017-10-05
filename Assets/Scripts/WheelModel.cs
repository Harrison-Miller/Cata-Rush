using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelModel : MonoBehaviour {

	public WheelCollider wheelCollider;
	public Quaternion relative;

	void Start () {
		relative = transform.localRotation;
	}

	void Update () {
		Vector3 pos;
		Quaternion quat;

		wheelCollider.GetWorldPose (out pos, out quat);

		transform.SetPositionAndRotation (pos, quat * relative);
	}
}
