

delete from ScreenModifications where ScreenId = (select Id from Screens where Code = 'Opportunity.GeneralTabScreen')
go

delete from ScreenFields where ScreenId = (select Id from Screens where Code = 'Opportunity.GeneralTabScreen')
go

delete from Screens where Code = 'Opportunity.GeneralTabScreen'
go
