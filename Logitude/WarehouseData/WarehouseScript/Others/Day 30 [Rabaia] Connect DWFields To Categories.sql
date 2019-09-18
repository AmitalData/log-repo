truncate table [DWObjectFieldCategories]

------------------------------------------------------

DECLARE @Id  varchar(50)
DECLARE @NewId  varchar(15)
declare @Cat1Code varchar(150)
declare @Cat2Code varchar(150)

	DECLARE DWObjectFieldsCursor CURSOR READ_ONLY
	FOR	
	SELECT Code,Category1,Category2
	FROM DWObjectFields	where DWObjectTableCode = 'Fact_Shipments'
	OPEN DWObjectFieldsCursor FETCH NEXT FROM DWObjectFieldsCursor INTO @Id,@Cat1Code,@Cat2Code
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
		--set @GeneralInvoiceDocumentTypeId = (select Id from DocumentTypes where Code = '999G' and Tenant = @Tenant)
		
		  
		if(@Cat1Code is not null)
		begin 
		         EXEC   [dbo].[usp_GetNextTableIdValue]
		                @pLastNumber = @NewId OUTPUT,
		                @pTableName = N'DWObjectFieldCategories' 
			insert into [dbo].[DWObjectFieldCategories]([Id],[DWObjectFieldCode],[DWCategoryCode]) values(@NewId,@Id,@Cat1Code) 	
		end		

		if(@Cat1Code is null and @Cat2Code is null)
		begin 
		    EXEC   [dbo].[usp_GetNextTableIdValue]
		                @pLastNumber = @NewId OUTPUT,
		                @pTableName = N'DWObjectFieldCategories'
			insert into [dbo].[DWObjectFieldCategories]([Id],[DWObjectFieldCode],[DWCategoryCode]) values(@NewId,@Id,'General') 	
		end		
		if(@Cat2Code is not null and @Cat2Code <> @Cat1Code)
		begin 
		    EXEC   [dbo].[usp_GetNextTableIdValue]
		                @pLastNumber = @NewId OUTPUT,
		                @pTableName = N'DWObjectFieldCategories'
			insert into [dbo].[DWObjectFieldCategories]([Id],[DWObjectFieldCode],[DWCategoryCode]) values(@NewId,@Id,@Cat2Code) 	
		end		
		if(@Cat2Code is null and @Cat1Code is null)
		begin 
		    EXEC   [dbo].[usp_GetNextTableIdValue]
		                @pLastNumber = @NewId OUTPUT,
		                @pTableName = N'DWObjectFieldCategories'
			insert into [dbo].[DWObjectFieldCategories]([Id],[DWObjectFieldCode],[DWCategoryCode]) values(@NewId,@Id,'General') 	
		end	
		FETCH NEXT FROM DWObjectFieldsCursor INTO @Id,@Cat1Code,@Cat2Code
	END
	CLOSE DWObjectFieldsCursor
	DEALLOCATE DWObjectFieldsCursor