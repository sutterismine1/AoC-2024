string cwd = "../../../../";
var reader = new StreamReader(cwd + "input.txt");
string text;
char[,] map = new char[50, 100];

static (int x, int y) printMap(char[,] map)
{
    (int, int) character = (-1, -1);
    for (int i = 0; i < map.GetLength(0); i++)
    {
        for (int j = 0; j < map.GetLength(1); j++)
        {
            Console.Write(map[i, j]);
            if (map[i, j] == '@')
            {
                character = (j, i);
            }
        }
        Console.WriteLine();
    }
    return character;
}

static HashSet<(int, int)> checkPushUp(char[,] map, int y, int x, (int x, int y) character)   //go row by row, see how many units are 'connected' and see if an obstacle is directly above a unit in the next row
{
    HashSet<(int x, int y)> connectedUnits = new HashSet<(int, int)>();
    List<(int x, int y)> previousRow = new List<(int, int)>();
    if (map[y, x] == '[')
    {
        previousRow.Add((x,y));
        previousRow.Add((x + 1,y));
    }
    else if (map[y,x] == ']')
    {
        previousRow.Add((x,y));
        previousRow.Add((x - 1,y));
    }
    else
    {
        return connectedUnits;
    }
    connectedUnits.UnionWith(previousRow);
    foreach (var block in previousRow)
    {
        if (map[block.y - 1, block.x] == '#')
        {
            return null;
        }
    }
    foreach(var block in previousRow) { 
        var findAboveRow = checkPushUp(map, y-1, block.x, character);
        if(findAboveRow == null)
        {
            return null;
        }
        else
        {
            connectedUnits.UnionWith(findAboveRow);
        }
    }
    return connectedUnits;
}

static HashSet<(int, int)> checkPushDown(char[,] map, int y, int x, (int x, int y) character)   //go row by row, see how many units are 'connected' and see if an obstacle is directly above a unit in the next row
{
    HashSet<(int x, int y)> connectedUnits = new HashSet<(int, int)>();
    List<(int x, int y)> previousRow = new List<(int, int)>();
    if (map[y, x] == '[')
    {
        previousRow.Add((x, y));
        previousRow.Add((x + 1, y));
    }
    else if (map[y, x] == ']')
    {
        previousRow.Add((x, y));
        previousRow.Add((x - 1, y));
    }
    else
    {
        return connectedUnits;
    }
    connectedUnits.UnionWith(previousRow);
    foreach (var block in previousRow)
    {
        if (map[block.y + 1, block.x] == '#')
        {
            return null;
        }
    }
    foreach (var block in previousRow)
    {
        var findBelowRow = checkPushDown(map, y + 1, block.x, character);
        if (findBelowRow == null)
        {
            return null;
        }
        else
        {
            connectedUnits.UnionWith(findBelowRow);
        }
    }
    return connectedUnits;
}

static (int, int) pushBoxesUp(char[,] map, int y, HashSet<(int x, int y)> connectedUnits, (int x, int y) character)
{
    char[,] newmap = new char[map.GetLength(0), map.GetLength(1)];
    Array.Copy(map, newmap, map.Length);
    foreach (var block in connectedUnits)
    {
        newmap[block.y - 1, block.x] = map[block.y, block.x];
        if (!connectedUnits.Contains((block.x, block.y + 1)))
        {
            newmap[block.y, block.x] = '.'; //accounts for scenarios where the box leaves a 'trail' or empty spot after being pushed
        }
    }
    if (map[character.y - 1, character.x] == '[')
    {
        newmap[character.y - 1, character.x] = '@';
        newmap[character.y - 1, character.x + 1] = '.';
    }
    else if (map[character.y - 1, character.x] == ']')
    {
        newmap[character.y - 1, character.x] = '@';
        newmap[character.y - 1, character.x - 1] = '.';
    }
    newmap[character.y, character.x] = '.';
    Array.Copy(newmap, map, map.Length);
    character = (character.x, character.y - 1);
    return character;
}

static (int, int) pushBoxesDown(char[,] map, int y, HashSet<(int x, int y)> connectedUnits, (int x, int y) character)
{
    char[,] newmap = new char[map.GetLength(0), map.GetLength(1)];
    Array.Copy(map, newmap, map.Length);
    foreach (var block in connectedUnits)
    {
        newmap[block.y+1, block.x] = map[block.y, block.x];
        if (!connectedUnits.Contains((block.x,block.y-1))) {
            newmap[block.y, block.x] = '.'; //accounts for scenarios where the box leaves a 'trail' or empty spot after being pushed
        }
    }
    if (map[character.y + 1, character.x] == '[')
    {
        newmap[character.y + 1, character.x] = '@';
        newmap[character.y + 1, character.x + 1] = '.';
    }
    else if (map[character.y + 1, character.x] == ']')
    {
        newmap[character.y + 1, character.x] = '@';
        newmap[character.y + 1, character.x - 1] = '.';
    }
    newmap[character.y, character.x] = '.';
    Array.Copy(newmap, map, map.Length);
    character = (character.x, character.y + 1);
    return character;
}

static (int, int) move(char[,] map, char instruction, (int x, int y) character)
{
    switch (instruction)
    {
        case '^':
            if (map[character.y - 1, character.x] == '.')   
            {
                map[character.y - 1, character.x] = '@';
                map[character.y, character.x] = '.';
                character = (character.x, character.y - 1);
                break;
            }
            if (map[character.y - 1, character.x] == '[' || map[character.y - 1, character.x] == ']')
            {
                int y = character.y - 1;
                HashSet<(int,int)> connectedUnits = checkPushUp(map, y, character.x, character);
                if (connectedUnits == null) { break; }   //the way is blocked, don't move anything
                character = pushBoxesUp(map, y, connectedUnits, character);  //this code is only reached if the boxes cannot be pushed
                break;
            }
            break;
        case '>':
            if (map[character.y, character.x + 1] == '.')
            {
                map[character.y, character.x + 1] = '@';
                map[character.y, character.x] = '.';
                character = (character.x + 1, character.y);
                break;
            }
            if (map[character.y, character.x + 1] == '[')
            {
                int start = -1;
                int x = character.x + 1;
                while (map[character.y, x] != '#')
                {
                    if (map[character.y, x] == '.') { start = x; break; }
                    x++;
                }
                if (start < 0) { break; }   //the way is blocked, don't move anything
                for (x = start; x > character.x; x--) //this code is only reached if start was found
                {
                    map[character.y, x] = map[character.y,x-1];
                }
                map[character.y, character.x] = '.';
                character = (character.x + 1, character.y);
                break;
            }
            break;
        case 'v':
            if (map[character.y + 1, character.x] == '.')
            {
                map[character.y + 1, character.x] = '@';
                map[character.y, character.x] = '.';
                character = (character.x, character.y + 1);
                break;
            }
            if (map[character.y + 1, character.x] == '[' || map[character.y + 1, character.x] == ']')
            {
                int y = character.y + 1;
                HashSet<(int, int)> connectedUnits = checkPushDown(map, y, character.x, character);
                if (connectedUnits == null) { break; }   //the way is blocked, don't move anything
                character = pushBoxesDown(map, y, connectedUnits, character);  //this code is only reached if the boxes cannot be pushed
                break;
            }
            break;
        case '<':
            if (map[character.y, character.x - 1] == '.')
            {
                map[character.y, character.x - 1] = '@';
                map[character.y, character.x] = '.';
                character = (character.x - 1, character.y);
                break;
            }
            if (map[character.y, character.x - 1] == ']')
            {
                int start = -1;
                int x = character.x - 1;
                while (map[character.y, x] != '#')
                {
                    if (map[character.y, x] == '.') { start = x; break; }
                    x--;
                }
                if (start < 0) { break; }   //the way is blocked, don't move anything
                for(x = start; x<character.x;x++)
                map[character.y, x] = map[character.y, x+1];  //this code is only reached if start was found
                map[character.y, character.x - 1] = '@';
                map[character.y, character.x] = '.';
                character = (character.x - 1, character.y);
                break;
            }
            break;

    }
    return character;
}

static long calculateCoords(char[,] map)
{
    long sum = 0;
    for (int i = 0; i < map.GetLength(0); i++)
    {
        for (int j = 0; j < map.GetLength(1); j++)
        {
            if (map[i, j] == '[')
            {
                sum += (100 * i) + j;
            }
        }
    }
    return sum;
}

int y = 0;
while ((text = reader.ReadLine()) != "")
{
    int x = 0;
    foreach (char c in text)
    {
        switch (c)
        {
            case '#':
                map[y, x] = '#';
                map[y, x + 1] = '#';
                break;
            case 'O':
                map[y, x] = '[';
                map[y, x + 1] = ']';
                break;
            case '.':
                map[y, x] = '.';
                map[y, x + 1] = '.';
                break;
            case '@':
                map[y, x] = '@';
                map[y, x + 1] = '.';
                break;
        }
        x+=2;
    }
    y++;
}
int instructionAscii;
var character = printMap(map);
while ((instructionAscii = reader.Read()) != -1)
{
    char instruction = (char)instructionAscii;
    Console.WriteLine(instruction);
    character = move(map, instruction, character);
}
printMap(map);
Console.WriteLine(calculateCoords(map));
reader.Close();