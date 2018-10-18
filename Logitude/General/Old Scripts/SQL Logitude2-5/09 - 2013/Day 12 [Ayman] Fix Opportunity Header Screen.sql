
--1 Run this Script
--2 Update CRM

begin transaction
begin

delete from ScreenFields where ScreenId = (select Id from Screens where Code = 'Opportunity.HeaderScreen')

update ObjectTables set HeaderScreenId = null where Name = 'Opportunity'

delete from Screens where Code = 'Opportunity.HeaderScreen'

END
commit transaction
