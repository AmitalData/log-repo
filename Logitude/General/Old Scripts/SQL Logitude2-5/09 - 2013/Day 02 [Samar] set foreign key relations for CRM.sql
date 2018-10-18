

alter table [OpportunityProducts] add constraint [OpportunityProduct_ProductType] foreign key ([OpportunityProductTypeCode]) references [ProductTypes]([Code]);
go

alter table [OpportunityProducts] add constraint [OpportunityProduct_ProductPeriod] foreign key ([OpportunityProductPeriodCode]) references [ProductPeriods]([Code]);
go

drop table [OpportunityProductTypes]
go

drop table [OpportunityProductPeriods]
go