using System;

namespace ScriptEngine.Lexer
{
    [Flags]
    public enum LexerState
    {
        DefVariable = 1 << 2,
        InBracket = 1 << 1,
        InString = 1 << 0,
        None = 0
    }
}