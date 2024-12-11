// See https://aka.ms/new-console-template for more information

string cwd = "../../../../";
string[] contents = File.ReadAllLines(cwd + "input.txt");

static bool checkSafetyWithoutRemoval(int[] values)
{
    for (int i = 1; i < values.Length - 1; i++)
    {
        if ((values[i - 1] <= values[i] && values[i] >= values[i + 1]) || (values[i - 1] >= values[i] && values[i] <= values[i + 1])) //if i is a local extrema
        {
            return false;
        }
        else
        {
            if (Math.Abs(values[i] - values[i - 1]) >= 4)
            {
                return false;
            }
        }
    }//check edge cases that values[-1] is not 4 away from or equal to values[-2]
    if (Math.Abs(values[values.Length - 1] - values[values.Length - 2]) >= 4)
    {
        return false;
    }
    if (values[values.Length - 1] == values[values.Length - 2])
    {
        return false;
    }
    return true;
}
static bool checkSafety(int[] values)
{
    for(int i = 0; i < values.Length; i++)
    {
        int[] newvalues = values.Where((source, index) => index != i).ToArray();
        if (checkSafetyWithoutRemoval(newvalues))
        {
            Console.WriteLine("Removed from " + string.Join(",", values) + ": " + i);
            return true;
        }
    }
    return false;
}

int safeCount = 0;
foreach (string line in contents)
{
    string[] reportItems = line.Split(' ');
    int[] values = Array.ConvertAll(reportItems, int.Parse);
    if (checkSafety(values)){ safeCount++; }
}
Console.WriteLine(safeCount);