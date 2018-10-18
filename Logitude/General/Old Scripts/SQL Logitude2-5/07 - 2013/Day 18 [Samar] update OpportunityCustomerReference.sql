

alter table [Opportunities] drop constraint [Opportunity_Customer] 
go

alter table [Opportunities] add constraint [Opportunity_Card] foreign key ([CustomerId]) references [Cards]([Id]);
go