// See https://aka.ms/new-console-template for more information
string cwd = "../../../../"; 
string[] contents = File.ReadAllLines(cwd + "input.txt");

static bool checkSafety(string report)
{
    string[] reportItems = report.Split(' ');
    int[] values = Array.ConvertAll(reportItems, int.Parse);
    char incOrDec = 'n';
    if (values[1] <= values[0])
    {
        incOrDec = 'd';
    }
    else if (values[1] >= values[0])
    {
        incOrDec = 'i';
    }
    for (int i = 1; i<=values.Length-1; i++)
    {
        if (incOrDec == 'd')
        {
            if (values[i] >= values[i - 1])
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
        }
        else if(incOrDec == 'i')
        {
            if (values[i] <= values[i - 1])
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
        }
    }
    return true;
}

int safeCount = 0;
foreach  (string line in contents)
{
    if (checkSafety(line)) {  safeCount++; }
}
Console.WriteLine(safeCount);