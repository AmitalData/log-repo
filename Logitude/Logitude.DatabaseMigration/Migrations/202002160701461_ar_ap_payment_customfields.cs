namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ar_ap_payment_customfields : DbMigration
    {
        public override void Up()
        {
          
            AddColumn("dbo.APPayments", "Field1", c => c.String(maxLength: 250));
            AddColumn("dbo.APPayments", "Field2", c => c.String(maxLength: 250));
            AddColumn("dbo.APPayments", "Field3", c => c.String(maxLength: 250));
            AddColumn("dbo.APPayments", "Field4", c => c.String(maxLength: 250));
            AddColumn("dbo.APPayments", "Field5", c => c.String(maxLength: 250));
            AddColumn("dbo.APPayments", "Field6", c => c.String(maxLength: 250));
            AddColumn("dbo.APPayments", "Field7", c => c.String(maxLength: 250));
            AddColumn("dbo.APPayments", "Field8", c => c.String(maxLength: 250));
            AddColumn("dbo.APPayments", "Field9", c => c.String(maxLength: 250));
            AddColumn("dbo.APPayments", "Field10", c => c.String(maxLength: 250));

            AddColumn("dbo.ARPayments", "Field1", c => c.String(maxLength: 250));
            AddColumn("dbo.ARPayments", "Field2", c => c.String(maxLength: 250));
            AddColumn("dbo.ARPayments", "Field3", c => c.String(maxLength: 250));
            AddColumn("dbo.ARPayments", "Field4", c => c.String(maxLength: 250));
            AddColumn("dbo.ARPayments", "Field5", c => c.String(maxLength: 250));
            AddColumn("dbo.ARPayments", "Field6", c => c.String(maxLength: 250));
            AddColumn("dbo.ARPayments", "Field7", c => c.String(maxLength: 250));
            AddColumn("dbo.ARPayments", "Field8", c => c.String(maxLength: 250));
            AddColumn("dbo.ARPayments", "Field9", c => c.String(maxLength: 250));
            AddColumn("dbo.ARPayments", "Field10", c => c.String(maxLength: 250));
          
           
        }
        
        public override void Down()
        {
       
            DropColumn("dbo.ARPayments", "Field10");
            DropColumn("dbo.ARPayments", "Field9");
            DropColumn("dbo.ARPayments", "Field8");
            DropColumn("dbo.ARPayments", "Field7");
            DropColumn("dbo.ARPayments", "Field6");
            DropColumn("dbo.ARPayments", "Field5");
            DropColumn("dbo.ARPayments", "Field4");
            DropColumn("dbo.ARPayments", "Field3");
            DropColumn("dbo.ARPayments", "Field2");
            DropColumn("dbo.ARPayments", "Field1");

            DropColumn("dbo.APPayments", "Field10");
            DropColumn("dbo.APPayments", "Field9");
            DropColumn("dbo.APPayments", "Field8");
            DropColumn("dbo.APPayments", "Field7");
            DropColumn("dbo.APPayments", "Field6");
            DropColumn("dbo.APPayments", "Field5");
            DropColumn("dbo.APPayments", "Field4");
            DropColumn("dbo.APPayments", "Field3");
            DropColumn("dbo.APPayments", "Field2");
            DropColumn("dbo.APPayments", "Field1");
          
        }
    }
}
