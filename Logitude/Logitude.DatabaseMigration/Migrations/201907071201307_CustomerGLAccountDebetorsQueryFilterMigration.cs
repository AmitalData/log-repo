namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CustomerGLAccountDebetorsQueryFilterMigration : DbMigration
    {
        public override void Up()
        {
            Sql(" delete from AdvancedQueryFilters where QueryId =(select ID from Queries where Code ='DebetorsCustomers' and objecttableid =(select ID from ObjectTables where Name='glaccount'))");
        }
        
        public override void Down()
        {
        }
    }
}
