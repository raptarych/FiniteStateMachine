using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FiniteStateMachine
{
    class Program
    {
        static void LoadText(string input)
        {

            input = input.Substring(5, input.Length - 5);
            if (!input.Contains(".")) input += ".txt";
            if (!File.Exists(input)) throw new Exception("File not found");
            var line = 0;
            using (var fileStream = File.OpenText(input))
            {
                while (!fileStream.EndOfStream)
                {
                    line++;
                    var code = fileStream.ReadLine();
                    if (code.ToLower().StartsWith("int")) code = code.Substring(3, code.Length - 3).Trim(' ');
                    FiniteStates.ProcessCode(code);
                }
                Console.WriteLine("File successfully validated");
            }
        }

        static void ImportAutomat(string input)
        {
            input = input.Substring(7, input.Length - 7);
            if (!input.Contains(".")) input += ".xls";
            if (!File.Exists(input))
            {
                if (!File.Exists(input + "x"))
                    throw new Exception("File not found");
                input += "x";
            }

            var xlsWorker = new XlsWorker();
            var table = xlsWorker.Read(input);

            FiniteStates.SetTable(table);
            FiniteStates.MinimizeAutomat();
            xlsWorker.Write(new string(input.TakeWhile(ch => ch != '.').ToArray()) + "_optimized.xls", FiniteStates.GetTable);
            Console.WriteLine("Finite state automate successfully loaded");
        }
        static void OnCommand(string input)
        {
            try
            {
                if (input.StartsWith("load "))
                {
                    LoadText(input);
                    return;
                }

                if (input.StartsWith("import "))
                {
                    ImportAutomat(input);
                    return;
                }
                FiniteStates.ProcessCode(input);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        static void Main(string[] args)
        {
            var consoleHandler = new ConWorker();
            consoleHandler.AddHandler(OnCommand);
            consoleHandler.Start();
        }
    }
}
