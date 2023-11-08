using System.Text;

namespace CVUT.Сryptography;

internal class Alphabet
{
    static Alphabet()
    {
        NumberToLetter = "abcdefghijklmnopqrstuvwxyz".ToCharArray();
        Length = NumberToLetter.Length;

        LetterToNumber = new Dictionary<char, int>();
        for (var i = 0; i < NumberToLetter.Length; i++)
        {
            LetterToNumber.Add(NumberToLetter[i], i);
        }
    }

    internal static int Length { get; }
    internal static char[] NumberToLetter { get; }
    internal static Dictionary<char, int> LetterToNumber { get; }

    internal static string LettersIndexesToWord(int[] indexes)
    {
        if (indexes.Any(x => x < 0) || indexes.Any(x => x >= Length))
        {
            throw new ArgumentException("Input contains index out of alphabet range");
        }

        var result = new StringBuilder();

        foreach (var index in indexes)
        {
            result.Append(NumberToLetter[index]);
        }

        return result.ToString();
    }
}