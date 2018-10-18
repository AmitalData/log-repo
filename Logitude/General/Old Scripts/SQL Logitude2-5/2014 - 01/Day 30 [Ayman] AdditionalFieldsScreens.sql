
-- Change Screens from New to AdditionalFields
-- Run this Script then update Tenants & CRM

delete from ScreenFields where ScreenId = (select Id from Screens where Code = 'NewCustomer' and ObjectTableId = (select Id from ObjectTables where Name = 'Customer'))
delete from ScreenFields where ScreenId = (select Id from Screens where Code = 'NewActivity' and ObjectTableId = (select Id from ObjectTables where Name = 'Activity'))
delete from ScreenFields where ScreenId = (select Id from Screens where Code = 'NewOpportunity' and ObjectTableId = (select Id from ObjectTables where Name = 'Opportunity'))

delete from ScreenModifications where ScreenId = (select Id from Screens where Code = 'NewCustomer' and ObjectTableId = (select Id from ObjectTables where Name = 'Customer'))
delete from ScreenModifications where ScreenId = (select Id from Screens where Code = 'NewActivity' and ObjectTableId = (select Id from ObjectTables where Name = 'Activity'))
delete from ScreenModifications where ScreenId = (select Id from Screens where Code = 'NewOpportunity' and ObjectTableId = (select Id from ObjectTables where Name = 'Opportunity'))

delete from Screens where Code = 'NewCustomer' and ObjectTableId = (select Id from ObjectTables where Name = 'Customer')
delete from Screens where Code = 'NewActivity' and ObjectTableId = (select Id from ObjectTables where Name = 'Activity')
delete from Screens where Code = 'NewOpportunity' and ObjectTableId = (select Id from ObjectTables where Name = 'Opportunity')


