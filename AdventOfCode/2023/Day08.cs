	using AdventOfCodeSupport;

	namespace AdventOfCode._2023;

	public class Day08 : AdventBase
	{
	    protected override object InternalPart1()
	    {
		    var directions = ParseDirections();
		    var nodeMap = ParseNodes();

		    return Solve(directions, nodeMap, "AAA");
	    }

	    protected override object InternalPart2()
	    {
		    var directions = ParseDirections();
		    var nodeMap = ParseNodes();

		    var currentNodes = nodeMap.Keys.Where(x => x.EndsWith("A")).ToArray();

		    var stepCount = currentNodes.Select(x => Solve(directions, nodeMap, x, true));

		    var lcm = Lcm(stepCount.ToArray());
		    
		    return lcm;
	    }

	    private static int Gcd(int a, int b)
	    {
		    while (b != 0)
		    {
			    var temp = b;
			    b = a % b;
			    a = temp;
		    }
		    return a;
	    }

	    private static int Lcm(IReadOnlyList<int> numbers)
	    {
		    var result = numbers[0];
		    for (var i = 1; i < numbers.Count; i++)
		    {
			    result = Lcm(result, numbers[i]);
		    }
		    return result;
	    }

	    private static int Lcm(int a, int b)
	    {
		    return a * (b / Gcd(a, b));
	    }

	    private int Solve(
		    Queue<Direction> directions,
		    Dictionary<string, Node> nodeMap, 
		    string currentNode,
		    bool partTwo = false)
	    {
		    directions = new Queue<Direction>(directions);
		    
		    var steps = 0;
		    while (true)
		    {
			    var currentDirection = directions.Dequeue();

			    var node = nodeMap[currentNode];
			    currentNode = currentDirection == Direction.Left ? node.Left : node.Right;
			    steps++;
			    
			    if (partTwo ? currentNode[2] == 'Z' : currentNode == "ZZZ")
			    {
				    return steps;
			    }

			    directions.Enqueue(currentDirection);
		    }

		    return 0;
	    }

	    private Queue<Direction> ParseDirections()
	    {
		    var directionQueue = new Queue<Direction>();

		    for (var i = 0; i < Input.Lines[0].Length; ++i)
		    {
			    directionQueue.Enqueue(Input.Lines[0][i] == 'L' ? Direction.Left : Direction.Right);
		    }
		    
		    return directionQueue;
	    }

	    private Dictionary<string, Node> ParseNodes()
	    {
		    var dict = new Dictionary<string, Node>();
		    
		    for (var i = 2; i < Input.Lines.Length; ++i)
		    {
			    var line = Input.Lines[i];
			    var index = line[0..3];

			    var left = line[7..10];
			    var right = line[12..15];

			    dict[index] = new Node(left, right);
		    }

		    return dict;
	    }

	    private record struct Node(
		    string Left,
		    string Right);

	    private enum Direction
	    {
		    Left,
		    Right
	    }
	}