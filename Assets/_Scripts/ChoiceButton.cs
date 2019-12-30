using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChoiceButton : MonoBehaviour {

	public string option;
	public DialogueManager box;
	GameObject manager;
	AudioSource click;


	// Use this for initialization
	void Start () {
		manager = GameObject.Find ("Dialogue Panel");
		click = manager.GetComponent<AudioSource>();
	}

	// Update is called once per frame
	void Update () {

	}

	public void PlaySound (){
		click.Play ();
	}

	public void SetText(string newText) {
		this.GetComponentInChildren<Text> ().text = newText;
	}

	public void SetOption(string newOption) {
		this.option = newOption;
	}

	public void ParseOption() {
		string command = option.Split (',') [0];
		string commandModifier = option.Split (',') [1];
		box.playerTalking = false;
		if (command == "line") {
			box.lineNum = int.Parse(commandModifier)-1;
			box.ShowDialogue();
			box.lineNum++;
		} else if (command == "scene") {
			SceneManager.LoadScene("Scene" + commandModifier);
		}
		else if (command == "exit") {
			Application.Quit();
		}
	}
}
