namespace Logitude.OracleDatabaseMigration.Migrations
{
    using Devart.Data.Oracle.Entity.Migrations;
    using System;

    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Logitude.OracleDatabaseMigration.LogitudeModel.LogitudeMigrationContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;

 
            var config = Devart.Data.Oracle.Entity.Configuration.OracleEntityProviderConfig.Instance;
            config.Workarounds.ColumnTypeCasingConventionCompatibility = true; //if 
            config.Workarounds.DisableQuoting = true;
            // Apply the IgnoreSchemaName workaround
            config.Workarounds.IgnoreSchemaName = true;
            config.CodeFirstOptions.TruncateLongDefaultNames = true;
            //Devart.Data.Oracle.Entity.OracleEntityProviderServices.HandleNullStringsAsEmptyStrings = true;  



            // Create a custom connection to specify the database and set a SQL generator for MySql.
            var connectionInfo =
                //OracleConnectionInfo.CreateConnection("User Id=devart;  Password=devart; Server=srv64bit;");
                OracleConnectionInfo.CreateConnection(
               //"User Id=AMINET_MAIN;  Password=AMINET_MAIN;Direct=True;Data Source=Univ57;port=1521;sid=amital"
                "User Id=AMINET_MAIN;  Password=AMINET_MAIN;Direct=True;Data Source=Univ58;port=1521;sid=amital"

                );



            TargetDatabase = connectionInfo;
            SetSqlGenerator(connectionInfo.GetInvariantName(), new OracleEntityMigrationSqlGenerator());
         
            // Enable automatic migrations if you like


        }

        protected override void Seed(Logitude.OracleDatabaseMigration.LogitudeModel.LogitudeMigrationContext context)
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
