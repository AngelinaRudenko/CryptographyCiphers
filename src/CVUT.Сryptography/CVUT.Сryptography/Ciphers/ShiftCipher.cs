using System.Collections.Concurrent;
using System.Text;
// ReSharper disable ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator

namespace CVUT.Сryptography.Ciphers;

/// <summary>
/// ShiftCipher with key = 3 is Caesar cipher
/// </summary>
public class ShiftCipher : IBruteForce
{
    public string Encrypt(string text, int key)
    {
        if (string.IsNullOrEmpty(text))
        {
            return string.Empty;
        }

        var words = text.ToLowerInvariant().Split(' ');

        key %= Alphabet.NumberToLetter.Length;

        var result = new StringBuilder();

        foreach (var word in words)
        {
            foreach (var letter in word)
            {
                var letterNumber = Alphabet.LetterToNumber[letter];
                var newLetterNumber = (letterNumber + key) % Alphabet.NumberToLetter.Length;
                result.Append(Alphabet.NumberToLetter[newLetterNumber]);
            }

            result.Append(' ');
        }

        return result.ToString();
    }

    public static string Decrypt(string text, int key)
    {
        if (string.IsNullOrEmpty(text))
        {
            return string.Empty;
        }

        var words = text.ToLowerInvariant().Split(' ');

        key %= Alphabet.NumberToLetter.Length;

        var result = new StringBuilder();

        foreach (var word in words)
        {
            foreach (var letter in word)
            {
                var letterNumber = Alphabet.LetterToNumber[letter];
                var newLetterNumber = (letterNumber - key) % Alphabet.NumberToLetter.Length;

                if (newLetterNumber < 0)
                {
                    newLetterNumber = Alphabet.NumberToLetter.Length + newLetterNumber;
                }
                result.Append(Alphabet.NumberToLetter[newLetterNumber]);
            }
            result.Append(' ');
        }

        return result.ToString();
    }

    public IEnumerable<(string key, string text)> BruteForceDecrypt(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            throw new ArgumentException("Text to decrypt is null or empty", nameof(text));
        }

        text = text.ToLowerInvariant();

        var result = new ConcurrentBag<(string key, string text)>();

        Parallel.For(1, Alphabet.NumberToLetter.Length, (key) =>
        {
            result.Add(new ValueTuple<string, string>($"Move = {key}", Decrypt(text, key)));
        });

        return result;

        // let's assume that user is not silly and skip key = 0 and key = 26
        //for (var key = 1; key < Alphabet.NumberToLetter.Length; key++)
        //{
        //    yield return ($"Move = {key}", Decrypt(text, key));
        //}
    }
}