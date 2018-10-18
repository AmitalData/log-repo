Update ErrorLogs
set SearchFields = 
isnull(UserName,'') + ',' +
 isnull(Tier,'') + ',' +
 isnull(CAST(Tenant as varchar(15)) ,'') + ',' +
 isnull(Exception,'') + ',' 
 