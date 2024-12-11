using MyUtilities;
class Maze { 
    int startX;
    int startY;
    int xLowerBound;
    int yLowerBound;
    int xUpperBound;
    int yUpperBound;
    string cwd = "../../../../";
    string[] contents;
    char[,] board;
    public Maze()
    {
        contents = File.ReadAllLines(cwd + "input.txt");
        board = new char[contents.Length, contents[0].Length];
        foreach (var (index, value) in contents.Enumerate())
        {
            for (int i = 0; i < value.Length; i++)
            {
                char symbol = value.ToCharArray()[i];
                board[index, i] = symbol;
                if (symbol == '^')
                {
                    this.startX = i;
                    this.startY = index;
                }
            }
        }
        xLowerBound = 0; yLowerBound = 0;
        xUpperBound = board.GetLength(1)-1; yUpperBound = board.GetLength(0) - 1;
    }
    void printBoard()
    {
        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                Console.Write(board[i, j]);
            }
            Console.WriteLine();
        }
    }

    int patrolRec(int x, int y, char direction)
    {
        if (x > xUpperBound || x < xLowerBound || y < yLowerBound || y > yUpperBound)
        {
            return 0;
        }
        if (direction == 'u')
        {
            int distinct = 1;
            if (board[y, x] == 'X')
            {
                distinct = 0;
            }
            if (y > yLowerBound)
            {
                if (board[y - 1, x] == '#')
                {
                    direction = 'r';
                    board[y, x] = 'X';
                    return patrolRec(x + 1, y, direction) + distinct;
                }
            }

            board[y, x] = 'X';
            return patrolRec(x, y - 1, direction) + distinct;
        }
        else if (direction == 'r')
        {
            int distinct = 1;
            if (board[y, x] == 'X')
            {
                distinct = 0;
            }
            if (x < xUpperBound)
            {
                if (board[y, x + 1] == '#')
                {
                    direction = 'd';
                    board[y, x] = 'X';
                    return patrolRec(x, y + 1, direction) + distinct;
                }
            }
            board[y, x] = 'X';
            return patrolRec(x + 1, y, direction) + distinct;
        }
        else if (direction == 'd')
        {
            int distinct = 1;
            if (board[y, x] == 'X')
            {
                distinct = 0;
            }
            if (y < yUpperBound)
            {
                if (board[y + 1, x] == '#')
                {
                    direction = 'l';
                    board[y, x] = 'X';             
                    return patrolRec(x - 1, y, direction) + distinct;
                }
            }
            board[y, x] = 'X';
            return patrolRec(x, y + 1, direction) + distinct;
        }
        else
        {
            int distinct = 1;
            if (board[y, x] == 'X')
            {
                distinct = 0;
            }
            if (x > xLowerBound)
            {
                if (board[y, x - 1] == '#')
                {
                    direction = 'u';
                    board[y, x] = 'X';
                    return patrolRec(x, y - 1, direction) + distinct;
                }
            }
            board[y, x] = 'X';
            return patrolRec(x - 1, y, direction) + distinct;   
        }
    }

    int patrol()
    {
        int result = patrolRec(startX, startY, 'u');
        return result;
    } 
    static void Main(string[] args)
    {
        Maze maze = new Maze();
        maze.printBoard();
        Console.WriteLine(maze.patrol());
        maze.printBoard();
    }
}
