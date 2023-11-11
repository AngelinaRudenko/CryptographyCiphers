using System.Collections.Concurrent;
using System.Text;

namespace CVUT.Сryptography.Ciphers;

public class AffineCipher : IBruteForce
{
    // full copy of https://github.com/sukhdev01/Attacks-on-Affine-Cipher/blob/master/Brute_force_attacks_on_Affine_cipher.ipynb
    public IEnumerable<(string key, string text)> BruteForceDecrypt(string text)
    {
        var result = new ConcurrentBag<(string key, string text)>();

        for (var a = 1; a < 26; a++)
        {
            Parallel.For(0, 26, new ParallelOptions { MaxDegreeOfParallelism = 5 }, (b) =>
            {
                if (Gcd(a, b) != 1)
                {
                    return; // continue
                }

                var decryptedText = new StringBuilder();

                var aInv = ModInverse(a, 26);
                foreach (var letter in text)
                {
                    if (Alphabet.LetterToNumber.ContainsKey(letter)) // fix for method was failing with some empty character
                    {
                        var num = (Alphabet.LetterToNumber[letter] - b) * aInv;
                        num = (num % 26 + 26) % 26; // Ensure the result is non-negative
                        decryptedText.Append(Alphabet.NumberToLetter[num]);
                    }
                }

                result.Add(new ValueTuple<string, string>($"key1 = {a}, key2 = {b}", decryptedText.ToString()));
            });

            //for (var b = 0; b < 26; b++)
            //{
            //    if (Gcd(a, b) != 1)
            //    {
            //        continue;
            //    }

            //    var decryptedText = new StringBuilder();

            //    var aInv = ModInverse(a, 26);
            //    foreach (var letter in text)
            //    {
            //        if (Alphabet.LetterToNumber.ContainsKey(letter)) // fix for method was failing with some empty character
            //        {
            //            var num = (Alphabet.LetterToNumber[letter] - b) * aInv;
            //            num = (num % 26 + 26) % 26; // Ensure the result is non-negative
            //            decryptedText.Append(Alphabet.NumberToLetter[num]);
            //        }
            //    }

            //    yield return new ValueTuple<string, string>($"key1 = {a}, key2 = {b}", decryptedText.ToString());
            //}
        }

        return result;
    }

    private static int Gcd(int a, int b)
    {
        // return (a == 0) ? b : Gcd(b % a, a);

        while (true)
        {
            if (a == 0)
            {
                return b;
            }

            var temp = a;
            a = b % a;
            b = temp;
        }
    }

    private static int ModInverse(int a, int m)
    {
        a %= m;
        for (var x = 1; x < m; x++)
        {
            if ((a * x) % m == 1)
            {
                return x;
            }
        }
        return 1;
    }
}