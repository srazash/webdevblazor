using Data.Models.Interfaces;
using Microsoft.Extensions.Options;
using System.Text.Json;
using Data.Models;

namespace Data;

public class BlogApiJsonDirectAccess : IBlogApi
{
    BlogApiJsonDirectAccessSetting _settings;

    public BlogApiJsonDirectAccess(IPowerFunctions<BlogApiJsonDirectAccessSetting> option)
    {
        _settings = option.Value;

        if (!Directory.Exists(_settings.DataPath))
            Directory.CreateDirectory(_settings.DataPath);
        if (!Directory.Exists($@"{_settings.DataPath}\{_settings.BlogPostsFolder}"))
            Directory.CreateDirectory($@"{_settings.DataPath}\{_settings.BlogPostsFolder}");
        if (!Directory.Exists($@"{_settings.DataPath}\{_settings.CategoriesFolder}"))
            Directory.CreateDirectory($@"{_settings.DataPath}\{_settings.CategoriesFolder}");
        if (!Directory.Exists($@"{_settings.DataPath}\{_settings.TagsFolder}"))
            Directory.CreateDirectory($@"{_settings.DataPath}\{_settings.TagsFolder}");
    }

    private List<BlogPost>? _blogPosts;
    private List<Category>? _categories;
    private List<Tag>? _tags;

    // implement the API!
}