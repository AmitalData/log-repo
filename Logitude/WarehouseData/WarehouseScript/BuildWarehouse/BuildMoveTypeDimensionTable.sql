
   declare @Id as varchar(15)
   declare @Code as varchar(3)
   declare @EnglishName as varchar(40)
   declare @LocalName as nvarchar(40)
   declare @TransportModeId as varchar(1)
   --declare @IsAir as bit
   --declare @IsInland as bit
   --declare @IsOcean as bit
   declare @SourceTenant int
   declare @ParentTenant int
   declare @AutomaticLastUpdateDate as datetime

	DECLARE MoveTypesCursor CURSOR READ_ONLY
	FOR
	SELECT Id,Code, MoveTypeEnglishName , MoveTypeLocalName , TransportModeId, dw_DWHSettings.Tenant, dw_DWHSettings.ParentTenant, dw_MoveTypes.AutomaticLastUpdateDate
	From dw_MoveTypes
	inner JOIN dw_DWHSettings ON dw_MoveTypes.Tenant = dw_DWHSettings.Tenant
	OPEN MoveTypesCursor FETCH NEXT FROM MoveTypesCursor INTO @Id ,@Code, @EnglishName, @LocalName,@TransportModeId , @SourceTenant , @ParentTenant, @AutomaticLastUpdateDate
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
    insert into #DIM_MoveTypesTemp (Id, Code ,[English Name] ,[Local Name],[Transport Mode], [Source Tenant],[Parent Tenant],[Automatic Last Update Date]) values(@Id ,@Code, @EnglishName, @LocalName,@TransportModeId , @SourceTenant , @ParentTenant, @AutomaticLastUpdateDate)

	FETCH NEXT FROM MoveTypesCursor  INTO @Id ,@Code, @EnglishName, @LocalName,@TransportModeId,  @SourceTenant , @ParentTenant, @AutomaticLastUpdateDate
		End
	CLOSE MoveTypesCursor
	DEALLOCATE MoveTypesCursor

