	using AdventOfCodeSupport;

	namespace AdventOfCode._2023;

	public class Day11 : AdventBase
	{
	    protected override object InternalPart1()
	    {
		    var original = Input.Lines.Select(x => x.ToCharArray()).ToArray();
		    var (xExpansions, yExpansions) = Expand(original);
		    
		    var galaxyPoints = original
			    .Select((row, y) => row.Select((_, x) => new Point(x, y)))
			    .SelectMany(x => x)
			    .Where(x => original[x.Y][x.X] == '#')
			    .ToList();

		    long totalDistance = 0;
		    for (var i = 0; i < galaxyPoints.Count; i++)
		    {
			    var currentGalaxyPoint = galaxyPoints[i];
			    
			    var xExtension = xExpansions.Count(x => x < currentGalaxyPoint.X);
			    var yExtension = yExpansions.Count(y => y < currentGalaxyPoint.Y);
			    currentGalaxyPoint.X += xExtension;
			    currentGalaxyPoint.Y += yExtension;
			    
			    var targets = galaxyPoints[(i+ 1)..];

			    totalDistance += targets.Sum(x => FindManhattanDistance(currentGalaxyPoint, x, xExpansions, yExpansions));
		    }

		    return totalDistance;
	    }

	    protected override object InternalPart2()
	    {
		    var scale = 1_000_000 - 1;
		    
		    var original = Input.Lines.Select(x => x.ToCharArray()).ToArray();
		    var (xExpansions, yExpansions) = Expand(original);
		    
		    var galaxyPoints = original
			    .Select((row, y) => row.Select((_, x) => new Point(x, y)))
			    .SelectMany(x => x)
			    .Where(x => original[x.Y][x.X] == '#')
			    .ToList();

		    long totalDistance = 0;
		    for (var i = 0; i < galaxyPoints.Count; i++)
		    {
			    var currentGalaxyPoint = galaxyPoints[i];
			    
			    var xExtension = xExpansions.Count(x => x < currentGalaxyPoint.X);
			    var yExtension = yExpansions.Count(y => y < currentGalaxyPoint.Y);
			    currentGalaxyPoint.X += xExtension * scale;
			    currentGalaxyPoint.Y += yExtension * scale;
			    
			    var targets = galaxyPoints[(i+ 1)..];

			    totalDistance += targets.Sum(x => FindManhattanDistance(currentGalaxyPoint, x, xExpansions, yExpansions, scale));
		    }

		    return totalDistance;
	    }

	    private long FindManhattanDistance(
		    Point start,
		    Point end,
		    HashSet<long> xExpansions,
		    HashSet<long> yExpansions,
		    int scale = 1)
	    {
		    var xExtension = xExpansions.Count(x => x < end.X);
		    var yExtension = yExpansions.Count(y => y < end.Y);
		    
		    return Math.Abs(end.X + (xExtension * scale) - start.X) + Math.Abs(end.Y + (yExtension * scale) - start.Y);
	    }
	    
	    private static void PrintArr(IReadOnlyList<char[]> field)
	    {
		    Console.WriteLine("-----");
		    foreach (var t in field)
		    {
			    Console.WriteLine(t.Select(x => x != '\0' ? x  : '.').ToArray());
		    }
	    }

	    private (HashSet<long> XExpansions, HashSet<long> YExpansions) Expand(char[][] original)
	    {
		    var expandXs = new HashSet<long>();
		    for (var x = original[0].Length - 1; x > 0; --x)
		    {
			    if (original.All(row => row[x] == '.'))
			    {
				    expandXs.Add(x);
			    }
		    }
		    var expandYs = new HashSet<long>();
		    for (var y = 0; y < original.Length; ++y)
		    {
			    if (original[y].All(item => item == '.'))
			    {
				    expandYs.Add(y);
			    }
		    }

		    return (expandXs, expandYs);
	    }

	    record struct Point(
		    int X,
		    int Y);
	}