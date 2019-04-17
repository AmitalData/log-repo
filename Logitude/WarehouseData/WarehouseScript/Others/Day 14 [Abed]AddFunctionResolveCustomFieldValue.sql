
---- Shared DB
IF object_id(N'ResolveCustomFieldValue', N'FN') IS NOT NULL
  BEGIN DROP FUNCTION ResolveCustomFieldValue   end


IF object_id(N'dbo.ResolveCustomFieldValue', N'FN') IS  NULL
begin 
declare @functionsDataString as varchar(8000)
set @functionsDataString = 'CREATE FUNCTION dbo.ResolveCustomFieldValue (@FieldValue varchar(2000) ,  @DataTypeCode varchar(10))  
RETURNS sql_variant
AS  
BEGIN  
 DECLARE @MyValueOut sql_variant


if(@DataTypeCode = ''Date'' or  @DataTypeCode = ''DateTime'')
begin
set @MyValueOut = dbo.ResolveCustomFieldDateValue(@FieldValue,@DataTypeCode);
end

ELSE if(@DataTypeCode = ''Boolean'') 
begin
Set @MyValueOut = 0;
if(@FieldValue is not null)
begin SET @MyValueOut =  CAST(@FieldValue AS bit);   end
end


ELSE if(@DataTypeCode = ''Integer'' or @DataTypeCode = ''UnsInteger'') begin SET @MyValueOut =  CAST(@FieldValue AS int); end

ELSE if(@DataTypeCode = ''Decimal'' or @DataTypeCode = ''UnsDecimal'') 
begin 
if(len(@FieldValue)>=15)begin  set @FieldValue = STUFF(@FieldValue, len(@FieldValue)-2, 0, ''.'') end
SET @MyValueOut = CAST(@FieldValue AS DECIMAL(16, 3));
end

ELSE if(@DataTypeCode = ''Double'' or @DataTypeCode = ''SigDouble'') 
begin 
if(len(@FieldValue)>=15)begin  set @FieldValue = STUFF(@FieldValue, len(@FieldValue)-2, 0, ''.'') end
SET @MyValueOut = CONVERT(NUMERIC(16,3), @FieldValue)
end


ELSE if(@MyValueOut is null) begin set @MyValueOut = @FieldValue; end

  RETURN(@MyValueOut); 

END;  

';

 EXEC(@functionsDataString)
end 


 




  
 



