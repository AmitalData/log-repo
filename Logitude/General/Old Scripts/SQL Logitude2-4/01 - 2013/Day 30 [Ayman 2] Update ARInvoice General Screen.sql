
-- march 31 NOT excuted 

update Screens set NumberOfRows = 3 where Code = 'ARInvoice.GeneralTabScreen'
delete from ScreenFields where ScreenId = (select Id from Screens where Code = 'ARInvoice.GeneralTabScreen')

-- Then update Tenant 0