update TextCodes set Code = 'Shipment.F.FollowUpType' where Code=  'Shipment.F.FUType'
update TextCodes set Code = 'Shipment.F.FollowUpDate' where Code=  'Shipment.F.FUDate'
update TextCodes set Code = 'Shipment.F.FollowUpNotes' where Code=  'Shipment.F.FUNotes'
--delete from TextCodes where Code = 'Shipment.OpenShipmentsHelpText'
delete from TextCodes where Code = 'Shipment.AccountingOpenHelpText'
delete from TextCodes where Code = 'Shipment.LastWeeksUpdateHelpText'

UPDATE ObjectFields 
SET FieldName  = LTRIM(RTRIM(FieldName))
where FieldName =  'MissingDocuments '

UPDATE TextCodes 
SET Code  = LTRIM(RTRIM(Code))
where Code =  'Shipment.F.MissingDocuments '