using System;
using System.Collections.Generic;

namespace Lexer
{
    class LexerClass
    {
        private string input;
        private int line = 0, column = 0, position = 0;
        public List<Token> Tokens = new List<Token>();

        public LexerClass(string _input)
        {
            input = _input;
        }

        void Advance()
        {
            position++;
            column++;
        }
        void Advance(int positions)
        {
            position += positions;
            column += positions;
        }

        bool Inbounds(int amount)
        {
            return (position + amount < input.Length);
        }

        void AddToken(string name)
        {
            Token temp = new Token(name, column, line);
            Tokens.Add(temp);
        }

        void AddToken(string name, string attribute)
        {
            Token temp = new Token(name, attribute, column, line);
            Tokens.Add(temp);
        }

        public void Lex()
        {
            while(position < input.Length)
            {
                switch (input[position])
                {
                    //Whitespace
                    case ' ':
                    case '\t':
                        column++;
                        break;

                    case '\n':
                        line++;
                        column = 0;
                        break;

                    //comments
                    case '#':
                        while (Inbounds(1) && input[position + 1] != '\n')
                            Advance();
                        line++;
                        column = 0;
                        break;


                    //punctuators
                    case '{':
                        AddToken("left-brace");
                        break;
                    case '}':
                        AddToken("right-brace");
                        break;
                    case '(':
                        AddToken("left-paren");
                        break;
                    case ')':
                        AddToken("right-paren");
                        break;
                    case '[':
                        AddToken("left-bracket");
                        break;
                    case ']':
                        AddToken("right-bracket");
                        break;
                    case ',':
                        AddToken("comma");
                        break;
                    case ';':
                        AddToken("semicolon");
                        break;
                    case ':':
                        AddToken("colon");
                        break;

                    //operators
                    //relational operators
                    case '=':
                        if (Inbounds(1) && input[position + 1] == '=')
                        {
                            AddToken("relational-operator", "equality");
                            Advance();
                        }
                        else
                        {
                            AddToken("assignment-operator");
                        }
                        break;
                    case '!':
                        if (Inbounds(1) && input[position + 1] == '=')
                        {
                            AddToken("relational-operator", "not-equal");
                            Advance();
                        }
                        else
                        {
                            AddToken("invalid", "!");
                        }
                        break;
                    case '<':
                        if (Inbounds(1) && input[position + 1] == '=')
                        {
                            AddToken("relational-operator", "less-than-or-equal");
                            Advance();
                        }
                        else
                        {
                            AddToken("relational-operator", "less-than");
                        }
                        break;
                    case '>':
                        if (Inbounds(1) && input[position + 1] == '=')
                        {
                            AddToken("relational-operator", "greater-than-or-equal");
                            Advance();
                        }
                        else
                        {
                            AddToken("relational-operator", "greater-than");
                        }
                        break;

                    //arithmetic operators
                    case '+':
                        AddToken("arithmetic-operator", "add");
                        break;
                    case '-':
                        AddToken("arithmetic-operator", "subtract");
                        break;
                    case '*':
                        AddToken("arithmetic-operator", "multiply");
                        break;
                    case '/':
                        AddToken("arithmetic-operator", "divide");
                        break;
                    case '%':
                        AddToken("arithmetic-operator", "modulus");
                        break;

                    //bitwise operators
                    case '&':
                        AddToken("bitwise-operator", "&");
                        break;
                    case '|':
                        AddToken("bitwise-operator", "|");
                        break;
                    case '^':
                        AddToken("bitwise-operator", "^");
                        break;
                    case '~':
                        AddToken("bitwise-operator", "and");
                        break;

                    //logical operators are with reserved keywords

                    //conditional operator
                    case '?':
                        AddToken("conditional-operator");
                        break;

                    //characters
                    case '\'':
                        if (Inbounds(1) && input[position + 1] == '\\')
                        {
                            if (Inbounds(3) && input[position + 3] == '\'')
                            {
                                AddToken("character-literal", "escape-sequence:" + input[position + 1].ToString() + input[position + 2].ToString());
                                Advance(3);
                            }
                        }
                        else if (Inbounds(2) && input[position + 2] == '\'')
                        {
                            AddToken("character-literal", "character-character:" + input[position + 1].ToString());
                            Advance(2);
                        }
                        else
                        {
                            AddToken("invalid, too many characters in character literal", "Line: " + line + " column: " + column);
                            position = input.Length;
                        }
                        break;
                    //strings
                    case '\"':
                        string tempString = "";
                        while(Inbounds(1) && input[position + 1] != '\"')
                        {
                            if(input[position + 1] != '\n')
                            {
                                tempString += input[position + 1];

                            }
                            Advance();
                            if(input[position] == '\\')
                            {
                                tempString += input[position + 1];
                                Advance();
                            }
                            
                            
                        }
                        Advance();
                        AddToken("string-literal", tempString);
                        break;


                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                        if (Inbounds(1) && (input[position + 1] == 'x' || input[position + 1] == 'X'))
                        {
                            string temp = "0x";
                            Advance();
                            while (Inbounds(1) && (Char.IsNumber(input[position + 1]) || input[position + 1].ToString().ToLower() == "a" || input[position + 1].ToString().ToLower() == "b" || input[position + 1].ToString().ToLower() == "c" || input[position + 1].ToString().ToLower() == "d" || input[position + 1].ToString().ToLower() == "e" || input[position + 1].ToString().ToLower() == "f"))
                            {
                                temp += input[position + 1];
                                Advance();
                            }
                            AddToken("hexadecimal-integer-literal", temp);
                        }
                        else if (Inbounds(1) && (input[position + 1] == 'b' || input[position + 1] == 'B'))
                        {
                            string temp = "0b";
                            Advance();
                            while (input[position + 1] == '1' || input[position + 1] == '0')
                            {
                                temp += input[position + 1];
                                Advance();
                            }
                        }
                        else
                        {
                            //bool checks if already in second half of fraction
                            bool decimalExists = false;
                            bool exponentExists = false;
                            string temp = input[position].ToString();
                            //floating and integers
                            while (Inbounds(1) && (Char.IsNumber(input[position + 1]) || (input[position + 1] == '.' && !decimalExists) || ((input[position + 1] == 'e' || input[position + 1] == 'E') && !exponentExists)))
                            {
                                temp += input[position + 1].ToString();
                                Advance();
                                if (Inbounds(1) && (input[position + 1] == '.' && !decimalExists && !exponentExists))
                                {
                                    temp += input[position + 1].ToString();
                                    decimalExists = true;
                                    Advance();
                                }
                                if (Inbounds(1) && ((input[position + 1] == 'e' || input[position + 1] == 'E') && !exponentExists))
                                {
                                    temp += input[position + 1].ToString();
                                    exponentExists = true;
                                    Advance();
                                }
                            }
                            if (decimalExists || exponentExists)
                            {
                                AddToken("floating-point-literal", temp);
                            }
                            else
                            {
                                AddToken("decimal-integer-literal", temp);
                            }
                        }
                        break;


                    //identifier
                    //boolean literals
                    //keywords



                    default:
                        string literal = "";
                        if(Char.IsLetter(input[position]))
                        {
                            literal += input[position];
                            while(Inbounds(1) && (Char.IsLetterOrDigit(input[position + 1]) || input[position + 1] == '_'))
                            {
                                literal += input[position + 1];
                                Advance();
                            }
                            switch (literal)
                            {
                                //logical operators
                                case "and":
                                    AddToken("logical-operator", literal);
                                    break;
                                case "or":
                                    AddToken("logical-operator", literal);
                                    break;
                                case "not":
                                    AddToken("logical-operator", literal);
                                    break;

                                //boolean literals
                                case "false":
                                    AddToken("boolean-literal", literal);
                                    break;
                                case "true":
                                    AddToken("boolean-literal", literal);
                                    break;

                                //keywords
                                case "bool":
                                case "char":
                                case " def":
                                case "else":
                                case "float":
                                case "if":
                                case "int":
                                case "let":
                                case "var":
                                    AddToken("keyword", literal);
                                    break;
                                default:
                                    AddToken("identifier", literal);
                                    break;
                            }
                        }
                        else
                            AddToken("invalid", "Line: " + line + " column: " + column);
                        break;
                }
                Advance();
            }
            
            
        }


    }
}
