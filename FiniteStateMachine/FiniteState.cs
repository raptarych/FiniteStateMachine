using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiniteStateMachine
{
    class FiniteStates
    {
        private static Dictionary<string, Dictionary<string,string>> PossibleStates;

        public static void SetTable(Dictionary<string, Dictionary<string, string>> table) => PossibleStates = table;
        public static void ProcessCode(string input)
        {
            if (PossibleStates == null)
                throw new Exception("Finite state machine wasn't defined");
            var queue = new Queue<char>(input.ToCharArray()); //LIFO

            var currentState = PossibleStates.FirstOrDefault().Value.FirstOrDefault().Key;
            var currentIndex = 0;

            while (queue.Any())
            {
                currentIndex++;
                var currentChar = queue.Dequeue();
                var currentType = currentChar.ToString();

                if (!PossibleStates.ContainsKey(currentType)) throw new Exception($"Invalid symbol: {currentChar}");
                currentState = PossibleStates[currentType][currentState];
            }

            var result = PossibleStates["Output"][currentState] == "1";
            if (result)
                Console.WriteLine("Validated");
            else
                Console.WriteLine("Not validated");
        }

        public static void EchoError(int errorCharIndex, string input, string message, int line)
        {
            Console.WriteLine(input);
            var pointer = "";
            for (int i = 0; i < errorCharIndex - 1; i++) pointer += " ";
            pointer += "^";
            Console.WriteLine(pointer);
            throw new Exception($"Compilation error at line {line} symbol {errorCharIndex}: {message}");
        }
    }
}
