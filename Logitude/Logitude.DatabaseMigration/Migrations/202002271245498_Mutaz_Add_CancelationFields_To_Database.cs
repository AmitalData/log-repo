namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mutaz_Add_CancelationFields_To_Database : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ARPayments", "AccountingCancelationDate", c => c.DateTime());
            AddColumn("dbo.ARPayments", "CancelationNotes", c => c.String(maxLength: 500));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ARPayments", "CancelationNotes");
            DropColumn("dbo.ARPayments", "AccountingCancelationDate");
        }
    }
}
