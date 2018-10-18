delete from ScreenFields where ScreenId = (select Id from Screens where Code = 'Stage.GeneralTabScreen')
delete from Screens where Code = 'Stage.GeneralTabScreen'