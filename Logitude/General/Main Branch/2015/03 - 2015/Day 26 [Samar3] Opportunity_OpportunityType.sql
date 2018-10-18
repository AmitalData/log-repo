
alter table OpportunityTypes add primary key (Id)
go

alter table Opportunities add constraint [Opportunity_OpportunityType] foreign key ([OpportunityTypeId]) references [OpportunityTypes]([Id]);
go

alter table Opportunities alter column OpportunityTypeCode [varchar] (1) null    
go

alter table OpportunityTypes alter column Code [varchar] (1) null    
go