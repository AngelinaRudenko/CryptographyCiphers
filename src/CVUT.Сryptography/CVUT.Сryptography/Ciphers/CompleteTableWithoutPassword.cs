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

        var result = new List<(string key, string text)>();

        foreach (var tableSize in GetTableSizes(text))
        {
            var table = WriteToTableByColumn(tableSize.colsCount, tableSize.rowsCount, text);
            var rotatedText = ReadTableByRow(table);
            result.Add(new ($"Table columns = {tableSize.colsCount}, rows = {tableSize.rowsCount}", rotatedText));
        }

        return result;
    }

    public static IEnumerable<(int colsCount, int rowsCount)> GetTableSizes(string text)
    {
        // do not count columnsCount = 0 and columnsCount = text length
        for (var colsCount = 2; colsCount < text.Length; colsCount++)
        {
            // do not count rowsCount = 0 and rowsCount = text length
            for (var rowsCount = 2; rowsCount < text.Length; rowsCount++)
            {
                var cellsCount = colsCount * rowsCount;
                if (cellsCount - text.Length >= rowsCount) // column(s) is(are) empty, table is too big for word
                {
                    continue;
                }

                if (cellsCount < text.Length) // table is smaller than text
                {
                    continue;
                }

                yield return new(colsCount, rowsCount);
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