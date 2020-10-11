namespace Logitude.SystemLogs.OracleMigratrion.Migrations
{
    using Devart.Data.Oracle.Entity.Migrations;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Logitude.SystemLogs.OracleMigratrion.SystemLogContext>
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
                //"User Id=aminet_logs;  Password=aminet_logs;Direct=True;Data Source=Univ56;port=1521;sid=amital"
                "User Id=AMINETCST_LOGS;  Password=AMINETCST_LOGS;Direct=true;Data Source=localhost;port=1521;sid=amital"
                );



            TargetDatabase = connectionInfo;
            SetSqlGenerator(connectionInfo.GetInvariantName(), new OracleEntityMigrationSqlGenerator());
        }

        protected override void Seed(Logitude.SystemLogs.OracleMigratrion.SystemLogContext context)
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
