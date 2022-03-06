using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RoomDialogueCanvasHandler : MonoBehaviour
{
    private bool dialogueStarted = false;
    public DialogueHandler dHandler;
    public Dialogue dialogue;
    public List<string> messages;
    public Text namebox;
    public Text messagebox;
    private int index = 0;
    public string name;
    // Start is called before the first frame update
    void Start()
    {
        dHandler = new DialogueHandler();
        messages = new List<string>();
        messages.Add("안녕");
        messages.Add("난 고세구야");
        name = "고세구";
        dialogue = new Dialogue(name, messages);
        Debug.Log(messages);
        dHandler = new DialogueHandler();
        dHandler.startDialogue(dialogue);
    }

    // Update is called once per frame
    void Update()
    {
        namebox = GameObject.Find("Name").GetComponent<Text>();
        messagebox= GameObject.Find("Dialogue").GetComponent<Text>();
        if(!dialogueStarted){
            if(Input.anyKeyDown){
                Debug.Log("onpresskey");
                Debug.Log(dialogue);
                dHandler.startDialogue(dialogue);
                dialogueStarted = true;
            }
        } else {
            if(Input.GetKeyDown("space")){
                try{
                    Debug.Log("got here");
                    string message = messages[index];
                    Debug.Log(namebox);
                    Debug.Log(messagebox);
                    namebox.text = name;
                    messagebox.text = message;
                    index++;
                } catch (Exception e){
                    // end dialogue 
                }
            }
        }
    }
}
