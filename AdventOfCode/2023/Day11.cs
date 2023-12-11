	using AdventOfCodeSupport;

	namespace AdventOfCode._2023;

	public class Day11 : AdventBase
	{
	    protected override object InternalPart1()
	    {
		    var original = Input.Lines.Select(x => x.ToCharArray()).ToArray();
		    var expanded = Expand(original);
		    
		    var galaxyPoints = expanded
			    .Select((row, y) => row.Select((_, x) => new Point(x, y)))
			    .SelectMany(x => x)
			    .Where(x => expanded[x.Y][x.X] == '#')
			    .ToList();

		    var totalDistance = 0;
		    for (var i = 0; i < galaxyPoints.Count; i++)
		    {
			    var currentGalaxyPoint = galaxyPoints[i];
			    var targets = galaxyPoints[(i+ 1)..];

			    totalDistance += targets.Sum(x => FindManhattanDistance(currentGalaxyPoint, x));
		    }

		    return totalDistance;
	    }

	    protected override object InternalPart2()
	    {
		    return 0;
	    }

	    private int FindManhattanDistance(
		    Point start,
		    Point end)
	    {
		    return Math.Abs(end.X - start.X) + Math.Abs(end.Y - start.Y);
	    }
	    
	    private static void PrintArr(IReadOnlyList<char[]> field)
	    {
		    Console.WriteLine("-----");
		    foreach (var t in field)
		    {
			    Console.WriteLine(t.Select(x => x != '\0' ? x  : '.').ToArray());
		    }
	    }

	    private char[][] Expand(char[][] original)
	    {
		    var expandableList = new List<List<char>>();

		    var expandXs = new HashSet<int>();
		    for (var x = original[0].Length - 1; x > 0; --x)
		    {
			    if (original.All(row => row[x] == '.'))
			    {
				    expandXs.Add(x);
			    }
		    }
		    var expandYs = new HashSet<int>();
		    for (var y = 0; y < original.Length; ++y)
		    {
			    if (original[y].All(item => item == '.'))
			    {
				    expandYs.Add(y);
			    }
		    }

		    var newMaxLength = original[0].Length + expandXs.Count;

		    for (var y = 0; y < original.Length; ++y)
		    {
			    var row = original[y].ToList();

			    foreach (var expandX in expandXs)
			    {
				    row.Insert(expandX, '.');
			    }

			    if (expandYs.Contains(y))
			    {
				    expandableList.Add(new string('.', newMaxLength).ToList());
			    }
			    
			    expandableList.Add(row);
		    }


		    return expandableList.Select(x => x.ToArray()).ToArray();
	    }

	    record struct Point(
		    int X,
		    int Y);
	}