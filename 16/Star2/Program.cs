using System.Linq;
using System.Xml.Linq;

class Race
{
    private string[] contents;
    private char[,,] maze;
    private (int x, int y, int dir)[] nodes;
    (int, int, int) start;
    List<(int x, int y, int dir)> endStates = new List<(int x, int y, int dir)>();

    enum Direction
    {
        U,
        R,
        D,
        L
    }

    public Race()
    {
        string cwd = "../../../../";
        contents = File.ReadAllLines(cwd + "input.txt");
        maze = new char[contents.Length, contents[0].Length, 4];
        var tempNodes = new List<(int x, int y, int dir)>();
        for (int i = 0; i < contents.Length; i++)
        {
            for (int j = 0; j < contents[i].Length; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    maze[i, j, k] = contents[i][j];
                    if (maze[i, j, k] == 'S')
                    {
                        start = (j, i, (int)Direction.R);
                        tempNodes.Add((j, i, k));
                    }
                    else if (maze[i, j, k] != '#')
                    {
                        tempNodes.Add((j, i, k));
                        if (maze[i, j, k] == 'E') { endStates.Add((j, i, k)); }
                    }
                }
            }
        }
        nodes = tempNodes.ToArray();
    }
    public void printMaze()
    {
        for (int i = 0; i < maze.GetLength(0); i++)
        {
            for (int j = 0; j < maze.GetLength(1); j++)
            {
                Console.Write(maze[i,j,0]);
            }
            Console.WriteLine();
        }
    }


    public void solve()
    {
        int[] dx = {0, 1, 0, -1};
        int[] dy = { -1, 0, 1, 0 };
        Dictionary<int, double> dist = new Dictionary<int, double>();
        List<(int x, int y, int dir)> prev = new List<(int x, int y, int dir)>();
        List<int> queue = new List<int>();  //should have made this a priority queue for better efficiency
        var nodesEnum = nodes.Select((node, index) => new { index, node });
        foreach (var node in nodesEnum)
        {
            dist.Add(node.index, double.PositiveInfinity);
            if (node.node == start)
            {
                queue.Add(node.index);
                prev.Add((start.Item1 - 1, start.Item2,(int)Direction.R));
                continue;
            }
            queue.Add(node.index);
            prev.Add((-1, -1,-1));
        }
        dist[Array.IndexOf(nodes, start)] = 0;
        while (queue.Count > 0)
        {
            (int x, int y, int dir) vertex = nodes[queue.MinBy(node => dist[node])];
            var vertexIndex = Array.IndexOf(nodes, vertex);
            queue.Remove(queue.Where(t => t == vertexIndex).First());
            var previousNode = prev[vertexIndex];
            var neighbors = nodes.Select((node, index) => new { index, node })
                .Where(node => (node.node.x == vertex.x + dx[vertex.dir] && node.node.y == vertex.y + dy[vertex.dir] && node.node.dir == vertex.dir ||//go one step in current direction
                (node.node.x == vertex.x && node.node.y == vertex.y && node.node.dir == (vertex.dir+1) % 4) || 
                (node.node.x == vertex.x && node.node.y == vertex.y && node.node.dir == (vertex.dir-1 + 4) % 4)));   //turn 90 degrees in place.
            //Console.WriteLine("vertex" + vertex);
            foreach (var neighbor in neighbors)
            {
                var neighborIndex = neighbor.index;
                //Console.WriteLine(neighbor);
                double distanceAlt;
                if (neighbor.node.x == vertex.x && neighbor.node.y == vertex.y)  //if the node is a rotation
                {
                    distanceAlt = dist[vertexIndex] + 1000;
                }
                else { 
                    distanceAlt = dist[vertexIndex] + 1;
                }
                if (distanceAlt < dist[neighborIndex])
                {
                    dist[neighborIndex] = distanceAlt;
                    prev[neighborIndex] = vertex;
                }
            }
        }
        foreach (var state in endStates)
        {
            Console.WriteLine(dist[Array.IndexOf(nodes, state)]);
        }
        var end = endStates.MinBy(node => dist[Array.IndexOf(nodes, node)]);
        Console.WriteLine(dist[Array.IndexOf(nodes, end)]);
        /*(int x, int y, int dir) currentNode = end;
        while ((currentNode = prev[Array.IndexOf(nodes, currentNode)]) != (start.Item1 - 1, start.Item2, (int)Direction.R))
        {
            maze[currentNode.y, currentNode.x, 0] = 'O';
            Console.WriteLine(dist[Array.IndexOf(nodes, currentNode)]);
        }*/
        
        var nodeQueue = new List<(int x, int y, int dir)> { end };
        var visited = new HashSet<(int x, int y, int dir)> { };
        (int x, int y, int dir) currentNode;
        while (nodeQueue.Count > 0)
        {  
            currentNode = nodeQueue.First();
            var nodeIndex = Array.IndexOf(nodes, currentNode);
            visited.Add(currentNode);
            nodeQueue.Remove(currentNode);
            maze[currentNode.y, currentNode.x, 0] = 'O';
            //Console.WriteLine("vertex" + currentNode);
            var neighbors = nodes.Select((node, index) => new { index, node })
                .Where(node => (node.node.x == currentNode.x - dx[currentNode.dir] && node.node.y == currentNode.y - dy[currentNode.dir] && node.node.dir == currentNode.dir ||//go one step in current direction
                (node.node.x == currentNode.x && node.node.y == currentNode.y && node.node.dir == (currentNode.dir + 1) % 4) ||
                (node.node.x == currentNode.x && node.node.y == currentNode.y && node.node.dir == (currentNode.dir - 1 + 4) % 4)));   //turn 90 degrees in place.
            foreach (var nbr in neighbors) {
                //Console.WriteLine("nbr: " + nbr);
                double costFromNodeToNeigbour;
                if (nbr.node.x == currentNode.x && nbr.node.y == currentNode.y)  //if the node is a rotation
                {
                    costFromNodeToNeigbour = 1000;
                }
                else
                {
                    costFromNodeToNeigbour = 1;
                }
                double minCost;
                minCost = dist[nodeIndex] - costFromNodeToNeigbour;
                if (dist[nbr.index] == minCost)
                {
                    nodeQueue.Add(nbr.node);
                }
            }
        }
        Console.WriteLine(maze.Cast<char>().ToArray().Where(node => node == 'O').Count());
    }
    static void Main(string[] args)
    {
        Race r = new Race();
        r.printMaze();
        r.solve();
        r.printMaze();
    }
}
