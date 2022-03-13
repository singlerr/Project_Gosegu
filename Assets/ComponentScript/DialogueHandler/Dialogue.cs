using System.Collections;
using System.Collections.Generic;
using CsvHelper.Configuration.Attributes;
using ScriptEngine.Database;
using UnityEngine;

public class Dialogue
{
    [Mappings("화자")]
    public string speaker;
    [Mappings("대사")]
    public string message;
    [Mappings("사운드")]
    public string sound;
    [Mappings("지속시간")]
    public float playingTime;
    [Mappings("캐릭터 이미지")]
    public string image;
    [Mappings("배경화면 이미지")]
    public string backgroundImage;
    public Dialogue(){}
    public Dialogue(string name, string message){
        this.speaker = name;
        this.message = message;
    }
}
