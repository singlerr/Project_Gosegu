using System.Collections.Generic;
using YamlDotNet.Serialization;

namespace ComponentScript.DialogueHandler
{
    public class SceneInformation
    {
        [YamlMember(Alias = "name")]
        public string SceneName { get; set; }
        [YamlMember(Alias = "messengers")]
        public Dictionary<string,ChatContext> ChatRooms { get; set; }
        [YamlMember(Alias = "stock")]
        public Dictionary<string, StockDatum> StockData { get; set; } 
    }

    public class ChatContext
    {
        [YamlMember(Alias = "context")]
        public List<string> Messages { get; set; }
        [YamlMember(Alias = "delayed")]
        public List<string> ExceptionMessages { get; set; }
        [YamlMember(Alias = "hangout")]
        public ExtraAction ExtraAction { get; set; }
    }
    

    public class StockDatum
    {
        [YamlMember(Alias = "price")]
        public int Price { get; set; }
        [YamlMember(Alias = "description")]
        public string Description { get; set; }
    }

    public class ExtraAction
    {
        [YamlMember(Alias = "enabled")]
        public bool Enabled { get; set; }
        [YamlMember(Alias = "script")]
        public string TargetScript { get; set; }
    }
}