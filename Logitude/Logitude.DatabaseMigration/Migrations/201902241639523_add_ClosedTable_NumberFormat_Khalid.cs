namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_ClosedTable_NumberFormat_Khalid : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NumberFormats",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 4),
                        Name = c.String(),
                        SearchFields = c.String(),
                    })
                .PrimaryKey(t => t.Code);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.NumberFormats");
        }
    }
}
