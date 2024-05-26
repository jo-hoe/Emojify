using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace EmojifyCore.Logic.EmojifyStrategy
{
    public class AfterSentenceStrategy : AbstractEmojifyStrategy
    {
        private static readonly string[] SentenceDelimiters = new string[] { ".", "?", "!", "|", ":", ";", "\'", "\"", "\n" };
        private static readonly string[] EscapedDelimiters = SentenceDelimiters.Select(item => Regex.Escape(item)).ToArray();
        private static readonly string regexString = "(?<=[" + string.Join("", EscapedDelimiters) + "])";

        public AfterSentenceStrategy(IEmojiContainer emojiContainer) : base(emojiContainer) {}

        public override string Emojify(string text, int minimumCharacters = 0)
        {            
            StringBuilder stringBuilder = new StringBuilder();
            foreach(string wholeSentence in Regex.Split(text, regexString))
            {
                //string wholeSentence = sentenceMatch.Value;
                AfterTextStrategy afterTextStrategy = new AfterTextStrategy(emojiContainer);

                // check if string contains sentence delimiter
                if (SentenceDelimiters.Any(item => wholeSentence.Contains(item)))
                {
                    string sentenceWithOutDelimiter = wholeSentence.Substring(0, wholeSentence.Length - 1);
                    string delimiter = wholeSentence.Substring(wholeSentence.Length - 1);
                    stringBuilder.Append(afterTextStrategy.Emojify(sentenceWithOutDelimiter, minimumCharacters) + delimiter);
                } else
                {
                    stringBuilder.Append(afterTextStrategy.Emojify(wholeSentence, minimumCharacters));
                }
            }

            return stringBuilder.ToString();
        }
    }
}
