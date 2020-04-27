namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class DropAPPaymentMethodIdFromAPPayment_Maheera : DbMigration
    {
        public override void Up()
        {
        //    DropForeignKey("dbo.APPayments", "PaymentMethodId", "dbo.APPaymentMethods");
        //    DropIndex("dbo.APPayments", new[] { "PaymentMethodId" });
        //    DropColumn("dbo.APPayments", "PaymentMethodId");
        }

        public override void Down()
        {
            //AddColumn("dbo.APPayments", "PaymentMethodId", c => c.String(nullable: false, maxLength: 15, unicode: false));
            //CreateIndex("dbo.APPayments", "PaymentMethodId");
            //AddForeignKey("dbo.APPayments", "PaymentMethodId", "dbo.APPaymentMethods", "Id");
        }
    }
}
