using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LevelButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {

	Text text;
	public string scene;

	// Use this for initialization
	void Start () {
		text = gameObject.GetComponent<Text> ();
		EventTrigger trigger = GetComponentInParent<EventTrigger> ();
		EventTrigger.Entry entry = new EventTrigger.Entry ();

	}

	public void OnPointerEnter(PointerEventData eventData) {
		text.color = Color.yellow;
	}

	public void OnPointerExit(PointerEventData eventData) {
		text.color = Color.black;
	}

	public void OnPointerClick(PointerEventData eventData) {
		if (scene != "") {
			Application.LoadLevel (scene);

		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
