using System.Text.RegularExpressions;
using MathNet.Numerics.LinearAlgebra;

string cwd = "../../../../";

static (double AButtonPresses, double BButtonPresses) solve(double[,] AMatrix, double[] BVector)
{
    var A = Matrix<double>.Build.DenseOfArray(AMatrix);
    var B = Vector<double>.Build.Dense(BVector);
    var result = A.Solve(B);
    if (result.ForAll(val => Math.Abs(val - Convert.ToInt64(val)) <= 0.0001))
    {
        return (Math.Round(result[0]), Math.Round(result[1]));
    }
    return (0, 0);
}

try
{
    using StreamReader reader = new(cwd + "input.txt");
    string text;
    string xPattern = "X(\\+|\\=)(\\d+)";
    string yPattern = "Y(\\+|\\=)(\\d+)";
    long sum = 0;
    while ((text = reader.ReadLine()) != null)
    {
        double aX = Convert.ToInt64(Regex.Match(text, xPattern).Groups[2].Value);
        double aY = Convert.ToInt64(Regex.Match(text, yPattern).Groups[2].Value);
        text = reader.ReadLine();
        double bX = Convert.ToInt64(Regex.Match(text, xPattern).Groups[2].Value);
        double bY = Convert.ToInt64(Regex.Match(text, yPattern).Groups[2].Value);
        text = reader.ReadLine();
        double xValue = Convert.ToInt64(Regex.Match(text, xPattern).Groups[2].Value) + 10000000000000;
        double yValue = Convert.ToInt64(Regex.Match(text, yPattern).Groups[2].Value) + 10000000000000;
        var result = solve(new double[,] { { aX, bX },
                              { aY, bY } }, new double[] { xValue, yValue });
        sum += Convert.ToInt64(result.AButtonPresses * 3 + result.BButtonPresses * 1);
        reader.ReadLine();
    }
    Console.WriteLine(sum);
}
catch (IOException e)
{
    Console.WriteLine("The file could not be read:");
    Console.WriteLine(e.Message);
}