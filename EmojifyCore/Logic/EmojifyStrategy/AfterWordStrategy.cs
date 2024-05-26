using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace EmojifyCore.Logic.EmojifyStrategy
{
    public class AfterWordStrategy : AbstractEmojifyStrategy
    {
        public AfterWordStrategy(IEmojiContainer emojiContainer) : base(emojiContainer) { }

        public override string Emojify(string text, int minimumCharacters = 0)
        {
            foreach (Match match in GetWordMatches(text, minimumCharacters).Reverse())
            {
                string lowerValue = match.Value.ToLower();
                if (EmojiWords.ContainsKey(lowerValue))
                {
                    text = text.Insert(match.Index + match.Length, " " + EmojiWords[lowerValue].First());
                }
            }

            return text;
        }
    }
}
