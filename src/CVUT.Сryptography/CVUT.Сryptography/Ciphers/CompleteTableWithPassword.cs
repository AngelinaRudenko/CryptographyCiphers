using Combinatorics.Collections;

namespace CVUT.Сryptography.Ciphers;

public static class CompleteTableWithPassword
{
    public static IEnumerable<(string key, string text)> BruteForceDecrypt(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            throw new ArgumentException("Text to decrypt is null or empty", nameof(text));
        }

        text = text.ToLowerInvariant();

        var tableSizes = BaseTranspositionCipher.GetTableSizes(text).ToArray();
        var permutationsByElementsCount = new Dictionary<int, Permutations<int>>(); // key = elements count, value = permutations
        foreach (var colsCount in tableSizes.Select(x => x.colsCount).Distinct())
        {
            var colsNumbers = Enumerable.Range(0, colsCount).ToArray();
            permutationsByElementsCount.Add(colsCount, new Permutations<int>(colsNumbers, GenerateOption.WithoutRepetition));
        }

        var result = new List<ValueTuple<string, string>>(text.Length * 100);

        foreach (var tableSize in tableSizes)
        {
            var table = BaseTranspositionCipher.WriteToTableByRows(tableSize.colsCount, tableSize.rowsCount, text);

            Parallel.ForEach(permutationsByElementsCount[tableSize.colsCount], new ParallelOptions { MaxDegreeOfParallelism = 4 },
                prm =>
                {
                    var permutation = prm.ToArray();
                    var rotatedText = BaseTranspositionCipher.ReadTableByCols(table, permutation);
                    // TODO: can i predict key?
                    result.Add(new ValueTuple<string, string>(
                        $"Table columns = {tableSize.colsCount}, rows = {tableSize.rowsCount}, permutation = '{string.Join(",", permutation)}'",
                        rotatedText));

                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                });

            //foreach (var permutationByElementsCount in permutationsByElementsCount[tableSize.colsCount])
            //{
            //    var permutation = permutationByElementsCount.ToArray();
            //    var rotatedText = BaseTranspositionCipher.ReadTableByCols(table, permutation);
            //    // TODO: can i predict key?
            //    yield return new ValueTuple<string, string>(
            //        $"Table columns = {tableSize.colsCount}, rows = {tableSize.rowsCount}, permutation = '{string.Join(",", permutation)}'",
            //        rotatedText);
            //}
        }
        return result;
    }
}