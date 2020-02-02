namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mutaz_Add_Smallcashbook_Field_To_GLAccount : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GLAccounts", "Smallcashbook", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.GLAccounts", "Smallcashbook");
         }
    }
}
