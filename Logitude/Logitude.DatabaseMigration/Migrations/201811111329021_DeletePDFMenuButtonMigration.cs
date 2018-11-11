namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeletePDFMenuButtonMigration : DbMigration
    {
        public override void Up()
        {

            Sql("delete from TextCodes where ID=(select labeltextcodeid from MenuButtons where EventCode='DNBD')");
            Sql("delete from MenuButtons where eventcode='DNBD'");
        }
        
        public override void Down()
        {
        }
    }
}
