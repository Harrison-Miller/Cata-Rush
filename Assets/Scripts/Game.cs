using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour {

	Text title;
	bool finishedTitle;

	float titleStart;
	public float titleLength = 4.0f;

	public AudioClip win;

	Text next;
	Text repeat;

	bool won;

	Text time;
	float startTime;

	public float parTime;

	Text scoreText;

	Text retry;

	//public float wind;

	// Use this for initialization
	void Start () {
		title = GameObject.Find ("Canvas/Title").GetComponent<Text>();
		titleStart = Time.time;
		//wind = Random.Range (0, 360);
		finishedTitle = false;

		next = GameObject.Find ("Canvas/Next").GetComponent<Text> ();
		next.enabled = false;

		repeat = GameObject.Find ("Canvas/Repeat").GetComponent<Text> ();
		repeat.enabled = false;
		won = false;

		retry = GameObject.Find ("Canvas/Retry").GetComponent<Text> ();
		retry.enabled = false;

		time = GameObject.Find ("Canvas/Time").GetComponent<Text> ();
		time.enabled = false;

		scoreText = GameObject.Find ("Canvas/Score").GetComponent<Text> ();
		scoreText.enabled = false;
	}

	void updateTitle() {
		if (!finishedTitle && title) {
			float dt = Time.time - titleStart;
			float solidDuration = 2.0f;
			if (dt > solidDuration) {
				float r = (dt - solidDuration) / (titleLength - solidDuration);
				Color c = title.color;
				c.a = (255 - (r * 255)) / 255;
				title.color = c;

				if (c.a <= 0.0f) {
					finishedTitle = true;
					time.enabled = true;
					retry.enabled = true;
				}
			}
		} else if(!won) {
			float dt = Time.time - startTime;
			float m = dt / 60.0f;
			float s = dt - ((int)m) * 60.0f;

			string mstr = ((int)m).ToString ();
			if (mstr.Length == 1) {
				mstr = "0" + mstr;
			}

			string sstr = ((int)s).ToString ();
			if (sstr.Length == 1) {
				sstr = "0" + sstr;
			}

			time.text = mstr + ":" + sstr;
		}
	}

	public void finishedLevel() {
		if (won) {
			return;
		}

		time.enabled = false;
		retry.enabled = false;

		int score = 1000;

		float dt = Time.time - startTime;
		if (dt <= parTime) {
			score += 1000;
		}

		score += (int)((parTime - dt) * 50);

		int parm = (int)(parTime / 60);
		int pars = (int)(parTime - parm * 60);
		string mstr = ((int)parm).ToString ();
		if (mstr.Length == 1) {
			mstr = "0" + mstr;
		}

		string sstr = ((int)pars).ToString ();
		if (sstr.Length == 1) {
			sstr = "0" + sstr;
		}

		if (score < 100) {
			score = 100;
		} 

		scoreText.text = "Time: " + time.text + "\nPar:  " + mstr + ":" + sstr + "\n_________\n" + "Score: " + score;
		scoreText.enabled = true;

		Color c = title.color;
		c.a = 1.0f;
		title.color = c;
		title.enabled = true;

		won = true;
		GameObject cata = GameObject.Find ("Catapult");
		cata.GetComponent<CatapultController> ().finishLevel ();

		GameObject explObj = new GameObject ();
		AudioSource explAudio = explObj.AddComponent<AudioSource> ();
		explAudio.PlayOneShot(win);
		Destroy (explObj, 3.0f);

		//.. to do ui stuff

		next.enabled = true;
		repeat.enabled = true;

	}
	
	// Update is called once per frame
	void Update () {
		updateTitle ();
		//wind += Random.Range (-3, 3);
	}
}
