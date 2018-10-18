delete from ScreenFields where ScreenId in (select Id from Screens where Code = 'APILogs.GeneralTabScreen')

