Update Contacts
set SearchFields = 
isnull(EnglishName,'') + ',' +
 isnull(LocalName,'') + ',' +
 isnull(Email,'') + ',' + 
  isnull(BusinessPhone,'') + ',' + 
   isnull(Mobile,'') + ',' + 
isnull(Fax,'') + ','

Update Users
set SearchFields = 
isnull(EnglishName,'') + ',' +
 isnull(LocalName,'') + ',' +
  isnull(Position,'') + ',' +
 isnull(Email,'') + ',' + 
  isnull(BusinessPhone,'') + ',' + 
   isnull(Mobile,'') + ',' + 
isnull(Fax,'') + ','

Update u
set SearchFields = 
isnull(EnglishName,'') + ',' +
 isnull(LocalName,'') + ',' +
  isnull(Position,'') + ',' +
 isnull(Email,'') + ',' + 
  isnull(BusinessPhone,'') + ',' + 
   isnull(Mobile,'') + ',' + 
isnull(Fax,'') + ','
from Users as u,Contacts as c
Where u.Id = c.Id

-- scac code is null and prefix is null ( default case)
Update Cards
set SearchFields = 
isnull(EnglishName,'') + ',' +
 isnull(LocalName,'') + ',' +
 isnull(Code,'') + ',' + 
 isnull(VatNumber,'') + ',' + 
isnull(AccountingCard,'') + ','
Where SearchFields is null

-- add prefix
Update car
set SearchFields = 
isnull(EnglishName,'') + ',' +
 isnull(LocalName,'') + ',' +
 isnull(Code,'') + ',' + 
 isnull(VatNumber,'') + ',' + 
 isnull(air.Prefix,'') + ',' + 
isnull(AccountingCard,'') + ','
from Cards as car,Airlines as air
Where car.Id = air.Id and car.SearchFields is null

--add scac code
Update car
set SearchFields = 
isnull(EnglishName,'') + ',' +
 isnull(LocalName,'') + ',' +
 isnull(Code,'') + ',' + 
 isnull(VatNumber,'') + ',' + 
 isnull(ship.SCACCode,'') + ',' + 
isnull(AccountingCard,'') + ','
from Cards as car,ShippingLines as ship
Where car.Id = ship.Id and car.SearchFields is null



Update PotentialCustomers
set SearchFields = 
isnull(EnglishName,'') + ',' +
 isnull(LocalName,'') + ',' +
 isnull(ContactName,'') + ',' + 
 isnull(PhoneNumber,'') + ',' + 
 isnull(FaxNumber,'') + ',' + 
isnull(ContactEmail,'') + ','

   Update Incoterms
set SearchFields = 
isnull(Code,'') + ',' +
 isnull(Name,'') + ',' +
isnull(LocalName,'') + ','
 
    Update PaymentTerms
set SearchFields = 
isnull(EnglishName,'') + ',' +
isnull(LocalName,'') + ','

   Update Currencies
set SearchFields = 
isnull(Code,'') + ',' +
 isnull(EnglishName,'') + ',' +
isnull(LocalName,'') + ','

   Update VatTypes
set SearchFields = 
isnull(Code,'') + ',' +
 isnull(EnglishName,'') + ',' +
isnull(LocalName,'') + ','

   Update ChargesTypes
set SearchFields = 
isnull(Code,'') + ',' +
 isnull(EnglishName,'') + ',' +
isnull(LocalName,'') + ','

   Update Countries
set SearchFields = 
isnull(Code,'') + ',' +
 isnull(EnglishName,'') + ',' +
isnull(LocalName,'') + ','

   Update GlobalZones
set SearchFields = 
isnull(Code,'') + ',' +
 isnull(EnglishName,'') + ',' +
isnull(LocalName,'') + ','

   Update Branches
set SearchFields = 
isnull(EnglishName,'') + ',' +
isnull(LocalName,'') + ','

   Update Departments
set SearchFields = 
isnull(EnglishName,'') + ',' +
isnull(LocalName,'') + ','

   Update States
set SearchFields = 
isnull(Code,'') + ',' +
 isnull(EnglishName,'') + ',' +
isnull(LocalName,'') + ','

   Update DocumentTypes
set SearchFields = 
isnull(Name,'') + ',' +
isnull(Code,'') + ','

   Update EventTypes
set SearchFields = 
isnull(Code,'') + ',' +
 isnull(EnglishName,'') + ',' +
isnull(LocalName,'') + ','
where SearchFields is null

   Update PackageTypes
set SearchFields = 
isnull(Code,'') + ',' +
 isnull(EnglishName,'') + ',' +
isnull(LocalName,'') + ','

   Update Vessels
set SearchFields = 
isnull(Code,'') + ',' +
 isnull(EnglishName,'') + ',' +
isnull(LocalName,'') + ','

--   Update WareHouses
--set SearchFields = 
--isnull(Code,'') + ',' +
-- isnull(EnglishName,'') + ',' +
--isnull(LocalName,'') + ','

--   Update Addresses
--set SearchFields = 
--isnull(address1,'') + ',' +
-- isnull(address2,'') + ',' +
--  isnull(city,'') + ',' +
--   isnull(countryEnglishName,'') + ',' +
-- isnull(stateEnglishName,'') + ',' +
-- isnull(zipCode,'') + ',' +
--isnull(ATTN,'') + ','

   Update ChargesGroups
set SearchFields = 
isnull(Code,'') + ',' +
isnull(name,'') + ','

   Update DimensionsUnits
set SearchFields = 
isnull(Code,'') + ',' +
isnull(name,'') + ','

   Update DueTypes
set SearchFields = 
isnull(Code,'') + ',' +
isnull(name,'') + ','


   Update IATACodes
set SearchFields = 
isnull(Code,'') + ',' +
isnull(name,'') + ','

	Update TransportModes
set SearchFields = 
isnull(Id,'') + ',' +
isnull(name,'') + ','

   Update ARInvoiceStatus
set SearchFields = 
isnull(Code,'') + ',' +
isnull(name,'') + ','

   Update ARInvoiceTypes
set SearchFields = 
isnull(Code,'') + ',' +
isnull(name,'') + ','

   Update MarkUpTypes
set SearchFields = 
isnull(Code,'') + ',' +
isnull(name,'') + ','

   Update Measurements
set SearchFields = 
isnull(Code,'') + ',' +
isnull(name,'') + ',' +
isnull(shortName,'') + ',' +
isnull(weightUnitCode,'') + ','

  Update PartnerTypes
set SearchFields = 
isnull(id,'') + ',' +
isnull(name,'') + ','

  Update PrepaidCollects
set SearchFields = 
isnull(id,'') + ',' +
isnull(name,'') + ','

  Update RateClasses
set SearchFields = 
isnull(Code,'') + ',' +
isnull(name,'') + ','

  Update AWBPrintSpecifications
set SearchFields = 
isnull(Code,'') + ',' +
isnull(name,'') + ','

  Update ShipmentPayableStatus
set SearchFields = 
isnull(Code,'') + ',' +
isnull(name,'') + ','

  Update ShipmentReceivableStatus
set SearchFields = 
isnull(Code,'') + ',' +
isnull(name,'') + ','

  Update ShipmentTypes
set SearchFields = 
isnull(id,'') + ',' +
isnull(name,'') + ','

  Update Tenants
set SearchFields = 
isnull(company,'') + ',' +
isnull(email,'') + ',' +
isnull(language,'') + ',' +
isnull((cast (version as varchar(15))),'') + ',' +
isnull(vatNumber,'') + ',' +
isnull(direction,'') + ','

  Update VolumeUnits
set SearchFields = 
isnull(Code,'') + ',' +
isnull(name,'') + ','

  Update WeightUnits
set SearchFields = 
isnull(Code,'') + ',' +
isnull(name,'') + ','


  Update QuoteCustomerTypes
set SearchFields = 
isnull(Code,'') + ',' +
isnull(name,'') + ','


  Update TemplateFormats
set SearchFields = 
isnull(Code,'') + ',' +
isnull(name,'') + ','

Update por
set SearchFields = 
isnull(por.Code,'') + ',' +
 isnull(por.EnglishName,'') + ',' +
 isnull(cou.Code,'') + ',' +
 isnull(cou.EnglishName,'') + ',' +
isnull(por.LocalName,'') + ','
  FROM Ports as por,Countries as cou
  Where  por.CountryId = cou.Id

  -- addresses with states
  Update addr
set SearchFields = 
isnull(addr.address1,'') + ',' +
 isnull(addr.address2,'') + ',' +
  isnull(addr.city,'') + ',' +
   isnull(cou.EnglishName,'') + ',' +
 isnull(stat.EnglishName,'') + ',' +
 isnull(addr.zipCode,'') + ',' +
isnull(addr.ATTN,'') + ','
  FROM Addresses as addr,Countries as cou,States as stat
  Where  addr.CountryId = cou.Id and addr.StateId = stat.Id and addr.SearchFields is null

  -- addresses without states
    Update addr
set SearchFields = 
isnull(addr.address1,'') + ',' +
 isnull(addr.address2,'') + ',' +
  isnull(addr.city,'') + ',' +
   isnull(cou.EnglishName,'') + ',' +
 isnull(stat.EnglishName,'') + ',' +
 isnull(addr.zipCode,'') + ',' +
isnull(addr.ATTN,'') + ','
  FROM Addresses as addr,Countries as cou,States as stat
  Where  addr.CountryId = cou.Id and addr.SearchFields is null

    -- addresses without countries and states 
    Update addr
set SearchFields = 
isnull(addr.address1,'') + ',' +
 isnull(addr.address2,'') + ',' +
  isnull(addr.city,'') + ',' +
   isnull(cou.EnglishName,'') + ',' +
 isnull(stat.EnglishName,'') + ',' +
 isnull(addr.zipCode,'') + ',' +
isnull(addr.ATTN,'') + ','
  FROM  Addresses as addr,Countries as cou,States as stat
   Where  addr.CountryId is null and addr.stateid is null and addr.SearchFields is null

  --Where  addr.CountryId = cou.Id and addr.SearchFields is null
  Update Entstat
set SearchFields = 
isnull(Entstat.Code,'') + ',' +
 isnull(Entstat.name,'') + ',' +
isnull(objtbls.Name,'') + ','
  FROM EntityStatus as Entstat,ObjectTables as objtbls
  Where  Entstat.ObjectTableId = objtbls.Id

    Update objtbls
set SearchFields = 
isnull(objtbls.DBTableName,'') + ',' +
 isnull(objtbls.name,'') + ',' +
  isnull(objtbls.KeyPropertyPath,'') + ',' +
   --isnull(objfields.FieldName,'') + ',' +
isnull(objtbls.NewWizardControlName,'') + ','
  FROM ObjectTables as objtbls,ObjectFields as objfields
  --Where  objtbls.SortingByObjectField = objfields.Id

  Update Accounts1
set SearchFields = 
isnull(Code,'') + ',' +
 isnull(Name,'') + ',' +
  isnull(AccountTypeCode,'') + ',' +
isnull(ExternalAccountingCard,'') + ','

 -- maybe we need same script , but without billtoid 
--      Update inv
--set SearchFields = 
--isnull(inv.InvoiceNumber,'') + ',' +
-- isnull(inv.VatNumber,'') + ',' +
--  isnull(inv.DraftNumber,'') + ',' +
--   isnull(invent.EntityReference,'') + ',' +
--isnull(car.EnglishName,'') + ','
--  FROM Invoices as inv,Cards as car,InvoiceEntities as invent
--  Where  invent.InvoiceId = inv.Id and inv.BillToId = car.Id


  -- without carrier ( run first ) 
--      Update quo
--set SearchFields = 
--isnull(quo.QuoteNumber,'') + ',' +
-- isnull(quo.ShipperReference1,'') + ',' +
--  isnull(quo.ShipperReference2,'') + ',' +
--   isnull(quo.ConsigneeReference1,'') + ',' +
--   isnull(quo.ShipperName,'') + ',' +
--   isnull(quo.ConsigneeName,'') + ',' +
--   isnull(por.EnglishName,'') + ',' +
--     isnull(por.Code,'') + ',' +
--   isnull(por2.EnglishName,'') + ',' +
--   isnull(por2.Code,'') + ',' +
--   isnull(quo.CustomerName,'') + ',' +
--   isnull(car.EnglishName,'') + ',' +
--    isnull(car.Code,'') + ',' +
--isnull(quo.ConsigneeReference2,'') + ','
--  FROM Quotes as quo,Ports as por,Ports as por2,Cards as car
--  Where quo.FromPortId = por.Id and quo.ToPortid = por2.Id 

--      Update quo
--set SearchFields = 
--isnull(quo.QuoteNumber,'') + ',' +
-- isnull(quo.ShipperReference1,'') + ',' +
--  isnull(quo.ShipperReference2,'') + ',' +
--   isnull(quo.ConsigneeReference1,'') + ',' +
--   isnull(quo.ShipperName,'') + ',' +
--   isnull(quo.ConsigneeName,'') + ',' +
--   isnull(por.EnglishName,'') + ',' +
--     isnull(por.Code,'') + ',' +
--   isnull(por2.EnglishName,'') + ',' +
--   isnull(por2.Code,'') + ',' +
--   isnull(quo.CustomerName,'') + ',' +
--   isnull(car.EnglishName,'') + ',' +
--    isnull(car.Code,'') + ',' +
--isnull(quo.ConsigneeReference2,'') + ','
--  FROM Quotes as quo,Ports as por,Ports as por2,Cards as car
--  Where quo.FromPortId = por.Id and quo.ToPortid = por2.Id and quo.MainCarriageCarrierId = car.Id

--       Update shi
--set SearchFields = 
--isnull(shi.ShipmentNumber,'') + ',' +
-- isnull(shi.ShipperReference1,'') + ',' +
--  isnull(shi.ShipperReference2,'') + ',' +
--   isnull(shi.ConsigneeReference1,'') + ',' +
--    isnull(shi.ConsigneeReference2,'') + ',' +
--   isnull(shi.House,'') + ',' +
--    isnull(shi.AgentReference1,'') + ',' +
--   isnull(shi.AgentReference2,'') + ',' +
--    isnull(shi.CustomAgentImportReference,'') + ',' +
--	 isnull(shi.CustomAgentExportReference,'') + ',' +
--	  isnull(shi.FreightForwarderReference,'') + ',' +
--	   isnull(shi.CustomerReference,'') + ',' +
--isnull(shi.ConsigneeReference2,'') + ','
--  FROM Shipments as shi

--     Update shi
--set SearchFields = 
--isnull(shi.ShipmentNumber,'') + ',' +
-- isnull(shi.ShipperReference1,'') + ',' +
--  isnull(shi.ShipperReference2,'') + ',' +
--   isnull(shi.ConsigneeReference1,'') + ',' +
--    isnull(shi.ConsigneeReference2,'') + ',' +
--   isnull(shi.House,'') + ',' +
--    isnull(shi.AgentReference1,'') + ',' +
--   isnull(shi.AgentReference2,'') + ',' +
--    isnull(shi.CustomAgentImportReference,'') + ',' +
--	 isnull(shi.CustomAgentExportReference,'') + ',' +
--	  isnull(shi.FreightForwarderReference,'') + ',' +
--	   isnull(shi.CustomerReference,'') + ',' +
--   isnull(por.EnglishName,'') + ',' +
--     isnull(por.Code,'') + ',' +
--   isnull(por2.EnglishName,'') + ',' +
--   isnull(por2.Code,'') + ',' +
--isnull(shi.ConsigneeReference2,'') + ','
--  FROM Shipments as shi,Ports as por,Ports as por2
--  Where shi.FromPortId = por.Id and shi.ToPortid = por2.Id

--      Update shi
--set SearchFields = 
--isnull(shi.ShipmentNumber,'') + ',' +
-- isnull(shi.ShipperReference1,'') + ',' +
--  isnull(shi.ShipperReference2,'') + ',' +
--   isnull(shi.ConsigneeReference1,'') + ',' +
--    isnull(shi.ConsigneeReference2,'') + ',' +
--   isnull(shi.House,'') + ',' +
--    isnull(shi.AgentReference1,'') + ',' +
--   isnull(shi.AgentReference2,'') + ',' +
--    isnull(shi.CustomAgentImportReference,'') + ',' +
--	 isnull(shi.CustomAgentExportReference,'') + ',' +
--	  isnull(shi.FreightForwarderReference,'') + ',' +
--	   isnull(shi.CustomerReference,'') + ',' +
--   isnull(por.EnglishName,'') + ',' +
--     isnull(por.Code,'') + ',' +
--   isnull(por2.EnglishName,'') + ',' +
--   isnull(por2.Code,'') + ',' +
--   isnull(car.EnglishName,'') + ',' +
--    isnull(car.Code,'') + ',' +
--	 isnull(car2.EnglishName,'') + ',' +
--    isnull(car2.Code,'') + ',' +
--isnull(shi.ConsigneeReference2,'') + ','
--  FROM Shipments as shi,Ports as por,Ports as por2,Cards as car,Cards as car2
--  Where shi.FromPortId = por.Id and shi.ToPortid = por2.Id and shi.ShipperId = car.Id and shi.ConsigneeId = car2.Id