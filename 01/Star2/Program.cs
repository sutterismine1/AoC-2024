// See https://aka.ms/new-console-template for more information
string cwd = "../../../../";
string[] contents = File.ReadAllLines(cwd + "input.txt");
int length = contents.Length;
int[] list1 = new int[length];
int[] list2 = new int[length];
int[] results = new int[length];
for (int i = 0; i < length; i++)
{
    string line = contents[i];
    string[] lineArr = line.Split("   ");
    list1[i] = Convert.ToInt32(lineArr[0]);
    list2[i] = Convert.ToInt32(lineArr[1]);
}
for (int i = 0; i < list1.Length; i++)
{
    int similarityScore = list1[i] * list2.Count(x => x == list1[i]);
    results[i] = similarityScore;
}
Console.WriteLine(results.Sum());