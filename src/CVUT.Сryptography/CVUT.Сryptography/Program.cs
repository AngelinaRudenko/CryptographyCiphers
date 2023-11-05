using CVUT.Сryptography.Ciphers;

var inputPath = Path.Combine(Directory.GetCurrentDirectory(), "Input\\input.txt");
var text = await File.ReadAllTextAsync(inputPath);
text = text.ToLowerInvariant();

var outputPath = Path.Combine(Directory.GetCurrentDirectory(), $"Input\\output_{nameof(ShiftCipher)}.txt");

await ShiftCipher.BruteForceDecrypt(text, outputPath);

Console.WriteLine("Done");