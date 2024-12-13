class ClawMachine
{
    int buttonAXval;
    int buttonAYval;

    int buttonBXval;
    int buttonBYval;

    long prizeXval;
    long prizeYval;

    public ClawMachine(string buttonA, string buttonB, string prize)
    {
        ParseButton(buttonA, ref buttonAXval, ref buttonAYval);
        ParseButton(buttonB, ref buttonBXval, ref buttonBYval);
        ParsePrize(prize);
    }

    private void ParseButton(string button, ref int x, ref int y)
    {
        //Button info comes in the form "Button #: X+##, Y+##"
        //Header is irrelevant, so split that out to deal with the value portion.
        string[] initialSplit = button.Split(':');
        //" X+##, Y+##" is now the content in initialSplit[1]. Split further on +.
        string[] splitOnValueDelimiter = initialSplit[1].Split('+');
        //Value 0 is irrelevant, value 1 is "##, Y", value 2 is "##". Split value 1 on comma to reach the final X.
        string[] splitOnComma = splitOnValueDelimiter[1].Split(',');
        //X is now in splitOnComma[0], Y is in splitOnValueDelimiter[2].
        x = int.Parse(splitOnComma[0]);
        y = int.Parse(splitOnValueDelimiter[2]);
    }

    private void ParsePrize(string prize)
    {
        //Prize info comes in the form "Prize: X=####, Y=####"
        //Header is irrelevant, so split that out to deal with the value portion.
        string[] initialSplit = prize.Split(':');
        //" X=##, Y=##" is now the content in initialSplit[1]. Split further on =.
        string[] splitOnValueDelimiter = initialSplit[1].Split('=');
        //Value 0 is irrelevant, value 1 is "####, Y", value 2 is "####". Split value 1 on comma to reach the final X.
        string[] splitOnComma = splitOnValueDelimiter[1].Split(',');
        //X is now in splitOnComma[0], Y is in splitOnValueDelimiter[2].
        prizeXval = long.Parse(splitOnComma[0]);
        prizeYval = long.Parse(splitOnValueDelimiter[2]);
    }

    public bool SolveClawMachine(ref long minTokens, int partNum)
    {
        if (partNum == 2)
        {
            prizeXval += 10000000000000;
            prizeYval += 10000000000000;
        }
        long aButtonPresses = ((prizeXval * buttonBYval) - (prizeYval * buttonBXval)) / ((buttonAXval * buttonBYval) - (buttonAYval * buttonBXval));
        long bButtonPresses = ((prizeYval * buttonAXval) - (prizeXval * buttonAYval)) / ((buttonAXval * buttonBYval) - (buttonAYval * buttonBXval));
        bool pressLimitCheck = (partNum == 1) ? (aButtonPresses <= 100 && bButtonPresses <= 100) : true;
        bool solvedCheck = ((aButtonPresses * buttonAXval + bButtonPresses * buttonBXval) == prizeXval) && ((aButtonPresses * buttonAYval + bButtonPresses * buttonBYval) == prizeYval);
        if (pressLimitCheck && solvedCheck)
        {
            minTokens = (aButtonPresses * 3) + bButtonPresses;
            return true;
        }
        minTokens = -1;
        return false;
    }
}

void AdventDay13()
{
    var rawFile = File.ReadAllLines(@"Input.txt");
    //Input is a series of n(3 + 1) - 1 lines, with 3 lines describing a claw machine and a blank line in between.
    List<string> lines = new List<string>();
    foreach (string line in rawFile)
    {
        if (line != "")
        {
            lines.Add(line);
        }
    }
    List<ClawMachine> clawMachines = new List<ClawMachine>();
    for (int i = 0; i + 2 < lines.Count; i += 3)
    {
        clawMachines.Add(new ClawMachine(lines[i], lines[i + 1], lines[i + 2]));
    }
    long totalTokens = 0;
    foreach (ClawMachine machine in clawMachines)
    {
        long tokens = 0;
        if (machine.SolveClawMachine(ref tokens, 1))
        {
            totalTokens += tokens;
        }
    }
    long totalTokensPart2 = 0;
    foreach (ClawMachine machine in clawMachines)
    {
        long tokens = 0;
        if (machine.SolveClawMachine(ref tokens, 2))
        {
            totalTokensPart2 += tokens;
        }
    }
    MessageBox.Show("Part 1 tokens: " + totalTokens.ToString()
        + "\nPart 2 tokens: " + totalTokensPart2.ToString());
}