using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CourtRecord : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void switchActive(GameObject panel){
		if (panel.activeSelf) {
			panel.SetActive (false);
		} else {
			panel.SetActive (true);
		}
	}
}
