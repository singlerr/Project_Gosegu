using CsvHelper.Configuration.Attributes;

public class Dialogue
{
    [Name("배경화면 이미지")] public string backgroundImage;

    [Name("캐릭터 이미지")] public string image;

    [Name("대사")] public string message;

    [Name("지속시간")] public float playingTime;

    [Name("사운드")] public string sound;

    [Name("화자")] public string speaker;

    public Dialogue()
    {
    }

    public Dialogue(string name, string message)
    {
        speaker = name;
        this.message = message;
    }
}