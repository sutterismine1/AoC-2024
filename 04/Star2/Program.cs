string cwd = "../../../../";
string[] contents = File.ReadAllLines(cwd + "input.txt");

char[,] board = new char[contents.Length, contents[0].Length];
for (int i = 0; i < contents.Length; i++)
{
    for (int j = 0; j < contents[i].Length; j++)
    {
        board[i, j] = contents[i].ToCharArray()[j];
    }
}

static int findMAS(char[,] arr, int y, int x)
{
    if (y >= 1 && x >= 1 && x <= arr.GetLength(1) - 2 && y <= arr.GetLength(0) - 2)
    {
        //check M above
        if (arr[y - 1, x - 1] == 'M' && arr[y - 1, x + 1] == 'M' && arr[y + 1, x - 1] == 'S' && arr[y + 1, x + 1] == 'S')
        {
            return 1;
        }
        //check M right
        if (arr[y - 1, x + 1] == 'M' && arr[y + 1, x + 1] == 'M' && arr[y - 1, x - 1] == 'S' && arr[y + 1, x - 1] == 'S')
        {
            return 1;
        }
        //check M below
        if (arr[y + 1, x - 1] == 'M' && arr[y + 1, x + 1] == 'M' && arr[y - 1, x - 1] == 'S' && arr[y - 1, x + 1] == 'S')
        {
            return 1;
        }
        //check M left
        if (arr[y - 1, x - 1] == 'M' && arr[y + 1, x - 1] == 'M' && arr[y - 1, x + 1] == 'S' && arr[y + 1, x + 1] == 'S')
        {
            return 1;
        }
    }
    return 0;
}

int total = 0;
for (int i = 0; i < board.GetLength(0); i++)
{
    for (int j = 0; j < board.GetLength(1); j++)
    {
        if (board[i, j] == 'A')
        {
            total += findMAS(board, i, j);
        }
    }
}
Console.WriteLine(total);