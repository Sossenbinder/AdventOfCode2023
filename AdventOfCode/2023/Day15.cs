	using AdventOfCodeSupport;
	using MoreLinq.Extensions;

	namespace AdventOfCode._2023;

	public class Day15 : AdventBase
	{
	    protected override object InternalPart1()
	    {
		    var input = Input.Text.Split(',');

		    return input.Sum(RunHash);
	    }

	    protected override object InternalPart2()
	    {
		    var input = Input.Text.Split(',');

		    var boxMap = new Dictionary<int, Queue<Lens>>();
		    foreach (var step in input)
		    {
			    var hasEquals = step.Contains('=');
			    var indexOfSymbol = step.IndexOf(hasEquals ? '=' : '-');
			    var label = step[..indexOfSymbol];
			    var boxId = RunHash(label);

			    if (!boxMap.TryGetValue(boxId, out var box))
			    {
				    box = new Queue<Lens>();
				    boxMap[boxId] = box;
			    }
			    
			    if (hasEquals)
			    {
				    var strength = int.Parse(step[(indexOfSymbol + 1)..]);
				    var knownLens = box.FirstOrDefault(x => x.Label == label);
				    if (knownLens is not null)
				    {
					    knownLens.Strength = strength;
					    continue;
				    }
				    
				    box.Enqueue(new Lens()
				    {
					    Label = label,
					    Strength = strength,
				    });
			    }
			    else
			    {
				    if (box.All(x => x.Label != label))
				    {
					    continue;
				    }

				    boxMap[boxId] = new Queue<Lens>(box.Where(x => x.Label != label));
			    }
		    }

		    var sum = boxMap.Sum(x =>
		    {
			    var boxSum = 1 + x.Key;
			    var slot = 1;

			    var total = 0;
			    while (x.Value.Count != 0)
			    {
				    var entry = x.Value.Dequeue();

				    total += boxSum * slot * entry.Strength;
				    
				    slot++;
			    }
			    
			    return total;
		    });
		    
		    return sum;
	    }

	    private static int RunHash(string instructionSet)
	    {
		    var currentValue = 0;
		    foreach (var character in instructionSet)
		    {
			    currentValue += character;
			    currentValue *= 17;
			    currentValue %= 256;
		    }

		    return currentValue;
	    }

	    public class Lens
	    {
		    public string Label { get; set; }

		    public int Strength { get; set; }
	    }
	}