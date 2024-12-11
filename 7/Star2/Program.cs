using MyUtilities;
string cwd = "../../../../";
string[] contents = File.ReadAllLines(cwd + "input.txt");

static string toBase3(int n, int numberOfBits)
{
    char[] result = new char[numberOfBits];
    int i = result.Length - 1;
    if (n == 0)
    {
        for (int j = 0; j < result.Length; j++)
        {
            result[j] = '0';
        }
        return new string(result);
    }
    while (n > 0)
    {
        result[i] = (char)((n % 3) + '0');
        n /= 3;
        i--;
    }
    for (int b = 0; b < numberOfBits; b++)
    {
        if (result[b] == '\0') { result[b] = '0'; }
    }
    return new string(result);
}

static long tryOperators(string line, int lineIndex)
{
    var linePart = line.Split(":");
    long resultValue = long.Parse(linePart[0]);
    string operandsRaw = linePart[1].Trim();
    string[] operandsStr = operandsRaw.Split(" ");
    long[] operands = operandsStr.Select(x => long.Parse(x)).ToArray();
    int numberOfOperators = operands.Length - 1;
    double highestDecimalRep = Math.Pow(3, numberOfOperators) - 1;
    for (int i = 0; i <= highestDecimalRep; i++)
    {
        char[] operators = new char[numberOfOperators];
        string ternary = toBase3(i, numberOfOperators);
        foreach (var (index, bit) in ternary.Enumerate())
        {
            if (bit == '0')
            {
                operators[index] = '+';
            }
            else if (bit == '1')
            {
                operators[index] = '*';
            }
            else
            {
                operators[index] = '|';
            }
        }
        long total = operands[0];
        for (int j = 1; j < operands.Length; j++)
        {
            if (operators[j - 1] == '+')
            {
                total += operands[j];
            }
            else if (operators[j-1] == '*')
            {
                total *= operands[j];
            }
            else
            {
                string temp = total.ToString();
                temp = temp + operands[j].ToString();
                total = long.Parse(temp);
            }
        }
        if (total == resultValue)
        {
            Console.Write(lineIndex + ": ");
            Console.WriteLine(resultValue);
            return resultValue;
        }
    }
    return 0;
}

long sum = 0;
for (int i = 0; i < contents.Length; i++)
{
    string line = contents[i];
    long result = tryOperators(line, i);
    sum += result;
}
Console.WriteLine(sum);