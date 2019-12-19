using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using BibleReadingPlanGeneratorLib;

namespace BibleReadingPlanGeneratorCLI
{
    class Program
    {
        static void Main(string[] args)
        {
            string json = File.ReadAllText(args[0]);
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
            BibleSpec spec = 
                JsonSerializer.Deserialize<BibleSpec>(json, options);

            var matches = spec.FilterBooks("so");

            //List<List<int>> wordCounts = spec.CountsSpecs[0].WordCounts;
            //for (int i = 0; i < spec.Books.Count; i++)
            //{
            //    spec.Books[i].ChapterCount = wordCounts[i].Count;
            //}
            //File.WriteAllText(args[1], JsonSerializer.Serialize(spec.Books, options));
        }
    }
}
