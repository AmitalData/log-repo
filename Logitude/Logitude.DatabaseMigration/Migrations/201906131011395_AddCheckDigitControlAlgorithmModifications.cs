namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCheckDigitControlAlgorithmModifications : DbMigration
    {
        public override void Up()
        {
            Sql(
                @"
                if not exists (select * from CheckDigitControlAlgorithms where Code = 'NONE')
                insert into CheckDigitControlAlgorithms(Code, Name, SearchFields) values('NONE', 'None', 'NONE, None')
                ");

            Sql(
                @"
                if not exists (select * from CheckDigitControlAlgorithms where Code = 'LUHN')
                insert into CheckDigitControlAlgorithms(Code, Name, SearchFields) values('LUHN', 'Luhn Algorithm', 'LUHN, Luhn Algorithm')
                ");

            Sql("update Tenants set CheckDigitControlAlgorithmCode = 'NONE' where CheckDigitControlAlgorithmCode is null");

            DropIndex("dbo.Tenants", new[] { "CheckDigitControlAlgorithmCode" });
            AlterColumn("dbo.Tenants", "CheckDigitControlAlgorithmCode", c => c.String(nullable: false, maxLength: 4, unicode: false));
            CreateIndex("dbo.Tenants", "CheckDigitControlAlgorithmCode");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Tenants", new[] { "CheckDigitControlAlgorithmCode" });
            AlterColumn("dbo.Tenants", "CheckDigitControlAlgorithmCode", c => c.String(maxLength: 4, unicode: false));
            CreateIndex("dbo.Tenants", "CheckDigitControlAlgorithmCode");
        }
    }
}
