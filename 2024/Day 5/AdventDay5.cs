class Rule
{
    public bool DefaultRule = false;
    public int Page1;
    public int Page2;

    public Rule()
    {
        Page1 = 0;
        Page2 = 0;
        DefaultRule = true;
    }

    public Rule(string rule)
    {
        string[] parts = rule.Split('|');
        if (parts.Length != 2)
        {
            MessageBox.Show("Invalid rule format: " + rule);
            return;
        }
        Page1 = int.Parse(parts[0]);
        Page2 = int.Parse(parts[1]);
        DefaultRule = false;
    }
}

class Update
{
    private List<int> update;

    public Update(string update)
    {
        string[] parts = update.Split(',');
        this.update = new List<int>();
        foreach (string part in parts)
        {
            this.update.Add(int.Parse(part));
        }
    }

    //Returns the offending rule if the update is invalid, otherwise returns a "default" rule.
    public Rule Validate(ref List<Rule> rules, ref int returnVal)
    {
        Rule defaultRule = new Rule();
        int middle = -1;
        foreach (Rule rule in rules)
        {
            if (update.Contains(rule.Page1) && update.Contains(rule.Page2))
            {
                //Updates are considered invalid if the second page is before the first page
                int index1 = update.IndexOf(rule.Page1);
                int index2 = update.IndexOf(rule.Page2);
                if (index2 < index1)
                {
                    returnVal = -1;
                    return rule;
                }
            }
        }
        //If we get here, we have a valid update. Return the middle item.
        middle = (int)Math.Floor(update.Count / 2.0);
        returnVal = update[middle];
        return defaultRule;
    }

    public int Correct(ref List<Rule> rules)
    {
        int returnVal = -1;
        int attempts = 0;
        while (returnVal == -1)
        {
            Rule rule = Validate(ref rules, ref returnVal);
            if (!rule.DefaultRule)
            {
                //Swap the two pages
                int index1 = update.IndexOf(rule.Page1);
                int index2 = update.IndexOf(rule.Page2);
                int temp = update[index1];
                update[index1] = update[index2];
                update[index2] = temp;
                attempts++;
                if (attempts > 100)
                {
                    MessageBox.Show("Best attach.");
                }
            }
        }
        return returnVal;
    }
}

private void AdventDay5(object sender, MouseButtonEventArgs e)
{
    var rawFile = File.ReadAllLines(@"Input.txt");
    //Input is a series of n lines with various #|#, a blank line, then a series of n lines with various #,#,#,etc
    List<string> ruleLines = new List<string>();
    List<string> updateLines = new List<string>();
    bool ruleSection = true;
    foreach (string line in rawFile)
    {
        if (line == "")
        {
            ruleSection = false;
            continue;
        }
        if (ruleSection)
        {
            ruleLines.Add(line);
        }
        else
        {
            updateLines.Add(line);
        }
    }
    List<Rule> rules = new List<Rule>();
    foreach (string rule in ruleLines)
    {
        rules.Add(new Rule(rule));
    }
    List<Update> updates = new List<Update>();
    foreach (string update in updateLines)
    {
        updates.Add(new Update(update));
    }
    int validTotal = 0;
    int invalidTotal = 0;
    foreach (Update update in updates)
    {
        //Part 1: Validate the updates, total the middle values of valid updates
        int returnVal = -1;
        Rule rule = update.Validate(ref rules, ref returnVal);
        if (rule.DefaultRule)
        {
            validTotal += returnVal;
        }
        else
        {
            //Part 2: Correct the invalid updates, total the middle values of newly valid updates
            returnVal = update.Correct(ref rules);
            invalidTotal += returnVal;
        }
    }
    MessageBox.Show("Total of middle values for valid updates: " + validTotal.ToString()
        + "Total of middle values for corrected updates: " + invalidTotal.ToString());
}