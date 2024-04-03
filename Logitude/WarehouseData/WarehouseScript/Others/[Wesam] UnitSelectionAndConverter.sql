
---- Shared DB
IF object_id(N'DW_WegihtConverter', N'FN') IS NOT NULL
BEGIN DROP FUNCTION DW_WegihtConverter end




IF object_id(N'dbo.DW_WegihtConverter', N'FN') IS NULL
begin
declare @functionsDataString as varchar(8000)
set @functionsDataString = 'CREATE FUNCTION DW_WegihtConverter
 (
 @ValueWithoutUnitCode AS varchar(2000),
 @DefaultUnitCode AS varchar (10),
 @SelectedUnitCode AS varchar (10)
 )
RETURNS VARCHAR(MAX)
AS
BEGIN
  IF(@DefaultUnitCode = ''KG'' and @SelectedUnitCode = ''LB'') RETURN CAST(ROUND(CAST(@ValueWithoutUnitCode AS FLOAT(20)) / 0.45359237, 3) AS VARCHAR(2000))
  IF(@DefaultUnitCode = ''KG'' and @SelectedUnitCode = ''MT'') RETURN CAST(ROUND(CAST(@ValueWithoutUnitCode AS FLOAT(20)) / 1000, 3) AS VARCHAR(2000))
  IF(@DefaultUnitCode = ''LB'' and @SelectedUnitCode = ''KG'') RETURN CAST(ROUND(CAST(@ValueWithoutUnitCode AS FLOAT(20)) * 0.45359237, 3) AS VARCHAR(2000))
  IF(@DefaultUnitCode = ''LB'' and @SelectedUnitCode = ''MT'') RETURN CAST(ROUND(CAST(@ValueWithoutUnitCode AS FLOAT(20)) * 0.00045359237, 3) AS VARCHAR(2000))
  IF(@DefaultUnitCode = ''MT'' and @SelectedUnitCode = ''KG'') RETURN CAST(ROUND(CAST(@ValueWithoutUnitCode AS FLOAT(20)) * 1000, 3) AS VARCHAR(2000))
  IF(@DefaultUnitCode = ''MT'' and @SelectedUnitCode = ''LB'') RETURN CAST(ROUND(CAST(@ValueWithoutUnitCode AS FLOAT(20)) / 0.00045359237, 3) AS VARCHAR(2000))
  RETURN @ValueWithoutUnitCode
END


';



EXEC(@functionsDataString)
end






IF object_id(N'DW_GetNewValueWithSelectedUnitCode', N'FN') IS NOT NULL
BEGIN DROP FUNCTION DW_GetNewValueWithSelectedUnitCode end




IF object_id(N'dbo.DW_GetNewValueWithSelectedUnitCode', N'FN') IS NULL
begin
declare @functionsDataString2 as varchar(8000)
set @functionsDataString2 = 'CREATE FUNCTION DW_GetNewValueWithSelectedUnitCode
 (
 @ValueWithDefaultUnitCode AS varchar(2000),
 @SelectedUnitCode AS varchar (10)
 )
RETURNS VARCHAR(MAX)
AS
BEGIN
  IF(@ValueWithDefaultUnitCode IS NULL) RETURN NULL
  IF(CHARINDEX(''('', @ValueWithDefaultUnitCode) > 0)
  BEGIN
  set @ValueWithDefaultUnitCode = REPLACE(@ValueWithDefaultUnitCode, ''('', '' ('')
  END
  IF(CHARINDEX('' ('', @ValueWithDefaultUnitCode) > 0)
  BEGIN
  DECLARE @PositionToSplit INT
  SELECT @PositionToSplit  = CHARINDEX('' ('', @ValueWithDefaultUnitCode) 
  DECLARE @ValueWithoutUnitCode NVARCHAR(2000) = SUBSTRING(@ValueWithDefaultUnitCode, 0, @PositionToSplit)
  DECLARE @DefaultUnitCode NVARCHAR(10) = SUBSTRING(@ValueWithDefaultUnitCode, @PositionToSplit+2, LEN(@ValueWithDefaultUnitCode)-@PositionToSplit-2)
  RETURN
	CAST(dbo.DW_WegihtConverter(@ValueWithoutUnitCode, @DefaultUnitCode, @SelectedUnitCode) AS VARCHAR(2000))
  END
  RETURN @ValueWithDefaultUnitCode
END

';



EXEC(@functionsDataString2)
end
