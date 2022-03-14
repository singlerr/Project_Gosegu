using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using ComponentScript.DialogueHandler;
using YamlDotNet.RepresentationModel;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace ScriptEngine
{
    public class SceneInformationLoader
    {
        private string _path;

        public SceneInformationLoader(string path)
        {
            _path = path;
        }
        
        public SceneInformation Load()
        {
            var deserializer = new DeserializerBuilder().Build();
            return deserializer.Deserialize<SceneInformation>(new StreamReader(_path,Encoding.UTF8));
        }
    }
}