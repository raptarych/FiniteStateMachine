using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiniteStateMachine
{
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
        public Dictionary<CharState, int[9]> PossibleStates = new Dictionary<CharState, int[9]>()
        {
            { CharState.UndefinedIdentificator, new int[9] {0, 0, 0, 4, 0, 4, 0, 0, 0} },
            { CharState.OpenScope, new int[9] {0, 0, 3, 0, 0, 0, 0, 0, 0} },
            { CharState.Const, new int[9] {0, 0, 0, 4, 0, 4, 0, 0, 0} },
            { CharState.InnerComma, new int[9] {0, 2, 0, 6, 0, 6, 0, 0, 2} },
            { CharState.Space, new int[9] {0, 2, 0, 6, 0, 6, 0, 0, 2} },
            { CharState.Space, new int[9] {0, 2, 0, 6, 0, 6, 0, 0, 2} },
            { CharState.Space, new int[9] {0, 2, 0, 6, 0, 6, 0, 0, 2} },
        };
        static public void QueryCode(string input)
        {
            var queue = new Queue<char>(input.ToCharArray()); //LIFO
            CharState currentState;
            while (queue.Any())
            {
                var currentChar = queue.Dequeue();

            }
        }
    }
}
