using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EmojifyCore.Logic
{
    public class EmojiContainer : IEmojiContainer
    {
        public const string DefaultInputFilePath = @"Resources\EmojiSource.json";
        private static string inputFilePath = null;

        private static HashSet<string> emojiSet;
        private static Dictionary<string, HashSet<string>> emojiWords;

        public EmojiContainer(String emojiFilePath = DefaultInputFilePath)
        {
            inputFilePath = String.IsNullOrEmpty(emojiFilePath) ? DefaultInputFilePath : emojiFilePath;
            
            emojiWords = JsonConvert.DeserializeObject<Dictionary<string, HashSet<string>>>(File.ReadAllText(inputFilePath));

            var allEmojies = EmojiWords.Values.SelectMany(x => x).Distinct();
            emojiSet = new HashSet<string>(allEmojies);
        }

        public HashSet<string> EmojiSet
        {
            get { return new HashSet<string>(emojiSet); }
        }

        public Dictionary<string, HashSet<string>> EmojiWords
        {
            get { return new Dictionary<string, HashSet<string>>(emojiWords); }
        }

        /// <summary>
        /// Tests a given text if it contains any emojis knows to this class
        /// </summary>
        /// <param name="text">which shall be checked for emojis</param>
        /// <returns>true if at least one emoji is found, false otherwise</returns>
        public bool ContainsEmoji(string text)
        {
            return emojiSet.Any(text.Contains);
        }
    }
}
