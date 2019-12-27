using Ardalis.GuardClauses;

namespace BibleReadingPlanGeneratorLib
{
    public class ParseError
    {
        public string Text { get; }

        public string ErrorText { get; }

        public ParseError(string text, string errorText)
        {
            Text = text;
            Guard.Against.Null(errorText, nameof(errorText));
            ErrorText = errorText;
        }
    }
}
