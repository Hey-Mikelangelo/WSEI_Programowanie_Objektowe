using System;
using System.IO;

namespace ConsoleApp.Logger
{
    public abstract class WriterLogger : ILogger
    {
        protected TextWriter writer;

        public virtual void Log(params string[] messages)
        {
            writer.Write(DateTime.Now.ToString());
            for (int i = 0; i < messages.Length; i++)
            {
                writer.Write(messages[i]);
                writer.Write(" ");

            }
            writer.Write('\n');

            writer.Flush();
        }

        public abstract void Dispose();
    }
}


