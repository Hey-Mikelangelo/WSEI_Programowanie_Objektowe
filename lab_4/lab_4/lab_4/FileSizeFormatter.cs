using System;

namespace lab_4
{
    public static class FileSizeFormatter
    {
        // Load all suffixes in an array  
        static readonly string[] suffixes =
        { "B", "KB", "MB", "GB", "TB", "PB" };
        public static string FormatSize(Int64 bytes)
        {
            int counter = 0;
            decimal number = (decimal)bytes;
            if(bytes == 0)
            {
                return $"0{suffixes[0]}";
            }
            while (Math.Round(number / 1024) >= 1)
            {
                number = number / 1024;
                counter++;
            }
            if(counter >= suffixes.Length)
            {
                return $"{bytes}B";
            }
            return string.Format("{0:n1}{1}", number, suffixes[counter]);
        }
    }
    

}
