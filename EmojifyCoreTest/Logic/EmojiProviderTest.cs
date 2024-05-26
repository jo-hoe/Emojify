using EmojifyCore.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EmojifyCoreTest.Logic
{
    [TestClass]
    public class EmojiProviderTest
    {
        [TestMethod]
        public void ConstructorTest()
        {
            Assert.IsNotNull(new EmojiContainer());
        }

        [TestMethod]
        public void EmojiSetTest()
        {
            Assert.IsTrue(new EmojiContainer().EmojiSet.Count > 0);
        }

        [TestMethod]
        public void EmojiWordsTest()
        {
            Assert.IsTrue(new EmojiContainer().EmojiWords.Count > 0);
        }

        [TestMethod]
        public void ContainsEmojiAtEndTest()
        {
            Assert.IsTrue(new EmojiContainer().ContainsEmoji("test😄"));
        }

        [TestMethod]
        public void ContainsEmojiAtBeginningTest()
        {
            Assert.IsTrue(new EmojiContainer().ContainsEmoji("😄test"));
        }

        [TestMethod]
        public void ContainsEmojiInMiddleTest()
        {
            Assert.IsTrue(new EmojiContainer().ContainsEmoji("test😄test"));
        }

        [TestMethod]
        public void ContainsMulitpleEmojiTest()
        {
            Assert.IsTrue(new EmojiContainer().ContainsEmoji("test😄😄test"));
        }

        [TestMethod]
        public void ContainsJustEmojiTest()
        {
            Assert.IsTrue(new EmojiContainer().ContainsEmoji("😄"));
        }

        [TestMethod]
        public void ContainsNoEmojiTest()
        {
            Assert.IsFalse(new EmojiContainer().ContainsEmoji("test"));
        }
    }
}
