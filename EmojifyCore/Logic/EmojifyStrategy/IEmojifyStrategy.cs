
namespace EmojifyCore.Logic.EmojifyStrategy
{
    public interface IEmojifyStrategy
    {
        /// <summary>
        /// Tokenizes a string by words and add emojies these words.
        /// </summary>
        /// <param name="text">Text that should be emojified</param>
        /// <param name="minimumCharacters">Minimum character count for a word before it will be mapped to an emoji.</param>
        /// <returns>Emojified string</returns>
        string Emojify(string text, int minimumCharacters = 0);
    }
}
