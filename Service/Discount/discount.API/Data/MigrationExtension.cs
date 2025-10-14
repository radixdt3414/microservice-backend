using Microsoft.EntityFrameworkCore;

namespace discount.API.Data
{
    public static class MigrationExtension
    {
        public static IApplicationBuilder UseMigration(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var context =  scope.ServiceProvider.GetRequiredService<DiscountContext>();
            context.Database.MigrateAsync();
            return app;
        }
    }
}
