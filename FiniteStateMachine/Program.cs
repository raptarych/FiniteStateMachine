using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiniteStateMachine
{
    class Program
    {
        static void OnCommand(string input)
        {
            if (input.StartsWith("load "))
            {
                try
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
                            States.ProcessCode(code, line);
                        }
                        Console.WriteLine("File successfully validated");
                    }
                } catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                
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
