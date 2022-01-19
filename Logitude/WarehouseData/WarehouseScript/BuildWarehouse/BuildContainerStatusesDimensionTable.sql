DECLARE @Code as varchar(3)
DECLARE @Name as varchar(40)
DECLARE @AutomaticLastUpdateDate as datetime

DECLARE ContainerStatusesCursor CURSOR READ_ONLY
FOR
SELECT Code, Name, AutomaticLastUpdateDate
From dw_ContainerStatuses
where dw_ContainerStatuses.Code !='-1'
OPEN ContainerStatusesCursor FETCH NEXT FROM ContainerStatusesCursor INTO @Code , @Name, @AutomaticLastUpdateDate
WHILE @@FETCH_STATUS = 0
BEGIN
	
insert into #DIM_ContainerStatusesTemp (Code,Name,[Automatic Last Update Date]) values(@Code,@Name, @AutomaticLastUpdateDate)

FETCH NEXT FROM ContainerStatusesCursor INTO @Code , @Name, @AutomaticLastUpdateDate
	End
CLOSE ContainerStatusesCursor
DEALLOCATE ContainerStatusesCursor
