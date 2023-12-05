	using System.Collections.Concurrent;
	using AdventOfCodeSupport;

	namespace AdventOfCode._2023;

	public class Day05 : AdventBase
	{
	    protected override object InternalPart1()
	    {
		    var lineEnumerator = Input.Lines.AsEnumerable().GetEnumerator();
		    lineEnumerator.MoveNext();
		    
		    var sourceSet = lineEnumerator.Current.Split(':')[1].Trim().Split(' ').Select(long.Parse).ToList();

		    var resultSet = GetResultSet(lineEnumerator, sourceSet);

		    return resultSet.Min();
	    }

	    protected override object InternalPart2()
	    {
		    var seeds = Input.Lines[0].Split(':')[1].Trim().Split(' ').Select(long.Parse).ToList();

		    var completeResultSet = new ConcurrentBag<long>();
		    
		    for (var i = 0; i < seeds.Count; i += 2)
		    {
			    var start = seeds[i];
			    var range = seeds[i + 1];

			    var sourceSet = new HashSet<long>();
			    for (var x = start; x < (start + range); ++x)
			    {
				    sourceSet.Add(x);
			    }
			    
			    var minResult = sourceSet.Chunk(5000000).AsParallel()
				    .Select(chunk =>
				    {
					    var lineEnumerator = Input.Lines.AsEnumerable().GetEnumerator();
					    lineEnumerator.MoveNext();

					    var resultSet = GetResultSet(lineEnumerator, chunk.ToList());
					    return resultSet.Min();
				    })
				    .Min();

			    completeResultSet.Add(minResult);
		    }

		    return completeResultSet.Min();
	    }

	    private List<long> GetResultSet(IEnumerator<string> lineEnumerator, List<long> sourceSet)
	    {
		    while (lineEnumerator.MoveNext())
		    {
			    if (lineEnumerator.Current.Contains("map"))
			    {
				    lineEnumerator.MoveNext();

				    var ranges = new List<Range>();

				    while (lineEnumerator.Current != "")
				    {
					    var split = lineEnumerator.Current.Split(' ');
					    ranges.Add(new Range(long.Parse(split[0]), long.Parse(split[1]), long.Parse(split[2])));

					    if (!lineEnumerator.MoveNext())
					    {
						    break;
					    }
				    }

				    var newSourceSet = new List<long>();
				    foreach (var source in sourceSet)
				    {
					    var rangeMatch = ranges.FirstOrDefault(x => source >= x.SourceStart && source < (x.SourceStart + x.Length));
					    if (rangeMatch is not null)
					    {
						    newSourceSet.Add(rangeMatch.DestinationStart + (source - rangeMatch.SourceStart));
					    }
					    else
					    {
						    newSourceSet.Add(source);
					    }
				    }

				    sourceSet = newSourceSet;
			    }
		    }

		    return sourceSet;
	    }

	    private record Range(
		    long DestinationStart,
		    long SourceStart,
		    long Length);
	}