-- execute then update

delete from ScreenFields where ScreenId = (select Id from Screens where Code = 'Customer.HeaderScreen')