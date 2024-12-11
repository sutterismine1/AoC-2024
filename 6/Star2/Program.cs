using MyUtilities;
using System.Collections;
using System.Collections.Generic;

class Maze
{
    int startX;
    int startY;
    int xLowerBound;
    int yLowerBound;
    int xUpperBound;
    int yUpperBound;
    int negativeValue;
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
        xUpperBound = board.GetLength(1) - 1; yUpperBound = board.GetLength(0) - 1;
        //negativeValue = - (board.GetLength(0) * board.GetLength(1));
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

    bool patrolRec(int x, int y, char direction, ArrayList visited, char[,] localBoard)
    {
        while (true)
        {
            if (x > xUpperBound || x < xLowerBound || y < yLowerBound || y > yUpperBound)
            {
                return true;
            }
            if (localBoard[y,x] == '#')
            {
                if (direction == 'u')
                {
                    y = y + 1;
                    direction = 'r';
                }
                else if (direction == 'r')
                {
                    x = x - 1;
                    direction = 'd';
                }
                else if (direction == 'd')
                {
                    y = y - 1;
                    direction = 'l';
                }
                else if (direction == 'l')
                {
                    x = x + 1;
                    direction = 'u';
                }
            }
            var state = (x, y, direction);
            if (visited.Contains(state))
            {
                return false;
            }
            visited.Add(state);
            if (direction == 'u')
            {
                int distinct = 1;
                if (localBoard[y, x] == 'X')
                {
                    distinct = 0;
                }/*
                if (y > yLowerBound)
                {
                    if (localBoard[y - 1, x] == '#')
                    {
                        direction = 'r';
                        localBoard[y, x] = 'X';
                        x = x + 1;
                        continue;
                    }
                }*/
                localBoard[y, x] = 'X';
                y = y - 1;
                continue;
            }
            else if (direction == 'r')
            {
                int distinct = 1;
                if (localBoard[y, x] == 'X')
                {
                    distinct = 0;
                }/*
                if (x < xUpperBound)
                {
                    if (board[y, x + 1] == '#')
                    {
                        direction = 'd';
                        localBoard[y, x] = 'X';
                        y = y + 1;
                        continue;
                    }
                }*/
                localBoard[y, x] = 'X';
                x = x + 1;
                continue;
            }
            else if (direction == 'd')
            {
                int distinct = 1;
                if (localBoard[y, x] == 'X')
                {
                    distinct = 0;
                }/*
                if (y < yUpperBound)
                {
                    if (localBoard[y + 1, x] == '#')
                    {
                        direction = 'l';
                        localBoard[y, x] = 'X';
                        x = x - 1; 
                        continue;
                    }
                }*/
                localBoard[y, x] = 'X';
                y = y + 1;
                continue;
            }
            else
            {
                int distinct = 1;
                if (localBoard[y, x] == 'X')
                {
                    distinct = 0;
                }/*
                if (x > xLowerBound)
                {
                    if (localBoard[y, x - 1] == '#')
                    {
                        direction = 'u';
                        localBoard[y, x] = 'X';
                        y = y- 1;
                        continue ;
                    }
                }*/
                localBoard[y, x] = 'X';
                x = x - 1;
                continue;
            }
        }
    }

    int patrol(List<Tuple<int, int>> positions)
    {
        int sum = 0;
        char[,] tempB = new char[board.GetLength(0),board.GetLength(1)];
        Array.Copy(board, tempB, board.GetLength(0)*board.GetLength(1));
        for (int k = 0; k < positions.Count;k++) {
            var position = positions[k];
            int i = position.Item1; int j = position.Item2;
            ArrayList visited = new ArrayList();
            if (i == startY && j == startX) { continue; }
            if (board[i, j] == '#') { continue; }
            board[i, j] = '#';
            bool result = patrolRec(startX, startY, 'u', visited, board);
            if (result == false)
            {
                sum++;

            }
            Array.Copy(tempB, board, board.GetLength(0) * board.GetLength(1));
        }
        return sum;
    }
    List<Tuple<int, int>> patrolPositions()
    {
        ArrayList visited = new ArrayList();
        char[,] tempB = new char[board.GetLength(0), board.GetLength(1)];
        Array.Copy(board, tempB, board.GetLength(0) * board.GetLength(1));
        patrolRec(startX, startY, 'u', visited, board);
        List<Tuple<int, int>> positions = new List<Tuple<int, int>>();
        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                if (board[i,j] == 'X')
                {
                    positions.Add(Tuple.Create(i,j));
                }
            }
        }
        Array.Copy(tempB, board, board.GetLength(0) * board.GetLength(1));
        return positions;
    }
    static void Main(string[] args)
    {
        Maze maze = new Maze();
        maze.printBoard();
        List<Tuple<int, int>> positions = maze.patrolPositions();
        Console.WriteLine(positions.Count);
        Console.WriteLine(maze.patrol(positions));
    }
}
