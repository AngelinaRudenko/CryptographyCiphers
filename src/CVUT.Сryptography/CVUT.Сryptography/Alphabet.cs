using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace CVUT.Сryptography;

[SuppressMessage("ReSharper", "MergeIntoPattern")]
public static class Alphabet
{
    static Alphabet()
    {
        NumberToLetter = "abcdefghijklmnopqrstuvwxyz".ToCharArray();
        Length = NumberToLetter.Length;

        LetterToNumber = new ConcurrentDictionary<char, int>();
        for (var i = 0; i < NumberToLetter.Length; i++)
        {
            LetterToNumber.TryAdd(NumberToLetter[i], i);
        }
    }

    internal static int Length { get; }
    internal static char[] NumberToLetter { get; }
    internal static ConcurrentDictionary<char, int> LetterToNumber { get; }

    public static string LettersIndexesToWord(int[] indexes)
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

    public static double GetIndexOfCoincidence(string text)
    {
        var indexOfCoincidence = 0.0;
        var denominator = text.Length * (text.Length - 1);

        for (var i = 0; i < Length; i++)
        {
            double letterFrequencyInText = text.Count(x => x.Equals(NumberToLetter[i]));
            indexOfCoincidence += (letterFrequencyInText * (letterFrequencyInText - 1)) / denominator;
        }

        return indexOfCoincidence;
    }

    public static bool MayBeMonoAlphabeticSubstitution(string text)
    {
        var indexOfCoincidence = GetIndexOfCoincidence(text);
        return indexOfCoincidence > 0.05 && indexOfCoincidence < 0.08; // IC ~ 0,067 - probably mono alphabetic
    }
}