using CVUT.Сryptography.Ciphers;
using Xunit;

namespace Tests;

public class CompleteTableWithPasswordTest
{
    private readonly CompleteTableWithPassword _completeTableWithPassword;


    public CompleteTableWithPasswordTest()
    {
        _completeTableWithPassword = new CompleteTableWithPassword();
    }

    [Fact]
    public void BruteForceDecrypt()
    {
        const string text = "SIDBMKGYMNSUEAGOLE";
        var result = _completeTableWithPassword.BruteForceDecrypt(text).ToArray();

        Assert.Contains(result, x => x.text.Equals("BUYSOMEMILKANDEGGS", StringComparison.InvariantCultureIgnoreCase));
    }
}