using System.Text.RegularExpressions;

string cwd = "../../../../";
string[] contents = File.ReadAllLines(cwd + "input.txt");
string pattern = "p=(\\d+),(\\d+) v=(-?\\d+),(-?\\d+)";
char[,] map = new char[103, 101]; 
int seconds = 10000;
int xLower = 0;
int yLower = 0;
int xUpper = 101 - 1; //width - 1
int yUpper = 103 - 1; //height - 1

resetMap(map);
static void printMap(char[,] map)
{
    for (int i = 0; i < 103; i++) 
    {
        for (int j = 0; j < 101; j++)
        {
            Console.Write(map[i, j]);
        }
        Console.WriteLine();
    }
}

static void resetMap(char[,] map)
{
    for (int i = 0; i < 103; i++)   //map initialization (each spot is '.')
    {
        for (int j = 0; j < 101; j++)
        {
            map[i, j] = '.';
        }
    }
}

static bool checkMapForLine(char[,] map, int xUpper)
{
    bool line = false;
    for (int i = 0; i < 103; i++)   //map initialization (each spot is '.')
    {
        for (int j = 0; j < 101; j++)
        {
            if (map[i, j] == '#' && !line)
            {
                bool lineFound = true;
                for (int k = j; k < j + 10; k++)
                {
                    if (k < (xUpper - 10))
                    {
                        if (map[i, k] != '#')
                        {
                            line = false;
                            lineFound = false;
                        }

                    }
                    else { lineFound = false; }
                }
                if (lineFound) { line = true; }
            }
        }
    }
    return line;
}
for (int second = 0; second < seconds; second++) {  //try each second
    foreach (string line in contents)   // handle each robot individually
    {
        var robot = Regex.Match(line, pattern).Groups;
        int x = Convert.ToInt32(robot[1].Value);
        int y = Convert.ToInt32(robot[2].Value);
        int vX = Convert.ToInt32(robot[3].Value);
        int vY = Convert.ToInt32(robot[4].Value);
        for (int i = 0; i < second; i++)
        {
            x = x + vX;
            y = y + vY;
            if (x < xLower)
            {
                int difference = Math.Abs(xLower - x);
                x = xUpper + 1 - difference;
            }
            if (y < yLower)
            {
                int difference = Math.Abs(yLower - y);
                y = yUpper + 1 - difference;
            }
            if (y > yUpper)
            {
                int difference = Math.Abs(yUpper - y);
                y = yLower - 1 + difference;
            }
            if (x > xUpper)
            {
                int difference = Math.Abs(xUpper - x);
                x = xLower - 1 + difference;
            }
        }
        map[y, x] = '#';
    }
    Console.WriteLine(second);
    if (checkMapForLine(map, xUpper)) { printMap(map); }
    resetMap(map);
}