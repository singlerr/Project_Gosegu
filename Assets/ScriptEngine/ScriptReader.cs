using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using ComponentScript.DialogueHandler;
using CsvHelper;
using CsvHelper.Configuration;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ScriptEngine
{
    public class ScriptReader
    {
        private readonly string[] _paths;

        public ScriptReader(params string[] paths)
        {
            _paths = paths;
        }

        public async UniTask<Dictionary<string, DialogueContainer>> Read()
        {
            
            var dialogues = new Dictionary<string, DialogueContainer>();
            for (var i = 0; i < _paths.Length; i++)
            {
                var currentPath = _paths[i];
                if(! Path.GetExtension(currentPath).Equals(".csv"))
                    continue;
                var name = Path.GetFileNameWithoutExtension(currentPath);
                using var streamReader = new StreamReader(currentPath, Encoding.UTF8);
                using var csvReader = new CsvReader(streamReader, new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    BadDataFound = null,
                    Encoding = Encoding.UTF8
                });
                
                var records = csvReader.GetRecordsAsync<Dialogue>();

                var dialogueContainer = new DialogueContainer
                {
                    Dialogues = new List<Dialogue>()
                };
                
                await foreach (var dialogue in records) dialogueContainer.Dialogues.Add(dialogue);

                dialogues[name] = dialogueContainer;
            }

            return dialogues;
        }
    }
}