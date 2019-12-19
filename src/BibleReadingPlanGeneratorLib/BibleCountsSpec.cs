using System.Collections.Generic;
using Ardalis.GuardClauses;

namespace BibleReadingPlanGeneratorLib
{
    public class BibleCountsSpec
    {
        public string Name { get; set; } = "";

        public string Abbreviation { get; set; } = "";

        public List<List<int>> WordCounts { get; set; } = new List<List<int>>();

        public List<List<int>> CharacterCounts { get; set; } = new List<List<int>>();

        public BibleCountsSpec()
        {

        }

        public BibleCountsSpec(string name, string abbreviation, List<List<int>> wordCounts, List<List<int>> characterCounts)
        {
            Guard.Against.Null(name, nameof(name));
            Name = name;
            Guard.Against.Null(abbreviation, nameof(abbreviation));
            Abbreviation = abbreviation;
            Guard.Against.NullOrEmpty(wordCounts, nameof(wordCounts));
            WordCounts = wordCounts;
            Guard.Against.NullOrEmpty(characterCounts, nameof(characterCounts));
            CharacterCounts = characterCounts;
        }
    }
}
