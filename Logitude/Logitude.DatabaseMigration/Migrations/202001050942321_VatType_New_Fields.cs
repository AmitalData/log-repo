namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VatType_New_Fields : DbMigration
    {
        public override void Up()
        {
           // AddColumn("dbo.VatTypes", "PayablesExternalId", c => c.String(maxLength: 25, unicode: false));
           // AddColumn("dbo.VatTypes", "ReceivablesExternalId", c => c.String(maxLength: 25, unicode: false));
            Sql("update vattypes set ReceivablesExternalId=ExternalVATCard");

        }

        public override void Down()
        {
            DropColumn("dbo.VatTypes", "ReceivablesExternalId");
            DropColumn("dbo.VatTypes", "PayablesExternalId");
        }
    }
}
