-- General Script From 202006011347_FillQuoteClosingReasonTable.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
declare @Tenant as int
declare @TenantString as varchar(50)
declare @EntityId as varchar(15)
declare @UserId as varchar(15)
declare @UserEmail as varchar(150)
BEGIN
DECLARE TenantsCursor CURSOR READ_ONLY
FOR
SELECT Id
FROM Tenants
OPEN TenantsCursor FETCH NEXT FROM TenantsCursor INTO @Tenant
WHILE @@FETCH_STATUS = 0
BEGIN
set @TenantString = CONVERT(varchar(50), @Tenant)
set @UserEmail = 'system@tenant'+ @TenantString + '.com'
set @UserId = (select Id from Contacts where Email = @UserEmail and Tenant = @Tenant)
set @UserId = (select Id from Users where Id = @UserId and Tenant = @Tenant)
if (@UserId is not null)
begin
if not exists (select Id from QuoteClosingReasons where Tenant = @Tenant and Code = 'EQ')
begin
EXECUTE usp_GetNextTableIdValue @EntityId OUTPUT,'QuoteClosingReason'
insert into QuoteClosingReasons(Code, Name, SearchFields, Id, Tenant, CreateDate, UpdateDate, CreatedByUserId, UpdatedByUserId, Inactive)
values('EQ', 'Expensive Quote', 'EQ,Expensive Quote', @EntityId, @Tenant, GETDATE(), GETDATE(), @UserId, @UserId, 0)
end
if not exists (select Id from QuoteClosingReasons where Tenant = @Tenant and Code = 'GS')
begin
EXECUTE usp_GetNextTableIdValue @EntityId OUTPUT,'QuoteClosingReason'
insert into QuoteClosingReasons(Code, Name, SearchFields, Id, Tenant, CreateDate, UpdateDate, CreatedByUserId, UpdatedByUserId, Inactive)
values('GS', 'Given directly to the Shipping Line', 'GS,Given directly to the Shipping Line', @EntityId, @Tenant, GETDATE(), GETDATE(), @UserId, @UserId, 0)
end
if not exists (select Id from QuoteClosingReasons where Tenant = @Tenant and Code = 'LC')
begin
EXECUTE usp_GetNextTableIdValue @EntityId OUTPUT,'QuoteClosingReason'
insert into QuoteClosingReasons(Code, Name, SearchFields, Id, Tenant, CreateDate, UpdateDate, CreatedByUserId, UpdatedByUserId, Inactive)
values('LC', 'Lost to Competitor', 'LC,Lost to Competitor', @EntityId, @Tenant, GETDATE(), GETDATE(), @UserId, @UserId, 0)
end
if not exists (select Id from QuoteClosingReasons where Tenant = @Tenant and Code = 'LS')
begin
EXECUTE usp_GetNextTableIdValue @EntityId OUTPUT,'QuoteClosingReason'
insert into QuoteClosingReasons(Code, Name, SearchFields, Id, Tenant, CreateDate, UpdateDate, CreatedByUserId, UpdatedByUserId, Inactive)
values('LS', 'Lack of Service in the Last Shipment', 'LS,Lack of Service in the Last Shipment', @EntityId, @Tenant, GETDATE(), GETDATE(), @UserId, @UserId, 0)
end
if not exists (select Id from QuoteClosingReasons where Tenant = @Tenant and Code = 'XQ')
begin
EXECUTE usp_GetNextTableIdValue @EntityId OUTPUT,'QuoteClosingReason'
insert into QuoteClosingReasons(Code, Name, SearchFields, Id, Tenant, CreateDate, UpdateDate, CreatedByUserId, UpdatedByUserId, Inactive)
values('XQ', 'Expired Quote', 'XQ,Expired Quote', @EntityId, @Tenant, GETDATE(), GETDATE(), @UserId, @UserId, 0)
end
if not exists (select Id from QuoteClosingReasons where Tenant = @Tenant and Code = 'BM')
begin
EXECUTE usp_GetNextTableIdValue @EntityId OUTPUT,'QuoteClosingReason'
insert into QuoteClosingReasons(Code, Name, SearchFields, Id, Tenant, CreateDate, UpdateDate, CreatedByUserId, UpdatedByUserId, Inactive)
values('BM', 'Benchmarking', 'BM,Benchmarking', @EntityId, @Tenant, GETDATE(), GETDATE(), @UserId, @UserId, 0)
end
if not exists (select Id from QuoteClosingReasons where Tenant = @Tenant and Code = 'LT')
begin
EXECUTE usp_GetNextTableIdValue @EntityId OUTPUT,'QuoteClosingReason'
insert into QuoteClosingReasons(Code, Name, SearchFields, Id, Tenant, CreateDate, UpdateDate, CreatedByUserId, UpdatedByUserId, Inactive)
values('LT', 'Long Term Project', 'LT,Long Term Project', @EntityId, @Tenant, GETDATE(), GETDATE(), @UserId, @UserId, 0)
end
end
FETCH NEXT FROM TenantsCursor INTO @Tenant
END
CLOSE TenantsCursor
DEALLOCATE TenantsCursor
END
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('202006011347_FillQuoteClosingReasonTable.sxml', GETDATE(), 'declare @Tenant as int
declare @TenantString as varchar(50)
declare @EntityId as varchar(15)
declare @UserId as varchar(15)
declare @UserEmail as varchar(150)
BEGIN
DECLARE TenantsCursor CURSOR READ_ONLY
FOR
SELECT Id
FROM Tenants
OPEN TenantsCursor FETCH NEXT FROM TenantsCursor INTO @Tenant
WHILE @@FETCH_STATUS = 0
BEGIN
set @TenantString = CONVERT(varchar(50), @Tenant)
set @UserEmail = ''system@tenant''+ @TenantString + ''.com''
set @UserId = (select Id from Contacts where Email = @UserEmail and Tenant = @Tenant)
set @UserId = (select Id from Users where Id = @UserId and Tenant = @Tenant)
if (@UserId is not null)
begin
if not exists (select Id from QuoteClosingReasons where Tenant = @Tenant and Code = ''EQ'')
begin
EXECUTE usp_GetNextTableIdValue @EntityId OUTPUT,''QuoteClosingReason''
insert into QuoteClosingReasons(Code, Name, SearchFields, Id, Tenant, CreateDate, UpdateDate, CreatedByUserId, UpdatedByUserId, Inactive)
values(''EQ'', ''Expensive Quote'', ''EQ,Expensive Quote'', @EntityId, @Tenant, GETDATE(), GETDATE(), @UserId, @UserId, 0)
end
if not exists (select Id from QuoteClosingReasons where Tenant = @Tenant and Code = ''GS'')
begin
EXECUTE usp_GetNextTableIdValue @EntityId OUTPUT,''QuoteClosingReason''
insert into QuoteClosingReasons(Code, Name, SearchFields, Id, Tenant, CreateDate, UpdateDate, CreatedByUserId, UpdatedByUserId, Inactive)
values(''GS'', ''Given directly to the Shipping Line'', ''GS,Given directly to the Shipping Line'', @EntityId, @Tenant, GETDATE(), GETDATE(), @UserId, @UserId, 0)
end
if not exists (select Id from QuoteClosingReasons where Tenant = @Tenant and Code = ''LC'')
begin
EXECUTE usp_GetNextTableIdValue @EntityId OUTPUT,''QuoteClosingReason''
insert into QuoteClosingReasons(Code, Name, SearchFields, Id, Tenant, CreateDate, UpdateDate, CreatedByUserId, UpdatedByUserId, Inactive)
values(''LC'', ''Lost to Competitor'', ''LC,Lost to Competitor'', @EntityId, @Tenant, GETDATE(), GETDATE(), @UserId, @UserId, 0)
end
if not exists (select Id from QuoteClosingReasons where Tenant = @Tenant and Code = ''LS'')
begin
EXECUTE usp_GetNextTableIdValue @EntityId OUTPUT,''QuoteClosingReason''
insert into QuoteClosingReasons(Code, Name, SearchFields, Id, Tenant, CreateDate, UpdateDate, CreatedByUserId, UpdatedByUserId, Inactive)
values(''LS'', ''Lack of Service in the Last Shipment'', ''LS,Lack of Service in the Last Shipment'', @EntityId, @Tenant, GETDATE(), GETDATE(), @UserId, @UserId, 0)
end
if not exists (select Id from QuoteClosingReasons where Tenant = @Tenant and Code = ''XQ'')
begin
EXECUTE usp_GetNextTableIdValue @EntityId OUTPUT,''QuoteClosingReason''
insert into QuoteClosingReasons(Code, Name, SearchFields, Id, Tenant, CreateDate, UpdateDate, CreatedByUserId, UpdatedByUserId, Inactive)
values(''XQ'', ''Expired Quote'', ''XQ,Expired Quote'', @EntityId, @Tenant, GETDATE(), GETDATE(), @UserId, @UserId, 0)
end
if not exists (select Id from QuoteClosingReasons where Tenant = @Tenant and Code = ''BM'')
begin
EXECUTE usp_GetNextTableIdValue @EntityId OUTPUT,''QuoteClosingReason''
insert into QuoteClosingReasons(Code, Name, SearchFields, Id, Tenant, CreateDate, UpdateDate, CreatedByUserId, UpdatedByUserId, Inactive)
values(''BM'', ''Benchmarking'', ''BM,Benchmarking'', @EntityId, @Tenant, GETDATE(), GETDATE(), @UserId, @UserId, 0)
end
if not exists (select Id from QuoteClosingReasons where Tenant = @Tenant and Code = ''LT'')
begin
EXECUTE usp_GetNextTableIdValue @EntityId OUTPUT,''QuoteClosingReason''
insert into QuoteClosingReasons(Code, Name, SearchFields, Id, Tenant, CreateDate, UpdateDate, CreatedByUserId, UpdatedByUserId, Inactive)
values(''LT'', ''Long Term Project'', ''LT,Long Term Project'', @EntityId, @Tenant, GETDATE(), GETDATE(), @UserId, @UserId, 0)
end
end
FETCH NEXT FROM TenantsCursor INTO @Tenant
END
CLOSE TenantsCursor
DEALLOCATE TenantsCursor
END', DATEDIFF(MS,@StartTime,@EndTime), '7260051d8e4f146acd2b0c2ff202f521', 1);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

-- General Script From 202006011458_FillQuoteClosingReasonId.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
update Quotes
set QuoteClosingReasonId = (select Id from QuoteClosingReasons where Code = Quotes.QuoteClosingReasonCode and Tenant = Quotes.Tenant )
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('202006011458_FillQuoteClosingReasonId.sxml', GETDATE(), 'update Quotes
set QuoteClosingReasonId = (select Id from QuoteClosingReasons where Code = Quotes.QuoteClosingReasonCode and Tenant = Quotes.Tenant )', DATEDIFF(MS,@StartTime,@EndTime), 'd67cf9428add3fdc7833ecd1c5ecc86a', 1);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

-- General Script From 202006011500_AddQuoteClosingReasonEventTypesToTenants.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
declare @Tenant as int
declare @Code as varchar(10)
declare @Name as varchar(100)
declare @NewId as varchar(15)
declare @ObjectTableId as varchar(15)
BEGIN
DECLARE DataCursor CURSOR READ_ONLY
FOR
SELECT Id
FROM Tenants
OPEN DataCursor FETCH NEXT FROM DataCursor INTO @Tenant
WHILE @@FETCH_STATUS = 0
BEGIN
set @ObjectTableId = (select Id from ObjectTables where Name = 'QuoteClosingReason')
set @Code = 'MAIN'
set @Name = 'Marked as Inactive'
if not exists (select * from EventTypes where Code = @Code and Tenant = @Tenant)
begin
EXECUTE usp_GetNextTableIdValue @NewId OUTPUT,'EventType'
insert into EventTypes(Id, Code, ObjectTableId, Tenant, EnglishName, LocalName, AddedManually, IsManualEntry, IsFollowUp, ManualActivatedFollowUp, InActive, ShortView)
values
(
@NewId,
@Code,
@ObjectTableId,
@Tenant,
@Name,
@Name,
0,
0,
0,
0,
0,
1
)
end
set @Code = 'REAC'
set @Name = 'Reactivated'
if not exists (select * from EventTypes where Code = @Code and Tenant = @Tenant)
begin
EXECUTE usp_GetNextTableIdValue @NewId OUTPUT,'EventType'
insert into EventTypes(Id, Code, ObjectTableId, Tenant, EnglishName, LocalName, AddedManually, IsManualEntry, IsFollowUp, ManualActivatedFollowUp, InActive, ShortView)
values
(
@NewId,
@Code,
@ObjectTableId,
@Tenant,
@Name,
@Name,
0,
0,
0,
0,
0,
1
)
end
FETCH NEXT FROM DataCursor INTO @Tenant
END
CLOSE DataCursor
DEALLOCATE DataCursor
END
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('202006011500_AddQuoteClosingReasonEventTypesToTenants.sxml', GETDATE(), 'declare @Tenant as int
declare @Code as varchar(10)
declare @Name as varchar(100)
declare @NewId as varchar(15)
declare @ObjectTableId as varchar(15)
BEGIN
DECLARE DataCursor CURSOR READ_ONLY
FOR
SELECT Id
FROM Tenants
OPEN DataCursor FETCH NEXT FROM DataCursor INTO @Tenant
WHILE @@FETCH_STATUS = 0
BEGIN
set @ObjectTableId = (select Id from ObjectTables where Name = ''QuoteClosingReason'')
set @Code = ''MAIN''
set @Name = ''Marked as Inactive''
if not exists (select * from EventTypes where Code = @Code and Tenant = @Tenant)
begin
EXECUTE usp_GetNextTableIdValue @NewId OUTPUT,''EventType''
insert into EventTypes(Id, Code, ObjectTableId, Tenant, EnglishName, LocalName, AddedManually, IsManualEntry, IsFollowUp, ManualActivatedFollowUp, InActive, ShortView)
values
(
@NewId,
@Code,
@ObjectTableId,
@Tenant,
@Name,
@Name,
0,
0,
0,
0,
0,
1
)
end
set @Code = ''REAC''
set @Name = ''Reactivated''
if not exists (select * from EventTypes where Code = @Code and Tenant = @Tenant)
begin
EXECUTE usp_GetNextTableIdValue @NewId OUTPUT,''EventType''
insert into EventTypes(Id, Code, ObjectTableId, Tenant, EnglishName, LocalName, AddedManually, IsManualEntry, IsFollowUp, ManualActivatedFollowUp, InActive, ShortView)
values
(
@NewId,
@Code,
@ObjectTableId,
@Tenant,
@Name,
@Name,
0,
0,
0,
0,
0,
1
)
end
FETCH NEXT FROM DataCursor INTO @Tenant
END
CLOSE DataCursor
DEALLOCATE DataCursor
END', DATEDIFF(MS,@StartTime,@EndTime), '378c9c4533e5cc2475120d6951d86fa2', 1);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

-- General Script From 202005262050_FillFreightChargeTypeIdOfTariffTable.sxml File
BEGIN TRAN
BEGIN TRY
DECLARE @StartTime datetime
DECLARE @EndTime datetime
SELECT @StartTime = GETDATE()
declare @Tenant as int
declare @Id as varchar(15)
declare @TypeCode as varchar(15)
declare @FreightChargeTypeId as varchar(15)
BEGIN
DECLARE TariffCursor CURSOR READ_ONLY
FOR
SELECT Id, Tenant, TypeCode
FROM Tariffs
OPEN TariffCursor FETCH NEXT FROM TariffCursor INTO @Id, @Tenant, @TypeCode
WHILE @@FETCH_STATUS = 0
BEGIN
if (@TypeCode = 'AFC' or @TypeCode = 'OLC' or @TypeCode = 'OFC')
begin
if (@TypeCode = 'AFC')
begin
set @FreightChargeTypeId = (select Id from ChargesTypes where Tenant = @Tenant AND Code  = 'AFT')
end
else
begin
set @FreightChargeTypeId = (select Id from ChargesTypes where Tenant = @Tenant AND Code  = 'OFT')
end
update Tariffs set FreightChargeId = @FreightChargeTypeId  where Id = @Id and Tenant = @Tenant
end
FETCH NEXT FROM TariffCursor INTO @Id, @Tenant, @TypeCode
END
CLOSE TariffCursor
DEALLOCATE TariffCursor
END
SELECT @EndTime = GETDATE()
INSERT INTO [dbo].[DBScriptsHistory]([SxmlFileName], [ExecutionDate], [ScriptBody], [ElapsedTimeInMs], [HashValue], [Version])VALUES('202005262050_FillFreightChargeTypeIdOfTariffTable.sxml', GETDATE(), 'declare @Tenant as int
declare @Id as varchar(15)
declare @TypeCode as varchar(15)
declare @FreightChargeTypeId as varchar(15)
BEGIN
DECLARE TariffCursor CURSOR READ_ONLY
FOR
SELECT Id, Tenant, TypeCode
FROM Tariffs
OPEN TariffCursor FETCH NEXT FROM TariffCursor INTO @Id, @Tenant, @TypeCode
WHILE @@FETCH_STATUS = 0
BEGIN
if (@TypeCode = ''AFC'' or @TypeCode = ''OLC'' or @TypeCode = ''OFC'')
begin
if (@TypeCode = ''AFC'')
begin
set @FreightChargeTypeId = (select Id from ChargesTypes where Tenant = @Tenant AND Code  = ''AFT'')
end
else
begin
set @FreightChargeTypeId = (select Id from ChargesTypes where Tenant = @Tenant AND Code  = ''OFT'')
end
update Tariffs set FreightChargeId = @FreightChargeTypeId  where Id = @Id and Tenant = @Tenant
end
FETCH NEXT FROM TariffCursor INTO @Id, @Tenant, @TypeCode
END
CLOSE TariffCursor
DEALLOCATE TariffCursor
END', DATEDIFF(MS,@StartTime,@EndTime), '9f999a06fe8ba464ed210f3f9d3d7217', 1);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

