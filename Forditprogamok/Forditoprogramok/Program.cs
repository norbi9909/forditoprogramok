using Forditoprogramok;
using System;
using System.IO;

namespace filemanager
{

    class Program
    {
        static void Main(string[] args)
        {
            sourceHandler s = new sourceHandler(@"C:\Users\Norbi\Desktop\Forditprogamok\Forditoprogramok\bin\Debug\file1.txt", "file2.txt");
            s.openFileToRead();
            Console.WriteLine("Bemenet:");
            Console.WriteLine(s.Content);
            s.replaceContent();
            s.replaceFirst();
            Console.WriteLine("Kimenet:");
            Console.WriteLine(s.Content);
            s.openFileToWrite();
            Console.WriteLine("done");
            Console.ReadLine();

        }
    }
}
