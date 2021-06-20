using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_Processor
{
    public enum SymbolType
    {
        Error,
        Number,
        Op,
        Ident,
        Assign,
        Eof,
        Bracket
    }

    public struct Symbol
    {
        public SymbolType Type;
        public string Value;

        public Symbol(SymbolType type, string value)
        {
            Type = type;
            Value = value;
        }
    }

    public class Symbolizer
    {
        private int _index = 0;
        private string _inputString;

        public Symbolizer(string inputString)
        {
            _inputString = inputString;
            _index = 0;
        }

        public void Reset()
        {
            _index = 0;
        }

        public Symbol GetSymbol()
        {
            if (Eof) return GetEofSymbol();

            while (_index < _inputString.Length && _inputString[_index] == ' ') _index++;
            if (Eof) return GetEofSymbol();
            if (IsLetter(_inputString[_index])) return GetIdentSymbol();
            if (IsDigit(_inputString[_index])) return GetNumberSymbol();
            if (_inputString[_index] == '=') return GetAssignSymbol();
            if (IsBracket(_inputString[_index])) return GetBracket();
            if ("+-/*".Contains(_inputString[_index]))
            {
                _index++;
                return new Symbol(SymbolType.Op, String.Format("Item: {0}, Position: {1}", _inputString[_index - 1].ToString(), _index-1));
            }
            return GetErrorSymbol(_index, "Unknown char");
        }

        private bool Eof
        {
            get
            {
                return _index == _inputString.Length;
            }
        }

       

        private Symbol GetAssignSymbol()
        {
            int index = 0;
            var value = String.Empty;
            if (!Eof && _inputString[_index] == '=')
            {
                index = _index;
                _index++;
                value += _inputString[index];
                return new Symbol(SymbolType.Assign, String.Format("Item: {0}, Position: {1}", value, index));
            }
            else
            {
                return GetErrorSymbol(_index, "Unknown char");
            }
        }

        private Symbol GetEofSymbol()
        {
            return new Symbol(SymbolType.Eof, "");
        }

        private Symbol GetErrorSymbol(int index, string value)
        {
            _index++;
            return new Symbol(SymbolType.Error, String.Format("Error: {0}. Item: {2}, Position: {1}", value, index, _inputString[index]));
        }

        private Symbol GetNumberSymbol()
        {
            bool flag_isDigit = true;
            int index = 0;
            var value = String.Empty;
            while (!Eof && (IsDigit(_inputString[_index]) || IsLetter(_inputString[_index])))
            {
                if (IsLetter(_inputString[_index]))
                    flag_isDigit = false;
                value += _inputString[_index];
                index = _index;
                _index++;
            }
            if (!flag_isDigit)
                return new Symbol(SymbolType.Error, String.Format("Error. Item: {0}, Position: {1}", value, index));
            return new Symbol(SymbolType.Number, String.Format("Item: {0}, Position: {1}", value, index));
        }

        private Symbol GetIdentSymbol()
        {
            int index = 0;
            var value = String.Empty;
            while (!Eof && (IsLetter(_inputString[_index]) || IsDigit(_inputString[_index])) )
            {
                value += _inputString[_index];
                index = _index;
                _index++;
            }
            return new Symbol(SymbolType.Ident, String.Format("Item: {0}, Position: {1}", value, index));

        }

        private Symbol GetBracket()
        {
            var value = String.Empty;
            while(!Eof && IsBracket(_inputString[_index]))
            {
                value += _inputString[_index];
                _index++;
            }
            return new Symbol(SymbolType.Bracket, String.Format("Item: {0}, Position: {1}", value, _index));
        }

        private bool IsBracket(char c)
        {
            return c == '(' || c == ')';
        }

        private bool IsDigit(char c)
        {
            return c >= '0' && c <= '9';
        }

        private bool IsLetter(char c)
        {
            return (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z');
        }
    }
}
