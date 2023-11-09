namespace CVUT.Сryptography.Ciphers;

public interface IBruteForce
{
    IEnumerable<(string key, string text)> BruteForceDecrypt(string text);
}