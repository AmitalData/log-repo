namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addgusment_Interest_Fields : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.InterestBasesPeriods", "InterestRate", c => c.Decimal(nullable: false, precision: 2, scale: 2));
            AlterColumn("dbo.InterestBasesTypes", "Code", c => c.String(nullable: false, maxLength: 4, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.InterestBasesTypes", "Code", c => c.Int(nullable: false));
            AlterColumn("dbo.InterestBasesPeriods", "InterestRate", c => c.Decimal(nullable: false, precision: 4, scale: 2));
        }
    }
}
