using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

// TODO: Could use threading :)

namespace ChunkAnalyser
{
    class Program
    {
        static void Main(string[] args)
        {
            string result = string.Empty;
            byte[] target = new byte[16];

            if (args.Length >= 2)
            {
                if (File.Exists(args[0]))
                {
                    Console.WriteLine("File (" + args[0] + ") found");
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
                Console.Write("Chunk Analyser - ");
                Console.Write("Stejska.com - Erik Høyrup Jørgensen - 2014");
                Console.WriteLine();
                Console.WriteLine("Format: [FILE] [TARGET] [OPTIONAL OPTIONS]");
                Console.WriteLine();
                Console.WriteLine("Options:");
                Console.WriteLine("None so far");
                return;
            }

            using (StreamReader reader = new StreamReader(args[0]))
            {
                Console.WriteLine("\nFile header:");
                Console.WriteLine(reader.ReadLine());
                Console.WriteLine(reader.ReadLine());
                Console.WriteLine("Generated at: " + reader.ReadLine());
                reader.ReadLine();
                Console.WriteLine("Part " + reader.ReadLine());
                
                int end;
                if (int.TryParse(reader.ReadLine(), out end))
                    Console.WriteLine("Found " + end + " passwords");
                else
                    Console.WriteLine("ERROR: could not find header-pass-count");
                Console.WriteLine();
                Console.WriteLine("Analysing");
                
#if DEBUG
                int current = 0;
#endif
                using (MD5 md5 = MD5.Create())
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
#if DEBUG
                        current++;
                        Console.CursorTop = 11;
                        Console.WriteLine(current + "/" + end + "  -  " + (((float)current / end) * 100) + " %");
#endif

                        if (target.SequenceEqual(md5.ComputeHash(Encoding.UTF8.GetBytes(line))))
                        {
                            Console.WriteLine("HASH FOUND!");
                            Console.WriteLine("Key: " + line);
                            Console.WriteLine("Found at: " + DateTime.UtcNow);
                            Console.WriteLine("Save key to file? [Y/n]");
                            if (Console.ReadKey().Key == ConsoleKey.Y)
                            {
                                using(StreamWriter writer = new StreamWriter("keys.txt", true))
                                {
                                    writer.Write(DateTime.UtcNow + ":" + writer.NewLine);
                                    writer.Write("\t" + args[1] + " = " + line + writer.NewLine);
                                }
                            }
                            return;
                        }
                    }
                }
            }

            Console.WriteLine("Hash not found in dataset");
        }
    }
}
