using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PM.API.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<PMAPIContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.")));

builder.Services.AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.MapControllers();

app.Use(async (context, next) =>
{
    if (context.Request.Path.Value.Equals("/download", StringComparison.OrdinalIgnoreCase))
    {
        var redirectPath = builder.Configuration["Redirection:DownloadPath"];
        context.Response.Redirect(redirectPath);
    }
    else
    {
        await next();
    }
});

app.Run();
