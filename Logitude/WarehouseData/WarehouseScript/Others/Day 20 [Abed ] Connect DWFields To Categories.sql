truncate table [DWObjectFieldCategories]

------------------------------------------------------

DECLARE @Id  varchar(50)
DECLARE @NewId  varchar(15)
declare @Cat1Code varchar(150)
declare @Cat2Code varchar(150)
declare @FactCode varchar(50)

	DECLARE DWObjectFieldsCursor CURSOR READ_ONLY
	FOR	
	SELECT Code,Category1,Category2,DWObjectTableCode
 
	FROM DWObjectFields	where DWObjectTableCode = 'Fact_Shipments' or DWObjectTableCode = 'Fact_Charges' or DWObjectTableCode = 'Fact_Quotes' or DWObjectTableCode = 'Fact_Invoices' or DWObjectTableCode = 'Fact_ARInvoices'
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