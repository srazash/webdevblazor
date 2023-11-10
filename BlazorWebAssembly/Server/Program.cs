using Data;
using Data.Models.Interfaces;
using BlazorWebAssembly.Server.Endpoints;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("auth.json");

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddOptions<BlogApiJsonDirectAccessSetting>()
	.Configure(options =>
	{
		options.DataPath = @"../../BlogData";
		options.BlogPostsFolder = "BlogPosts";
		options.TagsFolder = "Tags";
		options.CategoriesFolder = "Categories";
	});

builder.Services.AddScoped<IBlogApi, BlogApiJsonDirectAccess>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, c =>
{
    c.Authority = builder.Configuration["Auth0:Authority"];
    c.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters{
		ValidAudience = builder.Configuration["Auth0:Audience"],
		ValidIssuer = builder.Configuration["Auth0:Authority"]
	};
});

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapBlogPostApi();
app.MapCategoryApi();
app.MapTagApi();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
