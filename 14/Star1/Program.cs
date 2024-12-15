using System.Text.RegularExpressions;

string cwd = "../../../../"; 
string[] contents = File.ReadAllLines(cwd + "input.txt");
string pattern = "p=(\\d+),(\\d+) v=(-?\\d+),(-?\\d+)";
int seconds = 100;
int xLower = 0;
int yLower = 0;
int xUpper = 101-1; //width - 1
int yUpper = 103-1; //height - 1
int quadXDivision = 50;
int quadYDivision = 51;
int[] quads = [0, 0, 0, 0];
foreach (string line in contents)
{
    var robot = Regex.Match(line, pattern).Groups;
    int x = Convert.ToInt32(robot[1].Value);
    int y = Convert.ToInt32(robot[2].Value);
    int vX = Convert.ToInt32(robot[3].Value);
    int vY = Convert.ToInt32(robot[4].Value);
    for (int second = 0;  second<seconds; second++)
    {
        x = x + vX;
        y = y + vY;
        if (x < xLower)
        {
            int difference = Math.Abs(xLower - x);
            x = xUpper+1 - difference;
        }
        if (y < yLower)
        {
            int difference = Math.Abs(yLower - y);
            y = yUpper+1 - difference;
        }
        if (y > yUpper)
        {
            int difference = Math.Abs(yUpper - y);
            y = yLower-1 + difference;
        }
        if (x > xUpper)
        {
            int difference = Math.Abs(xUpper - x);
            x = xLower-1 + difference;
        }
    }
    Console.WriteLine("(" + x + "," + y + ")");
    if(x < quadXDivision && y < quadYDivision) { quads[0]++; }
    else if(x < quadXDivision && y > quadYDivision) { quads[1]++;}
    else if(x > quadXDivision && y < quadYDivision) { quads[2]++; }
    else if(x > quadXDivision && y > quadYDivision) { quads[3]++; }
}
int product = 1;
foreach(int quadCount in quads)
{
    product *= quadCount;
}
Console.WriteLine(product);