using System.Collections.Generic;
using CsvHelper.Configuration.Attributes;

public class Dialogue
{
    [Name("순번")]
    public int index { get; set; }
    [Optional]
    [Name("대사 타입")] 
    public DialogueType dialogueType { get; set; } = DialogueType.Talk;
    [Optional]
    [Name("진행 타입")] 
    public DialogueTriggerType dialogueTriggerType { get; set; } = DialogueTriggerType.None;
    [Optional]
    [Name("배경 이미지")] 
    public string backgroundImage { get; set; }
    [Optional]
    [Name("캐릭터 이미지")] 
    public string image{ get; set; }
    [Optional]
    [Name("대화 내용")] 
    public string message{ get; set; }
    [Optional]
    [Name("연출 시간")]
    public double playingTime{ get; set; }
    [Optional]
    [Name("사운드")]
    public string sound{ get; set; }

    [Optional]
    [Name("화자")] 
    public string speaker{ get; set; }

    [Optional]
    [Name("선택지")] 
    public List<string> actionList { get; set; }
    [Optional]
    [Name("선택 결과")]
    public List<string> actionResultList { get; set; }
    public Dialogue()
    {
    }

    public Dialogue(string name, string message)
    {
        speaker = name;
        this.message = message;
    }

    public enum DialogueType
    {
        Talk,
        Action
    }

    public enum DialogueTriggerType
    {
        Click,
        None
    }
}