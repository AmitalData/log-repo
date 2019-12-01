namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addgussment : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.QuoteSettings", "AutomaticallyCloseDays", c => c.Int(nullable: false));
            AlterColumn("dbo.InterestBasesPeriods", "InterestRate", c => c.Decimal(nullable: false, precision: 4, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.InterestBasesPeriods", "InterestRate", c => c.Decimal(nullable: false, precision: 2, scale: 2));
            //DropColumn("dbo.QuoteSettings", "AutomaticallyCloseDays");
        }
    }
}
