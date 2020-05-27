
-- Shared DB

IF object_id(N'GetDateFormateAsNumber', N'FN') IS NOT NULL
  BEGIN DROP FUNCTION GetDateFormateAsNumber   end

IF object_id('GetDateFormateAsNumber') IS  NULL
BEGIN
 declare @dateString as varchar(3000)

 set @dateString = 'CREATE FUNCTION [dbo].[GetDateFormateAsNumber](@fieldValue date) RETURNS date WITH EXECUTE AS CALLER AS BEGIN   declare @Result  as date  set @Result = ''1-1-1''	 if(@fieldValue is not null)   begin    if(@fieldValue < ''2008-01-01 00:00:00.000'')  begin set @Result = ''2-2-2''  end  else if(@fieldValue> ''2027-12-31 00:00:00.000'')  begin  set @Result = ''3-3-3''	 end     else  begin  set @Result =@fieldValue end  end   RETURN(@Result);  END;';

 EXEC(@dateString)
			
END