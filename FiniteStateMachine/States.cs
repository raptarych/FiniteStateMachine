using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiniteStateMachine
{
    public enum CharType
    {
        Identificator = 1,
        OpenScope = 2,
        CloseScope = 3,
        Const = 4,
        Comma = 5
    }
    public enum CharState
    {
        Exception = 0,  
        Space = 1,
        UndefinedIdentificator = 2,
        OpenScope = 3,
        Const = 4,
        InnerComma = 5,
        DefinedIdentificator = 6,
        ClosingComma = 7,
        OuterComma = 8
    }
    class States
    {
        private static readonly Dictionary<CharType, int[]> PossibleStates = new Dictionary<CharType, int[]>
        {
            /* перевёрнутый вариант таблицы, которую в классе делали
             * только где ошибка Е была - я сделал индекс 0             */         
                                   
            { CharType.Identificator,   new[] {0, 2, 0, 6, 0, 6, 0, 0, 2} },
            { CharType.Const,           new[] {0, 0, 0, 4, 0, 4, 0, 0, 0} },
            { CharType.Comma,           new[] {0, 0, 8, 0, 5, 0, 5, 8, 0} },
            { CharType.OpenScope,       new[] {0, 0, 3, 0, 0, 0, 0, 0, 0} },
            { CharType.CloseScope,      new[] {0, 0, 0, 0, 7, 0, 7, 0, 0} }
        };

        private static CharType IdentifyCharType(char currentChar)
        {
            if (currentChar >= '0' && currentChar <= '9') return CharType.Const;
            if (currentChar == ',') return CharType.Comma;
            if (currentChar == '(') return CharType.OpenScope;
            if (currentChar == ')') return CharType.CloseScope;
            return CharType.Identificator;
        }

        public static void QueryCode(string input)
        {
            var queue = new Queue<char>(input.ToCharArray()); //LIFO

            CharState currentState = CharState.Space;
            var currentIndex = 0;

            while (queue.Any())
            {
                currentIndex++;
                var currentChar = queue.Dequeue();
                CharType currentType;

                currentType = IdentifyCharType(currentChar);

                currentState = (CharState) PossibleStates[currentType][(int) currentState];

                if (currentType == CharType.Const && currentState != CharState.Exception)
                {
                    while (currentType == CharType.Const && queue.Any())
                    {
                        currentIndex++;
                        currentChar = queue.Dequeue();
                        currentType = IdentifyCharType(currentChar);
                        if (currentType != CharType.Const) currentState = (CharState)PossibleStates[currentType][(int)currentState];
                    }
                }

                if (currentType == CharType.Identificator && currentState != CharState.Exception)
                {
                    while (currentType == CharType.Identificator && queue.Any())
                    {
                        currentIndex++;
                        currentChar = queue.Dequeue();
                        currentType = IdentifyCharType(currentChar);
                        if (currentType != CharType.Identificator) currentState = (CharState)PossibleStates[currentType][(int)currentState];
                    }
                }

                if (currentState == CharState.Exception)
                {
                    EchoError(currentIndex, input, "unexpected token");
                    return;
                }
            }

            if (currentState != CharState.UndefinedIdentificator && currentState != CharState.ClosingComma)
            {
                EchoError(currentIndex, input, "unexpected end");
                return;
            } 
            Console.WriteLine("File successfully validated");
        }

        public static void EchoError(int errorCharIndex, string input, string message)
        {
            Console.WriteLine(input);
            var pointer = "";
            for (int i = 0; i < errorCharIndex - 1; i++) pointer += " ";
            pointer += "^";
            Console.WriteLine(pointer);
            Console.WriteLine($"Compilation error at {errorCharIndex} symbol: {message}");
        }
    }
}
