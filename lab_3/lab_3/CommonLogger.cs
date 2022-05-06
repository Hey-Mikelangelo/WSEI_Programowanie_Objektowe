namespace ConsoleApp.Logger
{
    public class CommonLogger : ILogger
    {
        private ILogger[] loggers;
        public CommonLogger(params ILogger[] loggers)
        {
            this.loggers = loggers;
        }

        public void Dispose()
        {
            for (int i = 0; i < loggers.Length; i++)
            {
                loggers[i].Dispose();
            }
        }

        public void Log(params string[] messages)
        {
            for (int i = 0; i < loggers.Length; i++)
            {
                loggers[i].Log(messages);
            }
        }
    }
}


