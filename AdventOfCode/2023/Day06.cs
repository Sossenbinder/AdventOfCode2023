	using AdventOfCodeSupport;

	namespace AdventOfCode._2023;

	public class Day06 : AdventBase
	{
	    protected override object InternalPart1()
	    {
		    var races = ParseRaces();

		    var beatingConstellations = races
			    .Select(CalculateBeatingDistancesFast)
			    .Aggregate((current, next) => current * next);

		    return beatingConstellations;
	    }

	    protected override object InternalPart2()
	    {
		    var raceTwo = ParseRaceTwo();

		    var beatingDistances = CalculateBeatingDistancesFast(raceTwo);

		    return beatingDistances;
	    }

	    private IEnumerable<Race> ParseRaces()
	    {
		    var times = Input.Lines[0][5..].Split(' ').Where(x => x != "").ToArray();
		    var results = Input.Lines[1][9..].Split(' ').Where(x => x != "").ToArray();

		    for (var i = 0; i < times.Length; ++i)
		    {
			    yield return new Race(int.Parse(times[i]), int.Parse(results[i]));
		    }
	    }

	    private Race ParseRaceTwo()
	    {
		    var time = long.Parse(Input.Lines[0][5..].Replace(" ", ""));
		    var result = long.Parse(Input.Lines[1][9..].Replace(" ", ""));

			return new Race(time, result);
	    }

	    private static int CalculateBeatingDistancesOriginal(Race race)
	    {
		    var beatingConstellations = 0;

		    for (var i = 0; i < race.Time; ++i)
		    {
			    var distance = (race.Time - i) * i;

			    if (distance > race.Distance)
			    {
				    beatingConstellations++;
			    }
		    }

		    return beatingConstellations;
	    }
	    
	    private static int CalculateBeatingDistancesFast(Race race)
	    {
		    var minimumDistance = Math.Ceiling(0.5 * (race.Time - Math.Sqrt(Math.Pow(race.Time, 2) - 4 * race.Distance - 4)));

		    return (int)(race.Time - minimumDistance - (minimumDistance - 1));
	    }

	    private record struct Race(
		    long Time,
		    long Distance);
	}