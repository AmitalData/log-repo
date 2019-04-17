namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTariffSettingTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TariffSettings",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        DefaultPriceSteps = c.String(nullable: false, maxLength: 100, unicode: false),
                        Tenant = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);

            Sql(
                @"
	            declare @Tenant integer
	            declare @NewId as varchar(15)
	            DECLARE TenantsCursor CURSOR READ_ONLY
	            FOR
	            SELECT Id
	            From Tenants
	            OPEN TenantsCursor FETCH NEXT FROM TenantsCursor INTO @Tenant
	            WHILE @@FETCH_STATUS = 0
	            BEGIN
	
	            if not exists (select * from TariffSettings where Tenant = @Tenant)
	            begin
		            EXECUTE usp_GetNextTableIdValue @NewId OUTPUT,'TariffSetting'
		            insert into TariffSettings(Id, Tenant, DefaultPriceSteps) values(@NewId, @Tenant, '0,45,100,250,500')
	            end

	            FETCH NEXT FROM TenantsCursor INTO @Tenant	
	            END
	            CLOSE TenantsCursor
	            DEALLOCATE TenantsCursor
                ");
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TariffSettings");
        }
    }
}
