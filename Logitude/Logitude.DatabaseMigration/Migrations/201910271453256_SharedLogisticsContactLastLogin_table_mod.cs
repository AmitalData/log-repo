namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SharedLogisticsContactLastLogin_table_mod : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SharedLogisticsContactLastLogins", "CardId", c => c.String(nullable: false, maxLength: 15, unicode: false));
            AlterColumn("dbo.SharedLogisticsContactLastLogins", "PartnerTypeId", c => c.String(nullable: false, maxLength: 2, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SharedLogisticsContactLastLogins", "PartnerTypeId", c => c.String(maxLength: 2, unicode: false));
            AlterColumn("dbo.SharedLogisticsContactLastLogins", "CardId", c => c.String(maxLength: 15, unicode: false));
        }
    }
}
