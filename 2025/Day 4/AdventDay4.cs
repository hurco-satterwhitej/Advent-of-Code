//This check leaves something to be desired. But it's functional and didn't take an eternity to run, so whatever.
int CheckAdjacentRolls(ref char[,] grid, int i, int j, int maxCol, int maxRow)
{
    int adjacentRolls = 0;
    if (j > 0)
    {
        if (grid[i, j - 1] == '@')
        {
            adjacentRolls++;
        }
    }
    if (j < maxRow - 1)
    {
        if (grid[i, j + 1] == '@')
        {
            adjacentRolls++;
        }
    }
    if (i > 0)
    {
        if (grid[i - 1, j] == '@')
        {
            adjacentRolls++;
        }
    }
    if (i < maxCol - 1)
    {
        if (grid[i + 1, j] == '@')
        {
            adjacentRolls++;
        }
    }
    if (i > 0 && j > 0)
    {
        if (grid[i - 1, j - 1] == '@')
        {
            adjacentRolls++;
        }
    }
    if (i > 0 && j < maxRow - 1)
    {
        if (grid[i - 1, j + 1] == '@')
        {
            adjacentRolls++;
        }
    }
    if (i < maxCol - 1 && j > 0)
    {
        if (grid[i + 1, j - 1] == '@')
        {
            adjacentRolls++;
        }
    }
    if (i < maxCol - 1 && j < maxRow - 1)
    {
        if (grid[i + 1, j + 1] == '@')
        {
            adjacentRolls++;
        }
    }
    return adjacentRolls;
}

void AdventDay4()
{
    var rawFile = File.ReadAllLines(@"Input.txt");
    //Input is a series of lines containing "." and "@". @ represents a roll of paper,
    //which can be accessed by forklift if fewer than 4 rolls are in the adjacent eight positions.
    List<string> rawGrid = new List<string>();
    foreach (string line in rawFile)
    {
        rawGrid.Add(line);
    }
    //Convert to double char array so we can update the grid as we go.
    char[,] grid = new char[rawGrid.Count, rawGrid[0].Length];
    for (int i = 0; i < rawGrid.Count; i++)
    {
        for (int j = 0; j < rawGrid[i].Length; j++)
        {
            grid[i, j] = rawGrid[i][j];
        }
    }
    int maxCol = rawGrid.Count;
    int maxRow = rawGrid[0].Length;
    //DO WHILE LOOPS FTW
    bool firstPass = true;
    long movableCount = -1;
    long totalMoved = 0;
    do
    {
        movableCount = 0;
        List<Tuple<int, int>> movablePositions = new List<Tuple<int, int>>();
        for (int i = 0; i < maxCol; i++)
        {
            for (int j = 0; j < maxRow; j++)
            {
                if (grid[i, j] != '@')
                {
                    continue;
                }
                int adjacentRolls = CheckAdjacentRolls(ref grid, i, j, maxCol, maxRow);
                if (adjacentRolls < 4)
                {
                    movableCount++;
                    movablePositions.Add(new Tuple<int, int>(i, j));
                }
            }
        }
        foreach (Tuple<int, int> pos in movablePositions)
        {
            grid[pos.Item1, pos.Item2] = '.';
        }
        if (firstPass)
        {
            //Part 1 only cares about how many paper rolls were moved on the first pass.
            firstPass = false;
            MessageBox.Show("Part 1: " + movableCount.ToString());
        }
        totalMoved += movableCount;
    } while (movableCount > 0);
    //Part 2 is how many rolls can be moved in total, taking into account rolls that can be moved after others are moved.
    MessageBox.Show("Part 2: " + totalMoved.ToString());
}