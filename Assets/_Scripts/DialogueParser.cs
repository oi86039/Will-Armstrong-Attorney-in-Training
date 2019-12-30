using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;

public class DialogueParser : MonoBehaviour {

	struct DialogueLine {
		public string name;
		public string content;
		public int pose;
		public string position;
		public string color;
		public int background;
		public int music;
		public string[] options;

		public DialogueLine(string Name, string Content, int Pose, string Position, string Color, int Background, int Music) {
			name = Name;
			content = Content;
			pose = Pose;
			position = Position;
			color = Color;
			background = Background;
			music = Music;
			options = new string[0];
		}
	}

	List<DialogueLine> lines;

	// Use this for initialization
	void Start () {
		string file = "Assets/Data/Dialogue";
		string sceneNum = EditorSceneManager.GetActiveScene().name;
		sceneNum = Regex.Replace (sceneNum, "[^0-9]", "");
		file += sceneNum;
		file += ".txt";

		lines = new List<DialogueLine>();

		LoadDialogue (file);
	}

	// Update is called once per frame
	void Update () {

	}

	void LoadDialogue(string filename) {
		string line;
		StreamReader r = new StreamReader (filename);

		using (r) {
			do {
				line = r.ReadLine();
				if (line != null) {
					string[] lineData = line.Split(';');
					if (lineData[0] == "Player") {
						DialogueLine lineEntry = new DialogueLine(lineData[0], "", 0, "","",0,0);
						lineEntry.options = new string[lineData.Length-1];
						for (int i = 1; i < lineData.Length; i++) {
							lineEntry.options[i-1] = lineData[i];
						}
						lines.Add(lineEntry);
					} else if (lineData[0] == "Event") {
						DialogueLine lineEntry = new DialogueLine(lineData[0], lineData[1], 0, "","G",int.Parse(lineData[2]),int.Parse(lineData[3]));
						lines.Add(lineEntry);
					} else if (lineData[0] == "Evidence") {
						DialogueLine lineEntry = new DialogueLine(lineData[0], lineData[1], 0, "M","Y",int.Parse(lineData[2]),int.Parse(lineData[3]));
						lines.Add(lineEntry);
					} else if (lineData[0] == "Health") {
						DialogueLine lineEntry = new DialogueLine(lineData[0], "You've lost 20 health...", 0, "M","R",int.Parse(lineData[1]),int.Parse(lineData[2]));
						lines.Add(lineEntry);
					} else {
						DialogueLine lineEntry = new DialogueLine(lineData[0], lineData[1], int.Parse(lineData[2]), lineData[3], lineData[4], int.Parse(lineData[5]), int.Parse(lineData[6]));
						lines.Add(lineEntry);
					}
				}
			}
			while (line != null);
			r.Close();
		}
	}

	public string GetPosition(int lineNumber) {
		if (lineNumber < lines.Count) {
			return lines[lineNumber].position;
		}
		return "";
	}

	public string GetName(int lineNumber) {
		if (lineNumber < lines.Count) {
			return lines[lineNumber].name;
		}
		return "";
	}

	public string GetContent(int lineNumber) {
		if (lineNumber < lines.Count) {
			return lines[lineNumber].content;
		}
		return "";
	}

	public int GetPose(int lineNumber) {
		if (lineNumber < lines.Count) {
			return lines[lineNumber].pose;
		}
		return 0;
	}

	public string[] GetOptions(int lineNumber) {
		if (lineNumber < lines.Count) {
			return lines[lineNumber].options;
		}
		return new string[0];
	}

	public string GetColor(int lineNumber){
		if (lineNumber < lines.Count) {
			return lines[lineNumber].color;
		}
		return "";
}
	public int GetBackground(int lineNumber){
		if (lineNumber < lines.Count) {
			return lines[lineNumber].background;
		}
		return 0;
	}

	public int GetMusic(int lineNumber){
		if (lineNumber < lines.Count) {
			return lines[lineNumber].music;
		}
		return 0;
	}
}
