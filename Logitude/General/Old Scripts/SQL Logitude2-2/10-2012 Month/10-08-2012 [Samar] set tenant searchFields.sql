

Update TenantManagements
set SearchFields = 
isnull(CONVERT(varchar,Id),'') + ',' +
isnull(Name,'') + ',' +
isnull(CONVERT(varchar,NumberOfUsers),'')