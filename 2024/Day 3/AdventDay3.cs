private void AdventDay3(object sender, MouseButtonEventArgs e)
{
    var rawFile = File.ReadAllLines(@"Input.txt");
    //Input is a series of n lines with various "do", "dont", and "mul" statements.
    List<string> lines = new List<string>();
    foreach (string line in rawFile)
    {
        lines.Add(line);
    }
    List<string> newLines = new List<string>();
    //Part 2: remove "dont" sections from the lines.
    RemoveDontSections(lines, ref newLines);
    //Part 1: Multiply the values in the "mul" statements and add them together.
    int total = 0;
    string pattern = @"mul\(\d+\,\d+\)";
    foreach (string line in newLines)
    {
        foreach (Match match in Regex.Matches(line, pattern))
        {
            string result = match.Value;
            result = result.Replace("mul(", "");
            result = result.Replace(")", "");
            string[] parts = result.Split(',');
            total += int.Parse(parts[0]) * int.Parse(parts[1]);
        }
    }
    MessageBox.Show("Multiplied total: " + total.ToString());
}

private void RemoveDontSections(List<string> lines, ref List<string> newLines)
{
    bool endedLineOnDont = false;
    bool sectionEndedOnDont = false;
    //"mul" calls between a "don't()" and a "do()" are ignored.
    //All others have their contents multiplied and added to a total.
    foreach (string line in lines)
    {
        string dont = "don't()";
        string doo = "do()";
        string[] removedDonts = line.Split(new string[] { dont }, StringSplitOptions.None);
        //first line begins with "do" active, every other split begins with "dont" active
        //UNLESS the previous line ended on a "dont" statement, in which case the first line is safely ignored.
        string newLine = "";
        if (!endedLineOnDont)
        {
            newLine = removedDonts[0];
        }
        foreach (string str in removedDonts)
        {
            //There can be multiple "do" statements in a single line, but only one "dont" statement.
            //Discard the first split line to account for the initial split on "dont".
            string[] removedDos = str.Split(new string[] { doo }, StringSplitOptions.None);
            if (removedDos.Length > 1)
            {
                sectionEndedOnDont = false;
                for (int i = 1; i < removedDos.Length; i++)
                {
                    newLine += removedDos[i];
                }
            }
            else
            {
                sectionEndedOnDont = true;
            }
        }
        endedLineOnDont = sectionEndedOnDont;
        newLines.Add(newLine);
    }
}