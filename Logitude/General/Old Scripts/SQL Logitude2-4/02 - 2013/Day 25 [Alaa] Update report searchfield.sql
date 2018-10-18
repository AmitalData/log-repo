update reports set searchfields=
ISNULL( Name, '') + ','+
ISNULL( Description, '') + ','+
 isnull(CAST(Tenant as varchar(15)) ,'') + ',' 



