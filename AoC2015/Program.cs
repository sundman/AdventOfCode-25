using System.Diagnostics;
using System.Linq;
using System.Reflection;
using ConsoleApp;


var assembly = Assembly.GetExecutingAssembly();
var days = assembly.GetTypes().Where(t => t.GetInterfaces().Contains(typeof(IDay)) && t.IsClass).ToList();

var tabLenght = 12;
Console.WriteLine($"{"Day".PadRight(tabLenght)}{"Parsing".PadRight(tabLenght)}{"Part1".PadRight(tabLenght)}{"Part2".PadRight(tabLenght)}{"Ans1".PadRight(tabLenght * 2)}{"Ans2".PadRight(tabLenght * 2)}");
Console.WriteLine("".PadRight(tabLenght * 8, '='));
var total = new Stopwatch();
total.Start();
foreach (var day in days.OrderBy(x => x.Name))
{
    var inst = (IDay)Activator.CreateInstance(day);

    var timer = new Stopwatch();
    timer.Start();
    inst.ReadInput();
    var inputms = timer.ElapsedMilliseconds;

    timer.Restart();
    var part1 = inst.Part1();
    var part1ms = timer.ElapsedMilliseconds;

    timer.Restart();
    var part2 = inst.Part2();
    var part2ms = timer.ElapsedMilliseconds;

    Console.WriteLine(
        $"{day.Name.PadRight(tabLenght)}{$"{inputms} ms".PadRight(tabLenght)}{$"{part1ms} ms".PadRight(tabLenght)}{$"{part2ms} ms".PadRight(tabLenght)}{$"{part1}".PadRight(tabLenght * 2)}{$"{part2}".PadRight(tabLenght * 2)}");

}
Console.WriteLine();
Console.WriteLine($"Total runtime: {total.ElapsedMilliseconds} ms");
Console.ReadLine();
