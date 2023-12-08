//This didn't work.
//It was an attempt at Part 1.

struct GridPos
{
    public int Row;
    public int Col;

    public GridPos(int r, int c)
    {
        Row = r;
        Col = c;
    }
}

struct NumberPos
{
    public int val;
    public GridPos startingPos;
    public List<GridPos> SurroundingGrids;

    public NumberPos(int i, GridPos pos)
    {
        val = i;
        startingPos = pos;
        SurroundingGrids = new List<GridPos>();
        int len = val.ToString().Length;
        SurroundingGrids.Add(new GridPos(startingPos.Row - 1, startingPos.Col - 1));
        SurroundingGrids.Add(new GridPos(startingPos.Row, startingPos.Col - 1));
        SurroundingGrids.Add(new GridPos(startingPos.Row + 1, startingPos.Col - 1));
        for (int j = 0; j <= len; j++)
        {
            SurroundingGrids.Add(new GridPos(startingPos.Row - 1, startingPos.Col + j));
            SurroundingGrids.Add(new GridPos(startingPos.Row + 1, startingPos.Col + j));
        }
        SurroundingGrids.Add(new GridPos(startingPos.Row, startingPos.Col + len));
    }

    public bool SymbolIsAdjacent(GridPos symbolPos)
    {
        if(SurroundingGrids.Contains(symbolPos))
        {
            return true;
        }
        return false;
    }
}

private void AdventDay3()
{
    var rawFile = File.ReadAllLines(@"AdventDay3Input.txt");
    List<string> input = new List<string>(rawFile);
    List<NumberPos> parts = new List<NumberPos>();
    List<GridPos> symbols = new List<GridPos>();
    int finalVal = 0;
    for (int i = 0; i < input.Count; i++)
    {
        string line = input[i];
        string chars = "";
        for (int j = 0; j < line.Length; j++)
        {
            if(Char.IsDigit(line[j]))
            {
                chars += line[j];
            }
            else
            {
                if (line[j] != '.')
                {
                    symbols.Add(new GridPos(i, j));
                }
                if(chars != "")
                {
                    int startingRow = i;
                    int startingCol = line.IndexOf(chars);
                    int partNum = 0;
                    int.TryParse(chars, out partNum);
                    parts.Add(new NumberPos(partNum, new GridPos(startingRow, startingCol)));
                    chars = "";
                }
            }
        }
        if (chars != "")
        {
            int startingRow = i;
            int startingCol = line.IndexOf(chars);
            int partNum = 0;
            int.TryParse(chars, out partNum);
            parts.Add(new NumberPos(partNum, new GridPos(startingRow, startingCol)));
            chars = "";
        }
    }
    foreach(NumberPos part in parts)
    {
        foreach (GridPos symbol in symbols)
        {
            if (part.SymbolIsAdjacent(symbol))
            {
                finalVal += part.val;
            }
        }
    }
    MessageBox.Show(finalVal.ToString());
}