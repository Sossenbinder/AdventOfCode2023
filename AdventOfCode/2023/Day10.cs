	using AdventOfCodeSupport;

	namespace AdventOfCode._2023;

	public class Day10 : AdventBase
	{
		private char[][] Field = default!;

		private HashSet<char> _validPipeSymbols = ['|', '-', 'L', 'J', '7', 'F', 'S'];
		
		protected override object InternalPart1()
	    {
		    Field = Input.Lines.Select(x => x.ToCharArray()).ToArray();
		    var minPointMap = Solve();
		    return minPointMap.Values.Max();
	    }

		protected override object InternalPart2()
		{
			return ValidPipeSymbolApproach();
		}

		private int ValidPipeSymbolApproach()
		{
			Field = Input.Lines.Select(x => x.ToCharArray()).ToArray();
			var minPointMap = Solve();
		    
			// Build an empty field first
			var field = new char[Field.Length][];
			for (var y = 0; y < Field.Length; ++y)
			{
				field[y] = new char[Field[0].Length];
			}

			field = FloodFillOutside();
		    
			// Now, mark all the known spots of the main pipe we just traversed. This will avoid the "junk" pipes
			foreach (var point in minPointMap.Keys)
			{
				field[point.Y][point.X] = Field[point.Y][point.X];
			}
			
			PrintField(field);
			
			for (var y = 0; y < field.Length; y++)
			{
				var withinMainPipe = false;
				for (var x = 0; x < field[0].Length; x++)
				{
					var cell = field[y][x];
					if (cell == '\0')
					{
						if (withinMainPipe)
						{
							field[y][x] = 'M';
						}
					}
					else if (cell is '|' or 'F' or '7')
					{
						withinMainPipe = !withinMainPipe;
					}
				}
			}
			
			PrintField(field);
			
			return field.Sum(x => x.Count(y => y == 'M'));
			
			for (var y = 0; y < field.Length; y++)
			{
				var opened = false;
				bool comingFromLeft = false;
				for (var x = 0; x < field[y].Length; x++)
				{
					var cellChar = field[y][x];
					if (_validPipeSymbols.Contains(cellChar))
					{
						if (cellChar == '|')
						{
							opened = !opened;
							comingFromLeft = !comingFromLeft;
						}
						else if (cellChar == 'L')
						{
							comingFromLeft = true;
						}
						else if (cellChar == 'J')
						{
							opened = !comingFromLeft;
						}
						else if (cellChar == '7')
						{
							opened = comingFromLeft;
						}
						else if (cellChar == 'F')
						{
							comingFromLeft = false;
						}
						
						continue;
					}
			
					if (!opened || cellChar != '\0' || cellChar == '~')
					{
						continue;
					}
			
					field[y][x] = 'M';
				}
			}

			PrintField(field);
			
			
			/*for (var x = 0; x < field[0].Length; x++)
			{
				var opened = false;
				bool comingFromLeft = false;
				
				for (var y = 0; y < field.Length; y++)
				{
					var cellChar = field[y][x];
					if (_validPipeSymbols.Contains(cellChar))
					{
						if (cellChar == '-')
						{
							opened = !opened;
							comingFromLeft = !comingFromLeft;
						}
						else if (cellChar == '7')
						{
							comingFromLeft = true;
						}
						else if (cellChar == 'F')
						{
							opened = !comingFromLeft;
						}
						else if (cellChar == 'L')
						{
							opened = comingFromLeft;
						}
						else if (cellChar == 'J')
						{
							comingFromLeft = false;
						}
						
						continue;
					}
			
					if (!opened || cellChar != '\0' || cellChar == '~')
					{
						continue;
					}
			
					field[y][x] = 'M';
				}
			}
			
			PrintField(field);*/
			
			return field.Sum(x => x.Count(y => y == 'M'));
			
			//PrintField(field);
			
			for (var y = 0; y < field.Length; y++)
			{
				var opened = false;
				for (var x = 0; x < field[y].Length; x++)
				{
					var cellChar = field[y][x];
					var specialOpenCloseCondition = CheckSpecialOpenClose(cellChar, true);
					if (specialOpenCloseCondition != Operation.NotSpecial)
					{
						if (specialOpenCloseCondition == Operation.Flip)
						{
							opened = !opened;
							continue;
						}
						
						opened = specialOpenCloseCondition == Operation.On;
						continue;
					}
			
					if (!opened || cellChar != '\0' || cellChar == '~')
					{
						continue;
					}
			
					field[y][x] = 'M';
				}
			}
			
			PrintField(field);

			// for (var x = 0; x < field[0].Length; x++)
			// {
			// 	var opened = false;			
			// 	for (var y = 0; y < field.Length; y++)
			// 	{
			// 		var cellChar = field[y][x];
			// 		var specialOpenCloseCondition = CheckSpecialOpenClose(cellChar, false);
			// 		if (specialOpenCloseCondition != Operation.NotSpecial)
			// 		{
			// 			if (specialOpenCloseCondition == Operation.Flip)
			// 			{
			// 				opened = !opened;
			// 				continue;
			// 			}
			// 			
			// 			opened = specialOpenCloseCondition == Operation.On;
			// 			continue;
			// 		}
			//
			// 		if (!opened || cellChar != 'M')
			// 		{
			// 			if (cellChar == 'M')
			// 			{
			// 				field[y][x] = '\0';
			// 			}
			// 			
			// 			continue;
			// 		}
			//
			// 		field[y][x] = 'G';
			// 	}
			// }
			
			//PrintField(field);

			return field.Sum(x => x.Count(y => y == 'M'));
		}

		private Operation CheckSpecialOpenClose(char chr, bool fromLeft = true)
		{
			if (!_validPipeSymbols.Contains(chr))
			{
				return Operation.NotSpecial;
			}
			
			/*if ((chr == '|' && fromLeft) || (chr == '-' && !fromLeft))
			{
				return Operation.Flip;
			}*/
			
			if (fromLeft)
			{
				return chr is 'F' or 'J' or '|' ? Operation.On : Operation.Off;
			}
			
			if (!fromLeft)
			{
				return chr is 'L' or '7' ? Operation.On : Operation.Off;
			}
			
			return Operation.NotSpecial;
		}

		private enum Operation
		{
			NotSpecial,
			On,
			Off,
			Flip,
		}

		private char[][] FloodFillOutside()
		{
			Field = Input.Lines.Select(x => x.ToCharArray()).ToArray();
			var minPointMap = Solve();
		    
			// Build an empty field first
			var flowField = new char[Field.Length][];
			for (var y = 0; y < Field.Length; ++y)
			{
				flowField[y] = new char[Field[0].Length];
			}
		    
			// Now, mark all the known spots of the main pipe we just traversed. This will avoid the "junk" pipes
			foreach (var point in minPointMap.Keys)
			{
				flowField[point.Y][point.X] = 'X';
			}
		    
			// Now, we have an "island", and we will have "water" flow in from all potential corner spots

			// Grab all the corner points first (Very lazy algorithm, just grab all the points and remove the non-corner ones)
			var openPoints = new HashSet<Point>();
			for (var y = 0; y < flowField.Length; ++y)
			{
				for (var x = 0; x < flowField[0].Length; ++x)
				{
					openPoints.Add(new Point(y, x));
				}
			}

			//PrintField(flowField);

			// Flow in "water" from the outside
			var openQueue = new Queue<Point>(openPoints.Where(x => x.X == 0 || x.X == Field[0].Length - 1 || x.Y == 0 || x.Y == Field.Length - 1));
			while (openQueue.Count != 0)
			{
				var point = openQueue.Dequeue();

				foreach (var validNewPoint in GetValidAdjacentPoints(point, flowField))
				{
					flowField[validNewPoint.Y][validNewPoint.X] = '~';
					openQueue.Enqueue(validNewPoint);
				}
			}
			
			
			//PrintField(flowField);

			return flowField;
		}

		private void PrintField(char[][] field)
		{
			Console.WriteLine("-----");
			for (var y = 0; y < field.Length; ++y)
			{
				Console.WriteLine(field[y].Select(x => x != '\0' ? x  : '.').ToArray());
			}
		}

		private List<Point> GetValidAdjacentPoints(Point currentPoint, char[][] flowField)
		{
			List<Point> adjacentPoints = [PointGenerator(yTransform: -1), PointGenerator(yTransform: 1), PointGenerator(xTransform: -1), PointGenerator(xTransform: 1)];
			
			return adjacentPoints
				.Where(x => x.X >= 0 && x.X < Field[0].Length && x.Y >= 0 && x.Y < Field.Length)
				.Where(x => flowField[x.Y][x.X] != 'X' && flowField[x.Y][x.X] != '~')
				.ToList();

			Point PointGenerator(
				int yTransform = 0,
				int xTransform = 0) => currentPoint with
			{
				Y = currentPoint.Y + yTransform,
				X = currentPoint.X + xTransform,
				CurrentDistance = currentPoint.CurrentDistance + 1,
			};
		}

		private Dictionary<Point, int> Solve()
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

			return minPointMap;
		}

		private void ReplaceStartWithActualSymbol(Point startingPoint)
		{
			foreach (var potentialSymbol in _validPipeSymbols)
			{
				Field[startingPoint.Y][startingPoint.X] = potentialSymbol;
				var nextSymbols = GetPotentialNextPoints(startingPoint);

				if (nextSymbols.All(x => Field[x.Y][x.X] != '.'))
				{
					return;
				}
			}
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

		    return ((List<Point>)(symbol switch
		    {
			    '|' => [PointGenerator(yTransform: -1), PointGenerator(yTransform: +1) ],
			    '-' => [PointGenerator(xTransform: -1), PointGenerator(xTransform: +1) ],
			    'L' => [PointGenerator(yTransform: -1), PointGenerator(xTransform: +1) ],
			    'J' => [PointGenerator(yTransform: -1), PointGenerator(xTransform: -1)],
			    '7' => [PointGenerator(xTransform: -1), PointGenerator(yTransform: +1)],
			    'F' => [PointGenerator(xTransform: +1), PointGenerator(yTransform: +1)],
			    _ => [],
		    })).Where(x => x.X >= 0 && x.X < Field[0].Length && x.Y >= 0 && x.Y < Field.Length).ToHashSet();

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