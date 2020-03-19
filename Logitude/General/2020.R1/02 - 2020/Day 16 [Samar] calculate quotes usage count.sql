

update Quotes set UsageCount = (select COUNT(*) from Shipments where quoteId = Quotes.Id)

