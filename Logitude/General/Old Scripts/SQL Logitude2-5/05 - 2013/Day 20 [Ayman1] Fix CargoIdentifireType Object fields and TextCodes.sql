
delete from ObjectFields where ObjectTableId = (Select Id from ObjectTables where Name = 'Customs.CargoIdentifireType')
go

delete from TextCodes where ObjectTableId = (Select Id from ObjectTables where Name = 'Customs.CargoIdentifireType')
go

update ObjectFields set LookUpTableId = NULL where LookUpTableId = (Select Id from ObjectTables where Name = 'Customs.CargoIdentifireType')
go

delete from ObjectTables where name ='Customs.CargoIdentifireType'
go