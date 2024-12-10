struct TrailSpot
{
    public char Num;
    public int Row;
    public int Col;

    public TrailSpot(char num, int row, int col)
    {
        this.Num = num;
        this.Row = row;
        this.Col = col;
    }
}

void AdventDay10()
{
    var rawFile = File.ReadAllLines(@"Input.txt");
    //Input is a series of n lines with various numbers 0-9.
    List<string> lines = new List<string>();
    foreach (string line in rawFile)
    {
        lines.Add(line);
    }
    int rowCount = lines.Count;
    int colCount = lines[0].Length;
    char[,] input = new char[rowCount, colCount];
    foreach (string line in lines)
    {
        for (int i = 0; i < line.Length; i++)
        {
            input[lines.IndexOf(line), i] = line[i];
        }
    }
    int trailheadScore = SearchForAllTrailheadScores(ref input, rowCount, colCount);
    MessageBox.Show("Trailhead score: " + trailheadScore.ToString());
}

int SearchForAllTrailheadScores(ref char[,] input, int rowCount, int colCount)
{
    int runningScoreCount = 0;
    for (int i = 0; i < rowCount; i++)
    {
        for (int j = 0; j < colCount; j++)
        {
            //Begin the search if current index is a 0. Remaining searching will be recursive.
            if (input[i, j] == '0')
            {
                List<TrailSpot> trailheadEnds = new List<TrailSpot>();
                SearchAllDirectionsFromCurrentForTrailheadEnd(ref input, i, j, rowCount, colCount, ref trailheadEnds);
                runningScoreCount += trailheadEnds.Count;
            }
        }
    }
    return runningScoreCount;
}

void SearchAllDirectionsFromCurrentForTrailheadEnd(ref char[,] input, int currentRow, int currentCol, int maxRow, int maxCol, ref List<TrailSpot> trailheadEnds)
{
    char current = input[currentRow, currentCol];
    //If current == 9, then we have found a trailhead end. This is the end of the search.
    if (current == '9')
    {
        TrailSpot end = new TrailSpot(current, currentRow, currentCol);
        //Add only unique instances of trailhead ends.
        //Remove the add from the if line if doing part 2.
        if (!trailheadEnds.Contains(end))
        {
            trailheadEnds.Add(end);
        }
        return;
    }
    //Search along the north, east, south, and west directions for current + 1.
    //North
    if (currentRow - 1 >= 0)
    {
        if (current + 1 == input[currentRow - 1, currentCol])
        {
            //North is a gradual step up from current. Continue searching from this point.
            SearchAllDirectionsFromCurrentForTrailheadEnd(ref input, currentRow - 1, currentCol, maxRow, maxCol, ref trailheadEnds);
        }
    }
    //East
    if (currentCol + 1 < maxCol)
    {
        if (current + 1 == input[currentRow, currentCol + 1])
        {
            //East is a gradual step up from current. Continue searching from this point.
            SearchAllDirectionsFromCurrentForTrailheadEnd(ref input, currentRow, currentCol + 1, maxRow, maxCol, ref trailheadEnds);
        }
    }
    //South
    if (currentRow + 1 < maxRow)
    {
        if (current + 1 == input[currentRow + 1, currentCol])
        {
            //South is a gradual step up from current. Continue searching from this point.
            SearchAllDirectionsFromCurrentForTrailheadEnd(ref input, currentRow + 1, currentCol, maxRow, maxCol, ref trailheadEnds);
        }
    }
    //West
    if (currentCol - 1 >= 0)
    {
        if (current + 1 == input[currentRow, currentCol - 1])
        {
            //West is a gradual step up from current. Continue searching from this point.
            SearchAllDirectionsFromCurrentForTrailheadEnd(ref input, currentRow, currentCol - 1, maxRow, maxCol, ref trailheadEnds);
        }
    }
}