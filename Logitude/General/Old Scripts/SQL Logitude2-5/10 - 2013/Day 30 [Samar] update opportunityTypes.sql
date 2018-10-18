
update Opportunities set OpportunityTypeCode = 'N' where OpportunityTypeCode = 'Q'
delete from OpportunityTypes where Code = 'q'