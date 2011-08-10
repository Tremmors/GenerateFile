// Copyright (c) 2011 John Trimis 
//
// MIT license:
// Permission is hereby granted, free of charge, to any person obtaining a copy of 
// this software and associated documentation files (the "Software"), to deal in 
// the Software without restriction, including without limitation the rights to 
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of 
// the Software, and to permit persons to whom the Software is furnished to do so, 
// subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all 
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS 
// FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR 
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER 
// IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN 
// CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

namespace GenerateFile
{
    class Program
    {
        static void Main(string[] args)
        {   // Main

            if (args.Length != 2)
            {   // Invalid length of arguments

                HowDoIUseThis();

            }   // Invalid length of arguments
            else
            {   // Proper number of arguments

                // 256 bytes of data
                string data = "0123456789ABCDEF0123456789ABCDEF0123456789ABCDEF0123456789ABCDEF" +
                    "0123456789ABCDEF0123456789ABCDEF0123456789ABCDEF0123456789ABCDEF" +
                    "0123456789ABCDEF0123456789ABCDEF0123456789ABCDEF0123456789ABCDEF" +
                    "0123456789ABCDEF0123456789ABCDEF0123456789ABCDEF0123456789ABCDEF";

                byte[] buffer = Encoding.ASCII.GetBytes(data);

                string OutputFileName = args[0];
                long size = GetBytes(args[1]);
                using ( System.IO.FileStream fileOutput = new System.IO.FileStream( OutputFileName, System.IO.FileMode.Create ) )
                {

                    for ( long i = 0 ; i < size ; i = i + buffer.Length )
                    {   // Take 256byte steps

                        int ToWrite = buffer.Length;
                        if (i + buffer.Length > size)
                        {

                            ToWrite = (int)(size - i);

                        }
                        fileOutput.Write(buffer, 0, ToWrite);

                    }   // Take 256byte steps
                    fileOutput.Flush();
                    fileOutput.Close();
                }
                Console.WriteLine(string.Format("File '{0}' (size:{1}) written successfully", OutputFileName, size));

            }   // Proper number of arguments

        }   // Main

        static void HowDoIUseThis()
        {   // HowDoIUseThis

            Console.WriteLine("GenerateFile.exe {FileName} {Size}");
            Console.WriteLine("     FileName   The filename of the file being split or combined");
            Console.WriteLine("     Size   How big");
            Console.WriteLine("     Unit   What unit of measurement (case insensitive)");
            Console.WriteLine("         B         = Bytes");
            Console.WriteLine("         BYTES     = Bytes");
            Console.WriteLine("         K         = KiloBytes");
            Console.WriteLine("         KB        = KiloBytes");
            Console.WriteLine("         KILOBYTES = KiloBytes");
            Console.WriteLine("         M         = MegaBytes");
            Console.WriteLine("         MB        = MegaBytes");
            Console.WriteLine("         MEG       = MegaBytes");
            Console.WriteLine("         MEGS      = MegaBytes");
            Console.WriteLine("         MEGABYTES = MegaBytes");
            Console.WriteLine("         G         = GigaBytes");
            Console.WriteLine("         GB        = GigaBytes");
            Console.WriteLine("         GIG       = GigaBytes");
            Console.WriteLine("         GIGS      = GigaBytes");
            Console.WriteLine("         GIGABYTES = GigaBytes");

        }   // HowDoIUseThis

        /// <summary>
        ///     Get a long denoting how many bytes are represented by a given string
        /// </summary>
        /// <param name="ChunkSize">
        ///     A string like:
        ///         123kb
        ///         1Meg
        ///         10bytes
        ///         etc.
        /// </param>
        /// <returns>
        ///     How many bytes that represents.
        /// </returns>
        public static long GetBytes(string ChunkSize)
        {   // GetBytes

            long output = -1;
            Regex r = new Regex("^([0-9]+)([a-zA-Z]+)");
            Match mc = r.Match(ChunkSize);
            if (mc.Groups.Count == 3)
            {   // Scalar + Unit

                long scalar;
                long Multiplier = 1;
                if (long.TryParse(mc.Groups[1].Value, out scalar))
                {   // Scalar parsable

                    switch (mc.Groups[2].Value)
                    {   // Switch for the Unit

                        case "K":
                        case "KB":
                        case "KILOBYTES":
                            Multiplier = 1024;
                            break;
                        case "M":
                        case "MB":
                        case "MEG":
                        case "MEGS":
                        case "MEGABYTES":
                            Multiplier = 1024 * 1024;
                            break;
                        case "G":
                        case "GB":
                        case "GIG":
                        case "GIGS":
                        case "GIGABYTES":
                            Multiplier = 1024 * 1024 * 1024;
                            break;

                    }   // Switch for the Unit
                    output = scalar * Multiplier;

                }   // Scalar parsable

            }   // Scalar + Unit
            return output;

        }   // GetBytes
    }
}
