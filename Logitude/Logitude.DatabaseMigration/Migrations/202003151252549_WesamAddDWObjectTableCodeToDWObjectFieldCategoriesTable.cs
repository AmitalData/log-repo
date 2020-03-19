namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WesamAddDWObjectTableCodeToDWObjectFieldCategoriesTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DWObjectFieldCategories", "DWObjectTableCode", c => c.String(nullable: true, maxLength: 50, unicode: false));
            Sql(@"
                truncate table [DWObjectFieldCategories]

INSERT INTO [dbo].[DWCategories] ([Code],[Name],[Index]) VALUES('Charges','Charges',70)

------------------------------------------------------

DECLARE @Id  varchar(50)
DECLARE @NewId  varchar(15)
declare @Cat1Code varchar(150)
declare @Cat2Code varchar(150)
declare @FactCode varchar(50)

	DECLARE DWObjectFieldsCursor CURSOR READ_ONLY
	FOR	
	SELECT Code,Category1,Category2,DWObjectTableCode
	FROM DWObjectFields	where DWObjectTableCode = 'Fact_Shipments' or DWObjectTableCode = 'Fact_Charges'
	OPEN DWObjectFieldsCursor FETCH NEXT FROM DWObjectFieldsCursor INTO @Id,@Cat1Code,@Cat2Code,@FactCode
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
		--set @GeneralInvoiceDocumentTypeId = (select Id from DocumentTypes where Code = '999G' and Tenant = @Tenant)
		
		  
		if(@Cat1Code is not null)
		begin 
		         EXEC   [dbo].[usp_GetNextTableIdValue]
		                @pLastNumber = @NewId OUTPUT,
		                @pTableName = N'DWObjectFieldCategories' 
			insert into [dbo].[DWObjectFieldCategories]([Id],[DWObjectFieldCode],[DWCategoryCode],[DWObjectTableCode]) values(@NewId,@Id,@Cat1Code,@FactCode) 	
		end		

		if(@Cat1Code is null and @Cat2Code is null)
		begin 
		    EXEC   [dbo].[usp_GetNextTableIdValue]
		                @pLastNumber = @NewId OUTPUT,
		                @pTableName = N'DWObjectFieldCategories'
			insert into [dbo].[DWObjectFieldCategories]([Id],[DWObjectFieldCode],[DWCategoryCode],[DWObjectTableCode]) values(@NewId,@Id,'General',@FactCode) 	
		end		
		if(@Cat2Code is not null and @Cat2Code <> @Cat1Code)
		begin 
		    EXEC   [dbo].[usp_GetNextTableIdValue]
		                @pLastNumber = @NewId OUTPUT,
		                @pTableName = N'DWObjectFieldCategories'
			insert into [dbo].[DWObjectFieldCategories]([Id],[DWObjectFieldCode],[DWCategoryCode],[DWObjectTableCode]) values(@NewId,@Id,@Cat2Code,@FactCode) 	
		end		
		if(@Cat2Code is null and @Cat1Code is null)
		begin 
		    EXEC   [dbo].[usp_GetNextTableIdValue]
		                @pLastNumber = @NewId OUTPUT,
		                @pTableName = N'DWObjectFieldCategories'
			insert into [dbo].[DWObjectFieldCategories]([Id],[DWObjectFieldCode],[DWCategoryCode],[DWObjectTableCode]) values(@NewId,@Id,'General',@FactCode) 	
		end	
		FETCH NEXT FROM DWObjectFieldsCursor INTO @Id,@Cat1Code,@Cat2Code,@FactCode
	END
	CLOSE DWObjectFieldsCursor
	DEALLOCATE DWObjectFieldsCursor
            ");
            AlterColumn("dbo.DWObjectFieldCategories", "DWObjectTableCode", c => c.String(nullable: false, maxLength: 50, unicode: false));
        }

        public override void Down()
        {
            DropColumn("dbo.DWObjectFieldCategories", "DWObjectTableCode");
        }
    }
}
