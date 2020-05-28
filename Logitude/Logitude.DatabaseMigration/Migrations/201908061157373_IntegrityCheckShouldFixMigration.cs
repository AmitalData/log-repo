namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IntegrityCheckShouldFixMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AccountingIntegrityChecks", "ShouldFix", c => c.Boolean(nullable: false));
            Sql("delete from ScreenFields where ObjectFieldId in(select ID from ObjectFields where FieldName ='HasException') and ScreenId =(select Id from Screens where ObjectTableId = (select ID from ObjectTables where Name='accountingintegritycheck'))");
        }
        
        public override void Down()
        {
            DropColumn("dbo.AccountingIntegrityChecks", "ShouldFix");
        }
    }
}
