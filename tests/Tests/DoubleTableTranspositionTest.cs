using CVUT.Сryptography.Ciphers;
using Xunit;

namespace Tests;

public class DoubleTableTranspositionTest
{
    private readonly DoubleTableTransposition _doubleTableTransposition;

    public DoubleTableTranspositionTest()
    {
        _doubleTableTransposition = new DoubleTableTransposition();
    }

    [Fact]
    public void BruteForceDecrypt()
    {
        const string text = "HOLLRLOEWD";
        var result = _doubleTableTransposition.BruteForceDecrypt(text).ToArray();

        Assert.Contains(result, x => x.text.Equals("HELLOWORLD", StringComparison.InvariantCultureIgnoreCase));
    }
}