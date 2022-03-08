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
    // dialouges: List of Dialogue / FIFO
    public Queue<Dialogue> dialogues;
    // dialogue: <등장인물, 메시지> 
    public Dialogue dialogue;
    public string message;
    public Text namebox;
    public Text messagebox;
    public string name;
    // Start is called before the first frame update
    void Start()
    {
        namebox = GameObject.Find("Name").GetComponent<Text>();
        messagebox= GameObject.Find("Dialogue").GetComponent<Text>();
        // query list of (speaker): (message) into Array
        // n = getDialogues(script);
        // dialogues = new Dialogue[n];
        // 당장은...
        dialogues = new Queue<Dialogue>();
        // teust seeder
        Dialogue d1 = new Dialogue("고세구", "안녕");
        Dialogue d2 = new Dialogue("아이네", "하이");
        dialogues.Enqueue(d1);
        dialogues.Enqueue(d2);

        dHandler = new DialogueHandler();
        
        message = "안녕";
        name = "고세구";
        dialogue = new Dialogue(name, message);
        dHandler = new DialogueHandler();
        dHandler.startDialogue(dialogue);
    }

    // Update is called once per frame
    void Update()
    {
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
                    Debug.Log(dialogues);
                    Dialogue currentDialogue = dialogues.Dequeue();
                    string name = currentDialogue.speaker;
                    string message = currentDialogue.message;
                    namebox.text = name;
                    ShowText(message);
                } catch (Exception e){
                    // end dialogue 
                }
            }
        }
    }
    // TypeWrtier 효과
    IEnumerator ShowText(string message) {
        for(int i = 0; i < message.Length; i++){
            string currentText = message.Substring(0, i);
            GameObject.Find("Dialogue").GetComponent<Text>().text = currentText;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
