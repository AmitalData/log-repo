namespace Simplog.Global.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_BluesnapContractType_Table_Khalid : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BluesnapContractTypes",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 4, unicode: false),
                        Name = c.String(nullable: false, maxLength: 40, unicode: false),
                        SearchFields = c.String(maxLength: 1000),
                    })
                .PrimaryKey(t => t.Code);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.BluesnapContractTypes");
        }
    }
}
