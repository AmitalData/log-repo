

delete from ScreenFields where ScreenId = (select id from Screens where Code = 'Opportunity.HeaderScreen')
go
