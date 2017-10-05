using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBar : MonoBehaviour {

	Texture2D greenTex;
	Texture2D blackTex;

	public Vector2 pos;
	public float length;
	public float height;
	public float percent;

	Texture2D redTex;

	public float lastSpot;

	// Use this for initialization
	void Start () {
		greenTex = new Texture2D (1, 1);
		greenTex.SetPixel (0, 0, new Color (0, 255, 0));
		greenTex.Apply ();

		blackTex = new Texture2D (1, 1);
		blackTex.SetPixel (0, 0, new Color (0, 0, 0));
		blackTex.Apply ();

		redTex = new Texture2D (1, 1);
		redTex.SetPixel (0, 0, new Color (255, 0, 0));
		redTex.Apply ();
	}

	// Update is called once per frame
	void Update () {
		
	}

	void OnGUI() {

		float x = pos.x * Screen.width;
		float y = pos.y * Screen.height;
		GUI.BeginGroup (new Rect (x - length/2, y, length, height));
		GUI.DrawTexture (new Rect (0, 0, length, height), blackTex);

		GUI.DrawTexture (new Rect (0, 0, length * percent, height), greenTex);

		GUI.DrawTexture (new Rect (lastSpot * length, -2, 2, height + 4), redTex);
		GUI.EndGroup ();
	}
}







