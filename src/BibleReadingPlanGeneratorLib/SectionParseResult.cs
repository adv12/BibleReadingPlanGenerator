using System.Collections.Generic;
using System.Linq;

namespace BibleReadingPlanGeneratorLib
{
    public class SectionParseResult
    {
        public SectionSpec SectionSpec { get; }

        public ParseError[] Errors { get; }

        public SectionParseResult(SectionSpec sectionSpec, IEnumerable<ParseError> errors)
        {
            SectionSpec = sectionSpec;
            if (errors != null)
            {
                Errors = errors.ToArray();
            }
        }
    }
}
