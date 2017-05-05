using System;
using System.Collections.Generic;
using System.Linq;

namespace FiniteStateMachine
{
    class FiniteStates
    {
        private static Dictionary<string, Dictionary<string,string>> PossibleStates;

        public static void SetTable(Dictionary<string, Dictionary<string, string>> table) => PossibleStates = table;
        public static Dictionary<string, Dictionary<string, string>> GetTable => PossibleStates;
        public static void ProcessCode(string input)
        {
            if (PossibleStates == null)
                throw new Exception("Finite state machine wasn't defined");
            var queue = new Queue<char>(input.ToCharArray()); //LIFO

            var currentState = PossibleStates.FirstOrDefault().Key;

            while (queue.Any())
            {
                var currentChar = queue.Dequeue();
                var currentType = currentChar.ToString();

                if (!PossibleStates.FirstOrDefault().Value.ContainsKey(currentType)) throw new Exception($"Invalid symbol: {currentType}");
                currentState = PossibleStates[currentState][currentType];
            }

            var result = PossibleStates[currentState]["Output"] == "1";
            Console.WriteLine(result ? "Validated" : "Not validated");
        }



        public static bool MinimizeAutomat()
        {
            var allowedSymbols = PossibleStates.FirstOrDefault()
                .Value.Select(val => val.Key)
                .Where(val => val != "Output")
                .ToList();
            //  1) Группировка
            //  1.1) 0-эквивалентность
            var grouping = PossibleStates.GroupBy(elem => elem.Value["Output"]).ToList();
            Console.WriteLine($"Current grouping: {string.Join(",", grouping.Select(group => $"{{{string.Join(",", group.Select(elem => elem.Key))}}}"))}");

            var currentSymbol = "";


            //  1.2) 1-эквивалентность
            foreach (var symbol in allowedSymbols)
            {
                currentSymbol = symbol;
                grouping = PossibleStates.GroupBy(arg =>
                {
                    var currentGroup = grouping.FirstOrDefault(gr => gr.Any(grElem => grElem.Key == arg.Key));
                    var currentGroupName = currentGroup?.Key;
                    var valid = PossibleStates[arg.Value[currentSymbol]]["Output"];
                    return $"{currentGroupName}_{currentSymbol}{valid}";
                }).ToList();
                Console.WriteLine($"Current grouping: {string.Join(",", grouping.Select(group => $"{{{string.Join(",", group.Select(elem => elem.Key))}}}"))}");
            }

            //  1.3) 2-эквивалентность
            grouping = PossibleStates.GroupBy(arg =>
            {
                var currentGroup = grouping.FirstOrDefault(gr => gr.Any(grElem => grElem.Key == arg.Key));
                var currentGroupName = currentGroup?.Key;
                var returnString = $"{currentGroupName}_";
                foreach (var symbol in allowedSymbols)
                {
                        
                    var group2 = grouping.FirstOrDefault(gr => gr.Any(grElem => grElem.Key == arg.Value[symbol]))?.Key;
                    returnString += $"{symbol}{group2}";
                }
                return returnString;
            })
            .ToList();

            if (grouping.All(group => group.Count() <= 1))
            {
                Console.WriteLine("Automat already minimized");
                return false;
            }
            Console.WriteLine($"Found similar states: {string.Join(",", grouping.Where(group => group.Count() > 1).Select(group => $"{{{string.Join(",", group.Select(elem => elem.Key))}}}"))}");

            //2) Замена 

            foreach (var group in grouping)
            {
                var statesToDelete = new List<string>();
                if (group.Count() <= 1) continue;
                var statesToMerge = group.Select(state => state.Key).ToList();
                var finalState = statesToMerge.FirstOrDefault();
                statesToMerge.Where(state => state != finalState).ToList().ForEach(state => statesToDelete.Add(state));
                var listToReplace = new List<string[]>();

                foreach (var state in PossibleStates)
                {
                    
                    foreach (var stateLink in state.Value)
                    {
                        if (stateLink.Key == "Output") continue;
                        if (statesToMerge.Contains(stateLink.Value) && finalState != stateLink.Value)
                            listToReplace.Add(new[] { state.Key, stateLink.Key, finalState });
                    }
                        
                }
                foreach (var item in listToReplace) PossibleStates[item[0]][item[1]] = item[2];
                foreach (var item in statesToDelete) PossibleStates.Remove(item);
            }
            
            
            Console.WriteLine("Successfully minimized!");
            return true;
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
