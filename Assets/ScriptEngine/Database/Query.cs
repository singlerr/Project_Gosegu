using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;

namespace ScriptEngine.Database
{
    public class Query<T>
    {
        private readonly StringBuilder _queryStringBuilder;
        private readonly OdbcCommand _command;
        public Query(OdbcCommand command)
        {
            _command = command;
            _queryStringBuilder = new StringBuilder();
        }

        public Query(string initial, OdbcCommand command)
        {
            _command = command;
            _queryStringBuilder = new StringBuilder();
            _queryStringBuilder.Append(initial);
        }

        public Query<T> And()
        {
            AppendWhiteSpace();
            _queryStringBuilder.Append("and");
            return this;
        }

        public Query<T> Or()
        {
            AppendWhiteSpace();
            _queryStringBuilder.Append("or");
            return this;
        }

        public Query<T> Eq(string column, object target)
        {
            AppendWhiteSpace();
            if (_queryStringBuilder.ToString().IndexOf("where", StringComparison.InvariantCultureIgnoreCase) < 0)
                _queryStringBuilder.Append("where");
            _queryStringBuilder.Append($"{column} = '{target}'");
            return this;
        }

        public Query<T> NotEq(string column, object target)
        {
            AppendWhiteSpace();
            if (_queryStringBuilder.ToString().IndexOf("where", StringComparison.InvariantCultureIgnoreCase) < 0)
                _queryStringBuilder.Append("where");
            _queryStringBuilder.Append($"{column} != '{target}'");
            return this;
        }
        public DataSet FindOrEmpty()
        {
            if (_queryStringBuilder[_queryStringBuilder.Length - 1] != ';')
                _queryStringBuilder.Append(';');
            _command.CommandType = CommandType.Text;
            _command.CommandText = _queryStringBuilder.ToString();

            var dataSet = new DataSet();
            
            var adapter = new OdbcDataAdapter(_command);
            adapter.Fill(dataSet);
            return dataSet;
        }
        private void AppendWhiteSpace()
        {
            _queryStringBuilder.Append(" ");
        }

    }

    public static class QueryExtension
    {
      
        public static List<Dialogue> FindDialoguesOrEmpty(this Query<Dialogue> query)
        {

            var dialogues = new List<Dialogue>();
            
            var data = query.FindOrEmpty();
            
            var dialogueType = typeof(Dialogue);

            var defined = dialogueType.GetProperties().Where(prop => prop.IsDefined(typeof(Mappings), false)).ToArray();
            if (defined.Any())
                throw new Exception("No marked fields found in Dialogue class.");

            var table = data.Tables[0];
            for (var j = 0; j < table.Rows.Count; j++)
            {
                var row = table.Rows[j];
                var dialogue = new Dialogue();
                for (var i = 0; i < defined.Count(); i++)
                {
                    var prop = defined[i];
                    var attrib = (Mappings) prop.GetCustomAttributes(typeof(Mappings), false)[0];


                    var dataTable = data.Tables[0];


                    var columnIndex = dataTable.Columns.IndexOf(attrib.Name);
                    if (columnIndex == -1)
                    {
                        Debug.Log($"Column {attrib.Name} not found in csv data. Skip this column.");
                        continue;
                    }
                    
                    prop.SetValue(dialogue,row.ItemArray[columnIndex]);
                }
                dialogues.Add(dialogue);
            }

            return dialogues;
        }
    }

    
}