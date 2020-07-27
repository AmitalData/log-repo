-- Procedure Script From DeleteTenantFromGlobalDB.dxml
EXEC('IF (OBJECT_ID(''[dbo].[DeleteTenantFromGlobalDB]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[DeleteTenantFromGlobalDB] END');
EXEC('--delete global records execute this on global database
Create  PROCEDURE dbo.DeleteTenantFromGlobalDB
(
@tenant int                     --Input parameter ,  tenant to delete
)
AS
BEGIN
delete from globalcontacts
where globaltenantid=@tenant
delete from globaltenants
where id=@tenant
END');


-- Procedure Script From usp_DeleteContactsUnseenEntitiesByContactId_Global.dxml
EXEC('IF (OBJECT_ID(''[dbo].[usp_DeleteContactsUnseenEntitiesByContactId]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[usp_DeleteContactsUnseenEntitiesByContactId] END');
EXEC('Create PROCEDURE [dbo].[usp_DeleteContactsUnseenEntitiesByContactId]
(
@pContactId   varchar(15),
@pObjectTableId   varchar(15),
@pTenant    int
)
as
BEGIN;
Delete  From ContactsUnseenEntities where [ContactId] =  @pContactId  and [Tenant] =  @pTenant and  [ObjectTableId] =  @pObjectTableId;
End;');


-- Procedure Script From GetNextGlobalTenantId.dxml
EXEC('IF (OBJECT_ID(''[dbo].[GetNextGlobalTenantId]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[GetNextGlobalTenantId] END');
EXEC('Create PROCEDURE GetNextGlobalTenantId
(
@pLastNumber INT OUTPUT
)
AS
BEGIN
Declare @Current As Int
Set @Current	= (SELECT  LastNumber
FROM GlobalTenantCounters  WHERE Id=1)
Set @Current = @Current + 1
Update GlobalTenantCounters
set LastNumber = LastNumber + 1
Where Id=1
End;
Set @pLastNumber = @Current');


-- Procedure Script From usp_GetForeignKeyName_Global.dxml
EXEC('IF (OBJECT_ID(''[dbo].[usp_GetForeignKeyName]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[usp_GetForeignKeyName] END');
EXEC('Create PROCEDURE dbo.usp_GetForeignKeyName
(
@keyName as varchar(500) output,
@baseTableName as varchar(500) ,
@foreignTableName as varchar(500) ,
@foreignColumnName as varchar(500)
)
AS
set @keyName = (select g.ForeignKey from
(
SELECT
f.name AS ForeignKey,
OBJECT_NAME(f.parent_object_id) AS TableName,
COL_NAME(fc.parent_object_id,
fc.parent_column_id) AS ColumnName,
OBJECT_NAME (f.referenced_object_id) AS ReferenceTableName,
COL_NAME(fc.referenced_object_id,
fc.referenced_column_id) AS ReferenceColumnName
FROM
sys.foreign_keys AS f
INNER JOIN sys.foreign_key_columns AS fc ON f.OBJECT_ID = fc.constraint_object_id
) as g
where g.TableName = @baseTableName
and g.ReferenceTableName = @foreignTableName
and g.ColumnName = @foreignColumnName
)');


-- Procedure Script From usp_UpdateMobileNotificationLogsMarkReadOrDelete.dxml
EXEC('IF (OBJECT_ID(''[dbo].[usp_UpdateMobileNotificationLogsMarkReadOrDelete]'', ''P'') IS NOT NULL) BEGIN DROP PROCEDURE [dbo].[usp_UpdateMobileNotificationLogsMarkReadOrDelete] END');
EXEC('Create PROCEDURE [dbo].[usp_UpdateMobileNotificationLogsMarkReadOrDelete]
(
@pType   varchar(10),
@pEmail   varchar(70),
@pNotificationId  varchar(50),
@pIsAll    bit
)
as
IF @pType = ''Delete''
BEGIN;
IF @pIsAll = 1
BEGIN;
Update   MobileNotificationLogs Set IsDelete = 1 where [Email] =  @pEmail ;
End;
ELSE
BEGIN;
Update   MobileNotificationLogs Set IsDelete = 1 where [Id] =  @pNotificationId ;
End;
End;
IF @pType = ''Read''
BEGIN;
IF @pIsAll = 1
BEGIN;
Update   MobileNotificationLogs Set IsRead = 1 where [Email] =  @pEmail ;
End;
ELSE
BEGIN;
Update   MobileNotificationLogs Set IsRead = 1 where [Id] =  @pNotificationId ;
End;
End;');


