using CVUT.Сryptography.Ciphers;

namespace Tests;

public class SubstitutionWithKeyTest
{
    [Fact]
    public void BruteForceDecrypt()
    {
        const string text = "COIIMWMQIL";
        var result = SubstitutionWithKey.BruteForceDecrypt(text).ToArray();

        Assert.Contains(result, x => x.text.Equals("HELLOWORLD", StringComparison.InvariantCultureIgnoreCase));
    }
}