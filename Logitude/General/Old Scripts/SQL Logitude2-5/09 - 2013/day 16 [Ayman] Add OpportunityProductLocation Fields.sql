
begin transaction
begin

alter table OpportunityProductLocations add TEU decimal null

alter table OpportunityProductLocations add Revenue decimal null

alter table OpportunityProductLocations add ChargeableWeight decimal null

alter table OpportunityProductLocations add NumberOfShipments int null

END
commit transaction