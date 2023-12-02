using System.Diagnostics;
using AdventOfCodeSupport;

var solutions = new AdventSolutions();
var today = solutions.GetDay(2023, 2);

var benchmark = false;
#if RELEASE
benchmark = true;
#endif

RunToday(today, benchmark);


static void RunToday(AdventBase today, bool benchmark = false)
{
    static string WithPreciseFormat(TimeSpan timeSpan) => $"{timeSpan.Seconds}s {timeSpan.Milliseconds}ms {timeSpan.Microseconds}us {timeSpan.Nanoseconds}us ";

    today.OnLoad();
    
    var sw = Stopwatch.StartNew();
    today.Part1();
    var firstPart = sw.Elapsed;
    Console.WriteLine($"Elapsed: {WithPreciseFormat(firstPart)}");
    sw.Restart();
    today.Part2();
    var secondPart = sw.Elapsed; 
    Console.WriteLine($"Elapsed: {WithPreciseFormat(secondPart)}");

    if (benchmark)
    {
        today.Benchmark();
    }
}
