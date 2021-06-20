using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_Processor
{

    public enum BasicSymbolType
    {
        Var,  // Ключевое слово var
        Ident, //Идентификатор
        Сolon, // Символ :
        Complex, // Ключевое слово complex
        Semicolon, // Символ ;
        Eof, //Конец строки
        Error, //Ошибка  
    }

    public struct BasicSymbol
    {
        public BasicSymbolType Type;
        public string Value;
        public int Index;
        public BasicSymbol(BasicSymbolType type, string value, int index = 0)
        {
            Type = type;
            Value = value;
            Index = index;
        }
    }

    class BasicScaner
    {

        private int _index = 0;
        private string _inputString;
        private int StringNum = 1;
        private int switchIndex = 0;
        private string result;
        private int temp_index;
        private int temp_index_last;
        private int count_space;
        public BasicScaner(string inputString)
        {
            _inputString = inputString;
            _index = 0;
        }

        public void Reset(int temp)
        {

            if (count_space > 1)
            {
                _index = temp - 1;
            }
            else
                _index = temp;
        }
        public void set_start_point(int temp_index)
        {
            this.temp_index = _index - temp_index;
        }
        public void set_last_point()
        {
            this.temp_index_last = _index;
        }
        public void set_space(int count_space)
        {
            this.count_space = count_space;
        }



        public BasicSymbol GetSymbol()
        {
            var value = String.Empty;
            int temp_index = 0;
            int temp_space = 0;

            while (_index < _inputString.Length && _inputString[_index] == ' ')
            {
                _index++;
                temp_index++;
                temp_space++;
            }
            set_space(temp_space);
            while (!Eof)
            {
                value += _inputString[_index];
                _index++;

                if (Eof)
                {
                    set_start_point(temp_index);
                    set_last_point();

                    break;
                }
                if (_inputString[_index] == '\n')
                {
                    StringNum++;
                }
                if (_inputString[_index] == ' ' || _inputString[_index] == ':' || _inputString[_index - 1] == ':' || _inputString[_index] == ';')
                {
                    set_start_point(temp_index);
                    set_last_point();
                    break;
                }
                temp_index++;

            }

            return CheckSymbolType(value);
        }

        public BasicSymbol CheckSymbolType(string value)
        {
            if (GetVarSymbol(value).Type != BasicSymbolType.Error)
            {
                return GetVarSymbol(value);
            }
            if (GetComplexSymbol(value).Type != BasicSymbolType.Error)
            {
                return GetComplexSymbol(value);
            }
            if (GetIdentSymbol(value).Type != BasicSymbolType.Error)
            {
                return GetIdentSymbol(value);
            }
            if (GetСolonSymbol(value).Type != BasicSymbolType.Error)
            {
                return GetСolonSymbol(value);
            }
            if (GetSemicolonSymbol(value).Type != BasicSymbolType.Error)
            {
                return GetSemicolonSymbol(value);
            }

            return GetVarSymbol(value);
        }

        public string mainSyntaxis()
        {
            BasicSymbolType type = BasicSymbolType.Eof;
            
            string message = "";
            switch (switchIndex)
            {
                case 0:
                    type = BasicSymbolType.Var;
                    message = " Ожидалось ключевое слово var, Получено: {0} Позиция:{1} Строка:{2}";
                    break;
                case 1:
                    type = BasicSymbolType.Ident;
                    message = " Ожидался идентификатор переменной, Получено: {0} Позиция:{1} Строка:{2}";
                    break;
                case 2:
                    type = BasicSymbolType.Сolon;
                    message = " Ожидался символ : Получено: {0} Позиция:{1} Строка:{2}";
                    break;
                case 3:
                    type = BasicSymbolType.Complex;
                    message = " Ожидалось ключевое слово complex, Получено: {0} Позиция:{1} Строка:{2}";
                    break;
                case 4:
                    type = BasicSymbolType.Semicolon;
                    message = " Ожидался символ ; Получено: {0} Позиция:{1} Строка:{2}";
                    break;
       
                default:
                    type = BasicSymbolType.Eof;
                    break;
            }
            if (type == BasicSymbolType.Eof)
                return "";
            BasicSymbol symbol = GetSymbol();

            if (symbol.Type == type)
            {
                result += ((int)symbol.Type).ToString() + String.Format(" Объект: {0}  Позиция:{1} Строка:{2}", symbol.Value, symbol.Index, StringNum) + "|";
                switchIndex++;
                mainSyntaxis();

            }
            else
            {
                result += ((int)symbol.Type).ToString() + String.Format(message, symbol.Value, symbol.Index, StringNum) + "|";

                Reset(temp_index);
                if (_inputString[_index] == ' ')
                    return result;
                else
                {
                    Reset(temp_index_last);
                }
                switchIndex++;
                if (type == BasicSymbolType.Eof)
                    return result;
                mainSyntaxis();
            }
            return result;
        }

        private BasicSymbol GetVarSymbol(string value)
        {
            if (value == "var" || value == "VAR")
            {

                return new BasicSymbol(BasicSymbolType.Var, value, _index);
            }
            else
            {
                return GetErrorSymbol(_index, value);
            }
        }

        private BasicSymbol GetIdentSymbol(string value)
        {
            int i = 0;
            bool wrong_symbol = false;
            while (i < value.Length)
            {
                if (!IsLetter(value[i]) && !IsDigit(value[i]) || IsDigit(value[0]))
                {
                    wrong_symbol = true;
                }
                i++;
            }
            if (value != String.Empty && !wrong_symbol)
            {

                return new BasicSymbol(BasicSymbolType.Ident, value, _index);
            }
            else
            {
                return GetErrorSymbol(_index, value);
            }

        }

        private BasicSymbol GetСolonSymbol(string value)
        {
            if (value == ":")
            {

                return new BasicSymbol(BasicSymbolType.Сolon, value, _index);
            }
            else
            {
                return GetErrorSymbol(_index, value);
            }
        }

        private BasicSymbol GetComplexSymbol(string value)
        {
            if (value == "complex" || value == "COMPLEX")
            {

                return new BasicSymbol(BasicSymbolType.Complex, value, _index);
            }
            else
            {
                return GetErrorSymbol(_index, value);
            }
        }
        private BasicSymbol GetSemicolonSymbol(string value)
        {
            if (value == ";")
            {

                return new BasicSymbol(BasicSymbolType.Semicolon, value, _index);
            }
            else
            {
                return GetErrorSymbol(_index, value);
            }
        }
      
        private BasicSymbol GetErrorSymbol(int index, string value)
        {
            return new BasicSymbol(BasicSymbolType.Error, value, _index);
        }

        private bool Eof
        {
            get
            {
                return _index == _inputString.Length;
            }
        }

        private bool IsDigit(char c)
        {
            return c >= '0' && c <= '9';
        }

        private bool IsLetter(char c)
        {
            return (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || (c >= 'а' && c <= 'я') || (c >= 'А' && c <= 'Я');
        }

    }
}
