class GardenPlot
{
    char PlantType;
    int row;
    int col;
    //Welcome to Flags *R* Us! What's an algorithm?
    public bool NorthFence;
    public bool EastFence;
    public bool SouthFence;
    public bool WestFence;
    public bool NorthCounted;
    public bool EastCounted;
    public bool SouthCounted;
    public bool WestCounted;

    bool counted;

    public GardenPlot(char type, int row, int col)
    {
        PlantType = type;
        this.row = row;
        this.col = col;
        counted = false;
    }

    public char GetPlantType()
    {
        return PlantType;
    }

    public bool IsCounted()
    {
        return counted;
    }

    public void SetFences(ref GardenPlot[,] garden, int maxRow, int maxCol)
    {
        //Bound check - in this instance, being at a bound forces having a fence.
        NorthFence = row == 0;
        EastFence = col == maxCol;
        SouthFence = row == maxRow;
        WestFence = col == 0;
        //If any of the fence flags are true, we are at that particular bound and do not check it.
        //When not at a bound, a fence is set if the plant type of the adjacent plot is different.
        if (!NorthFence)
        {
            NorthFence = garden[row - 1, col].PlantType != PlantType;
        }
        if (!EastFence)
        {
            EastFence = garden[row, col + 1].PlantType != PlantType;
        }
        if (!SouthFence)
        {
            SouthFence = garden[row + 1, col].PlantType != PlantType;
        }
        if (!WestFence)
        {
            WestFence = garden[row, col - 1].PlantType != PlantType;
        }
        NorthCounted = false;
        EastCounted = false;
        SouthCounted = false;
        WestCounted = false;
    }

    public int GetFenceCount()
    {
        if (!counted)
        {
            int count = 0;
            if (NorthFence)
            {
                count++;
            }
            if (EastFence)
            {
                count++;
            }
            if (SouthFence)
            {
                count++;
            }
            if (WestFence)
            {
                count++;
            }
            counted = true;
            return count;
        }
        return 0;
    }

    public bool HasUntraversedSides()
    {
        return (NorthFence && !NorthCounted) || (EastFence && !EastCounted) || (SouthFence && !SouthCounted) || (WestFence && !WestCounted);
    }
}

void AdventDay12()
{
    var rawFile = File.ReadAllLines(@"Input.txt");
    //Input is a series of n lines with various letters, representing a garden plot.
    List<string> lines = new List<string>();
    foreach (string line in rawFile)
    {
        lines.Add(line);
    }
    //Begin by constructing the full map, complete with fences.
    int rowCount = lines.Count;
    int colCount = lines[0].Length;
    GardenPlot[,] input = new GardenPlot[rowCount, colCount];
    for (int i = 0; i < rowCount; i++)
    {
        for (int j = 0; j < colCount; j++)
        {
            input[i, j] = new GardenPlot(lines[i][j], i, j);
        }
    }
    foreach (GardenPlot plot in input)
    {
        plot.SetFences(ref input, rowCount - 1, colCount - 1);
    }
    //Now traverse the map and count the number of fences for each plot.
    long runningTotalCost = 0;
    long discountedTotalCost = 0;
    for (int i = 0; i < rowCount; i++)
    {
        for (int j = 0; j < colCount; j++)
        {
            if (input[i, j].IsCounted())
            {
                continue;
            }
            long areaInPlot = 0;
            long fencesInPlot = 0;
            long sidesInPlot = 0;
            TraverseFullPlot(ref input, i, j, ref areaInPlot, ref fencesInPlot, ref sidesInPlot);
            runningTotalCost += (areaInPlot * fencesInPlot);
            discountedTotalCost += (areaInPlot * sidesInPlot);
        }
    }
    MessageBox.Show("Total cost of all fences: " + runningTotalCost.ToString()
        + "\nDiscounted cost: " + discountedTotalCost.ToString());
}

void TraverseFullPlot(ref GardenPlot[,] plot, int row, int col, ref long area, ref long fences, ref long numSides)
{
    if (plot[row, col].IsCounted())
    {
        return;
    }
    area++;
    int fencesInPlot = plot[row, col].GetFenceCount();
    if (fencesInPlot == 4)
    {
        //Special case handling for a lone plot.
        //Not clear why this case breaks my perimeter walk, but it does. This is easier.
        numSides += 4;
    }
    fences += fencesInPlot;
    //Bounds are already checked by the presence of fences.
    if (!plot[row, col].NorthFence)
    {
        TraverseFullPlot(ref plot, row - 1, col, ref area, ref fences, ref numSides);
        if (!plot[row, col].NorthCounted)
        {
            TraversePlotForSides(ref plot, row, col, ref numSides);
        }
    }
    if (!plot[row, col].EastFence)
    {
        TraverseFullPlot(ref plot, row, col + 1, ref area, ref fences, ref numSides);
        if (!plot[row, col].EastCounted)
        {
            TraversePlotForSides(ref plot, row, col, ref numSides);
        }
    }
    if (!plot[row, col].SouthFence)
    {
        TraverseFullPlot(ref plot, row + 1, col, ref area, ref fences, ref numSides);
        if (!plot[row, col].SouthCounted)
        {
            TraversePlotForSides(ref plot, row, col, ref numSides);
        }
    }
    if (!plot[row, col].WestFence)
    {
        TraverseFullPlot(ref plot, row, col - 1, ref area, ref fences, ref numSides);
        if (!plot[row, col].WestCounted)
        {
            TraversePlotForSides(ref plot, row, col, ref numSides);
        }
    }
}

void TraversePlotForSides(ref GardenPlot[,] plot, int row, int col, ref long numSides)
{
    //Lot of repeated code in here. Not proud of this. But it DOES work, and is pretty quick. So there's that.
    //North
    if (plot[row, col].NorthFence && !plot[row, col].NorthCounted)
    {
        plot[row, col].NorthCounted = true;
        numSides++;
        //Find the edge for this side, where an edge is defined as:
        // * A plot with both a vertical and horizontal fence.
        // * A plot with a vertical/horizontal fence next to an inner fence.
        int westEdge = col;
        int eastEdge = col;
        int nextWestRow = row;
        int nextEastRow = row;
        //Find the west edge on the north side, and determine the next starting point.
        if (!plot[row, westEdge].WestFence)
        {
            do
            {
                plot[row, westEdge].NorthCounted = true;
                westEdge--;
            }
            while (plot[row, westEdge].NorthFence && !plot[row, westEdge].WestFence);
        }
        if (!plot[row, westEdge].NorthFence)
        {
            nextWestRow--;
        }
        else
        {
            plot[row, westEdge].NorthCounted = true;
        }
        //Now do the same for the east edge.
        if (!plot[row, eastEdge].EastFence)
        {
            do
            {
                plot[row, eastEdge].NorthCounted = true;
                eastEdge++;
            }
            while (plot[row, eastEdge].NorthFence && !plot[row, eastEdge].EastFence);
        }
        if (!plot[row, eastEdge].NorthFence)
        {
            nextEastRow--;
        }
        else
        {
            plot[row, eastEdge].NorthCounted = true;
        }
        //Traverse the two found edges.
        TraversePlotForSides(ref plot, nextWestRow, westEdge, ref numSides);
        TraversePlotForSides(ref plot, nextEastRow, eastEdge, ref numSides);
    }
    //East
    if (plot[row, col].EastFence && !plot[row, col].EastCounted)
    {
        plot[row, col].EastCounted = true;
        numSides++;
        //Find the edge for this side, where an edge is defined as:
        // * A plot with both a vertical and horizontal fence.
        // * A plot with a vertical/horizontal fence next to an inner fence.
        int northEdge = row;
        int southEdge = row;
        int nextNorthCol = col;
        int nextSouthCol = col;
        //Find the north edge on the east side, and determine the next starting point.
        if (!plot[northEdge, col].NorthFence)
        {
            do
            {
                plot[northEdge, col].EastCounted = true;
                northEdge--;
            }
            while (plot[northEdge, col].EastFence && !plot[northEdge, col].NorthFence);
        }
        if (!plot[northEdge, col].EastFence)
        {
            nextNorthCol++;
        }
        else
        {
            plot[northEdge, col].EastCounted = true;
        }
        //Now do the same for the south edge.
        if (!plot[southEdge, col].SouthFence)
        {
            do
            {
                plot[southEdge, col].EastCounted = true;
                southEdge++;
            }
            while (plot[southEdge, col].EastFence && !plot[southEdge, col].SouthFence);
        }
        if (!plot[southEdge, col].EastFence)
        {
            nextSouthCol++;
        }
        else
        {
            plot[southEdge, col].EastCounted = true;
        }
        //Traverse the two found edges.
        TraversePlotForSides(ref plot, northEdge, nextNorthCol, ref numSides);
        TraversePlotForSides(ref plot, southEdge, nextSouthCol, ref numSides);
    }
    //South
    if (plot[row, col].SouthFence && !plot[row, col].SouthCounted)
    {
        plot[row, col].SouthCounted = true;
        numSides++;
        //Find the edge for this side, where an edge is defined as:
        // * A plot with both a vertical and horizontal fence.
        // * A plot with a vertical/horizontal fence next to an inner fence.
        int westEdge = col;
        int eastEdge = col;
        int nextWestRow = row;
        int nextEastRow = row;
        //Find the west edge on the south side, and determine the next starting point.
        if (!plot[row, westEdge].WestFence)
        {
            do
            {
                plot[row, westEdge].SouthCounted = true;
                westEdge--;
            }
            while (plot[row, westEdge].SouthFence && !plot[row, westEdge].WestFence);
        }
        if (!plot[row, westEdge].SouthFence)
        {
            nextWestRow++;
        }
        else
        {
            plot[row, westEdge].SouthCounted = true;
        }
        //Now do the same for the east edge.
        if (!plot[row, eastEdge].EastFence)
        {
            do
            {
                plot[row, eastEdge].SouthCounted = true;
                eastEdge++;
            }
            while (plot[row, eastEdge].SouthFence && !plot[row, eastEdge].EastFence);
        }
        if (!plot[row, eastEdge].SouthFence)
        {
            nextEastRow++;
        }
        else
        {
            plot[row, eastEdge].SouthCounted = true;
        }
        //Traverse the two found edges.
        TraversePlotForSides(ref plot, nextWestRow, westEdge, ref numSides);
        TraversePlotForSides(ref plot, nextEastRow, eastEdge, ref numSides);
    }
    //West
    if (plot[row, col].WestFence && !plot[row, col].WestCounted)
    {
        plot[row, col].WestCounted = true;
        numSides++;
        //Find the edge for this side, where an edge is defined as:
        // * A plot with both a vertical and horizontal fence.
        // * A plot with a vertical/horizontal fence next to an inner fence.
        int northEdge = row;
        int southEdge = row;
        int nextNorthCol = col;
        int nextSouthCol = col;
        //Find the north edge on the west side, and determine the next starting point.
        if (!plot[northEdge, col].NorthFence)
        {
            do
            {
                plot[northEdge, col].WestCounted = true;
                northEdge--;
            }
            while (plot[northEdge, col].WestFence && !plot[northEdge, col].NorthFence);
        }
        if (!plot[northEdge, col].WestFence)
        {
            nextNorthCol--;
        }
        else
        {
            plot[northEdge, col].WestCounted = true;
        }
        //Now do the same for the south edge.
        if (!plot[southEdge, col].SouthFence)
        {
            do
            {
                plot[southEdge, col].WestCounted = true;
                southEdge++;
            }
            while (plot[southEdge, col].WestFence && !plot[southEdge, col].SouthFence);
        }
        if (!plot[southEdge, col].WestFence)
        {
            nextSouthCol--;
        }
        else
        {
            plot[southEdge, col].WestCounted = true;
        }
        //Traverse the two found edges.
        TraversePlotForSides(ref plot, northEdge, nextNorthCol, ref numSides);
        TraversePlotForSides(ref plot, southEdge, nextSouthCol, ref numSides);
    }
}