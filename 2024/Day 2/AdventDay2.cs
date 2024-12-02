private void AdventDay2(object sender, MouseButtonEventArgs e)
{
    //Input contains a list of n numbers separated by spaces. 
    var rawFile = File.ReadAllLines(@"Input.txt");
    int safeCount = 0;
    foreach (string line in rawFile)
    {
        List<int> report = new List<int>();
        string[] split = line.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < split.Length; i++)
        {
            report.Add(int.Parse(split[i]));
        }
        //Part 1: Verify that the report is safe.
        bool safe = IsSafe(report);
        //Part 2: Add dampening such that any one index can be removed and the report is still safe.
        if (!safe)
        {
            for(int i = 0; i < report.Count; i++)
            {
                List<int> shorterReport = new List<int>(report);
                shorterReport.RemoveAt(i);
                if (IsSafe(shorterReport))
                {
                    safe = true;
                    break;
                }
            }
        }
        if (safe)
        {
            safeCount++;
        }
    }
    MessageBox.Show("Safe count: " + safeCount);
}

private bool IsSafe(List<int> report)
{
    bool safe = true;
    bool? ascending = null;
    for (int i = 0; i < report.Count - 1; i++)
    {
        int left = report[i];
        int right = report[i + 1];
        int diff = left - right;
        if (Math.Abs(diff) > 3 || Math.Abs(diff) < 1)
        {
            //Unsafe if the difference is not between 1 and 3.
            safe = false;
            break;
        }
        if (ascending == null)
        {
            if (diff < 0)
            {
                ascending = false;
            }
            else
            {
                ascending = true;
            }
        }
        else
        {
            if (ascending == true && diff < 0)
            {
                //Unsafe if switching from ascending to descending
                safe = false;
                break;
            }
            else if (ascending == false && diff > 0)
            {
                //Unsafe if switching from descending to ascending
                safe = false;
                break;
            }
        }
    }
    return safe;
}