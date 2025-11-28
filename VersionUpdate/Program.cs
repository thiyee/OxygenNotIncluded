using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VersionUpdate
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Usage: VersionUpdate <path_to_mod_info.yaml>");
                return;
            }

            string filePath = args[0];

            if (File.Exists(filePath))
            {
                var lines = File.ReadAllLines(filePath);
                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i].StartsWith("version:"))
                    {
                        var version = lines[i].Split(':').Last().Trim(' ', '\"').Split('.').Select(n => int.Parse(n)).ToArray();
                        if (version.Length == 3)
                        {
                            version[2] += 1;
                            if (version[2] >= 100)
                            {
                                version[2] = 0;
                                version[1]++;
                            }
                            if (version[1] >= 100)
                            {
                                version[1] = 0;
                                version[0]++;
                            }

                            lines[i] = $"version: \"{version[0]}.{version[1]}.{version[2]}\"";
                        }
                        break;
                    }
                }

                File.WriteAllLines(filePath, lines);
            }
        }
    }
}