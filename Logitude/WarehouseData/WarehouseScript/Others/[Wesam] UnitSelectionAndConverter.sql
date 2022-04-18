GO

SET ANSI_PADDING ON
GO

CREATE NONCLUSTERED INDEX [IX_Fact_Charges_Volumetric_Weight_UnitSelection] ON [dbo].[Fact_Charges]
(
	[Volumetric Weight] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO


GO
 
CREATE FUNCTION DW_WegihtConverter
 (
 @ValueWithoutUnitCode AS varchar(2000),
 @DefaultUnitCode AS varchar (10),
 @SelectedUnitCode AS varchar (10)
 )
RETURNS VARCHAR(MAX)
AS
BEGIN
  IF(@DefaultUnitCode = 'KG' and @SelectedUnitCode = 'LB') RETURN CAST(ROUND(CAST(@ValueWithoutUnitCode AS FLOAT(20)) / 0.45359237, 3) AS VARCHAR(2000))
  IF(@DefaultUnitCode = 'KG' and @SelectedUnitCode = 'MT') RETURN CAST(ROUND(CAST(@ValueWithoutUnitCode AS FLOAT(20)) / 1000, 3) AS VARCHAR(2000))
  IF(@DefaultUnitCode = 'LB' and @SelectedUnitCode = 'KG') RETURN CAST(ROUND(CAST(@ValueWithoutUnitCode AS FLOAT(20)) * 0.45359237, 3) AS VARCHAR(2000))
  IF(@DefaultUnitCode = 'LB' and @SelectedUnitCode = 'MT') RETURN CAST(ROUND(CAST(@ValueWithoutUnitCode AS FLOAT(20)) * 0.00045359237, 3) AS VARCHAR(2000))
  IF(@DefaultUnitCode = 'MT' and @SelectedUnitCode = 'KG') RETURN CAST(ROUND(CAST(@ValueWithoutUnitCode AS FLOAT(20)) * 1000, 3) AS VARCHAR(2000))
  IF(@DefaultUnitCode = 'MT' and @SelectedUnitCode = 'LB') RETURN CAST(ROUND(CAST(@ValueWithoutUnitCode AS FLOAT(20)) / 0.00045359237, 3) AS VARCHAR(2000))
  RETURN @ValueWithoutUnitCode
END


GO
 
CREATE FUNCTION DW_GetNewValueWithSelectedUnitCode
 (
 @ValueWithDefaultUnitCode AS varchar(2000),
 @SelectedUnitCode AS varchar (10)
 )
RETURNS VARCHAR(MAX)
AS
BEGIN
  IF(@ValueWithDefaultUnitCode IS NULL) RETURN NULL
  IF(CHARINDEX(' (', @ValueWithDefaultUnitCode) > 0)
  BEGIN
  DECLARE @PositionToSplit INT
  SELECT @PositionToSplit  = CHARINDEX(' (', @ValueWithDefaultUnitCode) 
  DECLARE @ValueWithoutUnitCode NVARCHAR(2000) = SUBSTRING(@ValueWithDefaultUnitCode, 0, @PositionToSplit)
  DECLARE @DefaultUnitCode NVARCHAR(10) = SUBSTRING(@ValueWithDefaultUnitCode, @PositionToSplit+2, LEN(@ValueWithDefaultUnitCode)-@PositionToSplit-2)
  RETURN
	CAST(dbo.DW_WegihtConverter(@ValueWithoutUnitCode, @DefaultUnitCode, @SelectedUnitCode) + ' (' + @SelectedUnitCode + ')' AS VARCHAR(2000))
  END
  RETURN @ValueWithDefaultUnitCode
END
