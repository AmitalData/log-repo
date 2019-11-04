namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DecCreateIndex : DbMigration
    {
        public override void Up()
        {

            //CREATE INDEX IX_DECLARATIONS_CUSTOMFILENO ON DECLARATIONS(CUSTOMFILENO ASC);
            CreateIndex("DECLARATIONS", "CUSTOMFILENO",name: "IX_DECLARATIONS_CUSTOMFILENO");
            CreateIndex("DECLARATIONS", "ISPAYMENTPROTESTED", name: "IX_DECLARATIONS_ISPTPROTESTED");
        }
        
        public override void Down()
        {
            DropIndex("DECLARATIONS","IX_DECLARATIONS_CUSTOMFILENO");
            DropIndex("DECLARATIONS", "IX_DECLARATIONS_ISPTPROTESTED");
        }
    }
}
