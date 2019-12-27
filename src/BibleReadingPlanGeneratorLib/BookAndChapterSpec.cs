using System.Text.Json.Serialization;
using Ardalis.GuardClauses;

namespace BibleReadingPlanGeneratorLib
{
    public class BookAndChapterSpec
    {
        public string BookName { get; set; } = null;

        public int ChapterNumber { get; set; } = 1;

        [JsonIgnore]
        public BibleSpec BibleSpec { get; set; } = null;

        [JsonIgnore]
        public int BookIndex
        {
            get
            {
                if (BibleSpec == null)
                {
                    return -1;
                }
                string lowerBookName = (BookName ?? "").ToLower();
                for (int i = 0; i < BibleSpec.Books.Count; i++)
                {
                    BookSpec book = BibleSpec.Books[i];
                    foreach (string name in book.Names)
                    {
                        if (lowerBookName == name.ToLower())
                        {
                            return i;
                        }
                    }
                }
                return -1;
            }
        }

        [JsonIgnore]
        public int BookNumber => BookIndex + 1;

        [JsonIgnore]
        public int ChapterIndex => ChapterNumber - 1;

        public BookAndChapterSpec()
        {

        }

        public BookAndChapterSpec(BibleSpec bibleSpec, string bookName, int chapterNumber)
        {
            Guard.Against.Null(bibleSpec, nameof(bibleSpec));
            BibleSpec = bibleSpec;
            Guard.Against.Null(bookName, nameof(bookName));
            BookName = bookName;
            Guard.Against.OutOfRange(chapterNumber, nameof(chapterNumber), 1, int.MaxValue);
            ChapterNumber = chapterNumber;
        }

        public override string ToString()
        {
            return "" + BookName + " " + ChapterNumber;
        }
    }
}
