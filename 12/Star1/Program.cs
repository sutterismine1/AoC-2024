
string cwd = "../../../../"; 
string[] contents = File.ReadAllLines(cwd + "input.txt");
char[,] garden = new char[contents.Length, contents[0].Length];

int xUpper = contents[0].Length-1;
int yUpper = contents.Length-1;
int xLower = 0;
int yLower = 0;

HashSet<(int, int)> visited = new HashSet<(int, int)> ();
for (int i = 0; i < contents.Length; i++)
{
    for (int j = 0; j < contents[i].Length; j++)
    {
        garden[i, j] = contents[i][j];
    }
}

(int, int) findRegion((int, int) xy, char plot, HashSet<(int, int)> visited, int area, int perimeter)   //returns (area, perimeter) of region
{
    area += 1;
    var x = xy.Item1;
    var y = xy.Item2;
    visited.Add((x, y));
    List<(int, int)> possibleMoves = new List<(int, int)> ();
    if(y > yLower && garden[y-1,x] == plot) { possibleMoves.Add((x, y-1)); }
    if(x < xUpper && garden[y, x+1] == plot) { possibleMoves.Add((x + 1, y)); }
    if(y < yUpper && garden[y + 1, x] == plot) { possibleMoves.Add((x, y + 1)); }
    if(x > xLower && garden[y, x-1] == plot) { possibleMoves.Add((x-1, y)); }
    perimeter += 4 - possibleMoves.Count();
    if (possibleMoves.All(x => visited.Contains(x)))
    {
        return (area, perimeter);
    }
    foreach ((int, int) move in possibleMoves)
    {
        if (!visited.Contains(move))
        {
            var result = findRegion(move, plot, visited, area, perimeter);
            area = result.Item1;
            perimeter = result.Item2;
        }
    }
    return (area, perimeter);
}
int sum = 0;
for (int i = 0; i < garden.GetLength(0); i++)
{
    for (int j = 0; j < garden.GetLength(1); j++)
    {
        if (!visited.Contains((j, i)))
        {
            var area = 0;
            var perimeter = 0;
            var value = findRegion((j, i), garden[i,j], visited, area, perimeter);
            Console.WriteLine(value);
            sum += value.Item1 * value.Item2;
        }
    }
}
Console.WriteLine(sum);