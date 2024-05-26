using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EmojifyCore.Logic
{
    public interface IEmojiContainer
    {
        HashSet<string> EmojiSet
        {
            get;
        }

        Dictionary<string, HashSet<string>> EmojiWords
        {
            get;
        }

        /// <summary>
        /// Tests a given text if it contains any emojis knows to this class
        /// </summary>
        /// <param name="text">which shall be checked for emojis</param>
        /// <returns>true if at least one emoji is found, false otherwise</returns>
        bool ContainsEmoji(string text);
    }
}
