namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ARPayment_FechaPago : DbMigration
    {
        public override void Up()
        {
            
            AddColumn("dbo.ARPayments", "FechaPago", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ARPayments", "FechaPago");
             
        }
    }
}
