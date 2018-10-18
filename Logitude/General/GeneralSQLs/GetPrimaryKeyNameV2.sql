

IF OBJECT_ID('[dbo].[usp_GetPrimaryKeyName]', 'P') IS NOT NULL
drop PROCEDURE [dbo].[usp_GetPrimaryKeyName]
GO

Create PROCEDURE [dbo].[usp_GetPrimaryKeyName]
(
    @keyName as varchar(500) output,
	@tableName as varchar(500) 
	
)
AS



set @keyName= (SELECT top (1) KU.CONSTRAINT_NAME
FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS TC
INNER JOIN
INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS KU
ON TC.CONSTRAINT_TYPE = 'PRIMARY KEY' AND
TC.CONSTRAINT_NAME = KU.CONSTRAINT_NAME
and ku.table_name=@tableName
ORDER BY KU.TABLE_NAME, KU.ORDINAL_POSITION);





