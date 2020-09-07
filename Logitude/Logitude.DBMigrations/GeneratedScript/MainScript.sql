-- General Script From BuildSearchKeywordFunction.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
IF EXISTS (SELECT *
FROM   sys.objects
WHERE  object_id = OBJECT_ID(N'[dbo].[BuildSearchKeywordFunction]'))
DROP FUNCTION [dbo].[BuildSearchKeywordFunction]
declare @dateString as varchar(3000)
set @dateString = 'CREATE FUNCTION dbo.BuildSearchKeywordFunction ( @stringToSplit VARCHAR(MAX)  , @firstweight int , @Secondweight int)
RETURNS
@returnList TABLE ([Keyword] [nvarchar] (500), [weight] int )
AS
BEGIN
set @stringToSplit =  RTrim(@stringToSplit)
DECLARE @IsFirstTime bit
set @IsFirstTime = 1;
DECLARE @name NVARCHAR(255)
DECLARE @pos INT
if(@stringToSplit!='' '') begin INSERT INTO @returnList  SELECT @stringToSplit ,@firstweight end
WHILE CHARINDEX('' '', @stringToSplit) > 0
BEGIN
SELECT @pos  = CHARINDEX('' '', @stringToSplit)
SELECT @name = SUBSTRING(@stringToSplit, 1, @pos-1)
if(@IsFirstTime= 0 and @name!='' '')   begin INSERT INTO @returnList  SELECT @stringToSplit ,@Secondweight end
SELECT @stringToSplit = SUBSTRING(@stringToSplit, @pos+1, LEN(@stringToSplit)-@pos)
set @IsFirstTime = 0;
END
if(@IsFirstTime= 0 and @stringToSplit!='' '')begin INSERT INTO @returnList SELECT @stringToSplit ,@Secondweight
end
RETURN
END'
EXEC(@dateString)
SELECT @EndTime = GETDATE()
UPDATE [dbo].[DBScriptsHistory] SET [ExecutionDate] = GETDATE(), [ScriptBody] = 'IF EXISTS (SELECT *
FROM   sys.objects
WHERE  object_id = OBJECT_ID(N''[dbo].[BuildSearchKeywordFunction]''))
DROP FUNCTION [dbo].[BuildSearchKeywordFunction]
declare @dateString as varchar(3000)
set @dateString = ''CREATE FUNCTION dbo.BuildSearchKeywordFunction ( @stringToSplit VARCHAR(MAX)  , @firstweight int , @Secondweight int)
RETURNS
@returnList TABLE ([Keyword] [nvarchar] (500), [weight] int )
AS
BEGIN
set @stringToSplit =  RTrim(@stringToSplit)
DECLARE @IsFirstTime bit
set @IsFirstTime = 1;
DECLARE @name NVARCHAR(255)
DECLARE @pos INT
if(@stringToSplit!='''' '''') begin INSERT INTO @returnList  SELECT @stringToSplit ,@firstweight end
WHILE CHARINDEX('''' '''', @stringToSplit) > 0
BEGIN
SELECT @pos  = CHARINDEX('''' '''', @stringToSplit)
SELECT @name = SUBSTRING(@stringToSplit, 1, @pos-1)
if(@IsFirstTime= 0 and @name!='''' '''')   begin INSERT INTO @returnList  SELECT @stringToSplit ,@Secondweight end
SELECT @stringToSplit = SUBSTRING(@stringToSplit, @pos+1, LEN(@stringToSplit)-@pos)
set @IsFirstTime = 0;
END
if(@IsFirstTime= 0 and @stringToSplit!='''' '''')begin INSERT INTO @returnList SELECT @stringToSplit ,@Secondweight
end
RETURN
END''
EXEC(@dateString)', [ElapsedTimeInMs] = DATEDIFF(MS,@StartTime,@EndTime), [HashValue] = '4245f55e25e3724f77cf861fca34af15', [Version] = 4 WHERE [SxmlFileName] = 'BuildSearchKeywordFunction.sxml';
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

