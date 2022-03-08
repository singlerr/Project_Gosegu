using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueHandler
{
    public Text namebox;
    public Text messagebox;
    void start() {
    }
    public void startDialogue(Dialogue dialogue){
        string firstMessage = dialogue.messages[1];
    }

    internal void displayNextMessage()
    {
        throw new NotImplementedException();
    }
}