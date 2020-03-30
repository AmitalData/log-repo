  
SELECT [Id] as [Key]
      ,[Value]
	  ,[Code]
      ,[Is Multiple Choice]
      ,[Source Tenant] as [Tenant]
  FROM [dbo].[DIM_CustomPickLists]
  where [Code] = @CustomPickListCode or [Code]='-1'