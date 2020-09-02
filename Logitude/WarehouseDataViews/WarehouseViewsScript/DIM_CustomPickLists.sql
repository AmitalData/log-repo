  
SELECT [Id] as [Key]
      ,[Value] 
      ,[Source Tenant] as [Tenant]
  FROM [dbo].[DIM_CustomPickLists]
  where [Code] = @CustomPickListCode or [Code]='-1'