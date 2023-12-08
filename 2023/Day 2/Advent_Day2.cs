struct game
{
    public int m_id;
    public int m_redCubes;
    public int m_greenCubes;
    public int m_blueCubes;

    public game(int id, int rc, int gc, int bc)
    {
        m_id = id;
        m_redCubes = rc;
        m_greenCubes = gc;
        m_blueCubes = bc;
    }

    public int GameIsPossibleValue(game input)
    {
        if (m_redCubes > input.m_redCubes || m_greenCubes > input.m_greenCubes || m_blueCubes > input.m_blueCubes)
        {
            //game is not possible
            return 0;
        }
        else
        {
            //game is possible
            return m_id;
        }
    }

    public int GamePower()
    {
        return m_redCubes * m_greenCubes * m_blueCubes;
    }
}

private void AdventDay2()
{
    var rawFile = File.ReadAllLines(@"AdventDay2Input.txt");
    List<string> input = new List<string>(rawFile);
    List<game> inputValue = new List<game>();
    int finalVal = 0;
    game inputConfig = new game(0, 12, 13, 14);
    foreach (string str in input)
    {
        string[] delimiters = { ":", ": ", ";"};
        string[] splitOutput = str.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
        //splitOutput[0] contains "Game #"; remaining contain the separated games
        //Split output further to get the game ID from splitOutput[0]
        string[] gameIDVals = splitOutput[0].Split(' ');
        int id = 0;
        int.TryParse(gameIDVals[1], out id);
        int validGameOutput = 0;
        int red = 0, green = 0, blue = 0;
        for (int i = 1; i < splitOutput.Length; i++)
        {
            validGameOutput = 0;
            string[] delimiterString = { ", " };
            string[] cubeSets = splitOutput[i].Split(delimiterString, StringSplitOptions.RemoveEmptyEntries);
            foreach(string str2 in cubeSets)
            {
                char[] separator = { ' ' };
                string[] cubes = str2.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                int temp = 0;
                int.TryParse(cubes[0], out temp);
                switch (cubes[1])
                {
                    case "red":
                        {
                            if( temp > red)
                                red = temp;
                            break;
                        }
                    case "green":
                        {
                            if (temp > green)
                                green = temp;
                            break;
                        }
                    case "blue":
                        {
                            if (temp > blue)
                                blue = temp;
                            break;
                        }
                    default:
                        MessageBox.Show(cubes[1]);
                        break;
                }
            }
            game inputGame = new game(id, red, green, blue);
            validGameOutput = inputGame.GamePower();
        }
        finalVal += validGameOutput;
    }
    MessageBox.Show(finalVal.ToString());
}