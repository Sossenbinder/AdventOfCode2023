	using AdventOfCodeSupport;

	namespace AdventOfCode._2023;

	public class Day14 : AdventBase
	{
	    protected override object InternalPart1()
	    {
		    var field = Input.Lines.Select(x => x.ToCharArray()).ToArray();
		    
		    for (var x = 0; x < field[0].Length; ++x)
		    {
			    TiltIndex(x, field);
		    }

		    return Enumerable.Range(1, field.Length).Reverse().Select((
			    score,
			    index) => field[index].Count(x => x == 'O') * score).Sum();
	    }

	    protected override object InternalPart2()
	    {
		    var field = Input.Lines.Select(x => x.ToCharArray()).ToArray();

		    var cycleMap = new Dictionary<int, int>();

		    var cycleCount = 1000000000;

		    var cycleFound = false;
		    for (var currentCycle = 0; currentCycle < cycleCount; currentCycle++)
		    {
			    for (var i = 0; i < 4; ++i)
			    {
				    for (var x = 0; x < field[0].Length; ++x)
				    {
					    TiltIndex(x, field);
				    }
				    field = RotateClockwise(field);
			    }

			    if (cycleFound)
			    {
				    continue;
			    }

			    var hashedField = string.Join('_', field.Select(x => new string(x))).GetHashCode();
			    if (cycleMap.TryGetValue(hashedField, out var firstOccurenceCycle))
			    {
				    var countForCycleRepetition = currentCycle - firstOccurenceCycle;
				    var transformedCount = currentCycle + cycleCount;
				    
				    while (transformedCount > cycleCount)
				    {
					    transformedCount -= countForCycleRepetition;
				    }

				    currentCycle = transformedCount + 1;
				    cycleFound = true;
			    }
			    else
			    {
				    cycleMap[hashedField] = currentCycle;
			    }
		    }
		    
		    return Enumerable.Range(1, field.Length).Reverse().Select((
			    score,
			    index) => field[index].Count(x => x == 'O') * score).Sum();
	    }
	    
	    private void PrintField(char[][] field)
	    {
		    Console.WriteLine("-----");
		    for (var y = 0; y < field.Length; ++y)
		    {
			    Console.WriteLine(field[y].Select(x => x != '\0' ? x  : '.').ToArray());
		    }
	    }

	    private char[][] RotateClockwise(char[][] field)
	    {
		    var newField = new char[field[0].Length][];
		    
		    for (var x = 0; x < field[0].Length; ++x)
		    {
			    var xRow = new char[field.Length];

			    for (var y = field.Length - 1; y >= 0; --y)
			    {
				    xRow[field.Length - 1 - y] = field[y][x];
			    }

			    newField[x] = xRow;
		    }

		    return newField;
	    }

	    private static void TiltIndex(int x, char[][] field)
	    {
		    var slideUpIndex = 0;
		    for (var y = 0; y < field.Length; ++y)
		    {
			    var symbol = field[y][x];
			    switch (symbol)
			    {
				    case 'O':
					    if (field[slideUpIndex][x] != 'O')
					    {
						    field[slideUpIndex][x] = 'O';
						    field[y][x] = '.';
					    }
					    slideUpIndex++;
					    break;
				    case '#':
					    slideUpIndex = y + 1;
					    break;
			    }
		    }
	    }
	}