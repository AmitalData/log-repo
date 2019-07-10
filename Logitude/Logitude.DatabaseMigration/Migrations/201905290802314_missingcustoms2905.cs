namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class missingcustoms2905 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Customs.Declarations", "MamanStatusCode", "Customs.MamanStatuses");
            DropIndex("Customs.Declarations", new[] { "MamanStatusCode" });
            CreateTable(
                "dbo.CustomsPartnerFtps",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Tenant = c.Int(nullable: false),
                        TypeCode = c.String(),
                        PartnerCode = c.String(),
                        InterfaceName = c.String(),
                        FtpDetailsId = c.String(maxLength: 15, unicode: false),
                        FileName = c.String(),
                        FileExt = c.String(),
                        CommunicationDetails = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FTPDetails", t => t.FtpDetailsId)
                .Index(t => t.FtpDetailsId);
            
            CreateTable(
                "dbo.TPGFileTypes",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 128),
                        LocalName = c.String(),
                        SearchFields = c.String(),
                        EnglishName = c.String(),
                        Inactive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Code);
            
            AddColumn("Customs.CourierMasters", "IntegratorCode", c => c.String(maxLength: 15, unicode: false));
            AddColumn("Customs.SupplierInvoices", "ChangeInSupplierInvoice", c => c.String(maxLength: 1, unicode: false));
            CreateIndex("Customs.CourierMasters", "IntegratorCode");
            AddForeignKey("Customs.CourierMasters", "IntegratorCode", "dbo.Cards", "Id");
            DropColumn("Customs.Declarations", "MamanStatusCode");
            DropColumn("Customs.Declarations", "MamanErrorXml");
        }
        
        public override void Down()
        {
            AddColumn("Customs.Declarations", "MamanErrorXml", c => c.String(unicode: false));
            AddColumn("Customs.Declarations", "MamanStatusCode", c => c.String(maxLength: 3, unicode: false));
            DropForeignKey("dbo.CustomsPartnerFtps", "FtpDetailsId", "dbo.FTPDetails");
            DropForeignKey("Customs.CourierMasters", "IntegratorCode", "dbo.Cards");
            DropIndex("dbo.CustomsPartnerFtps", new[] { "FtpDetailsId" });
            DropIndex("Customs.CourierMasters", new[] { "IntegratorCode" });
            DropColumn("Customs.SupplierInvoices", "ChangeInSupplierInvoice");
            DropColumn("Customs.CourierMasters", "IntegratorCode");
            DropTable("dbo.TPGFileTypes");
            DropTable("dbo.CustomsPartnerFtps");
            CreateIndex("Customs.Declarations", "MamanStatusCode");
            AddForeignKey("Customs.Declarations", "MamanStatusCode", "Customs.MamanStatuses", "Code");
        }
    }
}
