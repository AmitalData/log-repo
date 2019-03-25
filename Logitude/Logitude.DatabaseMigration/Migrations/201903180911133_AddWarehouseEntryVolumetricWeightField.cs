namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddWarehouseEntryVolumetricWeightField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WarehouseEntries", "TotalVolumetricWeight", c => c.Decimal(nullable: false, precision: 18, scale: 3));

            Sql(
                @"
	                declare @WarehouseEntryId as varchar(15)
	                declare @TotalVolumetricWeight as decimal(18,3)

	                DECLARE DataCursor CURSOR READ_ONLY
	                FOR
	                SELECT Id
	                FROM WarehouseEntries
	                OPEN DataCursor FETCH NEXT FROM DataCursor INTO @WarehouseEntryId
	                WHILE @@FETCH_STATUS = 0
	                BEGIN

		                set @TotalVolumetricWeight = (select sum(isnull(VolumetricWeight,0)) from WarehouseEntryPackages where WarehouseEntryId = @WarehouseEntryId)
		
		                update WarehouseEntries set TotalVolumetricWeight = round(@TotalVolumetricWeight,3)	

	                FETCH NEXT FROM DataCursor INTO @WarehouseEntryId
	                END
	                CLOSE DataCursor
	                DEALLOCATE DataCursor
                ");
        }
        
        public override void Down()
        {
            DropColumn("dbo.WarehouseEntries", "TotalVolumetricWeight");
        }
    }
}
