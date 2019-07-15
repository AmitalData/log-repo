namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CustomersTextCodesMigration : DbMigration
    {
        public override void Up()
        {
            Sql("update TextCodes  set LocalDefaultText =N'??????' where Code ='General.MH.Customers'");
            Sql("update TextCodes  set LocalDefaultText =N'????' where Code ='Customer'");

        }
        
        public override void Down()
        {
        }
    }
}
