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
            return combinations;
        }


        private static int CalculatePermutations(string template, int[] instructions, int templateIndex = 0, int instructionIndex = 0)
        {
            if (templateIndex == template.Length)
            {
                return instructionIndex + 1 == instructions.Length ? 1 : 0;
            }

            var startChar = template[templateIndex];
            var movedIndex = templateIndex + 1;

            if (startChar == '?')
            {
	            return CalculatePermutations(template[..templateIndex] + "." + template[(templateIndex+1)..], instructions, templateIndex, instructionIndex)
	                   + 
	                   CalculatePermutations(template[..templateIndex] + "#" + template[(templateIndex+1)..], instructions, templateIndex, instructionIndex);
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

            var contiguousLength = rightwardIndex - leftwardIndex;

            if (contiguousLength == instructions[instructionIndex])
            {
	            // Add a . after the group we just found 
	            var newTemplate = template[..(rightwardIndex + 1)] + "." + template[(rightwardIndex + 2)..];
	            return CalculatePermutations(newTemplate, instructions, movedIndex + rightwardIndex - 1, instructionIndex + 1);
            }
            
            return CalculatePermutations(template, instructions, movedIndex, instructionIndex);
        }
	}