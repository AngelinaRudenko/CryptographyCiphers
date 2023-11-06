using CVUT.Сryptography.Ciphers;

namespace Tests;

public class CompleteTableWithoutPasswordTest
{
    [Fact]
    public void GetTableSizes()
    {
        const string text = "HESLO";
        var result = CompleteTableWithoutPassword.GetTableSizes(text).ToArray();

        Assert.Equal(3, result.Length);
        Assert.Contains(result, x => x.colsCount == 3 && x.rowsCount == 2);
        Assert.Contains(result, x => x.colsCount == 2 && x.rowsCount == 3);
        Assert.Contains(result, x => x.colsCount == 2 && x.rowsCount == 4);
    }

    [Fact]
    public void WriteToTableByColumn()
    {
        const int cols = 3;
        const int rows = 2;
        const string text = "HESLO";
        var result = CompleteTableWithoutPassword.WriteToTableByColumn(cols, rows, text);

        Assert.Equal('H', result[0,0]);
        Assert.Equal('E', result[1,0]);
        Assert.Equal('S', result[0,1]);
        Assert.Equal('L', result[1,1]);
        Assert.Equal('O', result[0,2]);
        Assert.Equal('#', result[1,2]);
    }

    [Fact]
    public void ReadTableByRow()
    {
        const int cols = 3;
        const int rows = 2;
        const string text = "HESLO";
        var table = CompleteTableWithoutPassword.WriteToTableByColumn(cols, rows, text);
        var result = CompleteTableWithoutPassword.ReadTableByRow(table);

        Assert.Equal("HSOEL", result);
    }
}