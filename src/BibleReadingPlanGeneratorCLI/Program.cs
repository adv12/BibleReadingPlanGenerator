using System;
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

            Console.WriteLine("Enter a name for your Bible reading plan.");
            string name = Console.ReadLine();
            Console.WriteLine();
            int days = 0;
            while (days == 0)
            {
                Console.WriteLine("How many days will this plan span?");
                string daysString = Console.ReadLine().Trim();
                bool parsed = int.TryParse(daysString, out days);
                if (!parsed || days == 0)
                {
                    Console.WriteLine("Please enter a number greater than 0.");
                }
                Console.WriteLine();
            }
            List<GroupSpec> groupSpecs = new List<GroupSpec>();
            while (true)
            {
                GroupSpec groupSpec = GetGroup(spec);
                if (groupSpec == null)
                {
                    break;
                }
                if (groupSpec.Sections.Count > 0)
                {
                    groupSpecs.Add(groupSpec);
                }
            }
            PlanSpec planSpec = new PlanSpec(name, days, groupSpecs);
            Console.WriteLine(planSpec);
            Console.WriteLine();
            Console.WriteLine("Save this plan spec to a file? (Y/N)");
            string yesno = Console.ReadLine();
            Console.WriteLine();
            if (yesno.Trim().ToLower().StartsWith("y"))
            {
                Console.WriteLine("Enter a filename.");
                string filename = Console.ReadLine().Trim();
                json = JsonSerializer.Serialize(planSpec, options);
                File.WriteAllText(filename, json);
            }
        }

        static GroupSpec GetGroup(BibleSpec bibleSpec)
        {
            GroupSpec groupSpec = null;
            Console.WriteLine("Enter the name of a group to add or 'done' to stop adding groups.");
            string name = Console.ReadLine();
            Console.WriteLine();
            if (name.Trim().ToLower() == "done")
            {
                Console.WriteLine();
                return null;
            }
            List<SectionSpec> sectionSpecs = new List<SectionSpec>();
            while (true)
            {
                Console.WriteLine("Enter a Scripture range to add to this group or 'done' to stop adding ranges.");
                string input = Console.ReadLine();
                if (input.Trim().ToLower() == "done")
                {
                    Console.WriteLine();
                    break;
                }
                SectionParseResult result = bibleSpec.ParseSection(input);
                if (result.SectionSpec != null)
                {
                    Console.WriteLine("Added " + result.SectionSpec.ToString());
                    sectionSpecs.Add(result.SectionSpec);
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
            if (sectionSpecs.Count > 0)
            {
                int reps = 0;
                while (reps == 0)
                {
                    Console.WriteLine("Enter the number of repetitions for this group.");
                    string repsString = Console.ReadLine().Trim();
                    bool parsed = int.TryParse(repsString, out reps);
                    if (!parsed || reps == 0)
                    {
                        Console.WriteLine("Please enter a number greater than 0.");
                    }
                    Console.WriteLine();
                }
                groupSpec = new GroupSpec(name, sectionSpecs, reps);
            }
            return groupSpec;
        }
    }
}
