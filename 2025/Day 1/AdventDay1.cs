class CombinationLock
{
    uint CurrentPosition = 50;
    uint TimesStoppedAtZero = 0;
    uint TimesPassedThroughZero = 0;

    public CombinationLock() { }

    public void Rotate(string rotation)
    {
        char dir = rotation[0];
        uint distance = 0;
        uint.TryParse(rotation.Substring(1), out distance);
        //distance can theoretically be greater than 100, so we need to mod it first.
        //The quotient will tell us the number of times it passed through zero.
        TimesPassedThroughZero += distance / 100;
        distance = distance % 100;
        switch (dir)
        {
            case 'L':
                {
                    if (distance > CurrentPosition)
                    {
                        //Don't count passing through zero if we were already at zero.
                        if (CurrentPosition != 0)
                        {
                            TimesPassedThroughZero++;
                        }
                        CurrentPosition = 100 - (distance - CurrentPosition);
                    }
                    else
                    {
                        CurrentPosition -= distance;
                    }
                    break;
                }
            case 'R':
                {
                    if (CurrentPosition + distance >= 100)
                    {
                        CurrentPosition = (CurrentPosition + distance) - 100;
                        //Don't count passing through zero if we land on zero.
                        if (CurrentPosition != 0)
                        {
                            TimesPassedThroughZero++;
                        }
                    }
                    else
                    {
                        CurrentPosition += distance;
                    }
                    break;
                }
        }
        if (CurrentPosition == 0)
        {
            TimesStoppedAtZero++;
        }
    }

    public uint GetPasswordPart1()
    {
        return TimesStoppedAtZero;
    }

    public uint GetPasswordPart2()
    {
        return TimesPassedThroughZero + TimesStoppedAtZero;
    }
}

private void AdventDay1()
{
    //Input contains two lists of numbers. 
    var rawFile = File.ReadAllLines(@"Input.txt");
    CombinationLock combLock = new CombinationLock();
    foreach (var line in rawFile)
    {
        combLock.Rotate(line);
    }
    uint part1 = combLock.GetPasswordPart1();
    uint part2 = combLock.GetPasswordPart2();
    MessageBox.Show("Part 1: " + part1.ToString() + "\nPart 2: " + part2.ToString());
}