	using AdventOfCodeSupport;

	namespace AdventOfCode._2023;

	public class Day10 : AdventBase
	{
		private char[][] Field = default!;

		protected override void InternalOnLoad()
		{
			Field = Input.Lines.Select(x => x.ToCharArray()).ToArray();
		}

		protected override object InternalPart1()
	    {
		    var startingPoint = FindStartingPosition(Field);

		    ReplaceStartWithActualSymbol(startingPoint);

		    var minPointMap = new Dictionary<Point, int>
		    {
			    [startingPoint] = 0
		    };

		    var openPoints = new Queue<Point>();
		    openPoints.Enqueue(startingPoint);
		    
		    while (openPoints.Count != 0)
		    {
			    var currentPoint = openPoints.Dequeue();
			    var nextPoints = GetPotentialNextPoints(currentPoint);
			    var eligibleNextPoints = nextPoints.Where(x => !minPointMap.TryGetValue(x, out var pt) ||  x.CurrentDistance < pt);

			    foreach (var pt in eligibleNextPoints)
			    {
				    minPointMap[pt] = pt.CurrentDistance;
				    openPoints.Enqueue(pt);
			    }
		    }
		    
		    return minPointMap.Values.Max();
	    }

		private void ReplaceStartWithActualSymbol(Point startingPoint)
		{
			foreach (var potentialSymbol in (char[])['|', '-', 'L', 'J', '7', 'F'])
			{
				Field[startingPoint.Y][startingPoint.X] = potentialSymbol;
				var nextSymbols = GetPotentialNextPoints(startingPoint);

				if (nextSymbols.All(x => Field[x.Y][x.X] != '.'))
				{
					return;
				}
			}
		}

		protected override object InternalPart2()
	    {
		    return 0;
	    }

	    private static Point FindStartingPosition(IReadOnlyList<char[]> field)
	    {
		    for (var y = 0; y < field.Count; ++y)
		    {
			    for (var x = 0; x < field[y].Length; ++x)
			    {
				    if (field[y][x] == 'S')
				    {
					    return new Point(y, x);
				    }
			    }
		    }

		    return new Point(0, 0);
	    }

	    private HashSet<Point> GetPotentialNextPoints(Point currentPoint)
	    {
		    var symbol = Field[currentPoint.Y][currentPoint.X];

		    return symbol switch
		    {
			    '|' => [PointGenerator(yTransform: -1), PointGenerator(yTransform: +1) ],
			    '-' => [PointGenerator(xTransform: -1), PointGenerator(xTransform: +1) ],
			    'L' => [PointGenerator(yTransform: -1), PointGenerator(xTransform: +1) ],
			    'J' => [PointGenerator(yTransform: -1), PointGenerator(xTransform: -1)],
			    '7' => [PointGenerator(xTransform: -1), PointGenerator(yTransform: +1)],
			    'F' => [PointGenerator(xTransform: +1), PointGenerator(yTransform: +1)],
			    _ => [],
		    };

		    Point PointGenerator(
			    int yTransform = 0,
			    int xTransform = 0) => currentPoint with
		    {
			    Y = currentPoint.Y + yTransform,
			    X = currentPoint.X + xTransform,
			    CurrentDistance = currentPoint.CurrentDistance + 1,
		    };
	    }

	    public record Point(
		    int Y,
		    int X)
	    {
		    public int CurrentDistance { get; init; }

		    public virtual bool Equals(Point? other)
		    {
			    if (ReferenceEquals(null, other))
			    {
				    return false;
			    }

			    if (ReferenceEquals(this, other))
			    {
				    return true;
			    }

			    return Y == other.Y && X == other.X;
		    }

		    public override int GetHashCode()
		    {
			    return HashCode.Combine(Y, X);
		    }
	    }
	}