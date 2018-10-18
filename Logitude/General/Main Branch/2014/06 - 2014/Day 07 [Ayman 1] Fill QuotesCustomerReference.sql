
update Quotes set
CustomerReference1 = ShipperReference1,
CustomerReference2 = ShipperReference2
where QuoteCustomerTypeCode = 'SHI'
go

update Quotes set
CustomerReference1 = ConsigneeReference1,
CustomerReference2 = ConsigneeReference2
where QuoteCustomerTypeCode = 'CON'
go

