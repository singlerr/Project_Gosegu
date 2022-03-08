using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue
{
    public string speaker;
    public string message;
    public Dialogue(string name, string message){
        this.speaker = name;
        this.message = message;
    }
}
