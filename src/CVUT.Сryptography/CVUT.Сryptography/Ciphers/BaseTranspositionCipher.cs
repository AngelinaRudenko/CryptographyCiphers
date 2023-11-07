using System.Text;

namespace CVUT.Сryptography.Ciphers;

public abstract class BaseTranspositionCipher
{
    private const char Placeholder = '#';

    public static IEnumerable<(int rowsCount, int colsCount)> GetTableSizes(string text)
    {
        // do not count columnsCount = { 0, 1 text_length }
        for (var rowsCount = 2; rowsCount < text.Length; rowsCount++)
        {
            // start with cols, where cells >= text length
            var startWithColsCount = Math.Ceiling((double)text.Length / rowsCount);
            // row(s) is(are) empty, table is too big for word
            var finishWhenColsCount = Math.Ceiling((double)text.Length / (rowsCount - 1));

            for (var colsCount = Convert.ToInt32(startWithColsCount); colsCount < finishWhenColsCount; colsCount++)
            {
                yield return new ValueTuple<int, int>(rowsCount, colsCount);
            }
        }
    }

    public static char[,] WriteToTableByRows(int colsCount, int rowsCount, string text)
    {
        var table = new char[rowsCount, colsCount];
        var i = 0;
        for (var row = 0; row < rowsCount; row++)
        {
            for (var col = 0; col < colsCount; col++)
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

    public static string ReadTableByCols(char[,] table)
    {
        var result = new StringBuilder();
        for (var col = 0; col < table.GetLength(1); col++)
        {
            for (var row = 0; row < table.GetLength(0); row++)
            {
                if (table[row, col] != Placeholder)
                {
                    result.Append(table[row, col]);
                }
            }
        }
        return result.ToString();
    }

    public static string ReadTableByCols(char[,] table, int[] order)
    {
        // TODO: add more validations
        if (table.GetLength(1) != order.Length)
        {
            throw new ArgumentException("Order list has different length than table columns");
        }

        var result = new StringBuilder();
        for (var oldCol = 0; oldCol < table.GetLength(1); oldCol++)
        {
            var newCol = order[oldCol];

            for (var row = 0; row < table.GetLength(0); row++)
            {
                if (table[row, newCol] != Placeholder)
                {
                    result.Append(table[row, newCol]);
                }
            }
        }
        return result.ToString();
    }
}