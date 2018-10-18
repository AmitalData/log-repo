
IF OBJECT_ID('[dbo].[usp_GetPrimaryKeyName]', 'P') IS NOT NULL
drop PROCEDURE [dbo].[usp_GetPrimaryKeyName]
GO

Create PROCEDURE [dbo].[usp_GetPrimaryKeyName]
(
    @keyName as varchar(500) output,
	@tableName as varchar(500) ,
	@columnName as varchar(500)
	
)
AS

set @keyName = (select i.name
from sys.index_columns ic 
    join sys.indexes i on ic.index_id=i.index_id
    join sys.columns c on c.column_id=ic.column_id
where 
    i.[object_id] = object_id(@tableName) and 
    ic.[object_id] = object_id(@tableName) and 
    c.[object_id] = object_id(@tableName) and
    is_primary_key = 1 and c.name=@columnName)

