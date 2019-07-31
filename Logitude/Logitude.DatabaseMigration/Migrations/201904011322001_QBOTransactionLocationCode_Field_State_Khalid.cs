namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class QBOTransactionLocationCode_Field_State_Khalid : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.States", "QBOTransactionLocationCode", c => c.String(maxLength: 3, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.States", "QBOTransactionLocationCode");
        }
    }
}
