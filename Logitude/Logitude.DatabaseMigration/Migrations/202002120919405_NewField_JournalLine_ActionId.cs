namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewField_JournalLine_ActionId : DbMigration
    {
        public override void Up()
        {
            
            AddColumn("dbo.JournalLines", "ActionId", c => c.String(maxLength: 15, unicode: false));



            // move current Ids from ActionCode to ActionId column
            Sql("update  JournalLines set ActionId = ActionCode where ActionCode like '%-%'");

            // fill ActionCode from ActionId
            Sql("update	jl set     jl.ActionCode = t.Code from JournalLines jl join JournalActionTypes t on jl.ActionId = t.Id and jl.Tenant = t.Tenant where   ActionCode like '%-%'");

            // fill ActionId from ActionCode
            Sql("update	jl set     jl.ActionId = t.Id from JournalLines jl join JournalActionTypes t on jl.ActionCode = t.Code and jl.Tenant = t.Tenant where   ActionCode not like '%-%' and jl.ActionId is null");
        }
        
        public override void Down()
        {

            DropColumn("dbo.JournalLines", "ActionId");

            //AlterColumn("dbo.JournalLines", "ActionCode", c => c.String(maxLength: 15, unicode: false));
            //DropForeignKey("dbo.JournalLines", "ActionId", "dbo.JournalActionTypes");
            //DropIndex("dbo.JournalLines", new[] { "ActionId" });
            //CreateIndex("dbo.JournalLines", "ActionCode");
            //AddForeignKey("dbo.JournalLines", "ActionCode", "dbo.JournalActionTypes", "Id");
        }
    }
}
