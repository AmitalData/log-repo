namespace Simplog.Global.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BluesnapTransaction_table_khalid : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BluesnapTransactions",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        TransactionDate = c.DateTime(),
                        DocumentId = c.String(maxLength: 15, unicode: false),
                        LogitudeAmital = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
           
        }
        
        public override void Down()
        {
            DropTable("dbo.BluesnapTransactions");
        }
    }
}
