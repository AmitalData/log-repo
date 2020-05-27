namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteDateForVATInterestHelpTextMutaz : DbMigration
    {
        public override void Up()
        {
            Sql("DELETE FROM ObjectFields WHERE FieldName='DateForVATInterest'");
            Sql("DELETE FROM TextCodes WHERE Code='ARInvoice.DateForVATInterestHelpText'");
        }
        
        public override void Down()
        {
        }
    }
}
