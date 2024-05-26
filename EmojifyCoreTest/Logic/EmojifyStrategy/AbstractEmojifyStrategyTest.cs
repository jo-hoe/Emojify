using EmojifyCore.Logic;
using EmojifyCore.Logic.EmojifyStrategy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace EmojifyCoreTest.Logic.EmojifyStrategy
{
    [TestClass]
    public class AbstractEmojifyStrategyTest : AbstractEmojifyStrategy
    {
        public AbstractEmojifyStrategyTest() : base(new EmojiContainer()) { }

        [TestMethod]
        public void GetUniqueWordMatchesTest()
        {
            CollectionAssert.AllItemsAreUnique(GetWordMatches("test test").ToList());
        }

        [TestMethod]
        public void GetMultipleMatchesTest()
        {
            Assert.AreEqual(3, GetWordMatches("test1 test2 test3").Count());
        }

        [TestMethod]
        public void GetOneMatchTest()
        {
            var testWord = "test";
            Assert.AreEqual(testWord, GetWordMatches(testWord).First().Value);
        }

        [TestMethod]
        public void GetWordMatchesWithoutWhitespacesTest()
        {
            var testWord = "test";
            Assert.AreEqual(testWord, GetWordMatches(" " + testWord + " ").First().Value);
        }

        [TestMethod]
        public void GetWordMatchesWithoutPunctuationTest()
        {
            var testWord = "test";
            Assert.AreEqual(testWord, GetWordMatches("," + testWord + ".").First().Value);
        }

        public override string Emojify(string text, int minimumCharacters = 0)
        {
            throw new System.NotImplementedException();
        }
    }
}
