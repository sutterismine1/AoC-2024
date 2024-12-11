// See https://aka.ms/new-console-template for more information
string cwd = "../../../../";
string[] contents = File.ReadAllLines(cwd + "input.txt");
string[] list1 = new string[] { };
string[] list2 = new string[] { };
foreach(string line in contents)
{
    string[] lineArr = line.Split("   ");
    list1 = list1.Append<string>(lineArr[0]).ToArray();
    list2 = list2.Append<string>(lineArr[1]).ToArray();
}
Array.Sort(list1);
Array.Sort(list2);
int[] results = new int[list1.Length];

for (int i = 0; i < list1.Length; i++)
{
    int difference = Convert.ToInt32((string)list1[i]) - Convert.ToInt32((string)list2[i]);
    results[i] = Math.Abs(difference);
}
Console.WriteLine(results.Sum());