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
    // dialouges: List of Dialogue / FIFO
    public Queue<Dialogue> dialogues;
    // dialogue: <등장인물, 메시지> -> <지속시간 캐릭터별img 배경화면 사운드 인물 대사>: Script의 row를 반영합니다.
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

        // TODO: Dialogue Script를 List<Row: Dialogue>로 Parse한 후 dialogues로 Enqueue할 예정입니다.

        dialogues = new Queue<Dialogue>();
        // teust seeder
        Dialogue d1 = new Dialogue("고세구", "안녕! ");
        Dialogue d2 = new Dialogue("아이네", "하이네! ");
        dialogues.Enqueue(d1);
        dialogues.Enqueue(d2);
        
        message = "안녕";
        name = "고세구";
        dialogue = new Dialogue(name, message);
    }

    // Update is called once per frame
    void Update()
    {
        if(!dialogueStarted){
            if(Input.anyKeyDown){
                Debug.Log("onpresskey");
                Debug.Log(dialogue);
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
                    StartCoroutine(ShowText(message));
                } catch (Exception e){
                    // TODO: end dialogue 다음 Scene 처리 
                }
            }
        }
    }
    // TypeWrtier 효과
    IEnumerator ShowText(string message) {
        messagebox= GameObject.Find("Dialogue").GetComponent<Text>();
        for(int i = 0; i < message.Length; i++){
            string currentText = message.Substring(0, i);
            messagebox.text = currentText;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
