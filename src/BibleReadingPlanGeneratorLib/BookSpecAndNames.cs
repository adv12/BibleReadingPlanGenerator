using System.Collections.Generic;
using Ardalis.GuardClauses;

namespace BibleReadingPlanGeneratorLib
{
    public class BookSpecAndNames
    {
        public BookSpec BookSpec { get; set; }

        public List<string> Names { get; set; } = new List<string>();

        public BookSpecAndNames(BookSpec bookSpec, string name)
        {
            Guard.Against.Null(bookSpec, nameof(bookSpec));
            BookSpec = bookSpec;
            Guard.Against.Null(name, nameof(name));
            Names.Add(name);
        }

        public BookSpecAndNames(BookSpec bookSpec, IEnumerable<string> names)
        {
            Guard.Against.Null(bookSpec, nameof(bookSpec));
            BookSpec = bookSpec;
            Guard.Against.NullOrEmpty(names, nameof(names));
            Names.AddRange(names);
        }
    }
}
