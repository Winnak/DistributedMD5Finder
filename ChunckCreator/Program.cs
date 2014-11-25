﻿using System;
using System.Collections.Generic;
using System.IO;

namespace ChunkCreator
{
    class Program
    {
        static void Main(string[] args)
        {
            int length = 6;
            char[] charset = new char[]
            { 
                'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm',
                'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' 
            };
            int chunks = 30;
            string dir = "Chunks";
            string prefix = string.Empty;
            

            //////////////////////////////////////////////////
            // PRINT ARGUEMNTS
            //////////////////////////////////////////////////
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
                Console.WriteLine("-s\tChunk Amount");
                Console.WriteLine("-l\tLength of password");
                Console.WriteLine("-d\tDirectory");
                Console.WriteLine("-pre\tPrefix");
                Console.WriteLine();
                Console.WriteLine();
            }
            else
            {
                Console.Write("Chunk Creator - ");
                Console.Write("Erik Høyrup Jørgensen - ");
                Console.WriteLine("Stejska.com - 2014");

                for (int i = 0; i < args.Length; i++)
                {
                    if (args[i] == "-c")
                    {
                        try
                        {
                            charset = File.ReadAllText(args[i + 1]).ToCharArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                            Console.ReadKey();
                            return;
                        }
                    }
                    else if (args[i] == "-s")
                    {
                        if (!int.TryParse(args[i + 1], out chunks))
                        {
                            Console.WriteLine("INVALID CHUNK AMOUNT");
                            Console.ReadKey();
                            return;
                        }
                    }
                    else if (args[i] == "-l")
                    {
                        if (!int.TryParse(args[i + 1], out length))
                        {
                            Console.WriteLine("INVALID PASSWORD LENGTH");
                            Console.ReadKey();
                            return;
                        }
                    }
                    else if (args[i] == "-d")
                    {
                        dir = args[i + 1];
                    }
                    else if (args[i] == "-pre")
                    {
                        prefix = args[i + 1];
                    }
                }
            }


            //////////////////////////////////////////////////
            // PRINT INFO
            //////////////////////////////////////////////////
            Console.WriteLine("Password length: " + charset.Length);
            Console.Write("Charset length:\t " + charset.Length + " { ");
            {
                string s = string.Empty;
                int i = 0;

                foreach (var c in charset)
                {
                    s += c + ", ";
                    i++;
                }

                s = s.Substring(0, s.Length - 2);

                Console.WriteLine(s + " }");
            }


            //////////////////////////////////////////////////
            // START PROCCESSING
            //////////////////////////////////////////////////
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            
            double passwords = Math.Pow(charset.Length, length);

            for (int i = 1; i < chunks + 1; i++)
            {
                //TODO: Add loading bar
                using (StreamWriter file = new StreamWriter(dir + "\\" + prefix + i + ".dat"))
                {
                    //Info
                    file.WriteLine("File generated by Chunk Creator");
                    file.WriteLine("Stejska.com - Erik Høyrup Jørgensen");
                    file.WriteLine(DateTime.UtcNow);
                    
                    // Charset
                    foreach (var c in charset)
                    {
                        file.Write(c);
                    }
                    file.Write("\n");

                    // Part
                    file.WriteLine(i + " of " + chunks);

                    // Linecount
                    file.WriteLine(7 + (int)passwords / chunks); // ikke korrekt men ikke nødvendigt

                    // Passwords
                    var chunk = (passwords / chunks) * (i - 1);
                    for (int n = (int)chunk; n < chunk + (passwords / chunks); n++)
                    {
                        file.WriteLine(GenerateString(n, charset, length));
                    }
                }
            }

            Console.WriteLine("\nDone");
        }

        static char[] GenerateString(int index, char[] charset, int length)
        {
            char[] result = new char[length];

            for (int i = 0; i < length; i++)
            {
                result[i] = charset[index % charset.Length];
                index /= charset.Length;
            }

            return result;
        }
    }
}
