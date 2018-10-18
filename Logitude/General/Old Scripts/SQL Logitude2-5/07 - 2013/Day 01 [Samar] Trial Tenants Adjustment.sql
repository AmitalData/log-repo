-- do not run online
delete from QueryColumns
delete from AdvancedQueryFilters
delete from Queries where Code = 'Trail Tenant Managements'
delete from Queries where NameTextCodeId = (select Id from TextCodes where Code = 'TenantManagement.Q.TrailTenantManagements')
delete from TextCodes where Code = 'TenantManagement.Q.TrailTenantManagements'