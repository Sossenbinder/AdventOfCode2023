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
				    if (IsPerfectReflectionP1(verticalLines, y))
				    {
					    localResult += y * 100;
				    }
			    }
			    
			    for (var x = 1; x < lines[0].Length; ++x)
			    {
				    if (IsPerfectReflectionP1(lines, x))
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
		    var blocks = Input.Blocks;

		    var reflectionSum = 0;
		    foreach (var block in blocks)
		    {
			    var lines = block.Lines;

			    var localResult = 0;
		    
			    var verticalLines = Enumerable.Range(0, lines[0].Length).Select(index => new string(lines.Select(x => x[index]).ToArray())).ToList();
			    for (var y = 1; y < lines.Length; ++y)
			    {
				    if (IsPerfectReflectionP2(verticalLines, y))
				    {
					    localResult += y * 100;
				    }
			    }
			    
			    for (var x = 1; x < lines[0].Length; ++x)
			    {
				    if (IsPerfectReflectionP2(lines, x))
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

	    private bool IsPerfectReflectionP1(IReadOnlyCollection<string> lines, int index)
	    {
		    return lines.All(line =>
		    {
			    var upperVertical = line[..index];
			    var lowerVertical = line[index..];

			    return IsPerfectReflection(upperVertical, lowerVertical);
		    });
	    }
	    
	    private bool IsPerfectReflectionP2(IReadOnlyList<string> lines, int index)
	    {
		    var smudgeAccountedCount = lines.Count - 1;
		    var reflectionOutcomes = lines.Select((line, i) =>
		    {
			    var upperVertical = line[..index];
			    var lowerVertical = line[index..];

			    return (i, IsPerfectReflection(upperVertical, lowerVertical));
		    }).ToList();

		    if (reflectionOutcomes.Count(x => x.Item2) != smudgeAccountedCount)
		    {
			    return false;
		    }

		    var smudgedLineIndex = reflectionOutcomes.Single(x => !x.Item2).i;
		    var smudgedLine = lines[smudgedLineIndex];

		    for (var i = 0; i < smudgedLine.Length; ++i)
		    {
			    var smudgedArray = smudgedLine.ToCharArray();

			    smudgedArray[i] = '.';

			    if (IsPerfectReflectionP1(new List<string>() {new string(smudgedArray)}, index))
			    {
				    return true;
			    }
			    
			    smudgedArray[i] = '#';

			    if (IsPerfectReflectionP1(new List<string>() {new string(smudgedArray)}, index))
			    {
				    return true;
			    }
		    }
		    
		    return false;
	    }

	    private static bool IsPerfectReflection(
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