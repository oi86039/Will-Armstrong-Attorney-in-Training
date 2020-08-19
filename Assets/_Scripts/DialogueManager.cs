using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{

    DialogueParser parser;

    public string dialogue, characterName;
    public int lineNum, pose, backgroundNum, musicNum, health;
    string position, color;
    string[] options;
    public bool playerTalking;
    List<Button> buttons = new List<Button>();

    public Text dialogueBox;
    public Text nameBox;
    public Text healthBox;
    public GameObject choiceBox;
    AudioSource advance;

    // Use this for initialization
    void Start()
    {
        advance = GetComponent<AudioSource>();
        dialogue = SceneManager.GetActiveScene().name + ": Click to continue...";
        characterName = "";
        pose = 0;
        position = "L";
        color = "";
        backgroundNum = 0;
        musicNum = 0;
        health = 100;
        playerTalking = false;
        parser = GameObject.Find("DialogueParser").GetComponent<DialogueParser>();
        lineNum = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && playerTalking == false)
        {
            ChangeHealth();
            ShowDialogue();
            advance.Play();
            lineNum++;
        }

        UpdateUI();
    }

    public void ShowDialogue()
    {
        ResetImages();
        ParseLine();
    }

    void UpdateUI()
    {
        if (!playerTalking)
        {
            ClearButtons();
        }
        ChangeColor();
        dialogueBox.text = dialogue;
        nameBox.text = characterName;

    }

    void ChangeColor()
    {
        if (color == "B")
            dialogueBox.color = new Color(0.16f, 0.56f, 0.96f, 1.0f); //41,143,244,255
        else if (color == "R")
            dialogueBox.color = new Color(0.95f, 0.20f, 0.20f, 1.0f); //242,52,52,255
        else if (color == "G")
            dialogueBox.color = new Color(0.36f, 1.0f, 0.20f, 1.0f); //91,255,51,255
        else if (color == "Y")
            dialogueBox.color = new Color(1.0f, 0.87f, 0.35f, 1.0f); //255,223,90,255
        else if (color == "P")
            dialogueBox.color = new Color(0.64f, 0.06f, 1.0f, 1.0f); //162,15,255,255
        else
            dialogueBox.color = new Color(1.0f, 1.0f, 1.0f, 1.0f); //225,225,225,255
    }

    void ClearButtons()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            print("Clearing buttons");
            Button b = buttons[i];
            buttons.Remove(b);
            Destroy(b.gameObject);
        }
    }

    void ParseLine()
    {
        if (parser.GetName(lineNum) != "Player")
        {
            playerTalking = false;
            characterName = parser.GetName(lineNum);
            dialogue = parser.GetContent(lineNum);
            pose = parser.GetPose(lineNum);
            position = parser.GetPosition(lineNum);
            color = parser.GetColor(lineNum);
            backgroundNum = parser.GetBackground(lineNum);
            musicNum = parser.GetMusic(lineNum);
            DisplayImages();
            ChangeMusic();
        }
        else
        {
            playerTalking = true;
            options = parser.GetOptions(lineNum);
            DisplayImages();
            CreateButtons();
        }
    }

    void CreateButtons()
    {
        for (int i = 0; i < options.Length; i++)
        {
            GameObject button = (GameObject)Instantiate(choiceBox);
            Button b = button.GetComponent<Button>();
            ChoiceButton cb = button.GetComponent<ChoiceButton>();
            cb.SetText(options[i].Split(':')[0]);
            cb.option = options[i].Split(':')[1];
            cb.box = this;
            b.transform.SetParent(this.transform);
            b.transform.localPosition = new Vector3(-265 + (i * 175), -40);
            b.transform.localScale = new Vector3(1, 1, 1);
            buttons.Add(b);
        }
    }

    void ResetImages()
    {
        if (characterName != "")
        {
            GameObject background = GameObject.Find("Background");
            Image backSprite = background.GetComponent<Image>();
            backSprite.sprite = null;

            GameObject character = GameObject.Find(characterName);
            SpriteRenderer currSprite = character.GetComponent<SpriteRenderer>();
            currSprite.sprite = null;
        }
    }

    void DisplayImages()
    {
        if (characterName != "")
        {
            GameObject background = GameObject.Find("Background");
            Image backSprite = background.GetComponent<Image>();
            backSprite.sprite = background.GetComponent<Background>().backgroundList[backgroundNum];

            GameObject character = GameObject.Find(characterName);
            SetSpritePositions(character);
            SpriteRenderer currSprite = character.GetComponent<SpriteRenderer>();
            currSprite.sprite = character.GetComponent<Character>().characterPoses[pose];
        }
    }

    void ChangeMusic()
    {
        if (musicNum == -1)
        {
            GameObject source = GameObject.Find("Main Camera");
            AudioSource player = source.GetComponent<AudioSource>();
            player.Stop();
        }
        else if (musicNum != 0)
        {
            GameObject source = GameObject.Find("Main Camera");
            AudioSource player = source.GetComponent<AudioSource>();
            player.Stop();
            player.clip = source.GetComponent<Music>().musicList[musicNum];
            player.Play();
        }
    }

    void ChangeHealth()
    {
        if (characterName == "Health")
        {
            health -= 20;
        }
        healthBox.text = "Health: " + health;
        CheckHealth();
    }

    void CheckHealth()
    {
        if (health <= 20)
        {
            healthBox.color = new Color(1.0f, 0.20f, 0.20f, 1.0f);
        }
        if (health == 0)
        {
            lineNum = 918;
            ShowDialogue();
            lineNum++;
            health = -1;
        }
    }

    void SetSpritePositions(GameObject spriteObj)
    {
        if (position == "M")
        {
            spriteObj.transform.position = new Vector3(0, 0);
        }
        else if (position == "L")
        {
            spriteObj.transform.position = new Vector3(-8, 0);
        }
        else if (position == "R")
        {
            spriteObj.transform.position = new Vector3(8, 0);
        }
        spriteObj.transform.position = new Vector3(spriteObj.transform.position.x, spriteObj.transform.position.y, 0);
    }

}
