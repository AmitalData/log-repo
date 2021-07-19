declare @LastIdNumber as int
declare @NextIdNumber as int
declare @Tenant as int
declare @SystemUserId as varchar(15)

DECLARE BIReportFolders CURSOR READ_ONLY
FOR
	SELECT [Id] FROM Tenants WHERE [Id] not in (SELECT [Tenant] FROM BIReportFolders WHERE [Name]='General')
OPEN BIReportFolders FETCH NEXT FROM BIReportFolders INTO @Tenant
WHILE @@FETCH_STATUS = 0
BEGIN
	set @SystemUserId = (select [Id] from Users where [Tenant] = @Tenant and [SearchFields] like 'system@tenant%')
	set @LastIdNumber = (select [LastIdNumber] from DBIdCounters where [TableName] ='BIReportFolder')
	set @NextIdNumber = @LastIdNumber + 1
	insert into BIReportFolders 
	values('1-' + CAST(@NextIdNumber as varchar(15)),@Tenant,GETDATE(),@SystemUserId,GETDATE(),@SystemUserId,'General','General',null,0,1,null)
	update DBIdCounters set LastIdNumber = @NextIdNumber where TableName='BIReportFolder'

FETCH NEXT FROM BIReportFolders INTO @Tenant
END
CLOSE BIReportFolders
DEALLOCATE BIReportFolders