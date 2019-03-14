namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateFieldLength_SearchField_Reconciliaition : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Reconciliations", "SearchFields", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Reconciliations", "SearchFields", c => c.String(maxLength: 1000));
        }
    }
}
