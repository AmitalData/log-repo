-- **** Copy isactive fields first from tenants table to tenantmanegement before drop this field ***

alter table tenants
drop DF__Tenants__IsActiv__2C3E80C8

alter table tenants
drop column isactive