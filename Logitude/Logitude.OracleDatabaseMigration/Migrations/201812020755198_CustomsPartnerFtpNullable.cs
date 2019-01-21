namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CustomsPartnerFtpNullable : DbMigration
    {
        public override void Up()
        {
            DropIndex("Customs.CustomsPartnerFtps", new[] { "FtpDetailsId" });
            AlterColumn("Customs.CustomsPartnerFtps", "FtpDetailsId", c => c.String(maxLength: 15, unicode: false));
            CreateIndex("Customs.CustomsPartnerFtps", "FtpDetailsId");
        }
        
        public override void Down()
        {
            DropIndex("Customs.CustomsPartnerFtps", new[] { "FtpDetailsId" });
            AlterColumn("Customs.CustomsPartnerFtps", "FtpDetailsId", c => c.String(nullable: false, maxLength: 15, unicode: false));
            CreateIndex("Customs.CustomsPartnerFtps", "FtpDetailsId");
        }
    }
}
