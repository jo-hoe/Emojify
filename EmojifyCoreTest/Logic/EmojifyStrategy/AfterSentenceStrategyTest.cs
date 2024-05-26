﻿using EmojifyCore.Logic;
using EmojifyCore.Logic.EmojifyStrategy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace EmojifyCoreTest.Logic.EmojifyStrategy
{
    [TestClass]
    public class AfterSentenceStrategyTest
    {
        [TestMethod]
        [DataRow("notanemoji", "notanemoji", 0)]
        [DataRow("Apple 🍎.", "Apple.", 0)]
        [DataRow("apple 🍎.", "apple.", 0)]
        [DataRow("the apple is red 🍎", "the apple is red", 0)]
        [DataRow("the baby apple is red 👶🍎", "the baby apple is red", 0)]
        [DataRow("The apple, which is red 🍎.", "The apple, which is red.", 0)]
        [DataRow("The apple 🍎.which is red.", "The apple.which is red.", 0)]
        [DataRow("The apple 🍎|which is red|", "The apple|which is red|", 0)]
        [DataRow("up 🆙", "up", 0)]
        [DataRow("up 🆙", "up", 2)]
        [DataRow("up", "up", 3)]
        [DataRow("up", "up", 4)]
        public void EmojifyTest(string expected, string input, int wordCount)
        {
            Assert.AreEqual(expected, new AfterSentenceStrategy(new EmojiContainer()).Emojify(input, wordCount));
        }
    }
}
