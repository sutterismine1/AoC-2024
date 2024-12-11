using System;
using System.Collections.Generic;

string cwd = "../../../../";
string content = File.ReadAllText(cwd + "input.txt");
List<string> fileSystem = new List<string>();

static (int, int) lastFile(string[] arr, string file)
{
    int lastIndex = 0;
    int firstIndex = 0;
    bool foundFile = false;
    for (int i = arr.Length - 1; i >= 0; i--)
    {
        if (foundFile)
        {
            if (arr[i] != file)
            {
                firstIndex = i+1;
                break;
            }
        }
        else if (arr[i] == file)
        {
            lastIndex = i;
            foundFile = true;
        }
    }
    return (firstIndex, lastIndex);
}

static long checksum(string[] arr)
{
    int i = 0;
    long sum = 0;
    while (i < arr.Length)
    {
        if (arr[i] == ".")
        {
            i++;
            continue;
        }
        sum += long.Parse(arr[i]) * i;
        i++;
    }
    return sum;
}

static int getSpace(string[] arr, int index)
{
    int space = 1;
    if (index < arr.Length - 1)
    {
        int next = index + 1;
        while (arr[next] == ".")
        {
            space++;
            next++;
            if (next == arr.Length)
            {
                break;
            }
        }
    }
    return space;
}

int id = 0;
for (int i = 0; i < content.Length; i++)
{
    if (i % 2 == 0)
    {
        for (int j = 0; j < Convert.ToInt32(content[i] - '0'); j++)
        {
            fileSystem.Add(id.ToString());
        }
        id++;
    }
    else
    {
        for (int j = 0; j < Convert.ToInt32(content[i] - '0'); j++)
        {
            fileSystem.Add(".");
        }
    }
}
foreach (string file in fileSystem)
{
    Console.Write(file);
}
var query = from file in fileSystem
            where file != "."
            select file;
int numberOfFileIDs = query.Count();
int numberOfFiles = query.Distinct().Count();
string[] fileSystemArray = fileSystem.ToArray();
int currentFileIndex = numberOfFiles - 1;
while (currentFileIndex > 0)
{
    List<int> indexes = fileSystemArray   //Get starting index of each empty block
        .Select((value, index) => (value, index))
        .Where(pair => pair.value == "." && (pair.index == 0 || fileSystemArray[pair.index - 1] != "."))
        .Select(pair => pair.index).ToList();
    Dictionary<int, int> spaceOfPossibleSlots = new Dictionary<int, int>();
    foreach (int index in indexes)   //Get free space for each empty block
    {
        int space = getSpace(fileSystemArray, index);
        spaceOfPossibleSlots[index] = space;
    }
    var file = query.Distinct().ElementAt(currentFileIndex);
    (int firstIndex, int lastIndex) = lastFile(fileSystemArray, file);
    List<int> toRemove = new List<int>();
    bool moved = false;
    int toAdd = -1;
    foreach (int index in indexes)   //check each empty space if the file can fit in there
    {
        int space = spaceOfPossibleSlots[index];
        if (space >= (lastIndex - firstIndex + 1) && index <= firstIndex)
        {
            if (!moved)
            {
                for (int i = index; i < (index + (lastIndex - firstIndex + 1)); i++)
                {
                    fileSystemArray[i] = file;
                    moved = true;
                }
                for (int j = firstIndex; j <= lastIndex; j++)
                {
                    fileSystemArray[j] = "."; //update old spot to be empty
                }
            }
        }
    }
    /*Console.WriteLine();
    foreach (string aFile in fileSystemArray)
    {
        Console.Write(aFile);
    }
    Console.WriteLine();
    Console.WriteLine("Indexes: " + string.Join(", ", indexes));*/
    currentFileIndex--;
}
Console.WriteLine();
foreach (string file in fileSystemArray)
{
    Console.Write(file);
}
Console.WriteLine();
Console.WriteLine(checksum(fileSystemArray));