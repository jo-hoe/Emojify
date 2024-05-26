using EmojiDataStructureCreator;
using EmojiDataStructureCreator.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;

namespace EmojiDataStructureCreatorTest
{
    [TestClass]
    public class CreatorTest
    {
        private const string EmojiJsonFilePath = @"Resources\\emoji.json";

        [TestMethod]
        public void ParseInputTest()
        {
            string emojiFileContent = GetFileContent(EmojiJsonFilePath);
            var result = Creator.ParseInput(emojiFileContent);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ParseInputTestFirst()
        {
            string emojiFileContent = GetFileContent(EmojiJsonFilePath);
            var result = Creator.ParseInput(emojiFileContent)[0];
            Assert.IsInstanceOfType(result, typeof(InputEmojiItem));
        }

        [TestMethod]
        public void ToEmojiWordsTest()
        {
            List<InputEmojiItem> input = new List<InputEmojiItem>();
            input.Add(new InputEmojiItem()
            {
                Emoji = "😂",
                Aliases = new List<string> { "test", "test2" }
            });
            input.Add(new InputEmojiItem()
            {
                Emoji = "🙂",
                Tags = new List<string> { "test", "test3" }
            });
            var result = Creator.ToEmojiWords(input);
            Assert.AreEqual(3, result.Count);
        }

        private static string GetFileContent(string fileName)
        {

            if (!File.Exists(fileName))
            {
                throw new FileNotFoundException("File was not found", fileName);
            }

            return File.ReadAllText(fileName);
        }

    }
}
