using System.Text;

namespace my_uniq;

public static class App
{
    public static int Run(string[] args, TextReader input, TextWriter output, TextWriter error)
    {
        bool ignoreCase = false; 
        bool countMode = false; 
        string? filePath = null;

        foreach (string arg in args)
        {
            if (arg == "-i")
                ignoreCase = true;
            else if (arg == "-c")
                countMode = true;
            else if (arg.StartsWith("-"))
            {
                error.WriteLine($"Unknown option: {arg}");
                return 2; 
            }
            else if (filePath is null)
                filePath = arg;
            else
            {
                error.WriteLine("Too many arguments.");
                return 2;
            }
        }

        try
        {
            List<string> lines;

            if (filePath is not null)
                lines = File.ReadAllLines(filePath).ToList();
            else
                lines = ReadAllLines(input);

            WriteUniqueLines(lines, output, ignoreCase, countMode);

            return 0;
        }
        catch (FileNotFoundException)
        {
            error.WriteLine($"File not found: {filePath}");
            return 1;
        }
        catch (Exception ex)
        {
            error.WriteLine($"Error: {ex.Message}");
            return 1;
        }
    }

    private static List<string> ReadAllLines(TextReader input)
    {
        var lines = new List<string>();
        string? line;

        while ((line = input.ReadLine()) != null)
        {
            lines.Add(line);
        }

        return lines;
    }

    private static void WriteUniqueLines(
        List<string> lines,
        TextWriter output,
        bool ignoreCase,
        bool countMode)
    {
        if (lines.Count == 0)
            return;

        StringComparison comparison = ignoreCase
            ? StringComparison.OrdinalIgnoreCase
            : StringComparison.Ordinal;

        string current = lines[0];
        int count = 1;

        for (int i = 1; i < lines.Count; i++)
        {
            if (string.Equals(lines[i], current, comparison))
            {
                count++;
            }
            else
            {
                WriteLine(output, current, count, countMode);
                current = lines[i];
                count = 1;
            }
        }

        WriteLine(output, current, count, countMode);
    }

    private static void WriteLine(TextWriter output, string line, int count, bool countMode)
    {
        if (countMode)
            output.WriteLine($"{count} {line}");
        else
            output.WriteLine(line);
    }
}