using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Ardalis.GuardClauses;

namespace BibleReadingPlanGeneratorLib
{
    public class BibleSpec
    {
        public string Name { get; set; } = "";

        public List<BookSpec> Books { get; set; } = new List<BookSpec>();

        public List<BibleCountsSpec> CountsSpecs { get; set; } = new List<BibleCountsSpec>();

        public BibleSpec()
        {

        }

        public BibleSpec(string name, List<BookSpec> books, List<BibleCountsSpec> countSpecs)
        {
            Guard.Against.Null(name, nameof(name));
            Name = name;
            Guard.Against.NullOrEmpty(books, nameof(books));
            Books = books;
            Guard.Against.NullOrEmpty(countSpecs, nameof(countSpecs));
            CountsSpecs = countSpecs;
        }

        public List<BookSpecAndNames> FilterBooks(string searchText)
        {
            searchText = Regex.Replace(searchText.ToLower(), @"[^\w]", "").Replace("_", "");
            List<BookSpecAndNames> matches = new List<BookSpecAndNames>();
            if (searchText.Length == 0)
            {
                foreach (BookSpec book in Books)
                {
                    matches.Add(new BookSpecAndNames(book, book.Names[0]));
                }
                return matches;
            }
            bool startsWith = false;
            BookSpecAndNames last = null;
            foreach (BookSpec book in Books)
            {
                foreach (string name in book.Names)
                {
                    string canonicalName = Regex.Replace(name.ToLower(), @"[^\w]", "").Replace("_", "");
                    if (canonicalName.StartsWith(searchText))
                    {
                        if (!startsWith)
                        {
                            startsWith = true;
                            matches.Clear();
                        }
                        AddBookAndName(matches, ref last, book, name);
                    }
                    else if (!startsWith && canonicalName[0] == searchText[0])
                    {
                        int nameIndex = 0;
                        int lettersMatched = 0;
                        for (int searchIndex = 0; searchIndex < searchText.Length; searchIndex++)
                        {
                            for (int i = nameIndex; i < canonicalName.Length; i++)
                            {
                                if (canonicalName[i] == searchText[searchIndex])
                                {
                                    lettersMatched += 1;
                                    nameIndex = i;
                                    break;
                                }
                            }
                        }
                        if (lettersMatched == searchText.Length)
                        {
                            AddBookAndName(matches, ref last, book, name);
                        }
                    }
                }
            }
            return matches;
        }

        public void AddBookAndName(List<BookSpecAndNames> matches,
            ref BookSpecAndNames last, BookSpec book, string name)
        {
            if (last?.BookSpec == book)
            {
                last.Names.Add(name);
            }
            else
            {
                last = new BookSpecAndNames(book, name);
                matches.Add(last);
            }
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Books, CountsSpecs);
        }

        public override bool Equals(object obj)
        {
            BibleSpec that = obj as BibleSpec;
            return Name == that.Name &&
                Books == that.Books &&
                CountsSpecs == that.CountsSpecs;
        }

    }
}
