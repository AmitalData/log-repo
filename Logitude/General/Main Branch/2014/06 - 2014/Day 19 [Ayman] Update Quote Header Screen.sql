
-- Run this SQL then update Tenants
delete from ScreenFields where ScreenId = (select Id from Screens where Code = 'Quote.HeaderScreen')
go
