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
            string json = File.ReadAllText(args[0]);
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
            BibleSpec spec = 
                JsonSerializer.Deserialize<BibleSpec>(json, options);

            while (true)
            {
                Console.WriteLine("Enter a section spec:");
                string input = Console.ReadLine();
                if (input.Trim().ToLower() == "exit")
                {
                    break;
                }
                SectionParseResult result = spec.ParseSection(input);
                if (result.SectionSpec != null)
                {
                    Console.WriteLine(JsonSerializer.Serialize(result.SectionSpec, options));
                }
                else
                {
                    foreach (ParseError error in result.Errors)
                    {
                        Console.WriteLine("Error: " + error.Text + ": " + error.ErrorText);
                    }
                }
                Console.WriteLine();
            }

            //List<List<int>> wordCounts = spec.CountsSpecs[0].WordCounts;
            //for (int i = 0; i < spec.Books.Count; i++)
            //{
            //    spec.Books[i].ChapterCount = wordCounts[i].Count;
            //}
            //File.WriteAllText(args[1], JsonSerializer.Serialize(spec.Books, options));
        }
    }
}
