using System.Text;

namespace ConsoleApp.Logger
{
    public class SocketLogger : ILogger
    {
        private ClientSocket socket;
        public SocketLogger(string host, int port)
        {
            socket = new ClientSocket(host, port);
        }

        ~SocketLogger()
        {
            Dispose();
        }

        public void Log(params string[] messages)
        {
            string requestText = messages.ToString();
            byte[] requestBytes = Encoding.UTF8.GetBytes(requestText);

            socket.Send(requestBytes);
        }

        public void Dispose()
        {
            socket.Dispose();
        }
    }
}


