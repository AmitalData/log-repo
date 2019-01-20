namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class textcode_fixes : DbMigration
    {
        public override void Up()
        {
            Sql(@"update TextCodes set Code = 'TenantManagmentPrivateLabels.F.PrivateLabelShortName' where Code= 'TenantManagmentPrivateLabels.F.Private Label Short Name'
update TextCodes set Code = 'TenantManagmentPrivateLabels.F.PrivateLabelName' where Code= 'TenantManagmentPrivateLabels.F.Private Label Name' and ObjectTableId in (select Id from ObjectTables where Name = 'TenantManagmentPrivateLabels')
update TextCodes set Code = 'TenantManagmentPrivateLabels.F.PrivateLabelUrl' where Code= 'TenantManagmentPrivateLabels.F.Private Label Url' and ObjectTableId in (select Id from ObjectTables where Name = 'TenantManagmentPrivateLabels')
update TextCodes set Code = 'TenantManagmentPrivateLabels.F.ContactUsEmail' where Code= 'TenantManagmentPrivateLabels.F.Contact Us Email' and ObjectTableId in (select Id from ObjectTables where Name = 'TenantManagmentPrivateLabels')
update TextCodes set Code = 'TenantManagmentPrivateLabels.F.ReceiveAllStatuses' where Code= 'TenantManagmentPrivateLabels.F.Receive All Statuses' and ObjectTableId in (select Id from ObjectTables where Name = 'TenantManagmentPrivateLabels')
update TextCodes set Code = 'TenantManagmentPrivateLabels.F.InActive' where Code= 'TenantManagmentPrivateLabels.F.In Active' and ObjectTableId in (select Id from ObjectTables where Name = 'TenantManagmentPrivateLabels')
");
        }
        
        public override void Down()
        {
         
        }
    }
}
