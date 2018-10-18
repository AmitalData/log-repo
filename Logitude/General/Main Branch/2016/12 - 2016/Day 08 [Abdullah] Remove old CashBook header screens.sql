delete from ScreenFields where ScreenId = (select Id from Screens where Code = 'CashBook.CashBookHeaderScreen')
delete from ScreenFields where ScreenId = (select Id from Screens where Code = 'CashBook.CashBookGeneralScreen')
delete from Screens where Code = 'CashBook.CashBookHeaderScreen' 
delete from Screens where Code = 'CashBook.CashBookGeneralScreen' 