namespace CVUT.Сryptography.Models;

public class CipherAnswer
{
    public string Key { get; set; }
    public string DecodedText { get; set; }
    public string PlainText { get; set; }
    public decimal ProbabilityLogSum { get; set; }
    public int DistanceSum { get; set; }

    public override string ToString()
    {
        return $"Probability = {ProbabilityLogSum},\n\tKey = '{Key}'\n\tPlain text = '{PlainText}',\n\tDecoded text = '{DecodedText}'\n\tDistance = {DistanceSum}";
    }
}