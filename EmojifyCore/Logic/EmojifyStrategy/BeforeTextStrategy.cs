using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace EmojifyCore.Logic.EmojifyStrategy
{
    public class BeforeWordStrategy : AbstractEmojifyStrategy
    {
        public BeforeWordStrategy(IEmojiContainer emojiContainer) : base(emojiContainer) { }

        public override string Emojify(string text, int minimumCharacters = 0)
        {
            foreach (Match match in GetWordMatches(text, minimumCharacters).Reverse())
            {
                string lowerValue = match.Value.ToLower();
                if (EmojiWords.ContainsKey(lowerValue))
                {
                    var insertionText = EmojiWords[lowerValue].First() + " ";
                    text = text.Insert(match.Index, insertionText);
                }
            }

            return text;
        }
    }
}
