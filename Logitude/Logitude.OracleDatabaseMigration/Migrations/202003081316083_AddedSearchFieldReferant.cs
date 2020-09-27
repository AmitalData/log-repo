namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedSearchFieldReferant : DbMigration
    {
        public override void Up()
        {
            AddColumn("Customs.DeclarationReferantDatas", "SearchFields", c => c.String(maxLength: 1000));
        }
        
        public override void Down()
        {
            DropColumn("Customs.DeclarationReferantDatas", "SearchFields");
        }
    }
}
