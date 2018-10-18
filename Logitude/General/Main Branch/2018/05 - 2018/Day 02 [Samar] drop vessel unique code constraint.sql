

--declare @const_Name as varchar(100)

--set @const_Name = (select CONSTRAINT_NAME from INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE TABLE_NAME ='Vessels' and CONSTRAINT_TYPE = 'UNIQUE')

--print @const_Name

alter table vessels drop constraint [UQ_Tenant_Code_Vessels]