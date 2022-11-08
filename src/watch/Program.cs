using System.Diagnostics;
using System.Text;
using PeanutButter.EasyArgs;
using PeanutButter.Utils;
using watch;

if (!Platform.IsWindows)
{
    Console.Error.WriteLine("This platform is not supported (and should have a superior `watch` built-in anyway)");
    return 1;
}

var parserOptions = new ParserOptions()
{
    ExitOnError = false,
    Description = new[] { "Usage:", "watch [options] command", "", "Options:" }
};
var opts = args.ParseTo<Options>(out var uncollected, parserOptions);
var cmd = string.Join(" ", uncollected);
var batLines = new List<string>()
{
    "@echo off",
    cmd
};
using var tempFile = new AutoTempFile(
    Path.GetTempPath(),
    $"watch-{Guid.NewGuid()}.bat",
    Encoding.UTF8.GetBytes(string.Join(Environment.NewLine, batLines))
);

Console.CancelKeyPress += delegate
{
    tempFile.Dispose();
};

var lastOutput = null as string;
var stopwatch = new Stopwatch();

while (true)
{
    stopwatch.Stop();
    if (lastOutput is not null)
    {
        var toSleep = (int)((opts.Interval * 1000) - stopwatch.ElapsedMilliseconds);
        if (toSleep > 0)
        {
            Thread.Sleep(toSleep);
        }
    }
    stopwatch.Reset();
    stopwatch.Start();

    var lines = new List<string>();
    using var io = ProcessIO.Start(tempFile.Path);
    
    lines.Clear();
    foreach (var line in io.StandardError)
    {
        lines.Add($"ERR: {line}");
    }

    foreach (var line in io.StandardOutput)
    {
        lines.Add(line);
    }

    var all = string.Join("\n", lines);
    Console.Clear();
    if (!opts.NoTitle)
    {
        var pre = $"Every {opts.Interval:0.0}s: {cmd}";
        var post = $"{Environment.MachineName}: {DateTime.Now:F}";
        var missing = Console.WindowWidth - pre.Length - post.Length - 1;
        var spaces = new string(' ', missing);
        Console.WriteLine(
            $"{pre}{spaces}{post}\n"
        );
    }

    Console.Write(all);

    if (lastOutput is null)
    {
        lastOutput = all;
        continue;
    }

    if (opts.ExitOnChange && lastOutput != all)
    {
        Environment.Exit(io.ExitCode);
    }

    lastOutput = all;
        
    if (io.ExitCode == 0)
    {
        continue;
    }

    if (opts.BeepOnError)
    {
        Console.Beep();
    }

    if (opts.ExitOnError)
    {
        Environment.Exit(io.ExitCode);
    }
}

return 0;