using CVUT.Сryptography;
using CVUT.Сryptography.Ciphers;
using CVUT.Сryptography.Models;
using System.Collections.Generic;
using System.Diagnostics;
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
    new AffineCipher(),
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
        if (cipher1.GetType() == typeof(ShiftCipher) && cipher2.GetType() == typeof(ShiftCipher))
        {
            continue;
        }

        ciphersCombinations.Add(new List<IBruteForce> { cipher1, cipher2 });
    }
}

#endregion

var output = new StringBuilder();
var outputPath = Path.Combine(Directory.GetCurrentDirectory(), "Files\\output.txt");
await File.WriteAllTextAsync(outputPath, string.Empty); // clear the file

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

    Console.WriteLine($"{DateTime.Now:HH:mm:ss} Start brute force for Task {taskNumber}");
    var stopWatch = new Stopwatch();
    stopWatch.Start();

    var text = rawTask.Substring(textStartsAtIndex).Trim().ToLowerInvariant();

    var skipTask = false;
    for (var i = 0; i < text.Length; i++)
    {
        var chr = text[i];
        if (!Alphabet.LetterToNumber.ContainsKey(chr))
        {
            Console.WriteLine($"Character '{chr}' is not presented in the dictionary. Character id int text = '{i}'.");
            skipTask = true;
            break;
        }
    }

    if (skipTask)
    {
        continue;
    }

    var mayBeMonoAlphabeticSubstitution = Alphabet.MayBeMonoAlphabeticSubstitution(text);
    var ic = Alphabet.GetIndexOfCoincidence(text);

    output.AppendLine($"Task {taskNumber}");
    output.AppendLine($"Original text = '{text}'\n");
    output.AppendLine(mayBeMonoAlphabeticSubstitution
        ? $"Probably mono alphabetic substitution, IC = {ic}"
        : $"Probably not mono alphabetic substitution, IC = {ic}");

    var result = new AnswersList(5);

    foreach (var combination in ciphersCombinations)
    {
        var readTextsFrom = new List<(string key, string text)>() { new ("Plain text", text) };
        var bruteForceResults = new List<(string key, string text)>();

        Console.WriteLine($"\t{DateTime.Now:HH:mm:ss} Start ciphers combination {string.Join("->", combination.Select(cipher => cipher.GetType().Name))}");

        foreach (var cipher in combination)
        {
            foreach (var tempText in readTextsFrom)
            {
                foreach (var tempBruteForceResult in cipher.BruteForceDecrypt(tempText.text))
                {
                    bruteForceResults.Add(new ValueTuple<string, string>($"{tempText.key} -> {cipher.GetType().Name}: {tempBruteForceResult.key}", tempBruteForceResult.text));
                }
            }

            readTextsFrom = bruteForceResults.ToList();

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        Console.WriteLine($"\t{DateTime.Now:HH:mm:ss} Finished ciphers combination {string.Join("->", combination.Select(cipher => cipher.GetType().Name))}");

        foreach (var bruteForceResult in bruteForceResults)
        {
            var suggestion = symSpell.WordSegmentation(bruteForceResult.text);

            if (result.GetMinProbability() > suggestion.probabilityLogSum)
            {
                continue;
            }

            var cipherAnswer = new CipherAnswer
            {
                Key = bruteForceResult.key,
                DecodedText = bruteForceResult.text,
                PlainText = suggestion.segmentedString,
                ProbabilityLogSum = suggestion.probabilityLogSum,
                DistanceSum = suggestion.distanceSum
            };

            result.AddIfHigherProbability(cipherAnswer);
        }

        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();
        GC.WaitForPendingFinalizers();
    }


    stopWatch.Stop();
    var elapsedTime = stopWatch.Elapsed;
    Console.WriteLine($"{DateTime.Now:HH:mm:ss} Finished brute force for Task {taskNumber}. Elapse time {elapsedTime.Hours:00}:{elapsedTime.Minutes:00}:{elapsedTime.Seconds:00}\n");

    foreach (var answer in result.Answers)
    {
        output.AppendLine(answer.ToString());
    }

    output.AppendLine("\n\n");
    await File.AppendAllTextAsync(outputPath, output.ToString());
}

Console.WriteLine("Done");
