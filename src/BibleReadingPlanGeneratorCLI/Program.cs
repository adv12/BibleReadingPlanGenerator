using System;
using System.IO;
using System.Text.Json;
using BibleReadingPlanGeneratorLib;

namespace BibleReadingPlanGeneratorCLI
{
    class Program
    {
        static void Main(string[] args)
        {
            string countsJson = File.ReadAllText(args[0]);
            BibleCountsSpec[] countsSpec =
                JsonSerializer.Deserialize<BibleCountsSpec[]>(countsJson);
        }
    }
}
