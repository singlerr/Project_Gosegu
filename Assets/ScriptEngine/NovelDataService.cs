using System;
using System.Data;
using System.Data.Odbc;
using System.IO;
using JetBrains.Annotations;
using ScriptEngine.Database;
using UnityEngine;

namespace ScriptEngine
{
    public class NovelDataService
    {
        [NotNull]
        private OdbcConnection _connection;

        private NovelDataService(string csvParentPath)
        {
            if (!Directory.Exists(csvParentPath))
                throw new Exception($"{csvParentPath} does not exists.");

            if (Directory.GetFiles(csvParentPath).Length == 0)
                throw new Exception($"No csv files found in {csvParentPath}");
            
            _connection = new OdbcConnection("Driver={Microsoft Text Driver (*.txt; *.csv)};" +
                                                 "Dbq=%s;Extensions=asc,csv,tab,txt;" +
                                                 "Persist Security Info=False"
                                                     .Replace("%s",csvParentPath));
        }

        public Query<Dialogue> NewQuery(string csvName)
        {
            return new Query<Dialogue>($"select * from {csvName}" ,new OdbcCommand
            {
                Connection = _connection
            });
        }
        public static NovelDataService StartConnection(string csvParentPath)
        {
            return new NovelDataService(csvParentPath);
        }
        
    }
}