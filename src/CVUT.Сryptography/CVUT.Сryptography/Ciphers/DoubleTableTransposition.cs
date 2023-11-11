using System.Collections.Concurrent;

namespace CVUT.Сryptography.Ciphers;

public class DoubleTableTransposition : IBruteForce
{
    public IEnumerable<(string key, string text)> BruteForceDecrypt(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            throw new ArgumentException("Text to decrypt is null or empty", nameof(text));
        }

        text = text.ToLowerInvariant();

        var tableSizes = BaseTransposition.GetTableSizes(text).ToArray();

        var result = new ConcurrentBag<(string key, string text)>();

        foreach (var firstTableSize in tableSizes)
        {
            var firstTable = BaseTransposition.WriteToTableByRows(firstTableSize.colsCount, firstTableSize.rowsCount, text);
            var firstRotatedText = BaseTransposition.ReadTableByCols(firstTable);

            Parallel.ForEach(tableSizes, new ParallelOptions { MaxDegreeOfParallelism = 5 }, (secondTableSize) =>
            {
                var secondTable = BaseTransposition.WriteToTableByRows(secondTableSize.colsCount,
                    secondTableSize.rowsCount, firstRotatedText);
                var secondRotatedText = BaseTransposition.ReadTableByCols(secondTable);

                if (text.Equals(secondRotatedText))
                {
                    return;
                }

                result.Add(new ValueTuple<string, string>(
                    $"First table columns = {firstTableSize.colsCount}, rows = {firstTableSize.rowsCount}, " +
                    $"second table columns = {secondTableSize.colsCount}, rows = {secondTableSize.rowsCount}",
                    secondRotatedText));
            });

            //foreach (var secondTableSize in tableSizes)
            //{
            //    var secondTable = BaseTransposition.WriteToTableByRows(secondTableSize.colsCount, secondTableSize.rowsCount, firstRotatedText);
            //    var secondRotatedText = BaseTransposition.ReadTableByCols(secondTable);

            //    if (text.Equals(secondRotatedText))
            //    {
            //        yield break;
            //    }

            //    yield return new ValueTuple<string, string>(
            //        $"First table columns = {firstTableSize.colsCount}, rows = {firstTableSize.rowsCount}, " +
            //        $"second table columns = {secondTableSize.colsCount}, rows = {secondTableSize.rowsCount}",
            //        secondRotatedText);
            //}
        }

        return result;
    }
}