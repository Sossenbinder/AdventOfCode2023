	using AdventOfCodeSupport;

	namespace AdventOfCode._2023;

	public class Day09 : AdventBase
	{
	    protected override object InternalPart1()
	    {
		    return Input.Lines
			    .Select(line => CalculateRowContinuation(line.Split(' ').Select(int.Parse).ToArray()))
			    .Sum();
	    }

	    protected override object InternalPart2()
	    {
		    return Input.Lines
			    .Select(line => CalculateRowContinuation(line.Split(' ').Select(int.Parse).Reverse().ToArray()))
			    .Sum();
	    }

	    private static int CalculateRowContinuation(IReadOnlyList<int> line)
	    {
		    if (line.Sum() == 0)
		    {
			    return 0;
		    }

		    var deltaLine = new int[line.Count - 1];

		    for (var i = 0; i < line.Count - 1; ++i)
		    {
			    deltaLine[i] = line[i + 1] - line[i];
		    }

		    return line[^1] + CalculateRowContinuation(deltaLine);
	    }
	}