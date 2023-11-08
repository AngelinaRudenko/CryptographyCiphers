using System.Text;
using Combinatorics.Collections;

namespace CVUT.Сryptography.Ciphers;

public class SubstitutionWithKey
{
    public static IEnumerable<(string key, string text)> BruteForceDecrypt(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            throw new ArgumentException("Text to decrypt is null or empty", nameof(text));
        }

        text = text.ToLowerInvariant();

        var encodedAlphabets = new Permutations<int>(
            Alphabet.LetterToNumber.Select(x => x.Value).ToArray(),
            GenerateOption.WithoutRepetition)
            .Select(x => x.ToArray());

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

            yield return new ValueTuple<string, string>($"Password = {GetPassword(encodedAlphabet)}", decodedText.ToString());
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

    // ReSharper disable once SuggestBaseTypeForParameter
    private static string GetPassword(int[] encodedAlphabet)
    {
        var passwordEndedIndex = encodedAlphabet.Length;

        for (var i = encodedAlphabet.Length; i <= 0; i--)
        {
            if (encodedAlphabet[i] == i)
            {
                continue;
            }
            passwordEndedIndex = i;
            break;
        }

        var password = new int[passwordEndedIndex];
        for (var i = 0; i < password.Length; i++)
        {
            password[i] = encodedAlphabet[i];
        }

        return Alphabet.LettersIndexesToWord(password);
    }
}