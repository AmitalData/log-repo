
delete from ScreenFields where ScreenId = (select Id from Screens where Code = 'Contact.GeneralTabScreen')

delete from Screens where Code = 'Contact.GeneralTabScreen'