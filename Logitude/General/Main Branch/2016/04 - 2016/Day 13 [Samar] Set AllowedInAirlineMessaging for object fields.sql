
update ObjectFields
set AllowedInAirlineMessaging = 1
where ObjectTableId = (select Id from ObjectTables where Name= 'Shipment')
and FieldName = 'DescriptionOfGoods' or FieldName = 'IssuingCarrierIATACode' or FieldName = 'CASSCode' or FieldName = 'AWBCommodityItemNumber' or FieldName = 'AWBAccountingInformation' 
or FieldName = 'AWBHandlingInformation' or FieldName = 'AWBSpecialHandlingCodeId1' or FieldName = 'AWBSpecialHandlingCodeId2' or FieldName = 'AWBSpecialHandlingCodeId3' 
or FieldName = 'AWBSpecialHandlingCodeId4' or FieldName = 'AWBSpecialHandlingCodeId5' or FieldName = 'AWBSpecialHandlingCodeId6' or FieldName = 'AWBSpecialHandlingCodeId7'
or FieldName = 'AWBSpecialHandlingCodeId8' or FieldName = 'AWBSpecialHandlingCodeId9' or FieldName = 'ReferenceNumber' or FieldName = 'SupplementaryShipmentInformation1'
or FieldName = 'SupplementaryShipmentInformation2' or FieldName = 'NominatedHandlingPartyId'or FieldName = 'OtherParticipantInformationId1'
or FieldName = 'OtherParticipantInformationId2' or FieldName = 'OtherParticipantInformationId3' or FieldName = 'AWBChargeRate'

update ObjectFields
set AllowedInAirlineMessaging = 1
where ObjectTableId = (select Id from ObjectTables where Name= 'Booking')
and FieldName = 'ShipperId' or FieldName = 'ConsigneeId' or FieldName = 'IssuingCarrierIATACode' or FieldName = 'CASSCode' or FieldName = 'BookingProductId' 
or FieldName = 'AWBCarrierTarrifReference' or FieldName = 'AWBSpecialHandlingCodeId1' or FieldName = 'AWBSpecialHandlingCodeId2' or FieldName = 'AWBSpecialHandlingCodeId3' 
or FieldName = 'AWBSpecialHandlingCodeId4' or FieldName = 'AWBSpecialHandlingCodeId5' or FieldName = 'AWBSpecialHandlingCodeId6' or FieldName = 'AWBSpecialHandlingCodeId7'
or FieldName = 'AWBSpecialHandlingCodeId8' or FieldName = 'AWBSpecialHandlingCodeId9' or FieldName = 'SpecialServicesRequest' or FieldName = 'OtherServicesInformation'

update ObjectFields
set AllowedInAirlineMessaging = 1
where ObjectTableId = (select Id from ObjectTables where Name= 'FlightsSchedulesRequest')
and FieldName = 'GrossWeight' or FieldName = 'Volume'