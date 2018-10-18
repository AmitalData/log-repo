
delete from ObjectFields where FieldName = 'PotentialShipperId'
go

delete from ObjectFields where FieldName = 'PotentialConigneeId'
go

delete from ObjectFields where FieldName = 'PotentialCustomerId'
go

delete from TextCodes where Code = 'Quote.F.PotentialShipperId'
delete from TextCodes where Code = 'Quote.PotentialShipperIdHelpText'

delete from TextCodes where Code = 'Quote.F.PotentialConigneeId'
delete from TextCodes where Code = 'Quote.PotentialConigneeIdHelpText'

delete from TextCodes where Code = 'Quote.F.PotentialCustomerId'
delete from TextCodes where Code = 'Quote.PotentialCustomerIdHelpText'
