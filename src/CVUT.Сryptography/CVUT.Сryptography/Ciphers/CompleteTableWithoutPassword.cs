namespace CVUT.Сryptography.Ciphers;

/// <summary>
/// Complete table without password, columnar transposition
/// </summary>
public static class CompleteTableWithoutPassword
{

    public static IEnumerable<(string key, string text)> BruteForceDecrypt(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            throw new ArgumentException("Text to decrypt is null or empty", nameof(text));
        }

        text = text.ToLowerInvariant();

        foreach (var tableSize in BaseTranspositionCipher.GetTableSizes(text))
        {
            var table = BaseTranspositionCipher.WriteToTableByRows(tableSize.colsCount, tableSize.rowsCount, text);
            var rotatedText = BaseTranspositionCipher.ReadTableByCols(table);
            yield return new ValueTuple<string, string>($"Table columns = {tableSize.colsCount}, rows = {tableSize.rowsCount}", rotatedText);
        }
    }
}