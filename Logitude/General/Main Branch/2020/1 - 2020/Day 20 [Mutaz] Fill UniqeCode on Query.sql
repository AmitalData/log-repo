  update Queries set UniqueCode = (select ObjectTables.Name from ObjectTables where Id= Queries.ObjectTableId)+'.'+Queries.UserId+'.'+Queries.Code where  userid is not null;
  update Queries set UniqueCode = (select ObjectTables.Name from ObjectTables where Id= Queries.ObjectTableId)+'.'+Queries.Code where Tenant = 0 and userid is null;
  ALTER TABLE Queries ALTER COLUMN UniqueCode VARCHAR(200) NOT NULL;
 
 --Aplay this only if you have same constrient
 --Alter TABLE QueryColumns DROP CONSTRAINT  UQ_Tenant_QueryId_ObjectFieldId_UserId

 