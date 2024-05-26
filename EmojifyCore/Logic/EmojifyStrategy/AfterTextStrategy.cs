using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace EmojifyCore.Logic.EmojifyStrategy
{
    public class AfterTextStrategy : AbstractEmojifyStrategy
    {
        public AfterTextStrategy(IEmojiContainer emojiProvider) : base(emojiProvider) { }

        public override string Emojify(string text, int minimumCharacters = 0)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (Match match in GetWordMatches(text, minimumCharacters))
            {
                string lowerValue = match.Value.ToLower();
                if (EmojiWords.ContainsKey(lowerValue))
                {
                    stringBuilder.Append(EmojiWords[lowerValue].First());
                }
            }

            return stringBuilder.Length == 0 ? text : text + " " + stringBuilder.ToString();
        }
    }
}
