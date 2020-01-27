  update Queries set UniqueCode = (select ObjectTables.Name from ObjectTables where Id= Queries.ObjectTableId)+'.'+Queries.UserId+'.'+Queries.Code where Tenant != 0 ;
  update Queries set UniqueCode = (select ObjectTables.Name from ObjectTables where Id= Queries.ObjectTableId)+'.'+Queries.Code where Tenant = 0 ;
  ALTER TABLE Queries ALTER COLUMN UniqueCode VARCHAR(200) NOT NULL;
 
 --Aplay this only if you have same constrient
 --Alter TABLE QueryColumns DROP CONSTRAINT  UQ_Tenant_QueryId_ObjectFieldId_UserId

 