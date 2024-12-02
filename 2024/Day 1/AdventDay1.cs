private void AdventDay1()
{
  //Input contains two lists of numbers. 
  var rawFile = File.ReadAllLines(@"Input.txt");
  List<int> firstList = new List<int>();
  List<int> secondList = new List<int>();
  foreach (string line in rawFile)
  {
      string[] split = line.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
      firstList.Add(int.Parse(split[0]));
      secondList.Add(int.Parse(split[1]));
  }
  if (firstList.Count != secondList.Count)
  {
      MessageBox.Show("Lists are not the same length. Cannot continue.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
      return;
  }
  //Part 1
  //Sort each list, take the absolute value of their difference, add the values together.
  firstList.Sort();
  secondList.Sort();
  int total = 0;
  for (int i = 0; i < firstList.Count; i++)
  {
      total += Math.Abs(firstList[i] - secondList[i]);
  }
  MessageBox.Show("Total: " + total, "Total", MessageBoxButton.OK, MessageBoxImage.Information);

  //Part 2
  //For each number in the first list, count the number of times it appears in the second list. Multiply the number and count together, then add all the results together.
  int runningTotal = 0;
  foreach (int i in firstList)
  {
      int secondCount = secondList.Count(x => x == i);
      runningTotal += (i * secondCount);
  }
  MessageBox.Show("Total: " + runningTotal, "Total", MessageBoxButton.OK, MessageBoxImage.Information);
}