using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sspe;
using System.Diagnostics;

namespace Sspe.DebuggingService
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Small Sensor Publshing Engine - Debugging Service");
            Engine engine = null;
            try
            {
                engine = new Engine();
                engine.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            Console.WriteLine("Enter on an empty line to exit.");
            Console.Write("> ");
            Console.ReadLine();

            if (engine != null)
                engine.Stop();
            Trace.Flush();
        }
    }
}
