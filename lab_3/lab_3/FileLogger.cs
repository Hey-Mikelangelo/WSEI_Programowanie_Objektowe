using System.IO;
using System.Text;

namespace ConsoleApp.Logger
{
    public class FileLogger : WriterLogger
    {
        private FileStream stream;
        private string fileName;
        private bool isDisposed;
        public FileLogger(string fileName)
        {
            this.fileName = fileName;

        }
        ~FileLogger(){
            Dispose();
        }

        public override void Log(params string[] messages)
        {
            stream = new FileStream(fileName, FileMode.Append);
            writer = new StreamWriter(stream, Encoding.UTF8);
            isDisposed = false;
            base.Log(messages);
            writer.Close();
            stream.Close();
        }

        public override void Dispose()
        {
            if (isDisposed) return;

            stream.Dispose();
            writer.Dispose();

            isDisposed = true;
        }
    }
}

