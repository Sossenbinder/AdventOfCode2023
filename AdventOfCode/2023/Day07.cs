	using AdventOfCodeSupport;

	namespace AdventOfCode._2023;

	public class Day07 : AdventBase
	{
	    protected override object InternalPart1()
	    {
		    var games = Parse();

		    var orderedGames = games
			    .OrderBy(x => x.Hand.CalculateTypeScoreOne());
		    for (var i = 0; i < 5; ++i)
		    {
			    var iCopy = i;
			    orderedGames = orderedGames.ThenBy(x => x.Hand.GetScoreAtPosOne(iCopy));
		    }

		    return orderedGames
			    .Select((
				    x,
				    i) => x.Bid * (i + 1))
			    .Sum();
	    }

	    protected override object InternalPart2()
	    {
		    var games = Parse();

		    var orderedGames = games
			    .OrderBy(x => x.Hand.CalculateTypeScoreTwo());
		    for (var i = 0; i < 5; ++i)
		    {
			    var iCopy = i;
			    orderedGames = orderedGames.ThenBy(x => x.Hand.GetScoreAtPosTwo(iCopy));
		    }

		    return orderedGames
			    .Select((
				    x,
				    i) => x.Bid * (i + 1))
			    .Sum();
	    }

	    private IEnumerable<Game> Parse() => Input.Lines
		    .Select(x =>
		    {
			    var split = x.Split(' ');
			    return new Game(new Hand(split[0]), int.Parse(split[1]));
		    })
		    .ToList();

	    private readonly record struct Hand(string Cards)
	    {
		    public HandType CalculateTypeScoreOne()
		    {
			    var cardGroup = Cards.GroupBy(x => x).ToDictionary(x => x.Key, x => x.ToList());
			    var groupCount = cardGroup.Count;

			    return groupCount switch
			    {
				    1 => HandType.FiveOfAKind,
				    2 when cardGroup.Values.Any(x => x.Count == 4) => HandType.FourOfAKind,
				    2 when cardGroup.Values.Any(x => x.Count == 3) => HandType.FullHouse,
				    3 when cardGroup.Values.Any(x => x.Count == 3) => HandType.ThreeOfAKind,
				    3 when (cardGroup.Values.Count(x => x.Count == 2) == 2) => HandType.TwoPair,
				    >= 2 and <= 4 => HandType.OnePair,
				    _ => HandType.HighCard
			    };
		    }
		    
		    public HandType CalculateTypeScoreTwo()
		    {
			    if (Cards.All(x => x == 'J'))
			    {
				    return HandType.FiveOfAKind;
			    }
			    
			    var cardGroup = Cards
				    .Where(x => x != 'J')
				    .GroupBy(x => x)
				    .ToDictionary(x => x.Key, x => x.ToList());
			    
			    if (Cards.Any(x => x == 'J'))
			    {
				    var topCard = cardGroup.MaxBy(x => x.Value.Count).Key;
				    var replacedCardSet = Cards.Replace('J', topCard);
				    cardGroup = replacedCardSet.GroupBy(x => x).ToDictionary(x => x.Key, x => x.ToList());
			    }
			    
			    var groupCount = cardGroup.Count;

			    return groupCount switch
			    {
				    1 => HandType.FiveOfAKind,
				    2 when cardGroup.Values.Any(x => x.Count == 4) => HandType.FourOfAKind,
				    2 when cardGroup.Values.Any(x => x.Count == 3) => HandType.FullHouse,
				    3 when cardGroup.Values.Any(x => x.Count == 3) => HandType.ThreeOfAKind,
				    3 when (cardGroup.Values.Count(x => x.Count == 2) == 2) => HandType.TwoPair,
				    >= 2 and <= 4 => HandType.OnePair,
				    _ => HandType.HighCard
			    };
		    }

		    public int GetScoreAtPosOne(int pos)
		    {
			    var chr = Cards[pos];
			    return chr switch
			    {
					'A' => 14,
					'K' => 13,
					'Q' => 12,
					'J' => 11,
					'T' => 10,
					_ => chr - '0'
			    };
		    }

		    public int GetScoreAtPosTwo(int pos)
		    {
			    var chr = Cards[pos];
			    return chr switch
			    {
				    'A' => 14,
				    'K' => 13,
				    'Q' => 12,
				    'T' => 10,
				    'J' => 1,
				    _ => chr - '0'
			    };
		    }
	    }

	    private record struct Game(
		    Hand Hand,
		    int Bid);

	    private enum HandType
	    {
		    FiveOfAKind = 6,
		    FourOfAKind = 5,
		    FullHouse = 4,
		    ThreeOfAKind = 3,
		    TwoPair = 2,
		    OnePair = 1,
		    HighCard = 0
	    }
	}