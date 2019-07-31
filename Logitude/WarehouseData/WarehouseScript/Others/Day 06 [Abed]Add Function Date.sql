
-- Shared DB

--IF object_id(N'GetDateFormateAsNumber', N'FN') IS NOT NULL
--  BEGIN DROP FUNCTION GetDateFormateAsNumber   end

IF object_id('GetDateFormateAsNumber') IS  NULL
BEGIN
 declare @dateString as varchar(3000)

 set @dateString = 'CREATE FUNCTION [dbo].[GetDateFormateAsNumber](@dateTime datetime) RETURNS int WITH EXECUTE AS CALLER AS BEGIN   declare @Result  as int declare @dateString as varchar(100) set @Result = -1	 if(@dateTime is not null)   begin    if(@dateTime < ''2008-01-01 00:00:00.000'')  begin set @Result = -2  end  else if(@dateTime> ''2027-12-31 00:00:00.000'')  begin  set @Result = -3	 end     else  begin set @dateString = CONVERT(VARCHAR(10), @dateTime, 120);  set @dateString = REPLACE(@dateString, ''-'', '''');   set @dateString = REPLACE(@dateString, ''/'', '''');   set @Result = @dateString     end  end   RETURN(@Result);  END;';

 EXEC(@dateString)
			
END