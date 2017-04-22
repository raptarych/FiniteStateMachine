using System;

namespace FiniteStateMachine
{
    class ConWorker
    {
        protected string InputString { get; set; }
        public delegate void ProcessInput(string input);

        private ProcessInput InputFunc;
        public void Start()
        {
            Console.WriteLine("Enter command:");
            while (true)
            {
                if (InputString == "exit") break;
                Console.Write(">");
                InputString = Console.ReadLine();
                InputFunc(InputString);
            }
        }

        public void AddHandler(ProcessInput inputFunc)
        {
            InputFunc += inputFunc;
        }
    }
}
