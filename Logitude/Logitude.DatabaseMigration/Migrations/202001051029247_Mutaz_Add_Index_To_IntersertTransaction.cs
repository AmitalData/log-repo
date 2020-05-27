namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mutaz_Add_Index_To_IntersertTransaction : DbMigration
    {
        public override void Up()
        {
            Sql("Create index Ix_IT_Entity on InterestTransactions(InterestEntityTypeCode, EntityId)");
         }
        
        public override void Down()
        {
         }
    }
}
