using AdventOfCodeSupport;

namespace AdventOfCode._2023;

public class Day01 : AdventBase
{
    protected override object InternalPart1()
    {
        var solution = 0;
        
        for (var i = 0; i < Input.Lines.Length; ++i)
        {
            var left = -1;
            var right = -1;
            
            var line = Input.Lines[i];

            for (var x = 0; x < line.Length; ++x)
            {
                if (line[x] - '0' is > 0 and < 10)
                {
                    left = line[x];
                    break;
                }
            }
            
            for (var x = line.Length - 1; x > 0; --x)
            {
                if (line[x] - '0' is > 0 and < 10)
                {
                    right = line[x];
                    break;
                }
            }

            solution += left * 10 + right;
        }
        return solution;
    }

    protected override object InternalPart2()
    {
        List<string> validNumbers = ["one", "two", "three", "four", "five", "six", "seven", "eight", "nine"];

        var solutions = new List<int>();
        foreach (var line in Input.Lines)
        {
            var replacedLine = line;

            var firstDigitIndex = line.Select((x, index) => (x - '0' is > 0 and < 10) ? index : line.Length + 1).Min();

            var firstStringOcc = validNumbers
                .Where(x =>
                {
                    var index = line.IndexOf(x);
                    return index != -1 && index < firstDigitIndex;
                })
                .MinBy(x => line.IndexOf(x));
            if (firstStringOcc is not null)
            {
                replacedLine = replacedLine.Replace(firstStringOcc, (validNumbers.IndexOf(firstStringOcc) + 1).ToString()); 
            }
            
            var lastDigitIndex = line.Select((x, index) => (x - '0' is > 0 and < 10) ? index : -1).Max();
            
            var lastStringOcc = validNumbers
                .Where(x =>
                {
                    var index = line.LastIndexOf(x);
                    return index != -1 && index > lastDigitIndex;
                })
                .MaxBy(x => line.LastIndexOf(x));

            if (lastStringOcc is not null)
            {
                replacedLine = replacedLine.Replace(lastStringOcc, (validNumbers.IndexOf(lastStringOcc) + 1).ToString()); 
            }
            
            var lineSolution = SolveLine(replacedLine);
            
            solutions.Add(lineSolution);
        }

        return solutions.Sum();
    }
    

    private static int SolveLine(string line)
    {
        var parsed = line
            .Where(x => int.TryParse(x.ToString(), out _))
            .ToList();

        return int.Parse($"{parsed.First()}{parsed.Last()}");
    }
}