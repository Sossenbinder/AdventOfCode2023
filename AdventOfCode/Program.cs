using System.Diagnostics;
using AdventOfCodeSupport;

var solutions = new AdventSolutions();
var today = solutions.GetDay(2023, 12);

var benchmark = false;
#if RELEASE
benchmark = true;
#endif

RunToday(today, benchmark);
return;


static void RunToday(AdventBase today, bool benchmark = false)
{
    if (benchmark)
    {
        today.Benchmark();
    }
    else
    {
        var sw = Stopwatch.StartNew();
        today.Part1();
        var firstPart = sw.Elapsed;
        Console.WriteLine($"Elapsed: {WithPreciseFormat(firstPart)}");
        sw.Restart();
        today.Part2();
        var secondPart = sw.Elapsed; 
        Console.WriteLine($"Elapsed: {WithPreciseFormat(secondPart)}");
    }

    return;

    static string WithPreciseFormat(TimeSpan timeSpan) => $"{timeSpan.Seconds}s {timeSpan.Milliseconds}ms {timeSpan.Microseconds}us {timeSpan.Nanoseconds}us ";
}
