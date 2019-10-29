
---- Shared DB
IF object_id(N'ResolveCustomFieldValue', N'FN') IS NOT NULL
  BEGIN DROP FUNCTION ResolveCustomFieldValue   end


IF object_id(N'dbo.ResolveCustomFieldValue', N'FN') IS  NULL
begin 
declare @functionsDataString as varchar(8000)
set @functionsDataString = 'CREATE FUNCTION dbo.ResolveCustomFieldValue (@FieldValue varchar(2000) ,@FieldName varchar(100) , @CustomFieldsValues varchar(1000))  
RETURNS sql_variant
AS  
BEGIN  
 DECLARE @MyValueOut sql_variant
  DECLARE @DataTypeCode varchar(10)
  set @MyValueOut = null;
  if(@CustomFieldsValues is not null)
  begin
set @CustomFieldsValues = dbo.SplitString(@CustomFieldsValues,@FieldName + '':'', 2) ;
 set @CustomFieldsValues = dbo.SplitString(@CustomFieldsValues,'','',1);
  set @DataTypeCode = dbo.SplitString(@CustomFieldsValues,'':'',2)

    -- set @CustomFieldsValues =( SELECT value  FROM STRING_SPLIT(@CustomFieldsValues, '','')  WHERE RTRIM(value) LIKE ''%'' + @FieldName + '':%'');
	-- if(@CustomFieldsValues is not null)
	--begin
	--set @DataTypeCode =( SELECT value  FROM STRING_SPLIT(@CustomFieldsValues, '':'')  WHERE RTRIM(value) <> @FieldName);
--	 end

  end
if(@DataTypeCode is not null)
begin

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


ELSE if(@DataTypeCode = ''Integer'' or @DataTypeCode = ''UnsInteger'') begin 

if CHARINDEX('' '',@FieldValue) > 0 
begin   set @FieldValue = REPLACE(@FieldValue, '' '', '''') 
end 

if CHARINDEX(''.'',@FieldValue) > 0
begin
 --set @FieldValue =( SELECT value  FROM STRING_SPLIT(@FieldValue, ''.'')  WHERE RTRIM(value) LIKE ''+'');
 set @FieldValue = dbo.SplitString(@FieldValue,''.'', 1) 
end
SET @MyValueOut =  CAST(@FieldValue AS bigint); 

end

ELSE if(@DataTypeCode = ''Decimal'' or @DataTypeCode = ''UnsDecimal'') 
begin 

if CHARINDEX('' '',@FieldValue) > 0 
begin   set @FieldValue = REPLACE(@FieldValue, '' '', '''') 
end 

if(len(@FieldValue)>=15)begin  set @FieldValue = STUFF(@FieldValue, len(@FieldValue)-2, 0, ''.'') end
SET @MyValueOut = CAST(@FieldValue AS DECIMAL(38, 3));
end

ELSE if(@DataTypeCode = ''Double'' or @DataTypeCode = ''SigDouble'') 
begin 

if CHARINDEX('' '',@FieldValue) > 0 
begin   set @FieldValue = REPLACE(@FieldValue, '' '', '''') 
end 

if(len(@FieldValue)>=15)begin  set @FieldValue = STUFF(@FieldValue, len(@FieldValue)-2, 0, ''.'') end
SET @MyValueOut = CONVERT(NUMERIC(38,3), @FieldValue)
end

ELSE if(@DataTypeCode = ''Text'' or @DataTypeCode = ''nText'') 
begin 
SET @MyValueOut = @FieldValue;
end

ELSE if(@DataTypeCode = ''PickList'') 
begin 
Set @MyValueOut = ''-1'';

if(@FieldValue is not null) begin set @MyValueOut = @FieldValue ;end

end

ELSE begin set @MyValueOut = null; end

end

  RETURN(@MyValueOut); 

END;  

';

 EXEC(@functionsDataString)
end 


 




  
 



