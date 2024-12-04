enum WordSearchDir
{
    Up,
    UpRight,
    Right,
    DownRight,
    Down,
    DownLeft,
    Left,
    UpLeft
}

private void AdventDay4(object sender, MouseButtonEventArgs e)
{
    var rawFile = File.ReadAllLines(@"Input.txt");
    //Input is a series of n lines with various X, M, A, S
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
    //Behold, brute force incrementing through the entire double array.
    int matchesFound = 0;
    int secondMatchesFound = 0;
    for (int i = 0; i < rowCount; i++)
    {
        for (int j = 0; j < colCount; j++)
        {
            //Part 1: Search for XMAS starting with X
            if (input[i, j] == 'X')
            {
                //Search along all possible directions from starting point of X
                foreach (WordSearchDir dir in WordSearchDir.GetValues(typeof(WordSearchDir)))
                {
                    if (SearchForXMASFromIndexThroughDir(ref input, rowCount, colCount, i, j, dir))
                    {
                        matchesFound++;
                    }
                }
            }
            //Part 2: Search for X of MAS centered on A
            if (input[i, j] == 'A')
            {
                if (SearchForXofMASFromIndex(ref input, rowCount, colCount, i, j))
                {
                    secondMatchesFound++;
                }
            }
        }
    }
    MessageBox.Show("XMAS found: " + matchesFound.ToString() + "\nX of MAS found: " + secondMatchesFound.ToString());
}

private bool SearchForXMASFromIndexThroughDir(ref char[,] input, int rowCount, int colCount, int row, int col, WordSearchDir dir)
{
    //This is NOT efficient. Tried to make it a little more elegant with the modifier thing, but it's still rough. But it'll work.
    //Looking for specifically XMAS. row and col are a starting point at X. Search the specified direction for M, then A, then S.
    int rowModifier = 0;
    int colModifier = 0;
    switch (dir)
    {
        case WordSearchDir.Up:
            {
                //Safeguard against upper bound
                if (row - 3 < 0)
                {
                    return false;
                }
                rowModifier = -1;
                break;
            }
        case WordSearchDir.UpRight:
            {
                //Safeguard against upper and right bounds
                if ((row - 3 < 0) || (col + 3 >= colCount))
                {
                    return false;
                }
                rowModifier = -1;
                colModifier = 1;
                break;
            }
        case WordSearchDir.Right:
            {
                //Safeguard against right bound
                if (col + 3 >= colCount)
                {
                    return false;
                }
                colModifier = 1;
                break;
            }
        case WordSearchDir.DownRight:
            {
                //Safeguard against lower and right bounds
                if ((row + 3 >= rowCount) || (col + 3 >= colCount))
                {
                    return false;
                }
                rowModifier = 1;
                colModifier = 1;
                break;
            }
        case WordSearchDir.Down:
            {
                //Safeguard against lower bound
                if (row + 3 >= rowCount)
                {
                    return false;
                }
                rowModifier = 1;
                break;
            }
        case WordSearchDir.DownLeft:
            {
                //Safeguard against lower and left bounds
                if ((row + 3 >= rowCount) || (col - 3 < 0))
                {
                    return false;
                }
                rowModifier = 1;
                colModifier = -1;
                break;
            }
        case WordSearchDir.Left:
            {
                //Safeguard against left bound
                if (col - 3 < 0)
                {
                    return false;
                }
                colModifier = -1;
                break;
            }
        case WordSearchDir.UpLeft:
            {
                //Safeguard against upper and left bounds
                if ((row - 3 < 0) || (col - 3 < 0))
                {
                    return false;
                }
                rowModifier = -1;
                colModifier = -1;
                break;
            }
        default:
            {
                return false;
            }
    }
    //Check for M, A, S in the direction specified by the modifier.
    if (input[row + (1 * rowModifier), col + (1 * colModifier)] != 'M')
    {
        return false;
    }
    if (input[row + (2 * rowModifier), col + (2 * colModifier)] != 'A')
    {
        return false;
    }
    if (input[row + (3 * rowModifier), col + (3 * colModifier)] != 'S')
    {
        return false;
    }
    return true;
}

private bool SearchForXofMASFromIndex(ref char[,] input, int rowCount, int colCount, int row, int col)
{
    //Compared to how the XMAS search was implemented, I'm rather proud of how elegant this one is.
    //Looking for specifically an X of MAS, such that it appears as follows:
    //M S | S S |  
    // A  |  A  | etc
    //M S | M M | 
    //row and col are a starting point at A. Search both of the opposite diagonals for M and S.
    //First: Guard against the bounds of the array. If A is on any edge, it's not possible to find the pattern.
    if (row - 1 < 0 || row + 1 >= rowCount || col - 1 < 0 || col + 1 >= colCount)
    {
        return false;
    }
    //Next, check the two diagonals for M and S.
    //Upper left and lower right need an M and S.
    char upperLeft = input[row - 1, col - 1];
    char lowerRight = input[row + 1, col + 1];
    if ((upperLeft + lowerRight) != ('M' + 'S'))
    {
        return false;
    }
    //Upper right and lower left need an M and S.
    char upperRight = input[row - 1, col + 1];
    char lowerLeft = input[row + 1, col - 1];
    if ((upperRight + lowerLeft) != ('M' + 'S'))
    {
        return false;
    }
    return true;
}