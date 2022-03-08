using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue
{
    public string speaker;
    public List<string> messages;
    public Dialogue(string name, List<string> messages){
        this.speaker = name;
        this.messages = messages;
    }
}
