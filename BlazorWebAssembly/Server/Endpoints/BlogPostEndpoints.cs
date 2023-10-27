using Data.Models;
using Data.Models.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlazorWebAssembly.Server.Endpoints;

public static class BlogPostEndpoints
{
    public static void MapBlogPostApi(this WebApplication app) 
    {
        app.MapGet("/api/BlogPosts", async (IBlogApi api,
            [FromQuery] int numberofposts,
            [FromQuery] int startindex) =>
        {
            return Results.Ok(await api.GetBlogPostsAsync(numberofposts, startindex));
        });
    }
}