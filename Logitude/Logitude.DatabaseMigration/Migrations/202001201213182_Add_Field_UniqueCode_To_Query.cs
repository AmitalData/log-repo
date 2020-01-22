namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Field_UniqueCode_To_Query : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Queries", "UniqeCode", c => c.String(nullable: true, maxLength: 200, unicode: false));
            //Sql(@"update Queries set UniqeCode = (select ObjectTables.Name from ObjectTables where Id= Queries.ObjectTableId)+'.'+Queries.Code");
            //AlterColumn("dbo.Queries", "UniqeCode", c => c.String(nullable: false, maxLength: 30, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Queries", "UniqeCode");
        }
    }
}
