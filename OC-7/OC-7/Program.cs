using System;
using System.Collections.Generic;

namespace OC_7
{
    class Program
    {
        static void Main(string[] args)
        {
            IReadFile Iread = new ReadFile();
            string path = "D:\\VisualStudio2019\\test.txt"; //any txt or asm file will work properly
            List<String> tmp_data = Iread.openFileToRead(path);
            Dictionary<Int32, String> exceptions = Iread.checkDataForExs(tmp_data);
            Console.WriteLine("File output:");
            for (int i = 0; i < tmp_data.Count; i++)
                Console.WriteLine(tmp_data[i]);
            Console.WriteLine("\nErrors:");
            foreach (KeyValuePair<int, string> item in exceptions)
            {
                Console.WriteLine("String: {0}, {1}", item.Key+1, item.Value);
            }
            if (exceptions == null)
                Console.WriteLine("None");
        }
    }
}
