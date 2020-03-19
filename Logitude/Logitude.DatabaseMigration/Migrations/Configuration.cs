namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Diagnostics;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Logitude.DatabaseMigration.LogitudeModel.LogitudeMigrationContext>
    {
        public Configuration()
        {
            ///Debugger.Launch();
            ///CommandTimeout = 5; // migration timeout
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Logitude.DatabaseMigration.LogitudeModel.LogitudeMigrationContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
