
begin transaction
begin

alter table Opportunities drop Opportunity_Currency

alter table Opportunities drop column CurrencyId

END
commit transaction