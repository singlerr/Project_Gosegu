using System;

namespace ScriptEngine.Lexer
{
    [Flags]
    public enum LexerState
    {
        InBracket = 2,
        InString = 1,
        None = 0
    }
}