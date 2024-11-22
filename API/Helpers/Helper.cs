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

    public static string CapitalizeLandmarkName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Landmark name must not be null or empty.");
        }

        // Split the name into words by spaces, capitalize each word, and join them back
        var words = name.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var capitalizedWords = words.Select(word => char.ToUpper(word[0]) + word.Substring(1).ToLower());
        return string.Join(' ', capitalizedWords);
    }


}
