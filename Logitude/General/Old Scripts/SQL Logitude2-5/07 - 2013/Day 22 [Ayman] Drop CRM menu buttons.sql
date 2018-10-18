
delete from MenuButtons where MenuButtonGroupId in (select Id from MenuButtonGroups where ObjectTableId = (Select Id from ObjectTables Where Name = 'Opportunity'))
go

delete from MenuButtons where MenuButtonGroupId in (select Id from MenuButtonGroups where ObjectTableId = (Select Id from ObjectTables Where Name = 'Activity'))
go

delete from MenuButtonGroups where ObjectTableId = (Select Id from ObjectTables Where Name = 'Opportunity')
go

delete from MenuButtonGroups where ObjectTableId = (Select Id from ObjectTables Where Name = 'Activity')
go
