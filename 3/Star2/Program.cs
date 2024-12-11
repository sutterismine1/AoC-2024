using System.Text.RegularExpressions;

string cwd = "../../../../";
string content = File.ReadAllText(cwd + "input.txt");

MatchCollection matches = Regex.Matches(content, "mul\\((\\d+),(\\d+)\\)");
int total = 0;
MatchCollection conditionMatches = Regex.Matches(content, "do\\(\\)|don't\\(\\)");
bool enabled = true;
int matchLocation = matches.Count - 1;
for(int i = conditionMatches.Count-1; i>=0; i--)
{
    Match conditionMatch = conditionMatches[i];
    int conditionLocation = conditionMatch.Groups[0].Index;
    string condition = conditionMatch.Groups[0].Value;
    if (condition == "do()") {  enabled = true; }
    if (condition == "don't()") { enabled = false; }
    while(matches[matchLocation].Index > conditionLocation) {
        Match match = matches[matchLocation];
        if (enabled)
        {
            total += Convert.ToInt32(match.Groups[1].Value) * Convert.ToInt32(match.Groups[2].Value);
        }
        matchLocation = matchLocation-1;
    }
}
while(matchLocation >= 0) {  //go through any "mul" to left of left most condition. They are enabled
    Match match = matches[matchLocation];
    total += Convert.ToInt32(match.Groups[1].Value) * Convert.ToInt32(match.Groups[2].Value);
    matchLocation = matchLocation - 1;
}
Console.WriteLine(total);