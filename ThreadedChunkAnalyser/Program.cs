using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;


namespace ThreadedChunkAnalyser
{
    class Program
    {
        static byte[] target = new byte[16];
        static bool found = false;

        static void Main(string[] args)
        {
            int threads = Environment.ProcessorCount;
            Queue<string> files = new Queue<string>();

            if (args.Length >= 2)
            {
                if (Directory.Exists(args[0]))
                {
                    Console.Write("Directory (" + args[0] + ") found, ");
                    
                    foreach (string file in Directory.GetFiles(args[0]))
                    {
                        files.Enqueue(file);
                    }

                    Console.WriteLine(files.Count + " files queued");
                }
                else
                {
                    Console.WriteLine("File not found\n");
                    return;
                }

                Console.Write("Target hash: ");
                for (int i = 0; i < 16; i++)
                {
                    target[i] = (byte)Convert.ToInt32(args[1].Substring(i * 2, 2), 16);
                    Console.Write((args[1].Substring(i * 2, 2)) + " ");
                }

                Console.WriteLine();
            }
            else
            {
                Console.Write("Threaded Chunk Analyser - ");
                Console.Write("Stejska.com - Erik Høyrup Jørgensen - 2014");
                Console.WriteLine();
                Console.WriteLine("Format: [DIRECTORY] [TARGET] [OPTIONAL OPTIONS]");
                Console.WriteLine();
                Console.WriteLine("Options:");
                Console.WriteLine("-t\t1 to" + threads + "\t(Default = " + threads + ")");
                return;
            }

            Thread[] activeThreads = new Thread[threads];
            
            for (int i = 0; i < threads; i++)
            {
                activeThreads[i] = new Thread(Solve);
            }

            while (!found && files.Count > 0)
            {
                for (int i = 0; i < threads; i++)
                {
                    if (!activeThreads[i].IsAlive)
                    {
                        activeThreads[i] = new Thread(Solve);
                        activeThreads[i].Start(files.Dequeue());
                    }
                }
            }
        }

        private static void Solve(object file)
        {
            string filename = (string)file;

            using (StreamReader reader = new StreamReader(filename))
            {
                reader.ReadLine();
                reader.ReadLine();
                reader.ReadLine();
                reader.ReadLine();
                var bam = reader.ReadLine();
                Console.WriteLine("Analysing: Part " + bam);

                int end;
                int.TryParse(reader.ReadLine(), out end);

                using (MD5 md5 = MD5.Create())
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {

                        if (target.SequenceEqual(md5.ComputeHash(Encoding.UTF8.GetBytes(line))))
                        {
                            Console.WriteLine("HASH FOUND! (" + bam + ")");
                            Console.WriteLine("Key: " + line);
                            Console.WriteLine("Found at: " + DateTime.UtcNow);
                            Console.WriteLine("Save key to file? [Y/n]");
                            if (Console.ReadKey().Key == ConsoleKey.Y)
                            {
                                using (StreamWriter writer = new StreamWriter("keys.txt", true))
                                {
                                    writer.Write(DateTime.UtcNow + ":" + writer.NewLine);
                                    writer.Write("\t" + target + " = " + line + writer.NewLine);
                                }
                            }
                            found = true;
                        }
                    }
                }
            }
        }
    }
}
