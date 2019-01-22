namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CustomsPartnerCommDet : DbMigration
    {
        public override void Up()
        {
            AddColumn("Customs.CustomsPartnerFtps", "CommunicationDetails", c => c.String(maxLength: 2000, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("Customs.CustomsPartnerFtps", "CommunicationDetails");
        }
    }
}
