namespace API.Helpers;

public static class Helper
{
    public static string CapitalizeStateName(string stateName)
    {
        if (string.IsNullOrWhiteSpace(stateName)) return stateName;

        return string.Join(" ", stateName
            .Split(' ')
            .Select(word => char.ToUpperInvariant(word[0]) + word.Substring(1).ToLowerInvariant()));
    }
}
