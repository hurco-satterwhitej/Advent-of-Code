class Antenna
{
    int row = -1;
    int col = -1;
    char frequency = '\0';

    public Antenna(int row, int col, char frequency)
    {
        this.row = row;
        this.col = col;
        this.frequency = frequency;
    }

    public List<Antenna> GetAntinodes(Antenna other, int maxRow, int maxCol)
    {
        List<Antenna> antinodes = new List<Antenna>();
        if (frequency != other.frequency)
        {
            return antinodes;
        }
        //Start with two freebies at the location of the two resonant antennae
        antinodes.Add(new Antenna(row, col, '#'));
        antinodes.Add(new Antenna(other.row, other.col, '#'));
        //Calculate the difference between the two antennae for the remaining
        int rowDiff = row - other.row;
        int colDiff = col - other.col;
        if (rowDiff == 0 && colDiff == 0)
        {
            return antinodes;
        }
        //Construct the antinodes along the first path
        int firstRow = row + rowDiff;
        int firstCol = col + colDiff;
        while ((0 <= firstRow && firstRow <= maxRow) && (0 <= firstCol && firstCol <= maxCol))
        {
            antinodes.Add(new Antenna(firstRow, firstCol, '#'));
            firstRow += rowDiff;
            firstCol += colDiff;
        }
        //Construct the second antinode
        int secondRow = other.row - rowDiff;
        int secondCol = other.col - colDiff;
        while ((0 <= secondRow && secondRow <= maxRow) && (0 <= secondCol && secondCol <= maxCol))
        {
            antinodes.Add(new Antenna(secondRow, secondCol, '#'));
            secondRow -= rowDiff;
            secondCol -= colDiff;
        }
        return antinodes;
    }

    public override bool Equals(object obj)
    {
        Antenna objAnt = obj as Antenna;
        if (objAnt == null) return false;
        return (row == objAnt.row && col == objAnt.col);
    }
}

private void AdventDay8(object sender, MouseButtonEventArgs e)
{
    var rawFile = File.ReadAllLines(@"Input.txt");
    //Input is a series of n lines with a final answer, followed by a series of operands separated by spaces.
    List<string> mapLines = new List<string>();
    foreach (string line in rawFile)
    {
        mapLines.Add(line);
    }
    int maxRow = mapLines.Count - 1;
    int maxCol = mapLines[0].Length - 1;
    List<Antenna> antennas = new List<Antenna>();
    for (int i = 0; i < mapLines.Count; i++)
    {
        for (int j = 0; j < mapLines[i].Length; j++)
        {
            if (mapLines[i][j] != '.')
            {
                antennas.Add(new Antenna(i, j, mapLines[i][j]));
            }
        }
    }
    List<Antenna> antinodeLocations = new List<Antenna>();
    for (int i = 0; i < antennas.Count; i++)
    {
        for (int j = 0; j < antennas.Count; j++)
        {
            if (i == j)
            {
                continue;
            }
            List<Antenna> antinodes = antennas[i].GetAntinodes(antennas[j], maxRow, maxCol);
            foreach (Antenna antinode in antinodes)
            {
                if (antinodeLocations.Find(x => x.Equals(antinode)) == null)
                {
                    antinodeLocations.Add(antinode);
                }
            }
        }
    }
    MessageBox.Show("Unique antinodes: " + antinodeLocations.Count.ToString());
}