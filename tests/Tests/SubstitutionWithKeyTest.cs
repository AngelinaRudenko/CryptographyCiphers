using CVUT.Сryptography.Ciphers;
using Xunit;

namespace Tests;

public class SubstitutionWithKeyTest
{
    private readonly SubstitutionWithPassword _substitutionWithPassword;

    public SubstitutionWithKeyTest()
    {
        _substitutionWithPassword = new SubstitutionWithPassword();
    }

    [Fact]
    public void BruteForceDecrypt()
    {
        const string text = "COIIMWMQIL";
        var result = _substitutionWithPassword.BruteForceDecrypt(text).ToArray();

        Assert.Contains(result, x => x.text.Equals("HELLOWORLD", StringComparison.InvariantCultureIgnoreCase));
    }
}