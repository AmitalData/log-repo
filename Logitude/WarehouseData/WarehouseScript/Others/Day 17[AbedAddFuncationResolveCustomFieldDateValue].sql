-- Shared DB


IF object_id(N'ResolveCustomFieldDateValue', N'FN') IS NOT NULL
BEGIN DROP FUNCTION dbo.ResolveCustomFieldDateValue   end


IF object_id(N'dbo.ResolveCustomFieldDateValue', N'FN') IS  NULL
begin 
declare @functionsDataString as varchar(8000)
set @functionsDataString = 'CREATE FUNCTION dbo.ResolveCustomFieldDateValue (@FieldValue varchar(2000) ,  @DataTypeCode varchar(10))  
RETURNS sql_variant
AS  
BEGIN  
 DECLARE @MyValueOut sql_variant
  DECLARE @Result date
  DECLARE @DateString as varchar(20)
 if(@DataTypeCode = ''Date'' ) begin  set @Result = ''1-1-1''; end 

if(@DataTypeCode = ''DateTime'' and  LEN(@FieldValue) >= 14)
begin
set @DateString = SUBSTRING(@FieldValue, 1, 4) + ''-''+ SUBSTRING(@FieldValue, 5, 2) + ''-''+ SUBSTRING(@FieldValue, 7, 2) +'' ''+ SUBSTRING(@FieldValue, 9, 2) + '':''+ SUBSTRING(@FieldValue, 11, 2) + '':''+ SUBSTRING(@FieldValue, 13, 2) 

set @Result = convert(varchar, Try_CAST(@DateString AS datetime), 20)
-- set @Result = CONVERT(DATE, @Result)

end

ELSE if(@DataTypeCode = ''Date''  and  LEN(@FieldValue) >= 8) 
begin

set @DateString = SUBSTRING(@FieldValue, 1, 4) + ''-''+ SUBSTRING(@FieldValue, 5, 2) + ''-''+ SUBSTRING(@FieldValue, 7, 2)
SET @Result =  convert(varchar, Try_CAST(@DateString AS datetime), 23) 
set @Result = dbo.GetDateFormateAsNumber(CONVERT(DATE, @Result))
end

set @MyValueOut = @Result;
  RETURN(@MyValueOut);  

END;';

 EXEC(@functionsDataString)
end 


 