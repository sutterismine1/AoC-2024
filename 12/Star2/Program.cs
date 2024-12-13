
string cwd = "../../../../";
string[] contents = File.ReadAllLines(cwd + "input.txt");
char[,] garden = new char[contents.Length, contents[0].Length];

int xUpper = contents[0].Length - 1;
int yUpper = contents.Length - 1;
int xLower = 0;
int yLower = 0;

HashSet<(int, int)> visited = new HashSet<(int, int)>();
for (int i = 0; i < contents.Length; i++)
{
    for (int j = 0; j < contents[i].Length; j++)
    {
        garden[i, j] = contents[i][j];
    }
}

(int, List<(double, double, char)>) findRegion((int, int) xy, char plot, HashSet<(int, int)> visited, int area, List<(double,double, char)> fences)   //returns (area, fences) of region
{
    area += 1;
    var x = xy.Item1;
    var y = xy.Item2;
    visited.Add((x, y));
    List<(int, int)> moves = new List<(int, int)> { (x, y - 1), (x + 1, y), (x, y + 1), (x - 1, y) };
    List<(int, int)> possibleMoves = new List<(int, int)>();
    List<(int, int)> fencesToBuild = new List<(int, int) >();
    if (y > yLower && garden[y - 1, x] == plot) { possibleMoves.Add((x, y - 1)); }
    if (x < xUpper && garden[y, x + 1] == plot) { possibleMoves.Add((x + 1, y)); }
    if (y < yUpper && garden[y + 1, x] == plot) { possibleMoves.Add((x, y + 1)); }
    if (x > xLower && garden[y, x - 1] == plot) { possibleMoves.Add((x - 1, y)); }
    fencesToBuild.AddRange(moves.Where(x => !possibleMoves.Contains(x)));
    foreach(var fence in fencesToBuild)
    {
        if(fence == ((x, y - 1)))
        {
            fences.Add((x,y - 0.5,'U'));
        }
        else if (fence == ((x, y + 1)))
        {
            fences.Add((x, y + 0.5,'D'));
        }
        else if (fence == ((x + 1, y)))
        {
            fences.Add((x+0.5, y,'R'));
        }
        else if (fence == ((x - 1, y)))
        {
            fences.Add((x-0.5, y,'L'));
        }
    }
    if (possibleMoves.All(x => visited.Contains(x)))
    {
        return (area, fences);
    }
    foreach ((int, int) move in possibleMoves)
    {
        if (!visited.Contains(move))
        {
            var result = findRegion(move, plot, visited, area, fences);
            area = result.Item1;
            fences = result.Item2;
        }
    }
    return (area, fences);
}
int sum = 0;
for (int i = 0; i < garden.GetLength(0); i++)
{
    for (int j = 0; j < garden.GetLength(1); j++)
    {
        if (!visited.Contains((j, i)))
        {
            var area = 0;
            var fences = new List<(double, double, char)>();
            int fenceCount = 0;
            var value = findRegion((j, i), garden[i, j], visited, area, fences);
            for (double k = -0.5; k < garden.GetLength(0)+0.5; k++) {
                var sortedFences = value.Item2.Where(x => x.Item1 == k).ToList();
                sortedFences.Sort();
                var regionFenceCount = sortedFences.Where((x, index) => index == 0 || (x.Item3 == sortedFences[index - 1].Item3 && x.Item2 != sortedFences[index - 1].Item2 + 1) || x.Item3 != sortedFences[index - 1].Item3).Count();
                fenceCount += regionFenceCount;
            }
            for (double k = -0.5; k < garden.GetLength(1) + 0.5; k++)
            {
                var sortedFences = value.Item2.Where(x => x.Item2 == k).ToList();
                sortedFences.Sort();
                var regionFenceCount = sortedFences.Where((x, index) => index == 0 || (x.Item3 == sortedFences[index - 1].Item3 && x.Item1 != sortedFences[index - 1].Item1 + 1) || x.Item3 != sortedFences[index -1].Item3).Count();
                fenceCount += regionFenceCount;
            }
            sum += value.Item1 * fenceCount;
        }
    }
}
Console.WriteLine(sum);