using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Parser
{
    public class StringTokenizer : ITokenizer
    {
        string _toParse;
        int _pos;
        int _maxPos;
        TokenType _curToken;
        double _doubleValue;
        string _stringValue;

        public StringTokenizer(string s)
            : this(s, 0, s.Length)
        {
        }

        public StringTokenizer(string s, int startIndex)
            : this(s, startIndex, s.Length)
        {
        }

        public StringTokenizer(string s, int startIndex, int count)
        {
            _curToken = TokenType.None;
            _toParse = s;
            _pos = startIndex;
            _maxPos = startIndex + count;
        }

        #region Input reader

        char Peek()
        {
            Debug.Assert(!IsEnd);
            return _toParse[_pos];
        }

        char Read()
        {
            Debug.Assert(!IsEnd);
            return _toParse[_pos++];
        }

        void Forward()
        {
            Debug.Assert(!IsEnd);
            ++_pos;
        }

        bool IsEnd
        {
            get { return _pos >= _maxPos; }
        }

        #endregion

        public TokenType CurrentToken
        {
            get { return _curToken; }
        }

        public bool Match(TokenType t)
        {
            if (_curToken == t)
            {
                GetNextToken();
                return true;
            }
            return false;
        }

        public bool MatchDouble(out double value)
        {
            value = _doubleValue;
            if (_curToken == TokenType.Number)
            {
                GetNextToken();
                return true;
            }
            return false;
        }

        public bool MatchInteger(int expectedValue)
        {
            if (_curToken == TokenType.Number
                && _doubleValue < Int32.MaxValue
                && (int)_doubleValue == expectedValue)
            {
                GetNextToken();
                return true;
            }
            return false;
        }

        public bool MatchInteger(out int value)
        {
            if (_curToken == TokenType.Number
                && _doubleValue < Int32.MaxValue)
            {
                value = (int)_doubleValue;
                GetNextToken();
                return true;
            }
            value = 0;
            return false;
        }

        public bool MatchString(string expectedValue)
        {
            if (_curToken == TokenType.Identifier
                && _stringValue == expectedValue)
            {
                GetNextToken();
                return true;
            }
            return false;
        }

        public bool MatchString(out string value)
        {
            if (_curToken == TokenType.Identifier)
            {
                value = _stringValue;
                GetNextToken();
                return true;
            }
            value = null;
            return false;
        }

        public TokenType GetNextToken()
        {
            if (IsEnd) return _curToken = TokenType.EndOfInput;
            char c = Read();
            while (Char.IsWhiteSpace(c))
            {
                if (IsEnd) return _curToken = TokenType.EndOfInput;
                c = Read();
            }
            switch (c)
            {
                case '+': _curToken = TokenType.Plus; break;
                case '-': _curToken = TokenType.Minus; break;
                case '*': _curToken = TokenType.Mult; break;
                case '/':
                    {
                        if (IsEnd) { _curToken = TokenType.EndOfInput; }
                        else if (Peek() == '/')
                        {
                            // '//': Single-line comment
                            do
                            {
                                c = Read();
                                if (IsEnd) return TokenType.EndOfInput;
                            } while (!IsEnd && Peek() != '\n'); // '\n' will be handled by white-space character trimming
                            return GetNextToken();
                        }
                        else if (Peek() == '*')
                        {
                            // '/*': Multi-line comment
                            c = Read();
                            Debug.Assert(c == '*');
                            bool cont = true;
                            if (IsEnd) { _curToken = TokenType.EndOfInput; cont = false; }

                            while (cont)
                            {
                                c = Read();
                                if (IsEnd) { _curToken = TokenType.EndOfInput; cont = false; }
                                else if (c == '*' && Peek() == '/')
                                {
                                    Read();
                                    return GetNextToken();
                                }
                            }
                        }
                        else
                        {
                            _curToken = TokenType.Div; break;
                        }

                        break;
                    }
                case '(': _curToken = TokenType.OpenPar; break;
                case ')': _curToken = TokenType.ClosePar; break;
                case '[': _curToken = TokenType.OpenSquare; break;
                case ']': _curToken = TokenType.CloseSquare; break;
                case '{': _curToken = TokenType.OpenBracket; break;
                case '}': _curToken = TokenType.CloseBracket; break;
                case '.': _curToken = TokenType.Dot; break;
                case ';': _curToken = TokenType.SemiColon; break;
                case ',': _curToken = TokenType.Comma; break;
                case ':':
                    {
                        if (!IsEnd && Peek() == ':')
                        {
                            _curToken = TokenType.DoubleColon;
                            Forward();
                        }
                        else
                        {
                            _curToken = TokenType.Colon;
                        }
                    }
                    break;
                default:
                    {
                        if (Char.IsDigit(c))
                        {
                            _curToken = TokenType.Number;
                            double val = (int)(c - '0');
                            while (!IsEnd && Char.IsDigit(c = Peek()))
                            {
                                val = val * 10 + (int)(c - '0');
                                Forward();
                            }
                            if (!IsEnd && !Char.IsDigit(c = Peek()) && !Char.IsWhiteSpace(c) && c != ';' )
                            {
                                _curToken = TokenType.Error;
                            } else
                            {
                                _doubleValue = val;
                            }
                        }
                        else if (Char.IsLetter(c) || c == '_')
                        {
                            _curToken = TokenType.Identifier;
                            StringBuilder sb = new StringBuilder();
                            sb.Append(c);
                            while(!IsEnd && ( Char.IsLetterOrDigit(c = Peek()) || c == '_' ) )
                            {
                                sb.Append(c);
                                Forward();
                            }
                            _stringValue = sb.ToString();
                        }
                        else _curToken = TokenType.Error;
                        break;
                    }
            }
            return _curToken;
        }

    }
}