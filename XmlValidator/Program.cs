using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace XmlValidator
{
    class Program
    {
        static int Main(string[] args)
        {
            // aggregator for file validity
            bool allValid = true;

            // need at least schema and xml file
            if (args.Length < 2)
            {
                Console.WriteLine("usage: XmlValidator.exe <schema> <file1> [<file2> ...]");
                return 1;
            }

            // check existence of schema file separately
            if (!File.Exists(args[0]))
            {
                Console.WriteLine("error: file '{0}' does not exist", args[0]);
                return 1;
            }

            // spacing purely for aesthetics :)
            Console.WriteLine("");

            // get arg list and expand wildcards
            List<string> fileList = new List<string>();
            fileList.AddRange(args.Skip(1));
            doExpansion(fileList);

            // process every unique filename
            foreach (string file in fileList.Distinct().ToList())
            {
                if (!File.Exists(file))
                {
                    // not throwing an error, because we might have other files to process
                    Console.WriteLine("\r\n{0}: file does not exist!\r\n", file);
                    continue;
                }

                // attempt validation
                XmlValidator validator = new XmlValidator();
                if (validator.Validate(args[0], file))
                {
                    Console.WriteLine("{0}: validation succeeded", file);
                }
                else
                {
                    Console.WriteLine("\r\n{0}: validation failed {1}\r\n", file, validator.ErrorMessage);
                    allValid = false;
                }
            }

            return allValid ? 0 : 1;
        }

        /// <summary>
        /// Expand all wildcard strings into file names
        /// </summary>
        private static void doExpansion(List<string> filenames)
        {
            List<string> expandedNames = new List<string>();
            foreach (string wildcardString in filenames.Where<string>(name => name.Contains("*")))
                expandedNames.AddRange(Directory.EnumerateFiles(".", wildcardString, SearchOption.TopDirectoryOnly));

            filenames.RemoveAll(name => name.Contains("*"));
            filenames.AddRange(expandedNames);
        }
    }
}
