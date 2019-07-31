namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsNorthAmericaField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Countries", "IsNorthAmerica", c => c.Boolean(nullable: false));

            Sql("update Countries set IsNorthAmerica = 1 where Code in ('US', 'CA', 'MX')");
        }
        
        public override void Down()
        {
            DropColumn("dbo.Countries", "IsNorthAmerica");
        }
    }
}
