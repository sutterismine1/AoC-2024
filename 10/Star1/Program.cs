class Map
{
    int[,] map;

    public Map(string[] contents)
    {
        map = new int[contents.Length, contents[0].Length];
        for (int i = 0; i < contents.Length; i++)
        {
            for (int j = 0; j < contents[i].Length; j++)
            {
                map[i, j] = contents[i][j] - '0';
            }
        }
    }
    public void printMap()
    {
        for (int i = 0;i < map.GetLength(0); i++)
        {
            for (int j = 0;j < map.GetLength(1); j++)
            {
                Console.Write(map[i, j]);
            }
            Console.WriteLine();
        }
    }
    public int getScore(int x, int y, int num, List<(int, int)> visited)
    {
        int score = 0;
        visited.Add((x, y));
        if (num == 9) //if we've reached 9, return 1
        {
            Console.WriteLine("reached 9 at " + x + ", " + y);
            return 1;
        }
        if (x < map.GetLength(1) - 1)   // if x+1 is not out of bounds 
        {
            if (map[y, x + 1] == num + 1 && !visited.Contains((x+1,y)))// if next number is to the right
            {
                score += getScore(x + 1, y, num + 1, visited);
            }
        }
        if (y < map.GetLength(0) - 1)   // if y+1 is not out of bounds 
        {
            if (map[y+1, x] == num + 1 && !visited.Contains((x, y+1)))// if next number is down
            {
                score += getScore(x, y+1, num + 1, visited);
            }
        }
        if (x > 0)   // if x-1 is not out of bounds 
        {
            if (map[y, x-1] == num + 1 && !visited.Contains((x - 1, y)))// if next number is left
            {
                score += getScore(x-1, y, num + 1, visited);
            }
        }
        if (y > 0)   // if y-1 is not out of bounds 
        {
            if (map[y - 1, x] == num + 1 && !visited.Contains((x, y-1)))// if next number is up
            {
                score += getScore(x, y - 1, num + 1, visited);
            }
        }
        return score;
    }
    public List<int> getTrailheadScores()
    {
        List<int> trailheadScores = new List<int>();
        for (int i = 0; i<map.GetLength(0); i++) {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                if (map[i, j] == 0) //for each trailhead
                {
                    Console.WriteLine("Trailhead: " + j + ", " + i);
                    Console.WriteLine();
                    List<(int x, int y)> visited = new List<(int x, int y)>(); 
                    trailheadScores.Add(getScore(j, i, 0, visited));
                }
            }
        }
        return trailheadScores;
    }
}
class Program
{
    static void Main(string[] args)
    {
        string cwd = "../../../../";
        string[] contents = File.ReadAllLines(cwd + "input.txt");
        Map myMap = new Map(contents);
        myMap.printMap();
        List<int> trailheads = myMap.getTrailheadScores();
        foreach(int trailhead in trailheads)
        {
            Console.WriteLine(trailhead);
        }
        Console.WriteLine(trailheads.Sum(x => x));
    }
}