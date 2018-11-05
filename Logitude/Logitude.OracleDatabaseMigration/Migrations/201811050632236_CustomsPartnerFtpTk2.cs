namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CustomsPartnerFtpTk2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Customs.CustomsPartnerFtps", "FtpDetailsId", "dbo.FTPDetails");
            DropIndex("Customs.CustomsPartnerFtps", new[] { "FtpDetailsId" });
            DropTable("Customs.CustomsPartnerFtps");
        }
        
        public override void Down()
        {
            CreateTable(
                "Customs.CustomsPartnerFtps",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        TypeCode = c.String(nullable: false, maxLength: 3, unicode: false),
                        PartnerCode = c.String(nullable: false, maxLength: 32, unicode: false),
                        InterfaceName = c.String(nullable: false, maxLength: 32, unicode: false),
                        FtpDetailsId = c.String(nullable: false, maxLength: 15, unicode: false),
                        FileName = c.String(maxLength: 256, unicode: false),
                        FileExt = c.String(maxLength: 3, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("Customs.CustomsPartnerFtps", "FtpDetailsId");
            AddForeignKey("Customs.CustomsPartnerFtps", "FtpDetailsId", "dbo.FTPDetails", "Id");
        }
    }
}
