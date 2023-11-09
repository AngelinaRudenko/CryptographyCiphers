namespace CVUT.Сryptography.Ciphers;

/// <summary>
/// Complete table without password, columnar transposition
/// </summary>
public class CompleteTableWithoutPassword : IBruteForce
{
    public IEnumerable<(string key, string text)> BruteForceDecrypt(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            throw new ArgumentException("Text to decrypt is null or empty", nameof(text));
        }

        text = text.ToLowerInvariant();

        foreach (var tableSize in BaseTransposition.GetTableSizes(text))
        {
            var table = BaseTransposition.WriteToTableByRows(tableSize.colsCount, tableSize.rowsCount, text);
            var rotatedText = BaseTransposition.ReadTableByCols(table);
            yield return new ValueTuple<string, string>($"Table columns = {tableSize.colsCount}, rows = {tableSize.rowsCount}", rotatedText);
        }
    }
}