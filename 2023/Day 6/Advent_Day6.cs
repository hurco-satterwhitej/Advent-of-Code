public class Race
{
    public long Time;
    public long DistanceToBeat;

    public Race(long time, long distanceToBeat)
    {
        Time = time;
        DistanceToBeat = distanceToBeat;
    }
}

private void AdventDay6(object sender, MouseButtonEventArgs e)
{
    var rawFile = File.ReadAllLines(@"Input.txt");
    List<string> lines = new List<string>();
    foreach (string line in rawFile)
    {
        lines.Add(line);
    }
    //Two lines, with leading identifier string that can be ignored.
    string[] splitTime = lines[0].Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
    string[] splitDist = lines[1].Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
    List<Race> races = new List<Race>();
    for (long i = 1; i < splitTime.Length; i++)
    {
        Race race = new Race(long.Parse(splitTime[i]), long.Parse(splitDist[i]));
        races.Add(race);
    }
    long total = 1;
    long bestTime = 0, bestDist = 0;
    List<Race> bestRaces = new List<Race>();
    for (int i = 0; i < races.Count; i++)
    {
        total *= GetWinningSolutionCount(races[i], ref bestTime, ref bestDist);
        bestRaces.Add(new Race(bestTime, bestDist));
    }
    MessageBox.Show("Weird multiplied value: " + total.ToString());
}

private long GetWinningSolutionCount(Race race, ref long bestTime, ref long bestDist)
{
    long winningCount = 0;
    long distToBeat = race.DistanceToBeat;
    for (long i = 1; i < race.Time; i++)
    {
        long acceleration = i;
        long movementDuration = race.Time - i;
        long distance = acceleration * movementDuration;
        if (distance > distToBeat)
        {
            winningCount++;
        }
    }
    return winningCount;
}