

delete from ScreenFields where ScreenId = (select id from Screens where Code = 'Customer.GeneralTabScreen')
delete from ScreenModifications where ScreenId = (select id from Screens where Code = 'Customer.GeneralTabScreen')
delete from screens where Code = 'Customer.GeneralTabScreen'

delete from ObjectTableTabs where Code = 'CSCD'
delete from TextCodes where Code = 'Customer.TH.CRMDetails'