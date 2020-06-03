-- Procedure Script From DeleteOldCommunicationLogs.dxml
EXEC('IF (OBJECT_ID(''[dbo].[DeleteOldCommunicationLogs]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[DeleteOldCommunicationLogs] END');
EXEC('create procedure [dbo].[DeleteOldCommunicationLogs]
as
begin
IF OBJECT_ID(''dbo.TempDeletedCommunicationLogs'') IS NOT NULL
DROP TABLE TempDeletedCommunicationLogs
SELECT * INTO TempDeletedCommunicationLogs
FROM (SELECT top(1000) Id,DocumentId
FROM CommunicationLogs
WHERE CreateDate < GETDATE() - 120) AS t
DELETE FROM CommunicationLogs WHERE Id IN (SELECT Id FROM TempDeletedCommunicationLogs)
UPDATE Documents SET MarkForDelete = 1 WHERE Id IN (SELECT DocumentId FROM TempDeletedCommunicationLogs)
end');


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
END', DATEDIFF(MS,@StartTime,@EndTime), 'b3800375557abb11d50a6e90fda81c7c', 1);
COMMIT TRAN
END TRY
BEGIN CATCH
IF @@TRANCOUNT > 0
ROLLBACK TRAN
END CATCH;

