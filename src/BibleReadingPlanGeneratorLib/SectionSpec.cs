using Ardalis.GuardClauses;

namespace BibleReadingPlanGeneratorLib
{
    public class SectionSpec
    {
        public BookAndChapterSpec Start { get; set; }

        public BookAndChapterSpec End { get; set; }

        public SectionSpec()
        {
            
        }

        public SectionSpec(BookAndChapterSpec start, BookAndChapterSpec end)
        {
            Guard.Against.Null(start, nameof(start));
            Start = start;
            Guard.Against.Null(end, nameof(end));
            End = end;
        }

        public void SetBibleSpec(BibleSpec spec)
        {
            Start.BibleSpec = End.BibleSpec = spec;
        }

        public override string ToString()
        {
            return "" + Start?.ToString() + " - " + End?.ToString();
        }
    }
}
