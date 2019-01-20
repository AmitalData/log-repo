namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class textcode_fixes2 : DbMigration
    {
        public override void Up()
        {
            Sql(@"update TextCodes set Code = 'Customer.F.StateId_Potential' where Code= 'Customer.F.State_Potential' and ObjectTableId in (select Id from ObjectTables where Name = 'Customer')
 update TextCodes set Code = 'Customer.F.EnglishName' where Code= 'Customer.F.Name' and ObjectTableId in (select Id from ObjectTables where Name = 'Customer')
 update TextCodes set Code = 'Customer.F.PaymentTermEnglishName' where Code= 'Customer.F.PaymentTermName' and ObjectTableId in (select Id from ObjectTables where Name = 'Customer')
 update TextCodes set Code = 'Customer.F.SalesmanUserEnglishName' where Code= 'Customer.F.SalesmanUserName' and ObjectTableId in (select Id from ObjectTables where Name = 'Customer')
 ");
        }
        
        public override void Down()
        {
        }
    }
}
