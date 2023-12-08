public class Card : IComparable<Card>
{
    public int num;
    public string data;
    public int copies;
    
    public Card(int num, string data)
    {
        this.num = num;
        this.data = data;
        copies = 1;
    }
    
    public int CompareTo(Card other)
    {
        return num.CompareTo(other.num);
    }
    
    public override bool Equals(object obj)
    {
        if (obj == null) return false;
        Card objAsCard = obj as Card;
        if (objAsCard == null) return false;
        return objAsCard.num == num;
    }
}

private void AdventDay4()
{
    var rawFile = File.ReadAllLines(@"AdventDay4Input.txt");
    List<string> input = new List<string>(rawFile);
    int cardsCounted = 0;
    int finalVal = 0;
    List<Card> cards = new List<Card>();
    foreach (string card in input)
    {
        string[] split = card.Split(':');
        string[] cardAndNum = split[0].Split(' ');
        int cardNum = 0;
        int.TryParse(cardAndNum[cardAndNum.Length - 1], out cardNum);
        cards.Add(new Card(cardNum, split[1]));
    }
    for (int j = 0; j < cards.Count; j++)
    {
        string[] numberSets = cards[j].data.Split('|');
        string[] winningStrings = numberSets[0].Split(' ');
        string[] cardStrings = numberSets[1].Split(' ');
        List<int> winningNumbers = new List<int>();
        List<int> cardNumbers = new List<int>();
        for (int i = 1; i < winningStrings.Length; i++)
        {
            if (winningStrings[i] == "")
                continue;
            int temp = 0;
            int.TryParse(winningStrings[i], out temp);
            winningNumbers.Add(temp);
        }
        for (int i = 1; i < cardStrings.Length; i++)
        {
            if (cardStrings[i] == "")
                continue;
            int temp = 0;
            int.TryParse(cardStrings[i], out temp);
            cardNumbers.Add(temp);
        }
        int runningTally = 0;
        foreach (int num in cardNumbers)
        {
            if (winningNumbers.Contains(num))
            {
                runningTally++;
            }
        }
        for (int i = 1; i <= runningTally && (cards[j].num + i - 1) < input.Count; i++)
        {
            cards[j + i].copies += cards[j].copies;
        }
        cardsCounted += cards[j].copies;
    }
    finalVal = cardsCounted;
    MessageBox.Show(finalVal.ToString());
}