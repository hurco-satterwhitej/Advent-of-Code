void PopulateListWithIDRange(string idInput, ref List<string> idRange)
{
    //Doing string manip, so take the input ranges and split into individual IDs.
    string[] splits = idInput.Split('-');
    if (splits.Length != 2)
    {
        idRange.Add(idInput);
        return;
    }
    long startId = 0;
    long endId = 0;
    long.TryParse(splits[0], out startId);
    long.TryParse(splits[1], out endId);
    for (long i = startId; i <= endId; i++)
    {
        idRange.Add(i.ToString());
    }
}

bool ValidateIDPart1(string id)
{
    //Love the smell of brute force in the morning.
    //Part 1 invalid IDs have exactly the same sequence repeated twice.
    //Simply split the ID in half and add the half together twice.
    string subId = id.Substring(0, id.Length / 2);
    string doubled = subId + subId;
    if (doubled.Length != id.Length)
    {
        //Cannot be invalid.
        return true;
    }
    if (doubled == id)
    {
        //Invalid ID found.
        return false;
    }
    return true;
}

bool ValidateIDPart2(string id)
{
    //I solved part 2 accidentally first with whatever this solution is.
    //Kept it and it made for a fast solve after the part 1 solution.
    //...
    //Invalid IDs consist of a sequence of repeated patterns.
    //If a match is not found by halfway through the ID, there is no match.
    for (int i = 1; i <= id.Length / 2; i++)
    {
        string subId = id.Substring(0, i);
        string[] splits = id.Split(new string[] { subId }, StringSplitOptions.None);
        bool idDifferenceFound = false;
        foreach (string split in splits)
        {
            if (split != "")
            {
                idDifferenceFound = true;
                break;
            }
        }
        if (idDifferenceFound)
        {
            continue;
        }
        //Invalid ID found.
        return false;
    }
    //Invalid ID found.
    return true;
}

void AdventDay2()
{
    //Input contains two lists of numbers. 
    var rawFile = File.ReadAllLines(@"Input.txt");
    List<string> idRanges = new List<string>();
    foreach (string line in rawFile)
    {
        string[] splits = line.Split(',');
        foreach (string split in splits)
        {
            idRanges.Add(split);
        }
    }
    ulong runningTallyPart1 = 0;
    ulong runningTallyPart2 = 0;
    foreach (string idInput in idRanges)
    {
        List<string> idRange = new List<string>();
        PopulateListWithIDRange(idInput, ref idRange);
        foreach (string id in idRange)
        {
            //Tally the invalid IDs.
            if (!ValidateIDPart1(id))
            {
                ulong idValue = 0;
                ulong.TryParse(id, out idValue);
                runningTallyPart1 += idValue;
            }
            if (!ValidateIDPart2(id))
            {
                ulong idValue = 0;
                ulong.TryParse(id, out idValue);
                runningTallyPart2 += idValue;
            }
        }
    }
    MessageBox.Show("Part 1: " + runningTallyPart1.ToString());
    MessageBox.Show("Part 2: " + runningTallyPart2.ToString());
}