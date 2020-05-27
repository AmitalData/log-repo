namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class objectfield_fieldcode_unique : DbMigration
    {
        public override void Up()
        {
             
            CreateIndex("dbo.ObjectFields", "FieldCode", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.ObjectFields", new[] { "FieldCode" });
             
        }
    }
}
