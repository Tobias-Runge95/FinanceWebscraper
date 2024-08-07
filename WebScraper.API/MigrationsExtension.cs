using Microsoft.EntityFrameworkCore;
using WebScraper.Database;

namespace WebWorkPlace.API.Extensions;

public static class MigrationsExtension
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using (IServiceScope scope = app.ApplicationServices.CreateScope())
        {
            using (WebScrapperDbContext context = scope.ServiceProvider.GetRequiredService<WebScrapperDbContext>())
            {
                context.Database.Migrate();
            }
        }
    }
}