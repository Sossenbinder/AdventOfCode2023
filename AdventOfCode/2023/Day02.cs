using AdventOfCodeSupport;

namespace AdventOfCode._2023;

public class Day02 : AdventBase
{
    protected override void InternalOnLoad()
    {
        _ = Input;
    }

    protected override object InternalPart1()
    {
        var gameSum = 0;
        
        const int maxRed = 12;
        const int maxGreen = 13;
        const int maxBlue = 14;
        
        for (var game = 0; game < Input.Lines.Length; ++game)
        {
            var combinations = GetCubesOfLine(Input.Lines[game]);

            if (combinations.Any(combination => combination is not { Blue: <= maxBlue, Green: <= maxGreen, Red: <= maxRed }))
            {
                gameSum += game + 1;
            }
        }

        return gameSum;
    }

    protected override object InternalPart2()
    {
        var gameSum = 0;
        
        for (var game = 0; game < Input.Lines.Length; ++game)
        {
            var combinations = GetCubesOfLine(Input.Lines[game]);

            var maxRed = combinations.Max(x => x.Red);
            var maxGreen = combinations.Max(x => x.Green);
            var maxBlue = combinations.Max(x => x.Blue);
            gameSum += maxRed * maxGreen * maxBlue;
        }

        return gameSum;
    }

    private unsafe List<(int Blue, int Red, int Green)> GetCubesOfLine(string line)
    {
        var lineSpan = line.AsSpan();

        var noGamePrefixSpan = lineSpan[(lineSpan.IndexOf(':') + 1)..];

        Span<Range> gameRangesSpan = stackalloc Range[6];
        var gameSpansAvailable = noGamePrefixSpan.Split(gameRangesSpan, ';');

        var cubeCombinations = new List<(int Blue, int Red, int Green)>();
        for (var i = 0; i < gameSpansAvailable; ++i)
        {
            var gameRange = gameRangesSpan[i];
            var actualGame = noGamePrefixSpan[gameRange];

            Span<Range> cubeSections = stackalloc Range[3];
            var cubeSectionsCount = actualGame.Split(cubeSections, ',');

            var blue = 0;
            var red = 0;
            var green = 0;
            foreach (var cubeSection in cubeSections[..cubeSectionsCount])
            {
                Span<Range> result = stackalloc Range[2];
                var game = actualGame[cubeSection].Trim();
                game.Split(result, ' ');

                var amountPart = game[result[0]].Trim();
                var color = game[result[1]].Trim();

                var amount = int.Parse(amountPart);

                switch (color[0])
                {
                    case 'b':
                        blue += amount;
                        break;
                    case 'r':
                        red += amount;
                        break;
                    case 'g':
                        green += amount;
                        break;
                }
            }
            
            cubeCombinations.Add((blue, red, green));
        }

        return cubeCombinations;
    }
}