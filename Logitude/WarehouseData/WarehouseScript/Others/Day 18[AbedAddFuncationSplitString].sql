-- Shared DB


IF object_id(N'SplitString', N'FN') IS NOT NULL
BEGIN DROP FUNCTION dbo.SplitString   end


IF object_id(N'dbo.SplitString', N'FN') IS  NULL
begin 
declare @functionsDataString as varchar(8000)
set @functionsDataString = 'CREATE FUNCTION dbo.SplitString (@string NVARCHAR(MAX) ,  @delimiter varchar(10) , @index int)  
RETURNS varchar(20)
AS  
BEGIN 

 DECLARE @MyValueOut varchar(20) 
  DECLARE @IsEnd bit
    DECLARE @start INT, @end INT ,@Count INT
	 set  @Count = 0;
	 	 set  @IsEnd = 0;
    SELECT @start = 0, @end = CHARINDEX(@delimiter, @string) 
    WHILE @start < LEN(@string) + 1 and @IsEnd=0 BEGIN 
        IF @end = 0  
        SET @end = LEN(@string) +1
         set  @Count=@Count+1;
		
		 if(@index = @Count)
		 begin
		  set  @MyValueOut = SUBSTRING(@string, @start, @end - @start);
		  set  @IsEnd = 1;
		 end
        SET @start = @end + 1 
        SET @end = CHARINDEX(@delimiter, @string, @start)
    END 

    RETURN @MyValueOut;

END;';

 EXEC(@functionsDataString)
end 


 