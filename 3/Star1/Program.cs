using System.Text.RegularExpressions;

string cwd = "../../../../"; 
string content = File.ReadAllText(cwd + "input.txt");

MatchCollection matches = Regex.Matches(content, "mul\\((\\d+),(\\d+)\\)");
int total = 0;
foreach (Match match in matches)
{
    total += Convert.ToInt32(match.Groups[1].Value) * Convert.ToInt32(match.Groups[2].Value);
}
Console.WriteLine(total);