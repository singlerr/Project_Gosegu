using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using ScriptEngine;
using UnityEngine;
using UnityEngine.UI;
using static ComponentScript.EnvConstants;
public class RoomDialogueCanvasHandler : MonoBehaviour
{
    // dialouges: List of Dialogue / FIFO
    public Queue<Dialogue> dialogues;
    public string message;
    public Text namebox;
    public Text messagebox;
    public string name;


    private NovelDataService _dataService;

    // dialogue: <등장인물, 메시지> -> <지속시간 캐릭터별img 배경화면 사운드 인물 대사>: Script의 row를 반영합니다.
    public Dialogue dialogue;
    private bool dialogueStarted;


    // Start is called before the first frame update
    private void Start()
    {
        if (!Directory.Exists(CsvDirectory))
            Directory.CreateDirectory(CsvDirectory);
        _dataService = new NovelDataService(CsvDirectory);
        _dataService.StartService();

        if (!Directory.Exists(SceneConfigDirectory))
            Directory.CreateDirectory(SceneConfigDirectory);
        
        
        //    var a =  _dataService.NewQuery("csvName").Eq("캐릭터", "고세구").Or().Eq("캐릭터", "뢴트게늄").FindDialoguesOrEmpty();

        namebox = GameObject.Find("Name").GetComponent<Text>();
        messagebox = GameObject.Find("Dialogue").GetComponent<Text>();

        // TODO: Dialogue Script를 List<Row: Dialogue>로 Parse한 후 dialogues로 Enqueue할 예정입니다.

        dialogues = new Queue<Dialogue>();
        // teust seeder
        var d1 = new Dialogue("고세구", "안녕! ");
        var d2 = new Dialogue("아이네", "하이네! ");
        dialogues.Enqueue(d1);
        dialogues.Enqueue(d2);

        message = "안녕";
        name = "고세구";
        dialogue = new Dialogue(name, message);
    }
    

    // Update is called once per frame
    private void Update()
    {
        if (!dialogueStarted)
        {
            if (Input.anyKeyDown)
            {
                Debug.Log("onpresskey");
                Debug.Log(dialogue);
                dialogueStarted = true;
            }
        }
        else
        {
            if (Input.GetKeyDown("space"))
                try
                {
                    Debug.Log("got here");
                    Debug.Log(dialogues);
                    var currentDialogue = dialogues.Dequeue();
                    var name = currentDialogue.speaker;
                    var message = currentDialogue.message;
                    namebox.text = name;
                    StartCoroutine(ShowText(message));
                }
                catch (Exception e)
                {
                    // TODO: end dialogue 다음 Scene 처리 
                }
        }
    }

    // TypeWrtier 효과
    private IEnumerator ShowText(string message)
    {
        messagebox = GameObject.Find("Dialogue").GetComponent<Text>();
        for (var i = 0; i < message.Length; i++)
        {
            var currentText = message.Substring(0, i);
            messagebox.text = currentText;
            yield return new WaitForSeconds(0.1f);
        }
    }
}