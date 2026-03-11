using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Philiprehberger.Slugify;

/// <summary>
/// Controls how <see cref="Slug.Generate"/> produces a slug.
/// </summary>
public sealed record SlugOptions
{
    /// <summary>Maximum length of the resulting slug. Defaults to 200.</summary>
    public int MaxLength { get; init; } = 200;

    /// <summary>Word separator character(s). Defaults to <c>"-"</c>.</summary>
    public string Separator { get; init; } = "-";

    /// <summary>Whether to convert the slug to lowercase. Defaults to <c>true</c>.</summary>
    public bool Lowercase { get; init; } = true;
}

/// <summary>
/// Converts arbitrary text to clean, URL-safe slugs.
/// </summary>
public static class Slug
{
    private static readonly SlugOptions DefaultOptions = new();

    /// <summary>
    /// Converts <paramref name="text"/> to a URL slug.
    /// <list type="bullet">
    ///   <item>Decomposes Unicode characters and strips diacritics.</item>
    ///   <item>Replaces non-alphanumeric characters with the separator.</item>
    ///   <item>Collapses multiple consecutive separators into one.</item>
    ///   <item>Trims separators from both ends.</item>
    ///   <item>Truncates to <see cref="SlugOptions.MaxLength"/> characters.</item>
    /// </list>
    /// </summary>
    public static string Generate(string text, SlugOptions? options = null)
    {
        if (string.IsNullOrWhiteSpace(text))
            return string.Empty;

        options ??= DefaultOptions;
        var separator = options.Separator;

        // Normalise to NFD to decompose accented characters
        var normalised = text.Normalize(NormalizationForm.FormD);

        // Strip combining marks (diacritics)
        var sb = new StringBuilder(normalised.Length);
        foreach (var ch in normalised)
        {
            var category = CharUnicodeInfo.GetUnicodeCategory(ch);
            if (category != UnicodeCategory.NonSpacingMark)
                sb.Append(ch);
        }

        var clean = sb.ToString();

        // Optionally lower-case before replacing
        if (options.Lowercase)
            clean = clean.ToLowerInvariant();

        // Replace any sequence of non-alphanumeric characters with the separator
        var escapedSep = Regex.Escape(separator);
        clean = Regex.Replace(clean, @"[^a-zA-Z0-9]+", separator);

        // Collapse repeated separators
        if (!string.IsNullOrEmpty(separator))
            clean = Regex.Replace(clean, $"({escapedSep})+", separator);

        // Trim separators from both ends
        clean = clean.Trim(separator.ToCharArray());

        // Enforce max length, then trim any trailing separator that got cut
        if (clean.Length > options.MaxLength)
        {
            clean = clean[..options.MaxLength];
            clean = clean.TrimEnd(separator.ToCharArray());
        }

        return clean;
    }
}
