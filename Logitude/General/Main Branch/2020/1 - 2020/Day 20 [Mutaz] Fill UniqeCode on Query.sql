  update Queries set UniqueCode = (select ObjectTables.Name from ObjectTables where Id= Queries.ObjectTableId)+'.'+Queries.Code;

  ALTER TABLE Queries ALTER COLUMN UniqueCode VARCHAR(200) NOT NULL;
