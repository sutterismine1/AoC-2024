string cwd = "../../../../";
var reader = new StreamReader(cwd + "input.txt");
string text;
char[,] map = new char[50, 50];

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
            if (map[character.y - 1, character.x] == 'O')
            {
                int start = -1;
                int y = character.y - 1;
                while (map[y,character.x] != '#')
                {
                    if (map[y,character.x] == '.') { start = y; break; }
                    y--;
                }
                if (start < 0) { break; }   //the way is blocked, don't move anything
                map[start, character.x] = 'O';  //this code is only reached if start was found
                map[character.y - 1, character.x] = '@';
                map[character.y, character.x] = '.';
                character = (character.x, character.y-1);
                break;
            }
            break;
        case '>':
            if (map[character.y, character.x+1] == '.')
            {
                map[character.y, character.x+1] = '@';
                map[character.y, character.x] = '.';
                character = (character.x + 1, character.y);
                break;
            }
            if (map[character.y, character.x + 1] == 'O')
            {
                int start = -1;
                int x = character.x + 1;
                while (map[character.y, x] != '#')
                {
                    if (map[character.y, x] == '.') { start = x; break; }
                    x++;
                }
                if (start < 0) { break; }   //the way is blocked, don't move anything
                map[character.y, start] = 'O';  //this code is only reached if start was found
                map[character.y, character.x + 1] = '@';
                map[character.y, character.x] = '.';
                character = (character.x+1, character.y);
                break;
            }
            break;
        case 'v':
            if (map[character.y+1, character.x] == '.')
            {
                map[character.y+1, character.x] = '@';
                map[character.y, character.x] = '.';
                character = (character.x, character.y + 1);
                break;
            }
            if (map[character.y+1, character.x] == 'O')
            {
                int start = -1;
                int y = character.y + 1;
                while (map[y, character.x] != '#')
                {
                    if (map[y, character.x] == '.') { start = y; break; }
                    y++;
                }
                if (start < 0) { break; }   //the way is blocked, don't move anything
                map[start, character.x] = 'O';  //this code is only reached if start was found
                map[character.y+1, character.x] = '@';
                map[character.y, character.x] = '.';
                character = (character.x, character.y+1);
                break;
            }
            break;
        case '<':
            if (map[character.y, character.x-1] == '.')
            {
                map[character.y, character.x-1] = '@';
                map[character.y, character.x] = '.';
                character = (character.x - 1, character.y);
                break;
            }
            if (map[character.y, character.x-1] == 'O')
            {
                int start = -1;
                int x = character.x-1;
                while (map[character.y, x] != '#')
                {
                    if (map[character.y, x] == '.') { start = x; break; }
                    x--;
                }
                if (start < 0) { break; }   //the way is blocked, don't move anything
                map[character.y, start] = 'O';  //this code is only reached if start was found
                map[character.y, character.x-1] = '@';
                map[character.y, character.x] = '.';
                character = (character.x-1, character.y);
                break;
            }
            break;

    }
    return character;
}

static long calculateCoords(char[,] map)
{
    long sum = 0;
    for (int i = 0; i<map.GetLength(0); i++)
    {
        for (int j = 0; j < map.GetLength(1); j++)
        {
            if (map[j,i] == 'O')
            {
                sum += (100 * j) + i;
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
        map[y, x] = c;
        x++;
    }
    y++;
}
Console.WriteLine(map);
int instructionAscii;
var character = printMap(map);
Console.WriteLine(character.x + "." + character.y);
while ((instructionAscii = reader.Read()) != -1)
{
    char instruction = (char)instructionAscii;
    Console.WriteLine(instruction);
    character = move(map, instruction, character);
}
printMap(map);
Console.WriteLine(calculateCoords(map));
reader.Close();