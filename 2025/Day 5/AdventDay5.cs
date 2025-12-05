void AdventDay5()
{
    var rawFile = File.ReadAllLines(@"Input.txt");
    //Input is in two sections, separated by a blank line.
    //Section 1: A series of ID ranges, indicating fresh ingredients in the ranges and spoiled outside.
    //Section 2: Ingredient IDs.
    //Begin by building the two lists.
    List<Tuple<long, long>> ranges = new List<Tuple<long, long>>();
    List<long> ingredientIDs = new List<long>();
    bool section1 = true;
    foreach (string line in rawFile)
    {
        if (line == "")
        {
            section1 = false;
            continue;
        }
        if (section1)
        {
            string[] parts = line.Split('-');
            if (parts.Length != 2)
            {
                MessageBox.Show("Bad input somehow");
                return;
            }
            long start = 0, end = 0;
            long.TryParse(parts[0], out start);
            long.TryParse(parts[1], out end);
            ranges.Add(new Tuple<long, long>(start, end));
        }
        else
        {
            long id = 0;
            long.TryParse(line, out id);
            ingredientIDs.Add(id);
        }
    }
    //Gonna play this fast and loose with checking every range since they can overlap and I don't want to sanitize the input.
    long freshCount = 0;
    foreach (long id in ingredientIDs)
    {
        foreach (Tuple<long, long> range in ranges)
        {
            if (range.Item1 <= id && id <= range.Item2)
            {
                freshCount++;
                break;
            }
        }
    }
    MessageBox.Show("Part 1: " + freshCount.ToString());
    //Alright, fine, I'll try to sanitize the input. But I won't like it.
    //Combine ranges into single larger ranges where overlaps are found.
    //Sort ranges by start value to begin.
    ranges = ranges.OrderBy(r => r.Item1).ToList();
    List<Tuple<long, long>> combinedRanges = new List<Tuple<long, long>>();
    //Initialize combined list with the first range, which is guaranteed to be the start (or overlap with it).
    combinedRanges.Add(ranges[0]);
    for (int i = 1; i < ranges.Count; i++)
    {
        if (ranges[i].Item1 <= combinedRanges[combinedRanges.Count - 1].Item2)
        {
            //Overlap found - combine ranges.
            long newStart = combinedRanges[combinedRanges.Count - 1].Item1;
            long newEnd = (combinedRanges[combinedRanges.Count - 1].Item2 > ranges[i].Item2) ? combinedRanges[combinedRanges.Count - 1].Item2 : ranges[i].Item2;
            combinedRanges[combinedRanges.Count - 1] = new Tuple<long, long>(newStart, newEnd);
        }
        else
        {
            //If no overlap was found, the range is unique and should be added as the next in line.
            combinedRanges.Add(ranges[i]);
        }
    }
    long potentiallyFreshIds = 0;
    foreach (Tuple<long, long> range in combinedRanges)
    {
        //Compensate for off-by-one.
        potentiallyFreshIds += (range.Item2 - range.Item1 + 1);
    }
    MessageBox.Show("Part 2: " + potentiallyFreshIds.ToString());
}