using System.Text;
// ReSharper disable ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator

namespace CVUT.Сryptography.Ciphers;

/// <summary>
/// ShiftCipher with key = 3 is Caesar cipher
/// </summary>
public static class ShiftCipher
{
    public static string Encrypt(string text, int key)
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

    public static async Task BruteForceDecrypt(string text, string outputFilePath)
    {
        if (string.IsNullOrEmpty(text))
        {
            throw new ArgumentException("Text to decrypt is null or empty", nameof(text));
        }

        text = text.ToLowerInvariant();

        var result = new List<(int key, string text)>();

        // let's assume that user is not silly and skip key = 0
        for (var key = 1; key < Alphabet.NumberToLetter.Length + 1; key++)
        {
            result.Add(new ValueTuple<int, string>(key, Decrypt(text, key)));
        }

        var stringResult = new StringBuilder("####### Simple shift #######");
        foreach (var pair in result)
        {
            stringResult.AppendLine($"Key = {pair.key}, Decoded text = '{pair.text}'");
        }

        await File.WriteAllTextAsync(outputFilePath, stringResult.ToString());
    }
}