string cwd = "../../../../"; 
string[] contents = File.ReadAllLines(cwd + "input.txt");
Dictionary<char, HashSet<(int, int)>> antennas = new Dictionary<char, HashSet<(int, int)>>();
for (int i = 0; i < contents.Length; i++)
{
    for (int j = 0; j < contents[i].Length; j++)
    {
        if (contents[i][j] != '.')
        {
            HashSet<(int, int)> value = new HashSet<(int, int)>();
            if (antennas.TryGetValue(contents[i][j], out value))
            {
                value.Add((j, i));
                antennas[contents[i][j]] = value;
            }
            else
            {
                HashSet<(int,int)> newSet = new HashSet<(int, int)>();
                newSet.Add((j, i));
                antennas.Add(contents[i][j], newSet);
            }
        }
    }
}
HashSet<(int,int)> resultSet = new HashSet<(int, int)>();
foreach(var item in antennas.Values)
{
    var result = from antenna1 in item
                 from antenna2 in item
                 where antenna1 != antenna2
                 select new
                 {
                     antenna1 = antenna1,
                     antenna2 = antenna2
                 };
    foreach (var antennaPair in result)
    {
        int newX = antennaPair.antenna1.Item1 - antennaPair.antenna2.Item1;
        int newY = antennaPair.antenna1.Item2 - antennaPair.antenna2.Item2;
        newX = antennaPair.antenna2.Item1 - newX;
        newY = antennaPair.antenna2.Item2 - newY;
        if (newX >= 0 && newY >= 0 && newX < contents[0].Length && newY < contents.Length)
        {
            resultSet.Add((newX, newY));
        }
    }
}
Console.WriteLine(resultSet.Count);