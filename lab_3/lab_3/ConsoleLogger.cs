using System;

namespace ConsoleApp.Logger
{
    public class ConsoleLogger : WriterLogger
    {
        public ConsoleLogger()
        {
            writer = Console.Out;
        }

        public override void Dispose()
        {
            return; //No dispose for Console log
        }
    }
}

