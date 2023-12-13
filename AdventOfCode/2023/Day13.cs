	using AdventOfCodeSupport;

	namespace AdventOfCode._2023;

	public class Day13 : AdventBase
	{
	    protected override object InternalPart1()
	    {
		    var blocks = Input.Blocks;

		    var reflectionSum = 0;
		    foreach (var block in blocks)
		    {
			    var lines = block.Lines;

			    var localResult = 0;
		    
			    var verticalLines = Enumerable.Range(0, lines[0].Length).Select(index => new string(lines.Select(x => x[index]).ToArray())).ToList();
			    for (var y = 1; y < lines.Length; ++y)
			    {
				    var isPerfectReflection = verticalLines.All(verticalLine =>
				    {
					    var upperVertical = verticalLine[..y];
					    var lowerVertical = verticalLine[y..];

					    return IsPerfectReflection(upperVertical, lowerVertical);
				    });

				    if (isPerfectReflection)
				    {
					    localResult += y * 100;
				    }
			    }
			    
			    for (var x = 1; x < lines[0].Length; ++x)
			    {
				    var isPerfectReflection = lines.All(verticalLine =>
				    {
					    var upperVertical = verticalLine[..x];
					    var lowerVertical = verticalLine[x..];

					    return IsPerfectReflection(upperVertical, lowerVertical);
				    });

				    if (isPerfectReflection)
				    {
					    localResult += x;
				    }
			    }

			    if (localResult == 0)
			    {
				    throw new Exception("No reflection found");
			    }

			    reflectionSum += localResult;
		    }

		    return reflectionSum;
	    }

	    protected override object InternalPart2()
	    {
		    return 0;
	    }

	    private bool IsPerfectReflection(
		    string left,
		    string right)
	    {
		    var minDistance = Math.Min(left.Length, right.Length);

		    for (var i = 0; i < minDistance; ++i)
		    {
			    if (left[left.Length - 1 - i] != right[i])
			    {
				    return false;
			    }
		    }
		    
		    return true;
	    }
	}