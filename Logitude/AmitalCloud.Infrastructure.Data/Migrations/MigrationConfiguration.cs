namespace AmitalCloud.Infrastructure.Data.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

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
