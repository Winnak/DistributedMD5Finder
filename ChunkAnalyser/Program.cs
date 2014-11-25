using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChunkAnalyser
{
    class Program
    {
        static void Main(string[] args)
        {
            #region args
            if (args.Length == 0)
            {
                Console.Write("Chunk Creator - ");
                Console.Write("Erik Høyrup Jørgensen - ");
                Console.WriteLine("Stejska.com - 2014");
                Console.WriteLine();
                Console.WriteLine("Format: [OPTIONAL OPTIONS]");
                Console.WriteLine();
                Console.WriteLine("Options:");
                Console.WriteLine("-c\tChar set file path");
                Console.WriteLine("-s\tChunkSize");
                Console.WriteLine("-l\tLength of password");
                Console.WriteLine("-d\tdirectory");
                Console.ReadKey();
            }
            #endregion
        }
    }
}
