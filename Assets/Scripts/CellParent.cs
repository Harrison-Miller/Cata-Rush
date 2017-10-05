using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellParent : MonoBehaviour {

	public bool explode = true;

	// Use this for initialization
	void Start () {

		float power = 1500*2;

		if (!explode) {
			Vector3 pos = transform.position;
			GameObject[] blocks = GameObject.FindGameObjectsWithTag ("Block");
			foreach (GameObject block in blocks) {
				Vector3 p2 = block.transform.position;
				float d = (p2 - pos).magnitude;
				if (d < 5) {
					block.GetComponent<Rigidbody> ().AddExplosionForce (power, pos, 5, 3);
				}
			}

		}

		transform.eulerAngles = new Vector3(Random.Range(0, 2)*180 + Random.Range(-700, 700)/100.0f,
			Random.Range(0, 2)*180  + Random.Range(-700, 700)/100.0f,
			Random.Range(0, 2)*180 +  + Random.Range(-700, 700)/100.0f);
		transform.localScale *= 0.75f;

		transform.position += new Vector3 (Random.value, Random.value, Random.value) * Random.Range (-50, 50) / 100.0f;


		foreach (Transform child in transform) {
			MeshCollider mc = child.gameObject.AddComponent<MeshCollider> ();
			mc.convex = true;
			child.gameObject.layer = LayerMask.NameToLayer("Cell");
			Rigidbody rb = child.gameObject.AddComponent<Rigidbody> ();
			child.gameObject.AddComponent<Cell> ();

			/*child.transform.eulerAngles = new Vector3 (Random.Range (-700, 700)/100.0f,
				Random.Range (-700, 700)/100.0f, Random.Range (-700, 700)/100.0f);*/

			Vector3 vec = new Vector3 (Random.value, Random.value, Random.value).normalized;
			vec = vec * Random.Range(-500, 500)/100.0f;

			rb.AddForce (vec, ForceMode.Impulse);

		}
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.childCount == 0) {
			Destroy (gameObject);
		}
	}
}
