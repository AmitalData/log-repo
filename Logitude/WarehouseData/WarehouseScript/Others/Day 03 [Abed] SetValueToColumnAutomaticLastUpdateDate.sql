update  Shipments set AutomaticLastUpdateDate = GETDATE() where AutomaticLastUpdateDate is null
update  Cards set AutomaticLastUpdateDate =GETDATE() where AutomaticLastUpdateDate is null
update  Directions set AutomaticLastUpdateDate =GETDATE() where AutomaticLastUpdateDate is null
update  Ports set AutomaticLastUpdateDate =GETDATE() where AutomaticLastUpdateDate is null
update TransportModes set AutomaticLastUpdateDate =GETDATE() where AutomaticLastUpdateDate is null
update  ShipmentLevels set AutomaticLastUpdateDate =GETDATE() where AutomaticLastUpdateDate is null
update  ShipmentTypes set AutomaticLastUpdateDate =GETDATE() where AutomaticLastUpdateDate is null
update  Departments set AutomaticLastUpdateDate =GETDATE() where AutomaticLastUpdateDate is null
update  Incoterms set AutomaticLastUpdateDate =GETDATE() where AutomaticLastUpdateDate is null
update  Users set AutomaticLastUpdateDate =GETDATE() where AutomaticLastUpdateDate is null
update  Currencies set AutomaticLastUpdateDate =GETDATE() where AutomaticLastUpdateDate is null
update  EntityStatus set AutomaticLastUpdateDate =GETDATE() where AutomaticLastUpdateDate is null
update  Addresses set AutomaticLastUpdateDate =GETDATE() where AutomaticLastUpdateDate is null
update  Countries set AutomaticLastUpdateDate =GETDATE() where AutomaticLastUpdateDate is null
update  Contacts set AutomaticLastUpdateDate =GETDATE() where AutomaticLastUpdateDate is null
update  States set AutomaticLastUpdateDate =GETDATE() where AutomaticLastUpdateDate is null
update  Customers set AutomaticLastUpdateDate =GETDATE() where AutomaticLastUpdateDate is null
update  PartnerTypes set AutomaticLastUpdateDate =GETDATE() where AutomaticLastUpdateDate is null
update  ShipmentMasterDatas set AutomaticLastUpdateDate =GETDATE() where AutomaticLastUpdateDate is null
update  Tenants set AutomaticLastUpdateDate =GETDATE() where AutomaticLastUpdateDate is null
update  Branches set AutomaticLastUpdateDate =GETDATE() where AutomaticLastUpdateDate is null
update  Ranks set AutomaticLastUpdateDate =GETDATE() where AutomaticLastUpdateDate is null
update  DWHSettings set AutomaticLastUpdateDate =GETDATE() where AutomaticLastUpdateDate is null
update  MoveTypes set AutomaticLastUpdateDate =GETDATE() where AutomaticLastUpdateDate is null
update  Vessels set AutomaticLastUpdateDate =GETDATE() where AutomaticLastUpdateDate is null
update  SpecialServicesTypes set AutomaticLastUpdateDate =GETDATE() where AutomaticLastUpdateDate is null
update  ObjectFields set AutomaticLastUpdateDate =GETDATE() where AutomaticLastUpdateDate is null
update  CustomPickLists set AutomaticLastUpdateDate =GETDATE() where AutomaticLastUpdateDate is null
update  Regions set AutomaticLastUpdateDate =GETDATE() where AutomaticLastUpdateDate is null
update  CustomerSizes set AutomaticLastUpdateDate =GETDATE() where AutomaticLastUpdateDate is null
update  Industries set AutomaticLastUpdateDate =GETDATE() where AutomaticLastUpdateDate is null
update  ShipmentComputedFields set AutomaticLastUpdateDate =GETDATE() where AutomaticLastUpdateDate is null
update  LeadSources set AutomaticLastUpdateDate =GETDATE() where AutomaticLastUpdateDate is null
update  OBLTypes set AutomaticLastUpdateDate =GETDATE() where AutomaticLastUpdateDate is null



 --____________________________________ Fact Charge_____________________________________

 update  ShipmentPayables set AutomaticLastUpdateDate =GETDATE() where AutomaticLastUpdateDate is null
 update  APInvoiceLines set AutomaticLastUpdateDate =GETDATE() where AutomaticLastUpdateDate is null
 update  APInvoices set AutomaticLastUpdateDate =GETDATE() where AutomaticLastUpdateDate is null
 update  ShipmentReceivables set AutomaticLastUpdateDate =GETDATE() where AutomaticLastUpdateDate is null
 update  ARInvoiceLines set AutomaticLastUpdateDate =GETDATE() where AutomaticLastUpdateDate is null
 update  ARInvoices set AutomaticLastUpdateDate =GETDATE() where AutomaticLastUpdateDate is null
 update  ChargesTypes set AutomaticLastUpdateDate =GETDATE() where AutomaticLastUpdateDate is null

 update  QuoteStages set AutomaticLastUpdateDate =GETDATE() where AutomaticLastUpdateDate is null
 update  QuoteClosingReasons set AutomaticLastUpdateDate =GETDATE() where AutomaticLastUpdateDate is null
 update  Quotes set AutomaticLastUpdateDate = GETDATE() where AutomaticLastUpdateDate is null
 --update  ChargesGroups set AutomaticLastUpdateDate =GETDATE() where AutomaticLastUpdateDate is null