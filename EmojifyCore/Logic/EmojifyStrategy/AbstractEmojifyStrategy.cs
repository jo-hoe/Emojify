using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace EmojifyCore.Logic.EmojifyStrategy
{
    public abstract class AbstractEmojifyStrategy : IEmojifyStrategy
    {
        private static readonly Regex WordRegex = new Regex(@"\b(?<word>\w+)\b", RegexOptions.Compiled);
        protected readonly IEmojiContainer emojiContainer;

        public AbstractEmojifyStrategy(IEmojiContainer emojiContainer)
        {
            this.emojiContainer = emojiContainer;
        }

        public abstract string Emojify(string text, int minimumCharacters = 0);

        protected Dictionary<string, HashSet<string>> EmojiWords
        {
            get { return emojiContainer.EmojiWords; }
        }

        /// <summary>
        /// Parses a given text using regex.
        /// </summary>
        /// <param name="text">text to parse</param>
        /// <returns>All matches for the words within the text.</returns>
        protected IOrderedEnumerable<Match> GetWordMatches(string text, int minimumCharacters = 0)
        {
            return GetRegexMatches(WordRegex, text, minimumCharacters);
        }

        private IOrderedEnumerable<Match> GetRegexMatches(Regex regex, string text, int minimumCharacters)
        {
            MatchCollection allMatches = regex.Matches(text);
            // list is reduced by word which meet length requirements
            IEnumerable<Match> reducedWordList = allMatches.Where(match => match.Length >= minimumCharacters);
            return reducedWordList.ToList().OrderBy(match => match.Index);
        }
    }
}
