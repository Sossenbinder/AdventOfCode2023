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

            var combinations = CalculatePermutations(field, instructions).ToList();
            return combinations.Count;
        }


        private static List<string> CalculatePermutations(string template, int[] instructions, int templateIndex = 0, int instructionIndex = 0)
        {
	        if (instructionIndex == instructions.Length && template.All(x => x != '?'))
	        {
		        return FindHashGroupsInString(template).Count() == instructions.Length ? [template] : [];
	        }
	        
            if (templateIndex >= template.Length)
            {
                return instructionIndex == instructions.Length ? [template] : [];
            }

            var startChar = template[templateIndex];
            var movedIndex = templateIndex + 1;

            if (startChar == '?')
            {
	            var list = CalculatePermutations(template[..templateIndex] + "." + template[(templateIndex + 1)..], instructions, templateIndex, instructionIndex);
	            list.AddRange(CalculatePermutations(template[..templateIndex] + "#" + template[(templateIndex+1)..], instructions, templateIndex, instructionIndex));
	            return list;
            }

            if (startChar == '.')
            {
	            return CalculatePermutations(template, instructions, movedIndex, instructionIndex);
            }

            var rightwardIndex = template.IndexOf('.', templateIndex);

            if (rightwardIndex == -1)
            {
	            rightwardIndex = template.IndexOf('?', templateIndex);
            }

            if (rightwardIndex == -1)
            {
	            rightwardIndex = template.Length;
            }
            
            var leftwardIndex = template.LastIndexOf('.', templateIndex);

            if (leftwardIndex == -1)
            {
	            leftwardIndex = template.LastIndexOf('?', templateIndex);
            }

            if (leftwardIndex == -1)
            {
	            leftwardIndex = 0;
            }

            var contiguousLength = rightwardIndex - 1 - leftwardIndex;

            if (instructionIndex < instructions.Length && contiguousLength == instructions[instructionIndex])
            {
	            var newInstructionIndex = instructionIndex + 1;

	            if (newInstructionIndex == instructions.Length)
	            {
		            var hashGroups = FindHashGroupsInString(template).ToList();
		            if (hashGroups.Count > instructions.Length)
		            {
			            return [];
		            }

		            if (hashGroups.Count == instructions.Length && template.All(x => x != '?') && hashGroups.Select((x, index) => instructions[index] == x).All(x => x))
		            {
			            return [template];
		            }
	            }
	            
	            if ((rightwardIndex + 1) >= template.Length)
	            {
		            return [];
	            }
	            
	            // Add a . after the group we just found 
	            var newTemplate = template[..rightwardIndex] + "." + template[(rightwardIndex + 1)..];
	            return CalculatePermutations(newTemplate, instructions, rightwardIndex - 1, newInstructionIndex);
            }
            
            return CalculatePermutations(template, instructions, movedIndex, instructionIndex);
        }

        private static IEnumerable<int> FindHashGroupsInString(string input)
        {
	        var count = 0;
	        var inGroup = false;

	        for (var i = 0; i < input.Length; i++)
	        {
		        if (input[i] == '#')
		        {
			        if (!inGroup)
			        {
				        count++;
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