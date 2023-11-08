using CVUT.Сryptography.Ciphers;

namespace Tests;

public class DoubleTableTranspositionTest
{
    [Fact]
    public void BruteForceDecrypt()
    {
        const string text = "HOLLRLOEWD";
        var result = DoubleTableTransposition.BruteForceDecrypt(text);

        Assert.Contains(result, x => x.text.Equals("HELLOWORLD", StringComparison.InvariantCultureIgnoreCase));
    }
}