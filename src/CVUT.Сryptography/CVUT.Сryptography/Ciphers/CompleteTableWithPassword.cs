using Combinatorics.Collections;

namespace CVUT.Сryptography.Ciphers;

// TODO: do not use, inefficient
public class CompleteTableWithPassword : IBruteForce
{
    public IEnumerable<(string key, string text)> BruteForceDecrypt(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            throw new ArgumentException("Text to decrypt is null or empty", nameof(text));
        }

        text = text.ToLowerInvariant();

        var tableSizes = BaseTransposition.GetTableSizes(text).ToArray();
        var permutationsByElementsCount = new Dictionary<int, Permutations<int>>(); // key = elements count, value = permutations
        foreach (var colsCount in tableSizes.Select(x => x.colsCount).Distinct())
        {
            var colsNumbers = Enumerable.Range(0, colsCount).ToArray();
            permutationsByElementsCount.Add(colsCount, new Permutations<int>(colsNumbers, GenerateOption.WithoutRepetition));
        }

        var result = new List<ValueTuple<string, string>>(text.Length * 100);

        foreach (var tableSize in tableSizes)
        {
            var table = BaseTransposition.WriteToTableByRows(tableSize.colsCount, tableSize.rowsCount, text);

            Parallel.ForEach(permutationsByElementsCount[tableSize.colsCount], new ParallelOptions { MaxDegreeOfParallelism = 5 },
                prm =>
                {
                    var permutation = prm.ToArray();
                    var rotatedText = BaseTransposition.ReadTableByCols(table, permutation);
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
            //    var rotatedText = BaseTransposition.ReadTableByCols(table, permutation);
            //    // TODO: can i predict key?
            //    yield return new ValueTuple<string, string>(
            //        $"Table columns = {tableSize.colsCount}, rows = {tableSize.rowsCount}, permutation = '{string.Join(",", permutation)}'",
            //        rotatedText);
            //}
        }
        return result;
    }
}