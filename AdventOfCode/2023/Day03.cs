	using AdventOfCodeSupport;

	namespace AdventOfCode._2023;

	public class Day03 : AdventBase
	{
		private record Offset(int X, int Y);

		private readonly List<Offset> _offsets = new()
		{
			new Offset(-1, -1), new Offset(0, -1), new Offset(1, -1), new Offset(-1, 0), new Offset(1, 0),
			new Offset(-1, 1), new Offset(0, 1), new Offset(1, 1)
		};
		
	    protected override object InternalPart1()
	    {
		    var sum = 0;
		    for (var y = 0; y < Input.Lines.Length; ++y)
		    {
			    var line = Input.Lines[y];
			    for (var x = 0; x < line.Length; ++x)
			    {
				    if (!IsSymbol(line[x]))
				    {
					    continue;
				    }

				    sum += CalculatePartOneSum(x, y);
			    }
		    }

		    return sum;
	    }

	    protected override object InternalPart2()
	    {
		    var sum = 0;
		    for (var y = 0; y < Input.Lines.Length; ++y)
		    {
			    var line = Input.Lines[y];
			    for (var x = 0; x < line.Length; ++x)
			    {
				    if (line[x] != '*')
				    {
					    continue;
				    }

				    sum += CalculatePartTwoSum(x, y);
			    }
		    }

		    return sum;
	    }

	    private static bool IsSymbol(char chr) => chr != '.' && !char.IsDigit(chr);

	    private int CalculatePartOneSum(
		    int X,
		    int Y)
	    {
		    var processedPoints = new HashSet<(int X, int Y)>();
		    var processedNumbers = new List<int>();
		    foreach (var offset in _offsets)
		    {
			    var x = X + offset.X;
			    var y = Y + offset.Y;

			    if (y < 0 || y > Input.Lines.Length)
			    {
				    continue;
			    }
			    
			    if (x < 0 || x >= Input.Lines[y].Length)
			    {
				    continue;
			    }

			    if (!char.IsDigit(Input.Lines[y][x]))
			    {
				    continue;
			    }

			    var (nr, newProcessedPoints) = GetExpandedNumberAt(y, x);
			    if (newProcessedPoints.Any(pt => !processedPoints.Add(pt)))
			    {
				    continue;
			    }
			    
			    
			    processedNumbers.Add(nr);
		    }

		    return processedNumbers.Sum();
	    }
	    
	    private int CalculatePartTwoSum(
		    int X,
		    int Y)
	    {
		    var processedPoints = new HashSet<(int X, int Y)>();
		    var processedNumbers = new List<int>();
		    foreach (var offset in _offsets)
		    {
			    var x = X + offset.X;
			    var y = Y + offset.Y;

			    if (y < 0 || y > Input.Lines.Length)
			    {
				    continue;
			    }
			    
			    if (x < 0 || x >= Input.Lines[y].Length)
			    {
				    continue;
			    }

			    if (!char.IsDigit(Input.Lines[y][x]))
			    {
				    continue;
			    }

			    var (nr, newProcessedPoints) = GetExpandedNumberAt(y, x);
			    if (newProcessedPoints.Any(pt => !processedPoints.Add(pt)))
			    {
				    continue;
			    }
			    
			    
			    processedNumbers.Add(nr);
		    }

		    if (processedNumbers.Count != 2)
		    {
			    return 0;
		    }

		    return processedNumbers[0] * processedNumbers[1];
	    }

	    private (int Number, List<(int X, int Y)> ProcessedPoints) GetExpandedNumberAt(
		    int y,
		    int x)
	    {
		    var line = Input.Lines[y];
		    
		    var left = x;

		    while (left > 0)
		    {
			    if (!char.IsDigit(line[left - 1]))
			    {
				    break;
			    }
			    
			    left--;
		    }

		    var right = x;

		    while (right < line.Length - 1)
		    {
			    if (!char.IsDigit(line[right + 1]))
			    {
				    break;
			    }
			    right++;
		    }

		    var result = line[left..(right + 1)];

		    return (int.Parse(result), Enumerable.Range(left, (right + 1) - left).Select(x => (x, y)).ToList());
	    }
	}