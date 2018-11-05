namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCustomsPartnerFtp : DbMigration
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

            //AlterColumn("dbo.JournalLines", "Reference1", c => c.String(maxLength: 30));
            //AlterColumn("dbo.JournalLines", "Reference2", c => c.String(maxLength: 30));
            //AlterColumn("dbo.JournalLines", "Reference3", c => c.String(maxLength: 30));
            //AlterColumn("dbo.LedgerTransactions", "Reference1", c => c.String(maxLength: 30));
            //AlterColumn("dbo.LedgerTransactions", "Reference2", c => c.String(maxLength: 30));
            //AlterColumn("dbo.LedgerTransactions", "Reference3", c => c.String(maxLength: 30));

            ///CREATE UNIQUE INDEX IX_CPF_PIT ON CUSTOMSPARTNERFTPS (PARTNERCODE ASC, INTERFACENAME ASC, TYPECODE ASC) 
        }

        public override void Down()
        {
            DropForeignKey("Customs.CustomsPartnerFtps", "FtpDetailsId", "dbo.FTPDetails");
            DropIndex("Customs.CustomsPartnerFtps", new[] { "FtpDetailsId" });
            //AlterColumn("dbo.LedgerTransactions", "Reference3", c => c.String(maxLength: 30, unicode: false));
            //AlterColumn("dbo.LedgerTransactions", "Reference2", c => c.String(maxLength: 30, unicode: false));
            //AlterColumn("dbo.LedgerTransactions", "Reference1", c => c.String(maxLength: 30, unicode: false));
            //AlterColumn("dbo.JournalLines", "Reference3", c => c.String(maxLength: 30, unicode: false));
            //AlterColumn("dbo.JournalLines", "Reference2", c => c.String(maxLength: 30, unicode: false));
            //AlterColumn("dbo.JournalLines", "Reference1", c => c.String(maxLength: 30, unicode: false));
            DropTable("Customs.CustomsPartnerFtps");

            //DROP INDEX IX_CPF_PIT;
        }
    }
}
