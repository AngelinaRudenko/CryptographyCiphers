using CVUT.Сryptography.Ciphers;

Console.WriteLine("Choose a cipher:\n0 - Caesar");

var cipherId = Convert.ToInt32(Console.ReadLine());

switch (cipherId)
{
    case 0:
        {
            Console.WriteLine("Choose an operation:\n0 - Encode\n1 - Decode and you know key\n2 - Brute force hack decode");

            var operationId = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Enter text");
            var text = Console.ReadLine();

            if (text == null)
            {
                return;
            }

            var key = 0;
            if (operationId == 0 || operationId == 1)
            {
                Console.WriteLine("Enter key");
                key = Convert.ToInt32(Console.ReadLine());
            }


            switch (operationId)
            {
                case 0:
                    Console.WriteLine(ShiftCipher.Encrypt(text, key));
                    break;
                case 1:
                    Console.WriteLine(ShiftCipher.Decrypt(text, key));
                    break;
                case 2:
                    foreach (var possibleSolution in ShiftCipher.BruteForceDecrypt(text))
                    {
                        Console.WriteLine($"Key = {possibleSolution.key}, Text = {possibleSolution}");
                        Console.WriteLine();
                    }
                    break;
                default:
                    Console.WriteLine("Unknown operation");
                    return;
            }

            break;
        }
    default:
        Console.WriteLine("Unknown cipher");
        return;
}