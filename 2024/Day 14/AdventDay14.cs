enum Quadrant
{
    Quadrant1,
    Quadrant2,
    Quadrant3,
    Quadrant4,
    Center
}

class RobotGuard
{
    public int x;
    public int y;
    int velocityX;
    int velocityY;

    public RobotGuard(string data)
    {
        string[] split = data.Split(' ');
        string[] posSplit = split[0].Split('=');
        string[] velSplit = split[1].Split('=');
        string[] posData = posSplit[1].Split(',');
        string[] velData = velSplit[1].Split(',');
        x = int.Parse(posData[0]);
        y = int.Parse(posData[1]);
        velocityX = int.Parse(velData[0]);
        velocityY = int.Parse(velData[1]);
    }

    public void UpdatePositionForIterations(int iterations, int rows, int cols)
    {
        for (int i = 0; i < iterations; i++)
        {
            x += velocityX;
            y += velocityY;
            if (x < 0)
            {
                x = cols + x;
            }
            else if (x >= cols)
            {
                x = x - cols;
            }
            if (y < 0)
            {
                y = rows + y;
            }
            else if (y >= rows)
            {
                y = y - rows;
            }
        }
    }

    public Quadrant GetQuadrant(int rows, int cols)
    {
        int middleRow = (int)Math.Floor(rows / 2.0);
        int middleCol = (int)Math.Floor(cols / 2.0);
        if (x < middleCol && y < middleRow)
        {
            return Quadrant.Quadrant2;
        }
        else if (x > middleCol && y < middleRow)
        {
            return Quadrant.Quadrant1;
        }
        else if (x < middleCol && y > middleRow)
        {
            return Quadrant.Quadrant3;
        }
        else if (x > middleCol && y > middleRow)
        {
            return Quadrant.Quadrant4;
        }
        else
        {
            return Quadrant.Center;
        }
    }
}

void AdventDay14()
{
    var rawFile = File.ReadAllLines(@"Input.txt");
    //Input is a series of n lines of data giving position and velocity data of a robot.
    List<RobotGuard> robotGuards = new List<RobotGuard>();
    foreach (string line in rawFile)
    {
        robotGuards.Add(new RobotGuard(line));
    }
    //Space is weirdly defined as a hard coded 103x101 grid. Shocked this isn't part of the input.
    int rows = 103;
    int cols = 101;
    int quad1Count = 0;
    int quad2Count = 0;
    int quad3Count = 0;
    int quad4Count = 0;
    for (int i = 0; i < 10000; i++)
    {
        foreach (RobotGuard guard in robotGuards)
        {
            guard.UpdatePositionForIterations(1, rows, cols);
        }
        PrintRobotMap(ref robotGuards, rows, cols, i);
        if (i == 99)
        {
            //Store answer for part 1
            foreach (RobotGuard guard in robotGuards)
            {
                switch (guard.GetQuadrant(rows, cols))
                {
                    case Quadrant.Quadrant1:
                        quad1Count++;
                        break;
                    case Quadrant.Quadrant2:
                        quad2Count++;
                        break;
                    case Quadrant.Quadrant3:
                        quad3Count++;
                        break;
                    case Quadrant.Quadrant4:
                        quad4Count++;
                        break;
                }
            }
        }
    }
    MessageBox.Show((quad1Count * quad2Count * quad3Count * quad4Count).ToString());
}

void PrintRobotMap(ref List<RobotGuard> robotGuards, int rows, int cols, int iter)
{
    //Windows Exporer used for actually detecting the tree. More fun this way.
    Bitmap bmp = new Bitmap(cols, rows);
    bool[,] grid = new bool[cols, rows];
    foreach (RobotGuard guard in robotGuards)
    {
        grid[guard.x, guard.y] = true;
    }
    for (int i = 0; i < cols; i++)
    {
        for (int j = 0; j < rows; j++)
        {
            if (grid[i, j])
            {
                bmp.SetPixel(i, j, System.Drawing.Color.Black);
            }
            else
            {
                bmp.SetPixel(i, j, System.Drawing.Color.White);
            }
        }
    }
    bmp.Save("RobotMap" + (iter + 1).ToString() + ".bmp");
}