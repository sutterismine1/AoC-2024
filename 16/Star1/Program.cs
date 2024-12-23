class Race {
    private string[] contents;
    private char[,] maze;
    private List<(int x, int y)> nodes;
    (int, int) start;
    (int, int) end;

    public Race()
    {
        string cwd = "../../../../";
        contents = File.ReadAllLines(cwd + "input.txt");
        maze = new char[contents.Length, contents[0].Length];
        nodes = new List<(int x, int y)>();      
        for (int i = 0; i < contents.Length; i++)
        {
            for (int j = 0; j < contents[i].Length; j++)
            {
                maze[i, j] = contents[i][j];
                if (maze[i, j] == 'S')
                {
                    start = (j, i);
                    nodes.Add((j, i));
                } else if (maze[i, j] != '#')
                {
                    nodes.Add((j, i));
                    if(maze[i,j] == 'E') { end = (j,i); }
                }
            }
        }
    }
    public void printMaze()
    {
        for (int i = 0; i < maze.GetLength(0); i++)
        {
            for (int j = 0; j < maze.GetLength(1); j++)
            {
                Console.Write(maze[i,j]);
            }
            Console.WriteLine();
        }
    }
    
    
    public void solve()
    {
        List<double> dist = new List<double>();
        List<(int, int)> prev = new List<(int, int)>();
        List<int> queue = new List<int>();
        var nodesEnum = nodes.Select((node, index) => new { index,node });
        foreach(var node in nodesEnum)
        {
            dist.Add(double.PositiveInfinity);
            if (node.node == start){
                queue.Add(node.index);
                prev.Add((start.Item1 - 1, start.Item2));
                continue;
            }
            queue.Add(node.index);
            prev.Add((-1,-1));
        }
        dist[nodes.IndexOf(start)] = 0;
        char currentDir = 'h';
        while(queue.Count > 0)
        {
            (int x, int y) vertex = nodes[queue.MinBy(node => dist[node])];
            queue.Remove(queue.Where(t=>t==nodes.IndexOf(vertex)).First());
            var previousNode = prev[nodes.IndexOf(vertex)];
            if (previousNode != (start.Item1 - 1, start.Item2)) { Console.WriteLine(previousNode + ": " + dist[nodes.IndexOf(previousNode)]); }
            currentDir = (Math.Abs(previousNode.Item2 - vertex.y) == 1 && previousNode.Item1 == vertex.x) ? 'v' : 'h';
            var neighbors = nodes.Select((node, index) => new { index, node })
                .Where(node => (Math.Abs(node.node.y - vertex.y) == 1 && node.node.x == vertex.x && queue.Any(t=>t==node.index)) || (Math.Abs(node.node.x - vertex.x) == 1 && node.node.y == vertex.y && queue.Any(t => t == node.index)))
                .Select(node => new
                {
                    node.index,
                    node.node,
                    direction = (Math.Abs(node.node.y - vertex.y) == 1 && node.node.x == vertex.x) ? 'v' : 'h'
                }).ToList(); //select all adjacent nodes that are in the queue
            foreach (var neighbor in neighbors)
            {
                double distance = dist[nodes.IndexOf(vertex)] + 1;
                if (neighbor.direction != currentDir)
                {
                    distance = dist[nodes.IndexOf(vertex)] + 1001;
                }
                if (distance < dist[nodes.IndexOf(neighbor.node)])
                {
                    dist[nodes.IndexOf(neighbor.node)] = distance;
                    prev[nodes.IndexOf(neighbor.node)] = vertex;
                }
            }
        }
        (int x, int y) currentNode = end;
        while((currentNode = prev[nodes.IndexOf(currentNode)]) != (start.Item1 - 1, start.Item2))
        {
            maze[currentNode.y, currentNode.x] = 'O';
            Console.WriteLine(dist[nodes.IndexOf(currentNode)]);
        }
        Console.WriteLine(end + ": " + dist[nodes.IndexOf(end)]);
    }
    static void Main(string[] args)
    {
        Race r = new Race();
        r.printMaze();
        r.solve();
        r.printMaze();
    }
}
