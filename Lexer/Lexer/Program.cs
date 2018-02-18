using System;
using System.IO;

namespace Lexer
{
    class Program
    {
        static void Main(string[] args)
        {
            //string testInput = "0x1A64be69 #fasdfjlskdjfl\n? \n{}[]\"hgafd\nfas\\\"fdasdf\"fasdf bool true;";
            string testInput = File.ReadAllText(args[0]);
            LexerClass test = new LexerClass(testInput);
            test.Lex();
            foreach (Token tok in test.Tokens)
            {
                if(tok.attribute !=null)
                    Console.WriteLine("<" + tok.name + ":" + tok.attribute + ">");
                else
                    Console.WriteLine("<" + tok.name + ">");
            }
            Console.ReadLine();
        }
    }
}
