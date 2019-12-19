using System.Collections.Generic;
using Ardalis.GuardClauses;

namespace BibleReadingPlanGeneratorLib
{
    public class BookSpec
    {
        public List<string> Names { get; set; } = new List<string>();

        public int ChapterCount { get; set; } = 1;

        public BookSpec()
        {

        }

        public BookSpec(IEnumerable<string> names, int chapterCount)
        {
            Guard.Against.NullOrEmpty(names, nameof(names));
            Names.AddRange(names);
            Guard.Against.OutOfRange(chapterCount, nameof(chapterCount), 1, int.MaxValue);
            ChapterCount = chapterCount;
        }
    }
}
