List<long> ExpandDiskMap(string diskMap)
{
    List<long> expanded = new List<long>();
    long id = 0;
    for (int i = 0; i < diskMap.Length; i++)
    {
        //If i % 2 == 0, add the specified number of IDs to the string.
        //If i % 2 == 1, add the specified number of '.' to the string.
        if (i % 2 == 0)
        {
            int numIDs = int.Parse(diskMap[i].ToString());
            for (int j = 0; j < numIDs; j++)
            {
                expanded.Add(id);
            }
            id++;
        }
        else
        {
            int numDots = int.Parse(diskMap[i].ToString());
            for (int j = 0; j < numDots; j++)
            {
                expanded.Add(-1);
            }
        }
    }
    return expanded;
}

void CompressExpandedMap(ref List<long> diskData, ref int diskSize)
{
    for (int i = 0; i < diskSize; i++)
    {
        if (diskData[i] == -1)
        {
            diskData[i] = diskData[diskSize - 1];
            diskSize--;
            while (diskData[diskSize - 1] == -1)
            {
                diskSize--;
            }
        }
    }
}

void CompressExpandedMapWithFullBlocks(ref List<long> diskData, int diskSize)
{
    long activeID = -1;
    long lastID = -1;
    int activeCount = 0;
    for (int i = diskSize - 1; i >= 0; i--)
    {
        while (diskData[i] == -1)
        {
            if (activeCount > 0)
            {
                int index = FindStartingIndexOfSafeContiguousBlock(ref diskData, i, activeCount);
                if (index != -1)
                {
                    //Contiguous block found. Transport the block to the starting index, replacing active block with -1.
                    for (int j = 0; j < activeCount; j++)
                    {
                        diskData[index + j] = activeID;
                        diskData[i + 1 + j] = -1;
                    }
                }
                lastID = activeID;
                activeID = -1;
                activeCount = 0;
            }
            i--;
            continue;
        }
        if (diskData[i] != activeID)
        {
            if (activeCount > 0)
            {
                int index = FindStartingIndexOfSafeContiguousBlock(ref diskData, i, activeCount);
                if (index != -1)
                {
                    //Contiguous block found. Transport the block to the starting index, replacing active block with -1.
                    for (int j = 0; j < activeCount; j++)
                    {
                        diskData[index + j] = activeID;
                        diskData[i + 1 + j] = -1;
                    }
                }
                lastID = activeID;
                activeID = -1;
                activeCount = 0;
            }
            if (diskData[i] != lastID)
            {
                activeID = diskData[i];
                activeCount++;
            }
        }
        else if (activeCount > 0)
        {
            activeCount++;
        }
    }
}

int FindStartingIndexOfSafeContiguousBlock(ref List<long> diskData, int maxIndex, int count)
{
    int index = -1;
    int currentCount = 0;
    for (int i = 0; i <= maxIndex; i++)
    {
        if (diskData[i] != -1)
        {
            index = -1;
            currentCount = 0;
            continue;
        }
        if (index == -1)
        {
            index = i;
        }
        currentCount++;
        if (currentCount >= count)
        {
            return index;
        }
    }
    return -1;
}

long PerformCompressedDiskChecksum(List<long> diskData, int diskSize)
{
    long checksum = 0;
    for (int i = 0; i < diskSize; i++)
    {
        if (diskData[i] != -1)
        {
            checksum += diskData[i] * i;
        }
    }
    return checksum;
}

private void AdventDay9()
{
    var rawFile = File.ReadAllLines(@"Input.txt");
    //Input is one line with a series of numbers, representing some encoded disk map.
    string diskMap = "";
    foreach (string line in rawFile)
    {
        diskMap = line;
    }
    List<long> diskData = ExpandDiskMap(diskMap);
    int diskSize = diskData.Count;

    //CompressExpandedMap(ref diskData, ref diskSize);
    CompressExpandedMapWithFullBlocks(ref diskData, diskSize);

    long sum = PerformCompressedDiskChecksum(diskData, diskSize);

    MessageBox.Show("Checksum result: " + sum.ToString());
}