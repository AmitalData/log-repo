using System.Data.Entity;
namespace AmitalCloud.Infrastructure.Data.Helpers
{
    using System.Data.Entity.Migrations;

    internal sealed class MigrationConfiguration<TContext> : DbMigrationsConfiguration<TContext> where TContext : DbContext
    {
        public MigrationConfiguration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(TContext context)
        {
        }
    }
}
