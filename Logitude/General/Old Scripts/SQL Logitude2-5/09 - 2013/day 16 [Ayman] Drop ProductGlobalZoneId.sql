

alter table OpportunityProductLocations drop OpportunityProductLocation_GlobalZone
go

begin transaction
begin

alter table OpportunityProductLocations drop column GlobalZoneId

delete from ObjectFields where FieldName = 'GlobalZoneId' and ObjectTableId  = (select Id from ObjectTables where Name = 'OpportunityProductLocation')

delete from TextCodes where Code like '%OpportunityProductLocation.%GlobalZoneId%'

END
commit transaction