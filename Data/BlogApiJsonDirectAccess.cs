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

    private void Load<T>(ref List<T>? list, string folder)
    {
        if (list == null)
        {
            list = new();
            var fullpath = $@"{_settings.DataPath}\{folder}";

            foreach (var f in Directory.GetFiles(fullpath))
            {
                var json = File.ReadAllText(f);
                var bp = JsonSerializer.Deserialize<T>(json);
                if (bp != null) list.Add(bp);
            }
        }
    }

    private Task LoadBlogPostsAsync()
    {
        Load<BlogPost>(ref _blogPosts, _settings.BlogPostsFolder);
        return Task.CompletedTask;
    }

    private Task LoadTagsAsync()
    {
        Load<Tag>(ref _tags, _settings.TagsFolder);
        return Task.CompletedTask;
    }

    private Task LoadCategoriesAsync()
    {
        Load<Category>(ref _categories, _settings.CategoriesFolder);
        return Task.CompletedTask;
    }
}