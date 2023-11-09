using CVUT.Сryptography;
using CVUT.Сryptography.Ciphers;
using CVUT.Сryptography.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;

var inputPath = Path.Combine(Directory.GetCurrentDirectory(), "Files\\input.txt");
var tasksAsString = await File.ReadAllTextAsync(inputPath);

#region Setup SymSpell

//set parameters
const int initialCapacity = 82765;
const int maxEditDistance = 0;
const int prefixLength = 7;
var symSpell = new SymSpell(initialCapacity, maxEditDistance, prefixLength);

//Load a frequency dictionary
const string frequencyDictionaryPath = @"C:\Users\Admin\.nuget\packages\symspell\6.7.2\contentFiles\any\netcoreapp3.0\frequency_dictionary_en_82_765.txt";
if (!symSpell.LoadDictionary(frequencyDictionaryPath, 0, 1))
{
    Console.Error.WriteLine("File not found: " + Path.GetFullPath(frequencyDictionaryPath));
    return;
}

#endregion

#region Prepare ciphers combinations

var ciphers = new IBruteForce[]
{
    new ShiftCipher(),
    //new SubstitutionWithPassword(),
    new CompleteTableWithoutPassword(),
    //new CompleteTableWithPassword(),
    new DoubleTableTransposition()
};

var ciphersCombinations = new List<List<IBruteForce>>();

foreach (var cipher in ciphers)
{
    ciphersCombinations.Add(new List<IBruteForce>{cipher});
}

foreach (var cipher1 in ciphers)
{
    foreach (var cipher2 in ciphers)
    {
        ciphersCombinations.Add(new List<IBruteForce> { cipher1, cipher2 });
    }
}

#endregion

var output = new StringBuilder();
foreach (var rawTask in tasksAsString.Split("Task ").Where(x => !string.IsNullOrEmpty(x)))
{
    var taskNumber = "";
    var textStartsAtIndex = 0;
    for (var i = 0; i < rawTask.Length; i++)
    {
        if (!rawTask[i].Equals(':'))
        {
            taskNumber += rawTask[i];
        }
        else
        {
            textStartsAtIndex = i + 1;
            break;
        }
    }

    var text = rawTask.Substring(textStartsAtIndex).Trim().ToLowerInvariant();


    var mayBeMonoAlphabeticSubstitution = Alphabet.MayBeMonoAlphabeticSubstitution(text);
    var ic = Alphabet.GetIndexOfCoincidence(text);

    Console.WriteLine(mayBeMonoAlphabeticSubstitution
        ? $"Probably mono alphabetic substitution, IC = {ic}"
        : $"Probably not mono alphabetic substitution, IC = {ic}");

    output.AppendLine($"Task {taskNumber}");
    output.AppendLine($"Original text = '{text}'\n");

    foreach (var combination in ciphersCombinations)
    {
        var readTextsFrom = new List<(string key, string text)>() { new ("Plain text", text) };
        var bruteForceResults = new List<(string key, string text)>();

        var combinationName = new StringBuilder();

        foreach (var cipher in combination)
        {
            combinationName.Append($"-> {cipher.GetType().Name}");

            foreach (var tempText in readTextsFrom)
            {
                foreach (var tempBruteForceResult in cipher.BruteForceDecrypt(tempText.text))
                {
                    bruteForceResults.Add(new ValueTuple<string, string>($"{tempText.key} -> {tempBruteForceResult.key}", tempBruteForceResult.text));
                }
            }

            readTextsFrom = bruteForceResults.ToList();

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        var result = new List<CipherAnswer>();
        foreach (var bruteForceResult in bruteForceResults)
        {
            var suggestion = symSpell.WordSegmentation(bruteForceResult.text);

            var cipherAnswer = new CipherAnswer
            {
                Key = bruteForceResult.key,
                DecodedText = bruteForceResult.text,
                PlainText = suggestion.segmentedString,
                ProbabilityLogSum = suggestion.probabilityLogSum,
                DistanceSum = suggestion.distanceSum
            };

            result.Add(cipherAnswer);
        }

        foreach (var item in result.OrderByDescending(x => x.ProbabilityLogSum).Take(5))
        {
            output.AppendLine(item.ToString());
        }

        output.AppendLine("\n\n");
    }
}

var outputPath = Path.Combine(Directory.GetCurrentDirectory(), "Files\\output.txt");
await File.WriteAllTextAsync(outputPath, output.ToString());

Console.WriteLine("Done");
