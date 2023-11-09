using CVUT.Сryptography.Ciphers;
using Xunit;

namespace Tests;

public class BaseTranspositionTest
{
    [Fact]
    public void GetTableSizes()
    {
        const string text = "HESLO";
        var result = BaseTransposition.GetTableSizes(text).ToArray();

        Assert.Equal(3, result.Length);
        Assert.Contains(result, x => x.colsCount == 4 && x.rowsCount == 2);
        Assert.Contains(result, x => x.colsCount == 3 && x.rowsCount == 2);
        Assert.Contains(result, x => x.colsCount == 2 && x.rowsCount == 3);
    }

    [Fact]
    public void WriteToTableByRows()
    {
        const int cols = 3;
        const int rows = 2;
        const string text = "HESLO";
        var result = BaseTransposition.WriteToTableByRows(cols, rows, text);

        Assert.Equal('H', result[0, 0]);
        Assert.Equal('E', result[0, 1]);
        Assert.Equal('S', result[0, 2]);
        Assert.Equal('L', result[1, 0]);
        Assert.Equal('O', result[1, 1]);
        Assert.Equal('#', result[1, 2]);
    }

    [Theory]
    [InlineData(2, 3, "HESLO", "HLEOS")]
    [InlineData(2, 4, "HESLO", "HOESL")]
    [InlineData(3, 2, "HESLO", "HSOEL")]
    public void ReadTableByRow(int rows, int cols, string text, string expected)
    {
        var table = BaseTransposition.WriteToTableByRows(cols, rows, text);
        var result = BaseTransposition.ReadTableByCols(table);

        Assert.Equal(expected, result);
    }
}