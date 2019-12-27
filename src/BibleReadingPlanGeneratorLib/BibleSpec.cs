using System.Collections.Generic;
using System.Text;
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

        public SectionParseResult ParseSection(string text)
        {
            List<ParseError> errors = new List<ParseError>();
            SectionSpec spec = null;
            if (text.Contains("-"))
            {
                string[] pieces = text.Split("-");
                if (pieces.Length > 2)
                {
                    errors.Add(new ParseError(text, "Too many dashes"));
                }
                if (Regex.IsMatch(pieces[1], @"[^\d\s]"))
                {
                    // two separate books
                    BookAndChapterSpec start = ParseBookAndChapter(pieces[0],
                        EndpointType.Start, errors);
                    BookAndChapterSpec end = ParseBookAndChapter(pieces[1],
                        EndpointType.End, errors);
                    if (start != null && end != null)
                    {
                        spec = new SectionSpec(start, end);
                    }
                }
                else if (pieces.Length == 2)
                {
                    // single book range
                    spec = ParseSingleBookRange(text, errors);
                }
            }
            else
            {
                if (Regex.IsMatch(text, @"\d\s*$"))
                {
                    BookAndChapterSpec start = ParseBookAndChapter(text, EndpointType.Start, errors);
                    spec = new SectionSpec(start, start);
                }
                else
                {
                    BookSpec book = ParseBook(text, errors);
                    if (book != null)
                    {
                        BookAndChapterSpec start = new BookAndChapterSpec(this, book.Names[0], 1);
                        BookAndChapterSpec end = new BookAndChapterSpec(this, book.Names[0], book.ChapterCount);
                        spec = new SectionSpec(start, end);
                    }
                }
            }
            if (spec != null)
            {
                ValidateSectionSpec(text, spec, errors);
            }
            if (errors.Count > 0)
            {
                return new SectionParseResult(null, errors);
            }
            else
            {
                return new SectionParseResult(spec, errors);
            }
        }

        public SectionSpec ParseSingleBookRange(string text, List<ParseError> errors)
        {
            Match match = Regex.Match(text, @"^\s*(\d*\s*\p{L}+)\s*((\d+)(\s*-\s*(\d+))*)*\s*$");
            if (!match.Success)
            {
                errors.Add(new ParseError(text, "Not a single book range"));
                return null;
            }
            string bookSearchText = match.Groups[1].Value;
            BookSpec bookSpec = ParseBook(bookSearchText, errors);
            if (bookSpec == null)
            {
                return null;
            }
            string bookName = bookSpec.Names[0];
            if (!match.Groups[2].Success)
            {
                // just book specified
                BookAndChapterSpec start = new BookAndChapterSpec(this, bookName, 1);
                BookAndChapterSpec end = new BookAndChapterSpec(this, bookName, bookSpec.ChapterCount);
                return new SectionSpec(start, end);
            }
            else if (!match.Groups[4].Success)
            {
                // just book and chapter specified
                string strval = match.Groups[3].Value;
                int chapterNumber = int.Parse(strval);
                if (chapterNumber < 1 || chapterNumber > bookSpec.ChapterCount)
                {
                    errors.Add(new ParseError(strval, "Chapter number out of range"));
                    return null;
                }
                BookAndChapterSpec startAndEnd = new BookAndChapterSpec(this, bookName, chapterNumber);
                return new SectionSpec(startAndEnd, startAndEnd);
            }
            else if (match.Groups[5].Success)
            {
                bool valid = true;
                string startString = match.Groups[3].Value;
                int startChapter = int.Parse(startString);
                if (startChapter < 1 || startChapter > bookSpec.ChapterCount)
                {
                    errors.Add(new ParseError(startString, "Start chapter number out of range"));
                    valid = false;
                }
                string endString = match.Groups[5].Value;
                int endChapter = int.Parse(endString);
                if (endChapter < 1 || endChapter > bookSpec.ChapterCount)
                {
                    errors.Add(new ParseError(endString, "End chapter number out of range"));
                    valid = false;
                }
                if (!valid)
                {
                    return null;
                }
                BookAndChapterSpec start = new BookAndChapterSpec(this, bookName, startChapter);
                BookAndChapterSpec end = new BookAndChapterSpec(this, bookName, endChapter);
                return new SectionSpec(start, end);
            }
            return null;
        }

        public BookAndChapterSpec ParseBookAndChapter(string text, EndpointType type, List<ParseError> errors)
        {
            Match match = Regex.Match(text, @"^\s*(\d*\s*\p{L}+)\s*(\d+)*\s*$");
            if (!match.Success)
            {
                errors.Add(new ParseError(text, "Not a book/book+chapter"));
                return null;
            }
            string bookSearchText = match.Groups[1].Value;
            BookSpec bookSpec = ParseBook(bookSearchText, errors);
            if (bookSpec == null)
            {
                return null;
            }
            if (!match.Groups[2].Success)
            {
                int chapterNumber = 1;
                if (type == EndpointType.End)
                {
                    chapterNumber = bookSpec.ChapterCount;
                }
                return new BookAndChapterSpec(this, bookSpec.Names[0], chapterNumber);
            }
            else
            {
                string strval = match.Groups[2].Value;
                int chapterNumber = int.Parse(strval);
                if (chapterNumber < 1 || chapterNumber > bookSpec.ChapterCount)
                {
                    errors.Add(new ParseError(strval, "Chapter number out of range"));
                    return null;
                }
                return new BookAndChapterSpec(this, bookSpec.Names[0], chapterNumber);
            }
        }

        public BookSpec ParseBook(string text, List<ParseError> errors)
        {
            List<BookSpecAndNames> matches = FilterBooks(text);
            if (matches.Count == 0)
            {
                errors.Add(new ParseError(text, "No matching books"));
                return null;
            }
            BookSpec bookSpec = matches[0].BookSpec;
            StringBuilder sb = new StringBuilder(matches[0].Names[0]);
            bool multiple = false;
            for (int i = 0; i < matches.Count; i++)
            {
                if (matches[i].BookSpec != bookSpec)
                {
                    sb.Append(", ");
                    sb.Append(matches[i].Names[0]);
                    bookSpec = matches[i].BookSpec;
                    multiple = true;
                }
            }
            if (multiple)
            {
                errors.Add(new ParseError(text, "Multiple matches: " +
                    sb.ToString()));
                return null;
            }
            else
            {
                return bookSpec;
            }
        }

        public void ValidateSectionSpec(string text, SectionSpec spec,
            List<ParseError> errors)
        {
            if (spec.Start.BookIndex > spec.End.BookIndex ||
                (spec.Start.BookIndex == spec.End.BookIndex &&
                spec.Start.ChapterNumber > spec.End.ChapterIndex))
            {
                errors.Add(new ParseError(text, "Start and end in wrong order"));
            }
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

        private void AddBookAndName(List<BookSpecAndNames> matches,
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
    }
}
