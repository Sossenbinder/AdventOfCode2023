	using AdventOfCodeSupport;

	namespace AdventOfCode._2023;

	public class Day12 : AdventBase
	{
	    protected override object InternalPart1()
	    {
		    return Input.Lines.Sum(CalculatePermutations);
	    }

	    protected override object InternalPart2()
	    {
		    return 0;
	    }
	    
        private static int CalculatePermutations(string line)
        {
            var split = line.Split(' ');
            var field = split[0];
            var instructions = split[1].Split(',').Select(int.Parse).ToArray();

            var combinations = CalculatePermutations(field, instructions);
            return combinations.Count;
        }


        private static List<string> CalculatePermutations(string template, int[] instructions, int templateIndex = 0, int instructionIndex = 0)
        {
            if (templateIndex >= template.Length)
            {
	            var hashGroups = FindHashGroupsInString(template).ToArray();
                return instructionIndex == instructions.Length && instructions.Select((x, index) => hashGroups[index] == x).All(x => x) ? [template] : [];
            }

            var startChar = template[templateIndex];
            var movedIndex = templateIndex + 1;

            if (startChar == '?')
            {
	            var result = CalculatePermutations(template[..templateIndex] + "#" + template[(templateIndex + 1)..], instructions, templateIndex, instructionIndex);
	            result.AddRange(CalculatePermutations(template[..templateIndex] + "." + template[(templateIndex + 1)..], instructions, templateIndex, instructionIndex));
	            return result;
            }

            if (startChar == '.')
            {
	            return CalculatePermutations(template, instructions, movedIndex, instructionIndex);
            }

            var rightwardIndex = FindLastOccurrenceForwardSearch(template, '#', templateIndex, template.Length);

            if (rightwardIndex == -1)
            {
	            rightwardIndex = templateIndex;
            }

            var leftwardIndex = FindFirstOccurrenceBackwardSearch(template, '#', templateIndex);

            if (leftwardIndex == -1)
            {
	            leftwardIndex = templateIndex;
            }

            var contiguousLength = rightwardIndex - leftwardIndex;

            if (instructionIndex >= instructions.Length)
            {
	            return [];
            }

            if (contiguousLength == instructions[instructionIndex])
            {
	            string newTemplate = "";
	            if (rightwardIndex >= template.Length)
	            {
		            newTemplate = template[..rightwardIndex] + ".";
	            }
	            else
	            {
		            newTemplate = template[..rightwardIndex] + "." + template[(rightwardIndex + 1)..];
	            }
	            
	            // Add a . after the group we just found 
	            return CalculatePermutations(newTemplate, instructions, rightwardIndex + 1, instructionIndex + 1);
            }
          //   if (contiguousLength > instructions[instructionIndex])
          //   {
		        // return [];
          //   }

            return CalculatePermutations(template, instructions, movedIndex, instructionIndex);
        }
        
        private static int FindLastOccurrenceForwardSearch(string input, char character, int startPosition, int max)
        {
	        for (var i = startPosition; i < input.Length; i++)
	        {
		        if (input[i] != character)
		        {
			        return i;
		        }
	        }

	        return max;
        }
        
        private static int FindFirstOccurrenceBackwardSearch(string input, char character, int startPosition)
        {
	        if (startPosition - 1 == 0)
	        {
		        return startPosition;
	        }
	        
	        for (var i = startPosition - 1; i >= 0; i--)
	        {
		        if (input[i] != character)
		        {
			        return i + 1;
		        }
	        }

	        
	        return 0;
        }

        private static IEnumerable<int> FindHashGroupsInString(string input)
        {
	        var count = 0;
	        var inGroup = false;

	        for (var i = 0; i < input.Length; i++)
	        {
		        if (input[i] == '#')
		        {
			        count++;
			        if (!inGroup)
			        {
				        inGroup = true;
			        }
		        }
		        else
		        {
			        if (count != 0)
			        {
				        yield return count;
			        }
			        count = 0;
			        inGroup = false;
		        }
	        }
        }

	}