delete from Screens where Code = 'Warehouse.GeneralTabScreen'
delete from ScreenModifications where ScreenId in (select Id from Screens where Code = 'Warehouse.GeneralTabScreen')
delete from ScreenFields where ScreenId in (select Id from Screens where Code = 'Warehouse.GeneralTabScreen')
