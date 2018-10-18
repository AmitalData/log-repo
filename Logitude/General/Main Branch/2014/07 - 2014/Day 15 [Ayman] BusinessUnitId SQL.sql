
update Activities set BusinessUnitId = CONVERT(varchar,Tenant) where Tenant in (select id from Tenants)
go

update Opportunities set BusinessUnitId = CONVERT(varchar,Tenant) where Tenant in (select id from Tenants)
go

update Quotes set BusinessUnitId = CONVERT(varchar,Tenant) where Tenant in (select id from Tenants)
go

update Customers set BusinessUnitId = CONVERT(varchar,Tenant) where Tenant in (select id from Tenants)
go

update Activities set BusinessUnitId = (select BusinessUnitId from Users where Id = Activities.OwnerId and Tenant = Activities.Tenant)
where OwnerId is not null
go

update Opportunities set BusinessUnitId = (select BusinessUnitId from Users where Id = Opportunities.OwnerId and Tenant = Opportunities.Tenant)
where OwnerId is not null
go

update Quotes set BusinessUnitId = (select BusinessUnitId from Users where Id = Quotes.SalesmanUserId and Tenant = Quotes.Tenant)
where SalesmanUserId is not null
go

update Customers set BusinessUnitId = (select BusinessUnitId from Users where Id = Customers.SalesmanUserId and Tenant = Customers.Tenant)
where SalesmanUserId is not null
go
