using System.Collections.Generic;

string cwd = "../../../../";
string[] contents = File.ReadAllLines(cwd + "input.txt");
int width = 71;
int height = 71;
int numOfBytes = 0;
(int, int) end = (70, 70);

HashSet<(int x, int y)> walls = new HashSet<(int x, int y)>();
HashSet<(int x, int y)> nodes = new HashSet<(int x, int y)>();

(Dictionary<(int x, int y), double> dist, Dictionary<(int x, int y), (int x, int y)> prev) solve()
{
    Dictionary<(int x, int y), double> dist = new Dictionary<(int x, int y), double>();
    Dictionary<(int x, int y), (int x, int y)> prev = new Dictionary<(int x, int y), (int x, int y)>();
    List<(int x, int y)> queue = new List<(int x, int y)>();
    foreach (var node in nodes)
    {
        dist.Add(node, double.PositiveInfinity);
        prev.Add(node, (-1, -1));
        queue.Add(node);
    }
    dist[(0, 0)] = 0;

    while (queue.Count > 0)
    {
        var vertex = queue.MinBy(x => dist[x]);
        queue.Remove(vertex);
        var neighbors = queue.Where(t => t.x == vertex.x && Math.Abs(t.y - vertex.y) == 1 ||
                                         t.y == vertex.y && Math.Abs(t.x - vertex.x) == 1);
        foreach (var neighbor in neighbors)
        {
            double distance = dist[vertex] + 1;
            if (distance < dist[neighbor])
            {
                dist[neighbor] = distance;
                prev[neighbor] = vertex;
            }
        }

    }
    return (dist, prev);
}

for (int i = 0; i < height; i++)
{
    for (int j = 0; j < width; j++)
    {
        if (walls.Contains((j, i)))
        {
            Console.Write('#');
        }
        else
        {
            Console.Write('.');
            nodes.Add((j, i));
        }
    }
    Console.WriteLine();
}
/*for (int i = 0; i < 1024; i++)
{
    string line = contents[i];
    var coord = line.Split(',');
    int x = int.Parse(coord[0]);
    int y = int.Parse(coord[1]);
    walls.Add((x, y));
    nodes.Remove((x, y));
}*/

(Dictionary<(int x, int y), double> dist, Dictionary<(int x, int y), (int x, int y)> prev) result;
(Dictionary<(int x, int y), double> dist, Dictionary<(int x, int y), (int x, int y)> prev)[] prevResult = new (Dictionary<(int x, int y), double> dist, Dictionary<(int x, int y), (int x, int y)> prev)[1];
HashSet<(int x, int y)> lastPath = new HashSet<(int x, int y)>();
bool first = true;
for (; numOfBytes < contents.Length; numOfBytes++) {
    string line = contents[numOfBytes];
    var coord = line.Split(',');
    int x = int.Parse(coord[0]);
    int y = int.Parse(coord[1]);
    walls.Add((x, y));
    nodes.Remove((x, y));
    if (lastPath.Contains((x, y)) || first)
    {
        result = solve();
        first = false;
    }
    else
    {
        result = prevResult[0];
    }

    Console.WriteLine(numOfBytes + "Bytes: " + result.dist[end]);
    if (result.dist[end] == double.PositiveInfinity) { Console.WriteLine(x + "," + y); break; }
    (int x, int y) currentNode = end;
    HashSet<(int x, int y)> path = new HashSet<(int x, int y)>();
    while (currentNode != (-1, -1))
    {
        path.Add(currentNode);
        currentNode = result.prev[currentNode];
    }
    for (int i = 0; i < height; i++)
    {
        for (int j = 0; j < width; j++)
        {
            if (walls.Contains((j, i)))
            {
                Console.Write('#');
            }
            else if (path.Contains((j, i)))
            {
                Console.Write('O');
            }
            else
            {
                Console.Write('.');
            }
        }
        Console.WriteLine();
    }
    prevResult[0] = result;
    lastPath = path;
}