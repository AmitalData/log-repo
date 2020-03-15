namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ExceptionReasons : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Customs.ReferantExceptions", "ExceptionReasonsCode", "Customs.ExceptionReasons");
            DropIndex("Customs.ReferantExceptions", new[] { "ExceptionReasonsCode" });
            DropPrimaryKey("Customs.ExceptionReasons");
            DropPrimaryKey("Customs.ReferantExceptions");
            AlterColumn("Customs.ExceptionReasons", "Code", c => c.String(nullable: false, maxLength: 4, unicode: false));
            AlterColumn("Customs.ReferantExceptions", "ExceptionReasonsCode", c => c.String(nullable: false, maxLength: 4, unicode: false));
            AddPrimaryKey("Customs.ExceptionReasons", "Code");
            AddPrimaryKey("Customs.ReferantExceptions", new[] { "DeclarationId", "ExceptionReasonsCode" });
            CreateIndex("Customs.ReferantExceptions", "ExceptionReasonsCode");
            AddForeignKey("Customs.ReferantExceptions", "ExceptionReasonsCode", "Customs.ExceptionReasons", "Code");
        }
        
        public override void Down()
        {
            DropForeignKey("Customs.ReferantExceptions", "ExceptionReasonsCode", "Customs.ExceptionReasons");
            DropIndex("Customs.ReferantExceptions", new[] { "ExceptionReasonsCode" });
            DropPrimaryKey("Customs.ReferantExceptions");
            DropPrimaryKey("Customs.ExceptionReasons");
            AlterColumn("Customs.ReferantExceptions", "ExceptionReasonsCode", c => c.String(nullable: false, maxLength: 4));
            AlterColumn("Customs.ExceptionReasons", "Code", c => c.String(nullable: false, maxLength: 4));
            AddPrimaryKey("Customs.ReferantExceptions", new[] { "DeclarationId", "ExceptionReasonsCode" });
            AddPrimaryKey("Customs.ExceptionReasons", "Code");
            CreateIndex("Customs.ReferantExceptions", "ExceptionReasonsCode");
            AddForeignKey("Customs.ReferantExceptions", "ExceptionReasonsCode", "Customs.ExceptionReasons", "Code");
        }
    }
}
