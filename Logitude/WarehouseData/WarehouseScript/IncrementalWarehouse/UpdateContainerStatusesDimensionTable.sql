
declare @MaxAutomaticLastUpdateDate as datetime
declare @LastUpdateDate as datetime

set @LastUpdateDate = (select top(1) LastUpdateDate from dw_WaterMarks  where TableName = 'ContainerStatus' )
set @MaxAutomaticLastUpdateDate = (select  MAX( AutomaticLastUpdateDate) AutomaticLastUpdateDate from dw_ContainerStatuses )

 
if(@MaxAutomaticLastUpdateDate > @LastUpdateDate)

begin

declare @Key as varchar(3)
declare @Code as varchar(3)
declare @Name as varchar(40)
declare @AutomaticLastUpdateDate as datetime

DECLARE ContainerStatusesCursor CURSOR READ_ONLY
FOR
SELECT Code, Name, AutomaticLastUpdateDate
From dw_ContainerStatuses
where AutomaticLastUpdateDate > @LastUpdateDate
OPEN ContainerStatusesCursor FETCH NEXT FROM ContainerStatusesCursor INTO @Code , @Name , @AutomaticLastUpdateDate
WHILE @@FETCH_STATUS = 0
BEGIN

set @Key = (select Code from DIM_ContainerStatuses where Code = @Code)
if(@Key is  null) begin insert into DIM_ContainerStatuses (Code,Name,[Automatic Last Update Date]) values(@Code,@Name,@AutomaticLastUpdateDate); end
else begin update   DIM_ContainerStatuses set Name =@Name, [Automatic Last Update Date] = @AutomaticLastUpdateDate Where Code = @Code; end
		
FETCH NEXT FROM ContainerStatusesCursor INTO @Code , @Name, @AutomaticLastUpdateDate
	End
CLOSE ContainerStatusesCursor
DEALLOCATE ContainerStatusesCursor

update dw_WaterMarks set LastUpdateDate = @MaxAutomaticLastUpdateDate where TableName = 'ContainerStatus'


end


