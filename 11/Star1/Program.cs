using MyUtilities;
using System;
using System.Collections.Generic;

Dictionary<string, List<string>> memo = new();

List<string> ProcessStone(string stone)
{
    List<string> result;
    if (memo.TryGetValue(stone, out var memoResult))
        return memoResult;  // Return cached result
    
    if (stone == "0")
        result = new List<string> { "1" };
    else if (stone.Length % 2 == 0)
    {
        int half = stone.Length / 2;
        string second = stone.Substring(half).TrimStart('0');
        if (string.IsNullOrEmpty(second)) { 
            second = "0";
        }
        result = new List<string>
        {
            stone.Substring(0, half),
            second
        };
    }
    else
    {
        long number = long.Parse(stone);
        result = new List<string> { (number * 2024).ToString() };
    }

    // Cache the result
    memo[stone] = result;
    return result;
}
long CountStonesAfterBlinks(Dictionary<string, long> stoneCounts, int blinks)
{
    Dictionary<string, long> currentStoneCounts = new(stoneCounts);
    Dictionary<string, long> nextStoneCounts = new();

    for (int i = 0; i < blinks; i++)
    {
        nextStoneCounts.Clear();

        foreach (var kvp in currentStoneCounts)
        {
            string stone = kvp.Key;
            long count = kvp.Value;

            var processedStones = ProcessStone(stone);
            foreach (var processedStone in processedStones)
            {
                if (nextStoneCounts.ContainsKey(processedStone))
                    nextStoneCounts[processedStone] += count;
                else
                    nextStoneCounts[processedStone] = count;
            }
        }

        var temp = currentStoneCounts;
        currentStoneCounts = nextStoneCounts;
        nextStoneCounts = temp;
        if (i < 10)
        {
            Console.WriteLine("After Blink " + (i + 1) + ":");
            foreach (var kvp in currentStoneCounts)
            {
                Console.Write(kvp.Key + " (" + kvp.Value + ") ");
            }
            Console.WriteLine();
        }
    }

    return currentStoneCounts.Values.Sum();  // Return the total number of stones
}
string cwd = "../../../../";
List<String> stones = File.ReadAllText(cwd + "input.txt").Split(" ").ToList();
Dictionary<string, long> stoneCounts = stones
    .GroupBy(stone => stone)
    .ToDictionary(g => g.Key, g => Convert.ToInt64(g.Count()));

Console.WriteLine("Initial:");
foreach (var kvp in stoneCounts)
{
    Console.Write(kvp.Key + " (" + kvp.Value + ") ");
}
Console.WriteLine();
Console.WriteLine();
long result = CountStonesAfterBlinks(stoneCounts, 75);
Console.WriteLine(result);