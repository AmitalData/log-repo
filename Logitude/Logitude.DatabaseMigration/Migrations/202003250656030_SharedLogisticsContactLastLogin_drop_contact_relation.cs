namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SharedLogisticsContactLastLogin_drop_contact_relation : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SharedLogisticsContactLastLogins", "ContactId", "dbo.Contacts");
           
            DropIndex("dbo.SharedLogisticsContactLastLogins", new[] { "ContactId" });
           
        }
        
        public override void Down()
        {
           
            CreateIndex("dbo.SharedLogisticsContactLastLogins", "ContactId");
        
            AddForeignKey("dbo.SharedLogisticsContactLastLogins", "ContactId", "dbo.Contacts", "Id");
        }
    }
}
