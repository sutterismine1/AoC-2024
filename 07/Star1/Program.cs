using MyUtilities;
string cwd = "../../../../"; 
string[] contents = File.ReadAllLines(cwd + "input.txt");

static long tryOperators(string line, int lineIndex)
{
    var linePart = line.Split(":");
    long resultValue = long.Parse(linePart[0]);
    string operandsRaw = linePart[1].Trim();
    string[] operandsStr = operandsRaw.Split(" ");
    long[] operands = operandsStr.Select(x=>long.Parse(x)).ToArray();
    int numberOfOperators = operands.Length - 1;
    double highestDecimalRep = Math.Pow(2, numberOfOperators)-1;
    for (int i = 0; i <= highestDecimalRep; i++)
    {
        char[] operators = new char[numberOfOperators];
        string binary = Convert.ToString(i, 2).PadLeft(numberOfOperators, '0');
        foreach (var (index, bit) in binary.Enumerate())
        {
            if (bit == '0')
            {
                operators[index] = '+';
            }
            else
            {
                operators[index] = '*';
            }
        }
        long total = operands[0];
        for (int j = 1; j < operands.Length; j++)
        {
            if (operators[j - 1] == '+')
            {
                total += operands[j];
            }
            else
            {
                total *= operands[j];
            }
        }
        if(total == resultValue)
        {
            Console.Write(lineIndex + ": ");
            Console.WriteLine(resultValue);
            return resultValue;
        }
    }
    return 0;
}

long sum = 0;
for(int i = 0; i < contents.Length; i++)
{
    string line = contents[i];
    long result = tryOperators(line, i);
    sum += result;
}
Console.WriteLine(sum);