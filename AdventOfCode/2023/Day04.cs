	using AdventOfCodeSupport;

	namespace AdventOfCode._2023;

	public class Day04 : AdventBase
	{
	    protected override object InternalPart1()
	    {
		    return Input.Lines.Select(AnalyzeCardPartOne).Sum();
	    }

	    protected override object InternalPart2()
	    {
		    var cardDict = new Dictionary<int, int>();
		    foreach (var i in Enumerable.Range(0, Input.Lines.Length))
		    {
			    cardDict[i] = 1;
		    }

		    for (var i = 0; i < Input.Lines.Length; i++)
		    {
			    var line = Input.Lines[i];
			    var winCount = AnalyzeCard(line);

			    var currentCardCount = cardDict[i];

			    for (var j = i + 1; j < winCount + i + 1; ++j)
			    {
				    if (j > Input.Lines.Length)
				    {
					    break;
				    }
				    
				    cardDict[j] += currentCardCount;
			    }
		    }

		    return cardDict.Values.Sum();
	    }

	    private static double AnalyzeCardPartOne(string line)
	    {
		    var winnerCount = AnalyzeCard(line);

		    if (winnerCount == 0)
		    {
			    return 0;
		    }
		    
		    return Math.Pow(2, winnerCount - 1);
	    }
	    
	    private static unsafe int AnalyzeCard(string line)
	    {
		    var winnerCount = 0;
		    var lineSpan = line.AsSpan()[9..];

		    Span<Range> splitSpans = stackalloc Range[2];
		    lineSpan.Split(splitSpans, '|');
		    
		    Span<Range> winnerSplits = stackalloc Range[20];
		    lineSpan[splitSpans[0]].Split(winnerSplits, ' ');

		    var winners = new HashSet<int>();
		    foreach (var winnerSplit in winnerSplits)
		    {
			    var item = lineSpan[winnerSplit].Trim();

			    if (item.Length == 0)
			    {
				    continue;
			    }
			    
			    winners.Add(int.Parse(item));
		    }

		    var numberSpan = lineSpan[splitSpans[1]];
		    Span<Range> numberSplits = stackalloc Range[100];
		    numberSpan.Split(numberSplits, ' ');

		    foreach (var numberSplit in numberSplits)
		    {
			    var item = numberSpan[numberSplit].Trim();

			    if (item.Length == 0)
			    {
				    continue;
			    }
			    
			    var nr = int.Parse(item);
			    if (winners.Contains(nr))
			    {
				    winnerCount++;
			    }
		    }
		    
		    return winnerCount;
	    }
	}