using System;

namespace ScriptEngine.Database
{
    public class Mappings : Attribute
    {
        public string Name;

        public Mappings(string name)
        {
            Name = name;
        }
    }
}