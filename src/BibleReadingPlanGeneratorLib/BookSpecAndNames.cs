using System;
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

        public override int GetHashCode()
        {
            return HashCode.Combine(BookSpec, Names);
        }

        public override bool Equals(object obj)
        {
            BookSpecAndNames that = obj as BookSpecAndNames;
            if (that == null)
            {
                return false;
            }
            return BookSpec.Equals(that.BookSpec) &&
                Names == that.Names;
        }
    }
}
