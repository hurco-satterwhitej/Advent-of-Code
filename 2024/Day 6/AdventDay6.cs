enum Direction
{
    Up,
    Right,
    Down,
    Left
}

enum GridStatus
{
    Unvisited = '.',
    Visited = 'X',
    Obstacle = '#'
}

class Grid
{
    GridStatus status;
    Dictionary<Direction, bool> visitedFrom;

    public Grid(char grid)
    {
        status = (GridStatus)grid;
        visitedFrom = new Dictionary<Direction, bool>();
        foreach (Direction dir in Enum.GetValues(typeof(Direction)))
        {
            visitedFrom.Add(dir, false);
        }
    }

    public Grid(char grid, Direction dir) : this(grid)
    {
        visitedFrom[dir] = true;
    }

    public bool IsWalkable()
    {
        return status != GridStatus.Obstacle;
    }

    public bool Visit(Direction dir)
    {
        if (status == GridStatus.Obstacle)
        {
            throw new Exception("Cannot visit an obstacle grid.");
        }
        status = GridStatus.Visited;
        if (visitedFrom[dir])
        {
            //We have already visited this grid from this direciton.
            //Loop detected!
            return false;
        }
        else
        {
            visitedFrom[dir] = true;
            return true;
        }
    }

    public bool IsVisited()
    {
        return status == GridStatus.Visited;
    }
}

class Map
{
    Grid[,] map;
    Direction dir;
    int startingRow;
    int startingCol;
    bool loopingMap;

    public Map(List<string> input)
    {
        dir = Direction.Up;
        loopingMap = false;
        startingRow = -1;
        startingCol = -1;
        map = new Grid[input.Count, input[0].Length];
        for (int i = 0; i < input.Count; i++)
        {
            string line = input[i];
            if (line.Contains('^'))
            {
                startingRow = i;
                startingCol = line.IndexOf('^');
                line = line.Replace('^', 'X');
            }
            for (int j = 0; j < line.Length; j++)
            {
                map[i, j] = new Grid(line[j]);
            }
        }
    }

    public void WalkMap()
    {
        int currentRow = startingRow;
        int currentCol = startingCol;
        bool onPatrol = true;

        while (onPatrol && !loopingMap)
        {
            onPatrol = PatrolDirection(ref currentRow, ref currentCol);
        }
    }

    private bool PatrolDirection(ref int currentRow, ref int currentCol)
    {
        switch (dir)
        {
            case Direction.Up:
                while (currentRow - 1 >= 0)
                {
                    if (map[currentRow - 1, currentCol].IsWalkable())
                    {
                        currentRow--;
                        bool newMove = map[currentRow, currentCol].Visit(dir);
                        if (!newMove)
                        {
                            //Looping
                            loopingMap = true;
                            return false;
                        }
                    }
                    else
                    {
                        //Reached an obstacle; change direction and continue patrol
                        dir = Direction.Right;
                        return true;
                    }
                }
                break;
            case Direction.Right:
                while (currentCol + 1 < map.GetLength(1))
                {
                    if (map[currentRow, currentCol + 1].IsWalkable())
                    {
                        currentCol++;
                        bool newMove = map[currentRow, currentCol].Visit(dir);
                        if (!newMove)
                        {
                            //Looping
                            loopingMap = true;
                            return false;
                        }
                    }
                    else
                    {
                        //Reached an obstacle; change direction and continue patrol
                        dir = Direction.Down;
                        return true;
                    }
                }
                break;
            case Direction.Down:
                while (currentRow + 1 < map.GetLength(0))
                {
                    if (map[currentRow + 1, currentCol].IsWalkable())
                    {
                        currentRow++;
                        bool newMove = map[currentRow, currentCol].Visit(dir);
                        if (!newMove)
                        {
                            //Looping
                            loopingMap = true;
                            return false;
                        }
                    }
                    else
                    {
                        //Reached an obstacle; change direction and continue patrol
                        dir = Direction.Left;
                        return true;
                    }
                }
                break;
            case Direction.Left:
                while (currentCol - 1 >= 0)
                {
                    if (map[currentRow, currentCol - 1].IsWalkable())
                    {
                        currentCol--;
                        bool newMove = map[currentRow, currentCol].Visit(dir);
                        if (!newMove)
                        {
                            //Looping
                            loopingMap = true;
                            return false;
                        }
                    }
                    else
                    {
                        //Reached an obstacle; change direction and continue patrol
                        dir = Direction.Up;
                        return true;
                    }
                }
                break;
        }
        //We have reached the edge of the map and are about to exit the patrol zone.
        return false;
    }

    public bool AddObstacle(int row, int col)
    {
        if (map[row, col].IsWalkable() && (row != startingRow || col != startingCol))
        {
            map[row, col] = new Grid('#');
            return true;
        }
        else
        {
            return false;
        }
    }

    public int CountWalked(ref List<Tuple<int, int>> walkedGrids)
    {
        int walked = 0;
        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                if (map[i, j].IsVisited())
                {
                    walked++;
                    walkedGrids.Add(new Tuple<int, int>(i, j));
                }
            }
        }
        return walked;
    }

    public bool MapContainsLoop()
    {
        return loopingMap;
    }
}

private void AdventDay6(object sender, MouseButtonEventArgs e)
{
    var rawFile = File.ReadAllLines(@"Input.txt");
    //Input is a series of n lines with various ., #, and one ^ character, defining a map.
    List<string> mapLines = new List<string>();
    foreach (string line in rawFile)
    {
        mapLines.Add(line);
    }
    //Part 1: Walk the map and count how many UNIQUE grids were walked.
    Map initialMap = new Map(mapLines);
    initialMap.WalkMap();
    //Make note of which grids were walked to save about 6 and a half minutes of brute force later.
    List<Tuple<int, int>> walkedPoints = new List<Tuple<int, int>>();
    MessageBox.Show("Walked " + initialMap.CountWalked(ref walkedPoints).ToString() + " grids.");
    //Part 2: Determine how many variations of the map are made into loops by adding an additional obstacle.
    //Checking only the grids that are known to be encountered saves a good bit of time, but it's still extremely not optimal.
    long startTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
    int loopCount = 0;
    //Boy howdy, I love brute forcing in the morning.
    foreach (Tuple<int, int> point in walkedPoints)
    {
        Map addedObstacleMap = new Map(mapLines);
        bool allowed = addedObstacleMap.AddObstacle(point.Item1, point.Item2);
        if (!allowed)
        {
            continue;
        }
        addedObstacleMap.WalkMap();
        if (addedObstacleMap.MapContainsLoop())
        {
            loopCount++;
        }
    }
    long endTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
    long duration = endTime - startTime;
    MessageBox.Show("Walked the full map. Time to complete: " + duration.ToString() + " ms.\nLoop count: " + loopCount.ToString());
}