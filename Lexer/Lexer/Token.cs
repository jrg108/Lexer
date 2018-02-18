
namespace Lexer
{
    class Token
    {
        public string name, attribute;
        int column, line;

        public Token(string _name, int _column, int _line)
        {
            name = _name;
            attribute = null;
            column = _column;
            line = _line;
        }

        public Token(string _name, string _attribute, int _column, int _line)
        {
            name = _name;
            attribute = _attribute;
            column = _column;
            line = _line;
        }
    }
}
