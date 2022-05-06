using System;

namespace ConsoleApp.Logger
{
    public interface ILogger : IDisposable
    {
        void Log(params String[] messages);
    }
}
