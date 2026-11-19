namespace Educ8.Documents;

/// <summary>Page-range parsing/formatting: "3", "1-10", "5-", "2,4,7-9".</summary>
public static class Ranges
{
    /// <summary>Parses a range spec against <paramref name="max"/> parts. Null/blank → null (= everything). Throws <see cref="FormatException"/>.</summary>
    public static SortedSet<int>? Parse(string? spec, int max)
    {
        if (string.IsNullOrWhiteSpace(spec) || spec.Trim() is "all" or "*") return null;
        var result = new SortedSet<int>();
        foreach (var raw in spec.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            var dash = raw.IndexOf('-');
            if (dash < 0)
            {
                result.Add(Number(raw));
                continue;
            }
            var from = dash == 0 ? 1 : Number(raw[..dash]);
            var to = dash == raw.Length - 1 ? max : Number(raw[(dash + 1)..]);
            if (to < from) throw new FormatException($"Range \"{raw}\" ends before it starts.");
            for (var i = from; i <= Math.Min(to, max); i++) result.Add(i);
        }
        result.RemoveWhere(i => i < 1 || i > max);
        if (result.Count == 0) throw new FormatException($"\"{spec}\" selects nothing; valid numbers are 1–{max}.");
        return result;

        static int Number(string s) => int.TryParse(s.Trim(), out var n) && n > 0
            ? n
            : throw new FormatException($"\"{s}\" isn't a page number.");
    }

    /// <summary>[1,2,3,5,7,8] → "1–3, 5, 7–8".</summary>
    public static string Format(IEnumerable<int> numbers)
    {
        var sorted = numbers.Distinct().Order().ToList();
        var groups = new List<string>();
        for (var i = 0; i < sorted.Count;)
        {
            var j = i;
            while (j + 1 < sorted.Count && sorted[j + 1] == sorted[j] + 1) j++;
            groups.Add(i == j ? $"{sorted[i]}" : $"{sorted[i]}–{sorted[j]}");
            i = j + 1;
        }
        return string.Join(", ", groups);
    }
}
