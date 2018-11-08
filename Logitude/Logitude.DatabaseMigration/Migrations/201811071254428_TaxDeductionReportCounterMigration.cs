namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TaxDeductionReportCounterMigration : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TaxDeductionReports", "ReportNumber", c => c.String(maxLength: 20, unicode: false));
            Sql("declare @Tenant as int declare @NewEntityId as varchar(15) declare @TaxDeductionCounterId as varchar(15) declare @TaxDeductionObjectTableId as varchar(15) set @TaxDeductionObjectTableId = (select Id from ObjectTables where Name = 'TaxDeductionReport') BEGIN DECLARE TenantsCursor CURSOR READ_ONLY FOR SELECT Id FROM Tenants  OPEN TenantsCursor FETCH NEXT FROM TenantsCursor INTO @Tenant WHILE @@FETCH_STATUS = 0 BEGIN  if not exists(select * from Counters where Tenant = @Tenant AND Code = 'TXDC' AND ObjectTableId = @TaxDeductionObjectTableId)  begin EXECUTE usp_GetNextTableIdValue @NewEntityId OUTPUT,'Counter'  insert into Counters(Id, Tenant, Code, Name, ObjectTableId) values(@NewEntityId, @Tenant, 'TXDC', 'Tax Deduction Report', @TaxDeductionObjectTableId) end set @TaxDeductionCounterId = (select Id from Counters where Tenant = @Tenant and ObjectTableId = @TaxDeductionObjectTableId and Code = 'TXDC') if not exists(select * from CounterDefinitions where Tenant = @Tenant AND CounterId = @TaxDeductionCounterId AND Parameter1 = 'TX') begin EXECUTE usp_GetNextTableIdValue @NewEntityId OUTPUT,'CounterDefinition' insert into CounterDefinitions(Id, Tenant, Parameter1, StartNumber, CounterId, Prefix, UniquePerPrefix) values(@NewEntityId, @Tenant, 'TX', 1000, @TaxDeductionCounterId, NULL, 0)  end  FETCH NEXT FROM TenantsCursor INTO @Tenant END  CLOSE TenantsCursor DEALLOCATE TenantsCursor END");
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TaxDeductionReports", "ReportNumber", c => c.Int());
        }
    }
}
