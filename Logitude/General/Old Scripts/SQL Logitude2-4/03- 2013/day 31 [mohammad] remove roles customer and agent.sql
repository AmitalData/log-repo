
delete from RoleFeatures where RoleId=(select id from roles where code='CUST')
delete from RoleFeatures where RoleId=(select id from roles where code='AGNT')
delete from ContactTenantRoleSet where RoleId=(select id from roles where code='CUST')
delete from ContactTenantRoleSet where RoleId=(select id from roles where code='AGNT')
delete from Roles where code='CUST' or code ='AGNT'

delete from RoleTypes where code='CU' or code='AG'
