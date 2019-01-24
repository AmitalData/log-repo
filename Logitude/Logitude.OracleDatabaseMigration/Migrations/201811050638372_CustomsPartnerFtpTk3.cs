namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CustomsPartnerFtpTk3 : DbMigration
    {
        public override void Up()
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FTPDetails", t => t.FtpDetailsId)
                .Index(t => t.FtpDetailsId);
            //Right now we didn’t manage to implement the lxml to support indexing in the db, so you must do it manually

            //CREATE UNIQUE INDEX IX_CPF_PIT ON CUSTOMSPARTNERFTPS(PARTNERCODE ASC, INTERFACENAME ASC, TYPECODE ASC) 
            CreateIndex(table: "Customs.CustomsPartnerFtps", name: "IX_CPF_PIT", unique: false, columns: new[] { "PARTNERCODE", "INTERFACENAME", "TYPECODE" });

        }
        
        public override void Down()
        {
            DropIndex("Customs.CustomsPartnerFtps", columns: new[] { "PARTNERCODE", "INTERFACENAME", "TYPECODE" });
            DropForeignKey("Customs.CustomsPartnerFtps", "FtpDetailsId", "dbo.FTPDetails");
            DropIndex("Customs.CustomsPartnerFtps", new[] { "FtpDetailsId" });
            DropTable("Customs.CustomsPartnerFtps");
        }
    }
}
