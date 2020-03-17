namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Abed_FixNameLengthInUsoCFDI : DbMigration
    {
        public override void Up()
        {
            //AlterColumn("dbo.UsoCFDIs", "Name", c => c.String(nullable: false, maxLength: 100, unicode: false));
        }
        
        public override void Down()
        {
            //AlterColumn("dbo.UsoCFDIs", "Name", c => c.String(nullable: false, maxLength: 40, unicode: false));
        }
    }
}
