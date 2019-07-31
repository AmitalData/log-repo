namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeServiceClassNameTOProcCode_Rabaia : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TasksScheduler", "ProcedureCode", c => c.String(maxLength: 100, unicode: false));
            Sql("update TasksScheduler set ProcedureCode = ServiceClassName ");
            //AlterColumn("dbo.ObjectTables", "NameField", c => c.String(maxLength: 60, unicode: false));
            DropColumn("dbo.TasksScheduler", "ServiceClassName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TasksScheduler", "ServiceClassName", c => c.String(maxLength: 100, unicode: false));
            //AlterColumn("dbo.ObjectTables", "NameField", c => c.String(maxLength: 15, unicode: false));
            DropColumn("dbo.TasksScheduler", "ProcedureCode");
        }
    }
}
