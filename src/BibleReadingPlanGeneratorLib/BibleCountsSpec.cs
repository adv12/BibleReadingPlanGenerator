using System;
using System.Collections.Generic;

namespace BibleReadingPlanGeneratorLib
{
    public class BibleCountsSpec
    {
        public string Abbreviation { get; set; }

        public string Name { get; set; }

        public List<List<int>> CharacterCounts { get; set; }

        public List<List<int>> WordCounts { get; set; }
    }
}
