using System.Text;

namespace CVUT.Сryptography.Ciphers;

/// <summary>
/// Complete table without password, columnar transposition
/// </summary>
public static class CompleteTableWithoutPassword
{
    private const char Placeholder = '#';

    public static IEnumerable<(string key, string text)> BruteForceDecrypt(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            throw new ArgumentException("Text to decrypt is null or empty", nameof(text));
        }

        text = text.ToLowerInvariant();

        foreach (var tableSize in GetTableSizes(text))
        {
            var table = WriteToTableByColumn(tableSize.colsCount, tableSize.rowsCount, text);
            var rotatedText = ReadTableByRow(table);
            yield return new ValueTuple<string, string>($"Table columns = {tableSize.colsCount}, rows = {tableSize.rowsCount}", rotatedText);
        }
    }

    public static IEnumerable<(int colsCount, int rowsCount)> GetTableSizes(string text)
    {
        // do not count columnsCount = { 0, 1 text_length }
        for (var colsCount = 2; colsCount < text.Length; colsCount++)
        {
            // start with rows, where cells >= text length
            var startWithRowsCount = Math.Ceiling((double)text.Length / colsCount);
            // column(s) is(are) empty, table is too big for word
            var finishWhenRowsCount = Math.Ceiling((double)text.Length / (colsCount - 1));

            for (var rowsCount = Convert.ToInt32(startWithRowsCount); rowsCount < finishWhenRowsCount; rowsCount++)
            {
                yield return new ValueTuple<int, int>(colsCount, rowsCount);
            }
        }
    }

    public static char[,] WriteToTableByColumn(int colsCount, int rowsCount, string text)
    {
        var table = new char[rowsCount, colsCount];
        var i = 0;
        for (var col = 0; col < colsCount; col++)
        {
            for (var row = 0; row < rowsCount; row++)
            {
                if (i < text.Length)
                {
                    table[row, col] = text[i];
                    i++;
                }
                else
                {
                    table[row, col] = Placeholder;
                }
            }
        }
        return table;
    }

    public static string ReadTableByRow(char[,] table)
    {
        var result = new StringBuilder();
        for (var row = 0; row < table.GetLength(0); row++)
        {
            for (var col = 0; col < table.GetLength(1); col++)
            {
                if (table[row, col] == Placeholder)
                {
                    break;
                }

                result.Append(table[row, col]);
            }
        }
        return result.ToString();

    }
}