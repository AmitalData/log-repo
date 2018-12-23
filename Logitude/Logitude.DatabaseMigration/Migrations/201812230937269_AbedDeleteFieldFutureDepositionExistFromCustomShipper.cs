namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AbedDeleteFieldFutureDepositionExistFromCustomShipper : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.CustomsShippers", "FutureDepositionExist");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CustomsShippers", "FutureDepositionExist", c => c.Boolean(nullable: false));
        }
    }
}
