namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFTPDeatilsPropertiesForAccountingTransfer_Samar : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AccountingSystems", "CanTransferToFTP", c => c.Boolean(nullable: false));
            AddColumn("dbo.AccountingSettings", "TransferToFTPActivated", c => c.Boolean(nullable: false));
            AddColumn("dbo.AccountingSettings", "TransferFTPDetailId", c => c.String(maxLength: 15, unicode: false));
            CreateIndex("dbo.AccountingSettings", "TransferFTPDetailId");            
            AddForeignKey("dbo.AccountingSettings", "TransferFTPDetailId", "dbo.FTPDetails", "Id");

            Sql("update AccountingSystems set CanTransferToFTP = 1 where Code = 'AI' or Code = 'GI'");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AccountingSettings", "TransferFTPDetailId", "dbo.FTPDetails");           
            DropIndex("dbo.AccountingSettings", new[] { "TransferFTPDetailId" });
            DropColumn("dbo.AccountingSystems", "CanTransferToFTP");
            DropColumn("dbo.AccountingSettings", "TransferFTPDetailId");
            DropColumn("dbo.AccountingSettings", "TransferToFTPActivated");            
        }
    }
}
