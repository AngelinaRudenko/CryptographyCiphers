using CVUT.Сryptography.Ciphers;

namespace Tests;

public class CompleteTableWithPasswordTest
{
    [Fact]
    public void BruteForceDecrypt()
    {
        const string text = "SIDBMKGYMNSUEAGOLE";
        var result = CompleteTableWithPassword.BruteForceDecrypt(text).ToArray();

        Assert.Contains(result, x => x.text.Equals("BUYSOMEMILKANDEGGS", StringComparison.InvariantCultureIgnoreCase));
    }
}