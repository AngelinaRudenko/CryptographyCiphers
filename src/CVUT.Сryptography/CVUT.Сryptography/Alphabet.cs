namespace CVUT.Сryptography;

internal class Alphabet
{
    static Alphabet()
    {
        NumberToLetter = "abcdefghijklmnopqrstuvwxyz".ToCharArray();

        LetterToNumber = new Dictionary<char, int>();
        for (var i = 0; i < NumberToLetter.Length; i++)
        {
            LetterToNumber.Add(NumberToLetter[i], i);
        }
    }

    internal static char[] NumberToLetter { get; }
    internal static Dictionary<char, int> LetterToNumber { get; }
}