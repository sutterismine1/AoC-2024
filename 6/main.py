import os

class Maze:
    def __init__(self):
        self.cwd = ""
        self.contents = []
        with open(os.path.join(self.cwd, "input.txt"), 'r') as f:
            self.contents = f.readlines()
        self.board = [list(line.strip()) for line in self.contents]
        self.startX = self.startY = -1
        self.xLowerBound = self.yLowerBound = 0
        self.xUpperBound = len(self.board[0]) - 1
        self.yUpperBound = len(self.board) - 1
        # Initialize the start position (^) on the maze
        for i, line in enumerate(self.contents):
            for j, symbol in enumerate(line.strip()):
                if symbol == '^':
                    self.startX = j
                    self.startY = i
                self.board[i][j] = symbol

    def print_board(self):
        for row in self.board:
            print("".join(row))

    def patrol_rec(self, x, y, direction, visited, local_board):
        while True:
            state = (x, y, direction)
            if state in visited:
                return False
            visited.append(state)
            if x > self.xUpperBound or x < self.xLowerBound or y < self.yLowerBound or y > self.yUpperBound:
                return True
            if direction == 'u':
                if y > self.yLowerBound:
                    if local_board[y - 1][x] == '#':
                        direction = 'r'
                        local_board[y][x] = 'X'
                        x = x + 1
                        continue
                local_board[y][x] = 'X'
                y = y - 1
                continue
            elif direction == 'r':
                if x < self.xUpperBound:
                    if local_board[y][x + 1] == '#':
                        direction = 'd'
                        local_board[y][x] = 'X'
                        y = y + 1
                        continue
                local_board[y][x] = 'X'
                x = x + 1
                continue
            elif direction == 'd':
                if y < self.yUpperBound:
                    if local_board[y + 1][x] == '#':
                        direction = 'l'
                        local_board[y][x] = 'X'
                        x = x - 1
                        continue
                local_board[y][x] = 'X'
                y = y + 1
                continue
            else:
                if x > self.xLowerBound:
                    if local_board[y][x - 1] == '#':
                        direction = 'u'
                        local_board[y][x] = 'X'
                        y = y - 1
                        continue
                local_board[y][x] = 'X'
                x = x - 1
                continue

    def patrol(self, positions):
        sum_result = 0
        temp_b = [row[:] for row in self.board]
        for position in positions:
            i, j = position
            if i == self.startY and j == self.startX:
                continue
            if self.board[i][j] == '#':
                continue
            self.board[i][j] = '#'
            visited = []
            result = self.patrol_rec(self.startX, self.startY, 'u', visited, self.board)
            if not result:
                sum_result += 1
            self.board = [row[:] for row in temp_b]
        return sum_result

    def patrol_positions(self):
        visited = []
        temp_b = [row[:] for row in self.board]
        self.patrol_rec(self.startX, self.startY, 'u', visited, self.board)
        positions = []
        for i in range(len(self.board)):
            for j in range(len(self.board[i])):
                if self.board[i][j] == 'X':
                    positions.append((i, j))
        self.board = [row[:] for row in temp_b]
        return positions


if __name__ == "__main__":
    maze = Maze()
    maze.print_board()
    positions = maze.patrol_positions()
    print(len(positions))
    print(maze.patrol(positions))
