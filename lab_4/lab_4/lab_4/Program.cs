namespace lab_4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                System.Console.WriteLine("No path is provided");
                return;
            }
            string path = args[0];
            DirectoryScanner directoryScanner = new DirectoryScanner();
            directoryScanner.Scan(path, maxFilesToShowCount: 100, showAllExtensions: false);

        }
    }
    

}
