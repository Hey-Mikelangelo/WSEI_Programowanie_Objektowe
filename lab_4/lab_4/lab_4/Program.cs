using System;
using System.ComponentModel;

namespace lab_4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Scanned folder path:");
            string path = Console.ReadLine();
            int filesToDisplayCount = TryParseStringToTypeUntillSuccess<int>("Files to display count:", (x) => x > 0);
            bool showAllExtensions = TryParseStringToTypeUntillSuccess<bool>("Show all extensions?:");

            DirectoryScanner directoryScanner = new DirectoryScanner();
            directoryScanner.Scan(path, maxFilesToShowCount: filesToDisplayCount, showAllExtensions: showAllExtensions);

        }

        private static T TryParseStringToTypeUntillSuccess<T>(string message, System.Func<T, bool> IsValid = null)
        {
            string inputS;
            T result = default;
            bool isParsing, isValid;
            do
            {
                Console.WriteLine(message);
                inputS = Console.ReadLine();
                isValid = false;
                object convertedValue = null;
                TypeConverter typeConverter = TypeDescriptor.GetConverter(typeof(T));
                try
                {
                    convertedValue = typeConverter.ConvertFromString(inputS);
                }
                catch
                {

                }
                isParsing = convertedValue != null;
                if (isParsing)
                {
                    result = (T)convertedValue;
                    isValid = (IsValid == null || IsValid(result));
                }
            }
            while (isParsing == false || isValid == false);
            return result;
        }
    }
   

}
