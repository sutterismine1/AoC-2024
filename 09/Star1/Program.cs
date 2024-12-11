using System.Collections.Generic;

string cwd = "../../../../"; 
string content = File.ReadAllText(cwd + "sample.txt");
List<string> fileSystem = new List<string>();

static int lastFileBlock(string[] arr)
{
    for (int i = arr.Length-1; i >= 0; i--)
    {
        if (arr[i] != ".")
        {
            return i;
        }
    }
    return -1;
}

static long checksum(string[] arr)
{
    int i = 0;
    long sum = 0;
    while (arr[i] != ".")
    {
        sum += long.Parse(arr[i]) * i;
        i++;
    }
    return sum;
}

int id = 0;
for (int i = 0; i < content.Length; i++)
{
    if (i % 2 == 0)
    {
        for (int j = 0; j < Convert.ToInt16(content[i] - '0'); j++)
        {
            fileSystem.Add(id.ToString());
        }
        id++;
    }
    else
    {
        for (int j = 0; j < Convert.ToInt16(content[i] - '0'); j++)
        {
            fileSystem.Add(".");
        }
    }
}
foreach(string file in fileSystem)
{
    Console.Write(file);
}
var query = from file in fileSystem
            where file != "."
            select file;
int numberOfFiles = query.Count();
string[] fileSystemArray = fileSystem.ToArray();
while (fileSystemArray[0..numberOfFiles].Contains("."))
{
    int newSlot = Array.IndexOf(fileSystemArray, ".");
    int lastIndex = lastFileBlock(fileSystemArray);
    fileSystemArray[newSlot] = fileSystemArray[lastIndex];
    fileSystemArray[lastIndex] = ".";
}
Console.WriteLine();
foreach(string file in fileSystemArray)
{
    Console.Write(file);
}
Console.WriteLine();
Console.WriteLine(checksum(fileSystemArray));