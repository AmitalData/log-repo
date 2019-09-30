namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AbedAddBCCFieldToDocumentTypeTemplate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DocumentTypeTemplates", "BCC", c => c.String(maxLength: 4000));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DocumentTypeTemplates", "BCC");
        }
    }
}
