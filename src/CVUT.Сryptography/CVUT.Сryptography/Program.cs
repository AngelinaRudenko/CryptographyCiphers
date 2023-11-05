﻿using CVUT.Сryptography.Ciphers;
using System.Text;

var inputPath = Path.Combine(Directory.GetCurrentDirectory(), "Files\\input.txt");
var text = await File.ReadAllTextAsync(inputPath);
text = text.ToLowerInvariant();


//set parameters
const int initialCapacity = 82765;
const int maxEditDistance = 0;
const int prefixLength = 7;
var symSpell = new SymSpell(initialCapacity, maxEditDistance, prefixLength);

//Load a frequency dictionary
var frequencyDictionaryPath = "C:\\Users\\Admin\\.nuget\\packages\\symspell\\6.7.2\\contentFiles\\any\\netcoreapp3.0\\frequency_dictionary_en_82_765.txt";
if (!symSpell.LoadDictionary(frequencyDictionaryPath, 0, 1))
{
    Console.Error.WriteLine("File not found: " + Path.GetFullPath(frequencyDictionaryPath));
    return;
}



var decodedCollection = ShiftCipher.BruteForceDecrypt(text);



var result = new List<(string key, string segmentedString, decimal probabilityLogSum, int distanceSum)>();

foreach (var pair in decodedCollection)
{
    var suggestion = symSpell.WordSegmentation(pair.text);
    result.Add(new ValueTuple<string, string, decimal, int>(pair.key.ToString(), suggestion.segmentedString, suggestion.probabilityLogSum, suggestion.distanceSum));
}


var output = new StringBuilder();
output.AppendLine($"####### {nameof(ShiftCipher)} #######");

foreach (var item in result.OrderByDescending(x => x.probabilityLogSum).Take(3))
{
    output.AppendLine($"Key = {item.key}, Probability Log Sum = {item.probabilityLogSum}, Distance Sum = {item.distanceSum}, Decoded text = '{item.segmentedString}'");
}

var outputPath = Path.Combine(Directory.GetCurrentDirectory(), "Files\\output.txt");
await File.WriteAllTextAsync(outputPath, output.ToString());

Console.WriteLine("Done");
