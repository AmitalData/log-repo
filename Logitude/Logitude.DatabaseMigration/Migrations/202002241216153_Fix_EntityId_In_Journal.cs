namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Fix_EntityId_In_Journal : DbMigration
    {
        public override void Up()
        {
            Sql("update journals set accountingentityid=id  where AccountingEntityCode ='10'  and accountingentityid!=id");
        }
        
        public override void Down()
        {
        }
    }
}
