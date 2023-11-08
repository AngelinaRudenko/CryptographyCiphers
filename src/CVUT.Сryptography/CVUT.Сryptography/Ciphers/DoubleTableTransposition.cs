namespace CVUT.Сryptography.Ciphers;

public class DoubleTableTransposition
{
    public static IEnumerable<(string key, string text)> BruteForceDecrypt(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            throw new ArgumentException("Text to decrypt is null or empty", nameof(text));
        }

        text = text.ToLowerInvariant();

        var tableSizes = TranspositionBase.GetTableSizes(text).ToArray();

        foreach (var firstTableSize in tableSizes)
        {
            var firstTable = TranspositionBase.WriteToTableByRows(firstTableSize.colsCount, firstTableSize.rowsCount, text);
            var firstRotatedText = TranspositionBase.ReadTableByCols(firstTable);

            foreach (var secondTableSize in tableSizes)
            {
                var secondTable = TranspositionBase.WriteToTableByRows(secondTableSize.colsCount, secondTableSize.rowsCount, firstRotatedText);
                var secondRotatedText = TranspositionBase.ReadTableByCols(secondTable);

                if (text.Equals(secondRotatedText))
                {
                    yield break;
                }

                yield return new ValueTuple<string, string>(
                    $"First table columns = {firstTableSize.colsCount}, rows = {firstTableSize.rowsCount}, " +
                    $"second table columns = {secondTableSize.colsCount}, rows = {secondTableSize.rowsCount}",
                    secondRotatedText);
            }
        }
    }
}