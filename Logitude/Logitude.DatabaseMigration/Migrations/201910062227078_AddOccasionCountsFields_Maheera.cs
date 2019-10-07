namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOccasionCountsFields_Maheera : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Occasions", "ParticipatedCustomers", c => c.Int(nullable: false));
            AddColumn("dbo.Occasions", "ParticipatedContacts", c => c.Int(nullable: false));
            AddColumn("dbo.Occasions", "InvitedCustomers", c => c.Int(nullable: false));
            AddColumn("dbo.Occasions", "InvitedContacts", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Occasions", "InvitedContacts");
            DropColumn("dbo.Occasions", "InvitedCustomers");
            DropColumn("dbo.Occasions", "ParticipatedContacts");
            DropColumn("dbo.Occasions", "ParticipatedCustomers");
        }
    }
}
