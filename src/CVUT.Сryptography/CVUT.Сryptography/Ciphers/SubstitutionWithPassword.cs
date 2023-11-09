using System.Text;
using Combinatorics.Collections;

namespace CVUT.Сryptography.Ciphers;

// TODO: do not use, inefficient
public class SubstitutionWithPassword : IBruteForce
{
    public IEnumerable<(string key, string text)> BruteForceDecrypt(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            throw new ArgumentException("Text to decrypt is null or empty", nameof(text));
        }

        text = text.ToLowerInvariant();

        var encodedAlphabets = new Permutations<int>(
            Alphabet.LetterToNumber.Select(x => x.Value).ToArray(),
            GenerateOption.WithoutRepetition)
            .Select(x => x.ToArray())
            .ToList();

        foreach (var encodedAlphabet in encodedAlphabets)
        {
            var decodedAlphabet = DecodeAlphabet(encodedAlphabet);
            var decodedText = new StringBuilder();

            foreach (var letter in text)
            {
                var encodedLetterIndex = Alphabet.LetterToNumber[letter];
                var decodedLetterIndex = decodedAlphabet[encodedLetterIndex];
                decodedText.Append(Alphabet.NumberToLetter[decodedLetterIndex]);
            }

            yield return new ValueTuple<string, string>($"Alphabet (find key by yourself) = {Alphabet.LettersIndexesToWord(encodedAlphabet)}", decodedText.ToString());
        }
    }

    // ReSharper disable once SuggestBaseTypeForParameter
    private static int[] DecodeAlphabet(int[] encodedAlphabet)
    {
        var decodedAlphabet = new int[encodedAlphabet.Length];

        for (var decodedLetterIndex = 0; decodedLetterIndex < encodedAlphabet.Length; decodedLetterIndex++)
        {
            var encodedLetterIndex = encodedAlphabet[decodedLetterIndex];
            decodedAlphabet[encodedLetterIndex] = decodedLetterIndex;
        }

        return decodedAlphabet;
    }
}