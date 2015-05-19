using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Parser
{
    [Flags]
    public enum TokenType
    {
        None = 0,
        IsAddOperator = 1,
        Plus = IsAddOperator,
        Minus = IsAddOperator + 2,
        IsMultOperator = 4,
        Mult = IsMultOperator,
        Div = IsMultOperator + 2,
        Number = 8,
        OpenPar = 16,
        ClosePar = 32,
        EndOfInput = 64,
        Error = 128,
        SemiColon = 256,
        Colon = 512,
        Comma = 1024,
        DoubleColon = 2048,
        Dot = 4096,
        CloseSquare = 8192,
        OpenBracket = 16384,
        CloseBracket = 32768,
        OpenSquare = 65536,
        Identifier = 131072
    }
}
