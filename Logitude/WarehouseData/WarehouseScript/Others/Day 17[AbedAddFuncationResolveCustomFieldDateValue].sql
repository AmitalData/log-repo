-- Shared DB


--IF object_id(N'ResolveCustomFieldDateValue', N'FN') IS NOT NULL
--BEGIN DROP FUNCTION dbo.ResolveCustomFieldDateValue   end


IF object_id(N'dbo.ResolveCustomFieldDateValue', N'FN') IS  NULL
begin 
declare @functionsDataString as varchar(8000)
set @functionsDataString = 'CREATE FUNCTION dbo.ResolveCustomFieldDateValue (@FieldValue varchar(2000) ,  @DataTypeCode varchar(10))  
RETURNS sql_variant
AS  
BEGIN  
 DECLARE @MyValueOut sql_variant
 set @MyValueOut = -1;

if(@DataTypeCode = ''DateTime'' and  LEN(@FieldValue) >= 14)
begin
set @MyValueOut = SUBSTRING(@FieldValue, 1, 4) + ''-''+ SUBSTRING(@FieldValue, 5, 2) + ''-''+ SUBSTRING(@FieldValue, 7, 2) +'' ''+ SUBSTRING(@FieldValue, 9, 2) + '':''+ SUBSTRING(@FieldValue, 11, 2) + '':''+ SUBSTRING(@FieldValue, 13, 2) 
set @MyValueOut = convert(varchar, CAST(@MyValueOut AS datetime), 20)
set @MyValueOut = dbo.GetDateFormateAsNumber(CONVERT(DATE, @MyValueOut))
end

ELSE if(@DataTypeCode = ''Date''  and  LEN(@FieldValue) >= 8) 
begin 
set @MyValueOut = SUBSTRING(@FieldValue, 1, 4) + ''-''+ SUBSTRING(@FieldValue, 5, 2) + ''-''+ SUBSTRING(@FieldValue, 7, 2)
SET @MyValueOut =  convert(varchar, CAST(@MyValueOut AS datetime), 23) 
set @MyValueOut = dbo.GetDateFormateAsNumber(CONVERT(DATE, @MyValueOut))
end

  RETURN(@MyValueOut);  

END;';

 EXEC(@functionsDataString)
end 


 