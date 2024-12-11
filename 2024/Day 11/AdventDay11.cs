void AdventDay11()
{
    var rawFile = File.ReadAllLines(@"Input.txt");
    //Input is one line with a series of numbers, representing engraved stones.
    //Solution works for both parts of the problem, only the number of iterations is different.
    //Measured as taking 35 ms in a debug build to run 75 iterations.
    string stoneInput = "";
    foreach (string line in rawFile)
    {
        stoneInput = line;
    }
    string[] split = stoneInput.Split(' ');
    //For the purposes of optimization, we will be using dictionaries to track the number of instances of each stone,
    //as well as caching the results of any given rule to be applied to a stone. This will ultimately reduce the
    //memory consumption by not having unique instances of stones in a massive list, whie also reducing computation time
    //for stone values that have already been processed.
    Dictionary<long, long> instances = new Dictionary<long, long>();
    Dictionary<long, Tuple<long, long>> cache = new Dictionary<long, Tuple<long, long>>();
    //Set up the initial dictionary based on the input.
    foreach (string s in split)
    {
        long l = long.Parse(s);
        if (!instances.ContainsKey(l))
        {
            instances.Add(l, 0);
        }
        instances[l] = instances[l] + 1;
    }
    //Part 1 requires 25 instances, Part 2 requires 75.
    for (int i = 0; i < 75; i++)
    {
        List<long> stonesToProcess = instances.Keys.ToList<long>();
        List<Tuple<long, long>> stoneInfo = new List<Tuple<long, long>>();
        //First: Determine how many instances of each stone will be processed.
        foreach (long stone in stonesToProcess)
        {
            stoneInfo.Add(new Tuple<long, long>(stone, instances[stone]));
        }
        //Second: Process the blink rule for all instances of each stone simultaneously.
        foreach (Tuple<long, long> tuple in stoneInfo)
        {
            ProcessBlinkForAllInstancesOfStone(ref instances, ref cache, tuple.Item1, tuple.Item2);
        }
    }
    List<long> finalStones = instances.Values.ToList<long>();
    long count = 0;
    foreach (long stone in finalStones)
    {
        count += stone;
    }
    MessageBox.Show("Number of stones: " + count.ToString());
}

public Tuple<long, long> ApplyBlinkRule(long num)
{
    //Rules are as follows, in order:
    // * 0 becomes 1
    // * Any number with an even number of digits is split into two numbers (leading 0s are removed)
    // * Everything else *= 2024
    //Values returned in a tuple so the split can correctly retrun two values.
    if (num == 0)
    {
        return new Tuple<long, long>(1, -1);
    }
    else
    {
        string str = num.ToString();
        int len = str.Length;
        if (len % 2 == 0)
        {
            string split1 = str.Substring(0, len / 2);
            string split2 = str.Substring(len / 2);
            return new Tuple<long, long>(long.Parse(split1), long.Parse(split2));
        }
        else
        {
            long newNum = num * 2024;
            return new Tuple<long, long>(newNum, -1);
        }
    }
}

public void ProcessBlinkForAllInstancesOfStone(ref Dictionary<long, long> instances, ref Dictionary<long, Tuple<long, long>> cache, long num, long toWalk)
{
    //The aim of this function is to simplify iterations by applying the blink rule to all instances of a stone per iteration,
    //while caching the results of running any stone value through the rule function.
    //If a stone value is new, cache it first before applying the change to all instances at once.
    if (cache.ContainsKey(num))
    {
        instances[num] = instances[num] - toWalk;
        if (instances[num] == 0)
        {
            instances.Remove(num);
        }
        Tuple<long, long> tuple = cache[num];
        if (tuple.Item1 != -1)
        {
            if (!instances.ContainsKey(tuple.Item1))
            {
                instances.Add(tuple.Item1, 0);
            }
            instances[tuple.Item1] = instances[tuple.Item1] + toWalk;
        }
        if (tuple.Item2 != -1)
        {
            if (!instances.ContainsKey(tuple.Item2))
            {
                instances.Add(tuple.Item2, 0);
            }
            instances[tuple.Item2] = instances[tuple.Item2] + toWalk;
        }
        return;
    }
    cache[num] = ApplyBlinkRule(num);
    ProcessBlinkForAllInstancesOfStone(ref instances, ref cache, num, toWalk);
}