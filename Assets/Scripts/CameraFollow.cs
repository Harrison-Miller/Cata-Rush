using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	public GameObject catapult;
	Vector3 offset;

	/*Vector3 startPos;

	bool cutscene;
	float startTime;
	float length = 3.0f;*/


	// Use this for initialization
	void Start () {
		offset = transform.localPosition;

		/*GameObject[] flags = GameObject.FindGameObjectsWithTag ("Flag");
		foreach(GameObject flag in flags) {
			startPos += flag.transform.position;
		}

		startPos /= flags.Length;

		Vector3 position = offset;
		position.z += startPos.z;
		gameObject.transform.position = position;

		print (position);

		catapult.GetComponent<CatapultController> ().lockControls = true;
		cutscene = true;

		startTime = Time.time;*/

	}
	
	// Update is called once per frame
	void Update () {
		/*if (cutscene) {
			print (Time.time);
			print (startTime);
			float dt = Time.time - startTime;
			float r = (dt-1) / (length-1);
			float z = Mathf.Lerp (startPos.z, offset.z + catapult.transform.transform.position.z, r);

			Vector3 position = offset;
			position.z += z;
			gameObject.transform.position = position;


			if (dt >= 3.0f) {
				print ("cutscene over");
				cutscene = false;
				catapult.GetComponent<CatapultController> ().lockControls = false;
			}

		}
		else */if (catapult) {
			Vector3 position = offset;
			position.z += catapult.transform.position.z;
			gameObject.transform.position = position;
		}
	}
}
