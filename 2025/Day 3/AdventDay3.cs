/*
 * Can't help myself, I love recursion.
 * "joltages" is the input list of values from which the highest value must be found.
 * "currentPlace" represents the index of the final number to be constructed. (0 = ones place, 1 = tens place, etc)
 * "previousHighestIndex" is the index of the last highest value found, ensuring we don't use the same value twice.
*/
long RecursiveJoltage(ref List<int> joltages, int currentPlace, int previousHighestIndex)
{
    if (currentPlace < 0)
    {
        //Bail condition
        return 0;
    }    
    long highestVal = 0;
    int currentHighestIndex = previousHighestIndex;
    for (int i = previousHighestIndex + 1; i < joltages.Count; i++)
    {
        //The highest joltage for this position in the value must both:
        //  1. Be greater than the current highest value found.
        //  2. Be at an index that allows enough remaining values to fill the remaining digits.
        if ((joltages[i] > highestVal) && (i < joltages.Count - currentPlace))
        {
            highestVal = joltages[i];
            currentHighestIndex = i;
        }
    }
    //Multiply the highest value found by the appropriate power of ten for its place value.
    highestVal *= (long)Math.Pow(10, currentPlace);
    //Recurse to get the highest value for the next battery.
    return highestVal + RecursiveJoltage(ref joltages, currentPlace - 1, currentHighestIndex);
}

void AdventDay3()
{
    var rawFile = File.ReadAllLines(@"Input.txt");
    //Input is a series of lines of digits, representing piles of batteries with different "joltages".
    List<string> batteryPiles = new List<string>();
    foreach (var line in rawFile)
    {
        batteryPiles.Add(line);
    }
    //Each pile's highest "joltage" is defined by activating N batteries in series, with N being the number of batteries to enable.
    //Joltage values range from 1-9. Piles cannot be sorted. The highest value must be found starting from the left, going to the right.
    long runningJoltagePart1 = 0;
    long runningJoltagePart2 = 0;
    foreach (string pile in batteryPiles)
    {
        List<int> joltages = new List<int>();
        for (int i = 0; i < pile.Length; i++)
        {
            joltages.Add(int.Parse(pile[i].ToString()));
        }
        //Part 1: Find the highest joltage by activating 2 batteries. Use one recursive repetition.
        runningJoltagePart1 += RecursiveJoltage(ref joltages, 1, -1);
        //Part 2: Find the highest joltage by activating 12 batteries. Use eleven recursive repetitions.
        runningJoltagePart2 += RecursiveJoltage(ref joltages, 11, -1);
    }
    MessageBox.Show("Part 1: " + runningJoltagePart1.ToString() + "\nPart 2: " + runningJoltagePart2.ToString());
}