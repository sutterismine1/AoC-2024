﻿using System.Collections;
using System.Diagnostics.Metrics;
using MyUtilities;

string cwd = "../../../../";
ArrayList rules = new ArrayList();
ArrayList updates = new ArrayList();
static IEnumerable<string> ReadLines(StreamReader reader)
{
    string line;
    while ((line = reader.ReadLine()) != null)
    {
        yield return line;
    }
}
static void swap(string[] arr, int i, int j)
{
    string temp = arr[i];
    arr[i] = arr[j];
    arr[j] = temp;
}

static int checkValidity(string[] arr, ArrayList rules)
{
    bool swapped = false;
    bool done = false;
    while (!done)
    {
        bool innerSwapped = false;
        foreach (var (index, value) in arr.Enumerate())
        {
            for (int i = 0; i < rules.Count; i++)
            {
                string[] rule = (string[])rules[i];
                if (rule[0].Equals(value))
                {
                    string after = rule[1];
                    for (int j = 0; j < arr.Count(); j++)
                    {
                        if (after.Equals(arr[j]))
                        {
                            if (j < index)
                            {
                                swap(arr, index, j);
                                swapped = true;
                                innerSwapped = true;
                            }
                        }
                    }
                }
                if (rule[1].Equals(value))
                {
                    string before = rule[0];
                    for (int j = 0; j < arr.Count(); j++)
                    {
                        if (before.Equals(arr[j]))
                        {
                            if (j > index)
                            {
                                swap(arr, index, j);
                                swapped = true;
                                innerSwapped = true;
                            }
                        }
                    }
                }
            }
        }
        if (!innerSwapped)
        {
            done = true;
        }
    }
    if (!swapped) { return 0; }
    return Convert.ToInt32(arr[arr.Length / 2]);
}

using (var reader = new StreamReader(cwd + "input.txt"))
{
    IEnumerator<string> enumerator = ReadLines(reader).GetEnumerator();
    while (enumerator.MoveNext())
    {
        string line = enumerator.Current;
        if (line == "") { break; }
        else { rules.Add(line.Split("|")); }
    }
    while (enumerator.MoveNext())
    {
        updates.Add(enumerator.Current.Split(","));
    }
}
foreach (string[] rule in rules)
{
    Console.WriteLine(rule[0]);
}
int total = 0;
foreach (string[] update in updates)
{
    int num = checkValidity(update, rules);
    if (num > 0)
    {
        total += num;
        Console.WriteLine("running total " + total);
    }
}