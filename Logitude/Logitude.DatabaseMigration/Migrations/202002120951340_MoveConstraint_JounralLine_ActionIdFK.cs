namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MoveConstraint_JounralLine_ActionIdFK : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.JournalLines", "ActionCode", "dbo.JournalActionTypes");
            DropIndex("dbo.JournalLines", new[] { "ActionCode" });

            AlterColumn("dbo.JournalLines", "ActionCode", c => c.String(maxLength: 2, unicode: false));

            CreateIndex("dbo.JournalLines", "ActionId");
            AddForeignKey("dbo.JournalLines", "ActionId", "dbo.JournalActionTypes", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.JournalLines", "ActionId", "dbo.JournalActionTypes");
            DropIndex("dbo.JournalLines", new[] { "ActionId" });
            AlterColumn("dbo.JournalLines", "ActionCode", c => c.String(maxLength: 15, unicode: false));
            CreateIndex("dbo.JournalLines", "ActionCode");
            AddForeignKey("dbo.JournalLines", "ActionCode", "dbo.JournalActionTypes", "Id");
        }
    }
}
