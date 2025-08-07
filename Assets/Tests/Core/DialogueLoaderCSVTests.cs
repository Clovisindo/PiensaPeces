using Game.Core;
using NUnit.Framework;

namespace Game.Core.Tests
{
    public class DialogueLoaderCSVTests
    {
        [TestCase("ID001;Hello", "ID001", "Hello", "Always")]
        [TestCase("ID002;Hi;HasItem", "ID002", "Hi", "HasItem")]
        [TestCase("ID003;Text with spaces", "ID003", "Text with spaces", "Always")]
        [TestCase("ID004;Test;Condition;ExtraField", "ID004", "Test", "Condition")]
        public void ShouldParseValidDataOK(string csvLine, string expectedId, string expectedText, string expectedCondition)
        {
            var result = DialogueLoaderCsv.Load(csvLine);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(expectedId, result[0].Id);
            Assert.AreEqual(expectedText, result[0].Text);
            Assert.AreEqual(expectedCondition, result[0].Condition);
        }

        [TestCase("")]
        [TestCase("OnlyOneField")]
        [TestCase(";;;")]
        [TestCase("    ")]
        public void ShouldIgnoreInvalidData(string csvLine)
        {
            var result = DialogueLoaderCsv.Load(csvLine);

            Assert.AreEqual(0, result.Count);
        }
    }
}