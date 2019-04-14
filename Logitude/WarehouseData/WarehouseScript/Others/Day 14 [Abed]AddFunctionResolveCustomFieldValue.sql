--IF object_id(N'dbo.ResolveCustomFieldDateValue', N'FN') IS NOT NULL
-- BEGIN DROP FUNCTION dbo.ResolveCustomFieldDateValue   end



--CREATE FUNCTION dbo.ResolveCustomFieldDateValue (@FieldValue varchar(2000) ,  @DataTypeCode varchar(10))  
--RETURNS sql_variant
--AS  
--BEGIN  
-- DECLARE @MyValueOut sql_variant
-- set @MyValueOut = -1;

--if(@DataTypeCode = 'DateTime' and  LEN(@FieldValue) >= 14)
--begin
--set @MyValueOut = SUBSTRING(@FieldValue, 1, 4) + '-'+ SUBSTRING(@FieldValue, 5, 2) + '-'+ SUBSTRING(@FieldValue, 7, 2) +   ' '+ SUBSTRING(@FieldValue, 9, 2) + ':'+ SUBSTRING(@FieldValue, 11, 2) + ':'+ SUBSTRING(@FieldValue, 13, 2) 
--set @MyValueOut = convert(varchar, CAST(@MyValueOut AS datetime), 20)
--set @MyValueOut = dbo.GetDateFormateAsNumber(CONVERT(DATE, @MyValueOut))
--end

--ELSE if(@DataTypeCode = 'Date'  and  LEN(@FieldValue) >= 8) 
--begin 
--set @MyValueOut = SUBSTRING(@FieldValue, 1, 4) + '-'+ SUBSTRING(@FieldValue, 5, 2) + '-'+ SUBSTRING(@FieldValue, 7, 2)
--SET @MyValueOut =  convert(varchar, CAST(@MyValueOut AS datetime), 23) 
--set @MyValueOut = dbo.GetDateFormateAsNumber(CONVERT(DATE, @MyValueOut))
--end

--  RETURN(@MyValueOut);  

--END;  







--IF object_id(N'dbo.ResolveCustomFieldValue', N'FN') IS NOT NULL
-- BEGIN DROP FUNCTION dbo.ResolveCustomFieldValue   end

--CREATE FUNCTION dbo.ResolveCustomFieldValue (@FieldValue varchar(2000) ,  @DataTypeCode varchar(10))  
--RETURNS sql_variant
--AS  
--BEGIN  
-- DECLARE @MyValueOut sql_variant

--if(@DataTypeCode = 'Date' and  @DataTypeCode = 'DateTime')
--begin
--set @MyValueOut = dbo.ResolveCustomFieldDateValue(@FieldValue,@DataTypeCode);
--end

--ELSE if(@DataTypeCode = 'Boolen' and  LEN(@FieldValue) = 4) begin SET @MyValueOut =  CAST(@FieldValue AS bit); end
--ELSE if(@DataTypeCode = 'Integer' or @DataTypeCode = 'UnsInteger') begin SET @MyValueOut =  CAST(@FieldValue AS int); end
--ELSE if(@DataTypeCode = 'Decimal' or @DataTypeCode = 'UnsDecimal') begin SET @MyValueOut = CAST(@FieldValue AS DECIMAL(16, 3)); end
--ELSE if(@DataTypeCode = 'Double' or @DataTypeCode = 'SigDouble') begin SET @MyValueOut = CAST(@FieldValue AS DECIMAL(16, 3)); end
--ELSE if(@MyValueOut is null)begin set @MyValueOut = @FieldValue; end

--  RETURN(@MyValueOut); 

--END;  
 



