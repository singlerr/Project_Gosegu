using System;
using System.Collections.Generic;
using System.IO;
using ComponentScript.DialogueHandler;

namespace ScriptEngine
{
    public class NovelDataService
    {
        private Dictionary<string, DialogueContainer> _containers;
        private readonly string _csvParentPath;

        public NovelDataService(string csvParentPath)
        {
            if (!Directory.Exists(csvParentPath))
                throw new Exception($"{csvParentPath} does not exists.");

            if (Directory.GetFiles(csvParentPath).Length == 0)
                throw new Exception($"No csv files found in {csvParentPath}");
            _csvParentPath = csvParentPath;
        }

        public List<Dialogue> Find(string csvName)
        {
            return _containers[csvName].Dialogues;
        }

        public async void StartService()
        {
            var files = Directory.GetFiles(_csvParentPath);

            var reader = new ScriptReader(files);
            _containers = await reader.Read();
        }
    }
}