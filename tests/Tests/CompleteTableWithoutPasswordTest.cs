using CVUT.Сryptography.Ciphers;
using Xunit;

namespace Tests;

public class CompleteTableWithoutPasswordTest
{
    private readonly CompleteTableWithoutPassword _completeTableWithoutPassword;

    public CompleteTableWithoutPasswordTest()
    {
        _completeTableWithoutPassword = new CompleteTableWithoutPassword();
    }

    [Fact]
    public void BruteForceDecrypt()
    {
        const string text = "HESLO";
        var result = _completeTableWithoutPassword.BruteForceDecrypt(text).ToArray();

        Assert.Equal(3, result.Length);
        Assert.Contains(result, x => x.text.Equals("HLEOS", StringComparison.InvariantCultureIgnoreCase));
        Assert.Contains(result, x => x.text.Equals("HOESL", StringComparison.InvariantCultureIgnoreCase));
        Assert.Contains(result, x => x.text.Equals("HSOEL", StringComparison.InvariantCultureIgnoreCase));
    }
}