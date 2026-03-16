# Philiprehberger.Slugify

[![CI](https://github.com/philiprehberger/dotnet-slugify/actions/workflows/ci.yml/badge.svg)](https://github.com/philiprehberger/dotnet-slugify/actions/workflows/ci.yml)
[![NuGet](https://img.shields.io/nuget/v/Philiprehberger.Slugify.svg)](https://www.nuget.org/packages/Philiprehberger.Slugify)
[![License](https://img.shields.io/github/license/philiprehberger/dotnet-slugify)](LICENSE)

Convert arbitrary text to clean, URL-safe slugs. Handles Unicode diacritics, collapses separators, and enforces a maximum length — no external dependencies.

## Install

```bash
dotnet add package Philiprehberger.Slugify
```

## Usage

```csharp
using Philiprehberger.Slugify;

Slug.Generate("Hello, World!");                    // "hello-world"
Slug.Generate("Héllo Wörld");                      // "hello-world"
Slug.Generate("  Multiple   Spaces  ");            // "multiple-spaces"
Slug.Generate("C# is great!");                     // "c-is-great"
Slug.Generate("über cool blog post");              // "uber-cool-blog-post"
```

### Custom options

```csharp
var options = new SlugOptions
{
    Separator = "_",
    Lowercase = false,
    MaxLength = 50
};

Slug.Generate("Hello World", options);  // "Hello_World"
```

## API

### `Slug`

| Method | Description |
|--------|-------------|
| `Generate(string text, SlugOptions? options)` | Convert text to a URL slug |

### `SlugOptions`

| Property | Default | Description |
|----------|---------|-------------|
| `MaxLength` | `200` | Maximum slug length; truncates at a separator boundary |
| `Separator` | `"-"` | Word separator inserted between tokens |
| `Lowercase` | `true` | Convert result to lowercase |

### How it works

1. Normalise to Unicode NFD to decompose accented characters.
2. Strip non-spacing combining marks (removes diacritics).
3. Optionally convert to lowercase.
4. Replace every run of non-alphanumeric characters with the separator.
5. Collapse consecutive separators into one.
6. Trim separators from both ends.
7. Truncate to `MaxLength`, trimming any trailing separator.

## Development

```bash
dotnet build src/Philiprehberger.Slugify.csproj --configuration Release
```

## License

MIT
