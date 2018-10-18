

update TextCodes set DefaultText = 'Indicates who the customer is, so that Logitude knows to refer to the relevant partner for statistics, billing and shared logistics. For Export, the Shipper is selected automatically. For Import, the Consignee is selected automatically.'
where Code = 'Shipment.CustomerIdHelpText'
go


delete from QueryColumns where QueryId = (Select Id from Queries where Code = 'Expected Departures')
go