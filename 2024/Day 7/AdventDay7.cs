class TreeNode
{
    public TreeNode Left;
    public TreeNode Center;
    public TreeNode Right;
    public long value;

    public TreeNode(long value)
    {
        this.value = value;
        Left = null;
        Center = null;
        Right = null;
    }
}

class EquationTree
{
    public long finalAnswer;
    List<long> operands;
    TreeNode root;
    public bool HasThreeBranches = false;

    public EquationTree(string equation)
    {
        string[] splitAnswer = equation.Split(':');
        finalAnswer = long.Parse(splitAnswer[0]);

        string[] splitOperands = splitAnswer[1].Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
        operands = new List<long>();
        foreach (string operand in splitOperands)
        {
            operands.Add(long.Parse(operand));
        }
        root = new TreeNode(operands[0]);
        //CreateTree();
        CreateThreeBranchTree();
    }

    private void CreateTree()
    {
        AddLeftAndRight(root, 1);
        HasThreeBranches = false;
    }

    private void CreateThreeBranchTree()
    {
        AddLeftRightAndCenter(root, 1);
        HasThreeBranches = true;
    }

    private void AddLeftAndRight(TreeNode node, int index)
    {
        if (index >= operands.Count)
        {
            return;
        }
        node.Left = new TreeNode(node.value + operands[index]);
        node.Right = new TreeNode(node.value * operands[index]);
        AddLeftAndRight(node.Left, index + 1);
        AddLeftAndRight(node.Right, index + 1);
    }

    private void AddLeftRightAndCenter(TreeNode node, int index)
    {
        if (index >= operands.Count)
        {
            return;
        }
        node.Left = new TreeNode(node.value + operands[index]);
        node.Right = new TreeNode(node.value * operands[index]);
        node.Center = new TreeNode(long.Parse(node.value.ToString() + operands[index].ToString()));
        AddLeftRightAndCenter(node.Left, index + 1);
        AddLeftRightAndCenter(node.Right, index + 1);
        AddLeftRightAndCenter(node.Center, index + 1);
    }

    public int CountValidAnswers()
    {
        int count = 0;
        TraverseToDepthAndValidateFinalAnswer(root, ref count);
        return count;
    }

    private void TraverseToDepthAndValidateFinalAnswer(TreeNode root, ref int count)
    {
        if (root.Left == null || root.Right == null)
        {
            if (root.value == finalAnswer)
            {
                count++;
            }
            return;
        }
        TraverseToDepthAndValidateFinalAnswer(root.Left, ref count);
        TraverseToDepthAndValidateFinalAnswer(root.Right, ref count);
    }

    public int CountValidThreeBranchAnswers()
    {
        int count = 0;
        TraverseThreeBranchesToDepthAndValidateFinalAnswer(root, ref count);
        return count;
    }

    private void TraverseThreeBranchesToDepthAndValidateFinalAnswer(TreeNode root, ref int count)
    {
        if (root.Left == null || root.Right == null || root.Center == null)
        {
            if (root.value == finalAnswer)
            {
                count++;
            }
            return;
        }
        TraverseThreeBranchesToDepthAndValidateFinalAnswer(root.Left, ref count);
        TraverseThreeBranchesToDepthAndValidateFinalAnswer(root.Right, ref count);
        TraverseThreeBranchesToDepthAndValidateFinalAnswer(root.Center, ref count);
    }
}

private void AdventDay7(object sender, MouseButtonEventArgs e)
{
    var rawFile = File.ReadAllLines(@"Input.txt");
    //Input is a series of n lines with a final answer, followed by a series of operands separated by spaces.
    List<EquationTree> equations = new List<EquationTree>();
    foreach (string line in rawFile)
    {
        equations.Add(new EquationTree(line));
    }
    long calibrationResult = 0;
    foreach (EquationTree tree in equations)
    {
        int solutionCount = 0;
        if (tree.HasThreeBranches)
        {
            solutionCount = tree.CountValidThreeBranchAnswers();
        }
        else
        {
            solutionCount = tree.CountValidAnswers();
        }
        if (solutionCount > 0)
        {
            calibrationResult += tree.finalAnswer;
        }
    }
    MessageBox.Show("Calibration result: " + calibrationResult.ToString());
}