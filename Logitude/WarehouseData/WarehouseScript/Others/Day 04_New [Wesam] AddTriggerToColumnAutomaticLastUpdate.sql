
--Shipments
IF not EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'TriggerOnCreate_AutomaticLastUpdateDateShipments'))
 Begin
 declare @SQLShipment as varchar(8000)
         SET @SQLShipment =   'CREATE TRIGGER TriggerOnCreate_AutomaticLastUpdateDateShipments ON Shipments AFTER Insert  AS  BEGIN UPDATE Shipments SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;'
         EXEC (@SQLShipment);
 end

 --Cards
IF not EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'TriggerOnCreate_AutomaticLastUpdateDateCards'))
 Begin
 declare @SQLCard as varchar(8000)
         SET @SQLCard ='CREATE TRIGGER TriggerOnCreate_AutomaticLastUpdateDateCards ON Cards AFTER Insert  AS  BEGIN UPDATE Cards SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;'
         EXEC (@SQLCard);
 end

--Tenants
 IF not EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'TriggerOnCreate_AutomaticLastUpdateDateTenants'))
 Begin
 declare @SQLTenant as varchar(8000)
         SET @SQLTenant = 'CREATE TRIGGER TriggerOnCreate_AutomaticLastUpdateDateTenants ON Tenants AFTER Insert  AS  BEGIN UPDATE Tenants SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;'
         EXEC (@SQLTenant);
 end

 --Directions
 IF not EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'TriggerOnCreate_AutomaticLastUpdateDateDirections'))
 Begin
 declare @SQLDirection as varchar(8000)
         SET @SQLDirection = 'CREATE TRIGGER TriggerOnCreate_AutomaticLastUpdateDateDirections ON Directions AFTER Insert  AS  BEGIN UPDATE Directions SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;'
         EXEC (@SQLDirection);
 end

  --Ports
 IF not EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'TriggerOnCreate_AutomaticLastUpdateDatePorts'))
 Begin
 declare @SQLPort as varchar(8000)
         SET @SQLPort ='CREATE TRIGGER TriggerOnCreate_AutomaticLastUpdateDatePorts ON Ports AFTER Insert  AS  BEGIN UPDATE Ports SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;'
         EXEC (@SQLPort);
 end


   --TransportMode
 IF not EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'TriggerOnCreate_AutomaticLastUpdateDateTransportModes'))
 Begin
 declare @SQLTransportMode as varchar(8000)
         SET @SQLTransportMode ='CREATE TRIGGER TriggerOnCreate_AutomaticLastUpdateDateTransportModes ON TransportModes AFTER Insert  AS  BEGIN UPDATE TransportModes SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;'
         EXEC (@SQLTransportMode);
 end


 --ShipmentLevels
 IF not EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'TriggerOnCreate_AutomaticLastUpdateDateShipmentLevels'))
 Begin

 declare @SQLShipmentLevel as varchar(8000)
         SET @SQLShipmentLevel = 'CREATE TRIGGER TriggerOnCreate_AutomaticLastUpdateDateShipmentLevels ON ShipmentLevels AFTER Insert  AS  BEGIN UPDATE ShipmentLevels SET AutomaticLastUpdateDate = GETDATE() WHERE Code IN (SELECT DISTINCT Code FROM Inserted)END;'
         EXEC (@SQLShipmentLevel);
 end


  --ShipmentTypes
 IF not EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'TriggerOnCreate_AutomaticLastUpdateDateShipmentTypes'))
 Begin
 declare @SQLShipmentType as varchar(8000)
         SET @SQLShipmentType ='CREATE TRIGGER TriggerOnCreate_AutomaticLastUpdateDateShipmentTypes ON ShipmentTypes AFTER Insert  AS  BEGIN UPDATE ShipmentTypes SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;'
         EXEC (@SQLShipmentType);
 end



   --Departments
 IF not EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'TriggerOnCreate_AutomaticLastUpdateDateDepartments'))
 Begin
 declare @SQLDepartment as varchar(8000)
         SET @SQLDepartment ='CREATE TRIGGER TriggerOnCreate_AutomaticLastUpdateDateDepartments ON Departments AFTER Insert  AS  BEGIN UPDATE Departments SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;'
         EXEC (@SQLDepartment);
 end



    --Branches
 IF not EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'TriggerOnCreate_AutomaticLastUpdateDateBranches'))
 Begin
 declare @SQLBranches as varchar(8000)
         SET @SQLBranches ='CREATE TRIGGER TriggerOnCreate_AutomaticLastUpdateDateBranches ON Branches AFTER Insert  AS  BEGIN UPDATE Branches SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;'
         EXEC (@SQLBranches);
 end

     --Incoterms
 IF not EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'TriggerOnCreate_AutomaticLastUpdateDateIncoterms'))
 Begin
 declare @SQLIncoterm as varchar(8000)
         SET @SQLIncoterm = 'CREATE TRIGGER TriggerOnCreate_AutomaticLastUpdateDateIncoterms ON Incoterms AFTER Insert  AS  BEGIN UPDATE Incoterms SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;'
         EXEC (@SQLIncoterm);
 end



      --Users
 IF not EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'TriggerOnCreate_AutomaticLastUpdateDateUsers'))
 Begin
 declare @SQLUser  as varchar(8000)
         SET @SQLUser ='CREATE TRIGGER TriggerOnCreate_AutomaticLastUpdateDateUsers ON Users AFTER Insert  AS  BEGIN UPDATE Users SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;'
         EXEC (@SQLUser);
 end


   --Currencies
 IF not EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'TriggerOnCreate_AutomaticLastUpdateDateCurrencies'))
 Begin
 declare @SQLCurrencies as varchar(8000)
         SET @SQLCurrencies ='CREATE TRIGGER TriggerOnCreate_AutomaticLastUpdateDateCurrencies ON Currencies AFTER Insert  AS  BEGIN UPDATE Currencies SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;'
         EXEC (@SQLCurrencies);
 end


      --EntityStatus
 IF not EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'TriggerOnCreate_AutomaticLastUpdateDateEntityStatus'))
 Begin
 declare @SQLEntityStatus as varchar(8000)
         SET @SQLEntityStatus ='CREATE TRIGGER TriggerOnCreate_AutomaticLastUpdateDateEntityStatus ON EntityStatus AFTER Insert  AS  BEGIN UPDATE EntityStatus SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;'
         EXEC (@SQLEntityStatus);

 end



      --Addresses
 IF not EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'TriggerOnCreate_AutomaticLastUpdateDateAddresses'))
 Begin
 declare @SQLAddresses as varchar(8000)
         SET @SQLAddresses = 'CREATE TRIGGER TriggerOnCreate_AutomaticLastUpdateDateAddresses ON Addresses AFTER Insert  AS  BEGIN UPDATE Addresses SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;'
         EXEC (@SQLAddresses);
 end


     --Countries
 IF not EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'TriggerOnCreate_AutomaticLastUpdateDateCountries'))
 Begin
 declare @SQLCountries as varchar(8000)
         SET @SQLCountries ='CREATE TRIGGER TriggerOnCreate_AutomaticLastUpdateDateCountries ON Countries AFTER Insert  AS  BEGIN UPDATE Countries SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;'
         EXEC (@SQLCountries);
 end


      --Contacts
 IF not EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'TriggerOnCreate_AutomaticLastUpdateDateContacts'))
 Begin
 declare @SQLContact as varchar(8000)
         SET @SQLContact ='CREATE TRIGGER TriggerOnCreate_AutomaticLastUpdateDateContacts ON Contacts AFTER Insert  AS  BEGIN UPDATE Contacts SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;'
         EXEC (@SQLContact);
 end





      --States
 IF not EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'TriggerOnCreate_AutomaticLastUpdateDateStates'))
 Begin
 declare @SQLState as varchar(8000)
         SET @SQLState ='CREATE TRIGGER TriggerOnCreate_AutomaticLastUpdateDateStates ON States AFTER Insert  AS  BEGIN UPDATE States SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;'
         EXEC (@SQLState);
 end


      --Customers
 IF not EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'TriggerOnCreate_AutomaticLastUpdateDateCustomers'))
 Begin
 declare @SQLCustomer as varchar(8000)
         SET @SQLCustomer = 'CREATE TRIGGER TriggerOnCreate_AutomaticLastUpdateDateCustomers ON Customers AFTER Insert  AS  BEGIN UPDATE Customers SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;'
         EXEC (@SQLCustomer);
 end


      --Ranks
 IF not EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'TriggerOnCreate_AutomaticLastUpdateDateRanks'))
 Begin
 declare @SQLRank as varchar(8000)
         SET @SQLRank ='CREATE TRIGGER TriggerOnCreate_AutomaticLastUpdateDateRanks ON Ranks AFTER Insert  AS  BEGIN UPDATE Ranks SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;'
         EXEC (@SQLRank);
 end


       --PartnerTypes
 IF not EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'TriggerOnCreate_AutomaticLastUpdateDatePartnerTypes'))
 Begin
 declare @SQLPartnerType as varchar(8000)
         SET @SQLPartnerType ='CREATE TRIGGER TriggerOnCreate_AutomaticLastUpdateDatePartnerTypes ON PartnerTypes AFTER Insert  AS  BEGIN UPDATE PartnerTypes SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;'
         EXEC (@SQLPartnerType);
 end


       --ShipmentMasterDatas
 IF not EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'TriggerOnCreate_AutomaticLastUpdateDateShipmentMasterDatas'))
 Begin
 declare @SQLShipmentMasterData as varchar(8000)
         SET @SQLShipmentMasterData ='CREATE TRIGGER TriggerOnCreate_AutomaticLastUpdateDateShipmentMasterDatas ON ShipmentMasterDatas AFTER Insert  AS  BEGIN UPDATE ShipmentMasterDatas SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;'
         EXEC (@SQLShipmentMasterData);
 end


        --DWHSettings
 IF not EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'TriggerOnCreate_AutomaticLastUpdateDateDWHSettings'))
 Begin
 declare @SQLDWHSetting as varchar(8000)
         SET @SQLDWHSetting = 'CREATE TRIGGER TriggerOnCreate_AutomaticLastUpdateDateDWHSettings ON DWHSettings AFTER Insert  AS  BEGIN UPDATE DWHSettings SET AutomaticLastUpdateDate = GETDATE() WHERE Tenant IN (SELECT DISTINCT Tenant FROM Inserted)END;'
         EXEC (@SQLDWHSetting);
 end


         --MoveTypes
 IF not EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'TriggerOnCreate_AutomaticLastUpdateDateMoveTypes'))
 Begin
 declare @SQLMoveType as varchar(8000)
         SET @SQLMoveType ='CREATE TRIGGER TriggerOnCreate_AutomaticLastUpdateDateMoveTypes ON MoveTypes AFTER Insert  AS  BEGIN UPDATE MoveTypes SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;'
         EXEC (@SQLMoveType);
 end


  --Vessels
 IF not EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'TriggerOnCreate_AutomaticLastUpdateDateVessels'))
 Begin
 declare @SQLVessel  as varchar(8000)
         SET @SQLVessel ='CREATE TRIGGER TriggerOnCreate_AutomaticLastUpdateDateVessels ON Vessels AFTER Insert  AS  BEGIN UPDATE Vessels SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;'
         EXEC (@SQLVessel);
 end



 

  --SpecialServicesTypes
 IF not EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'TriggerOnCreate_AutomaticLastUpdateDateSpecialServicesTypes'))
 Begin
 declare @SQLSpecialServicesType  as varchar(8000)
         SET @SQLSpecialServicesType ='CREATE TRIGGER TriggerOnCreate_AutomaticLastUpdateDateSpecialServicesTypes ON SpecialServicesTypes AFTER Insert  AS  BEGIN UPDATE SpecialServicesTypes SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;'
         EXEC (@SQLSpecialServicesType);
 end


 
  --ObjectFields
 IF not EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'TriggerOnCreate_AutomaticLastUpdateDateObjectFields'))
 Begin
 declare @SQLObjectFields  as varchar(8000)
         SET @SQLObjectFields ='CREATE TRIGGER TriggerOnCreate_AutomaticLastUpdateDateObjectFields ON ObjectFields AFTER Insert  AS  BEGIN UPDATE ObjectFields SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;'
         EXEC (@SQLObjectFields);
 end


   --CustomPickLists
 IF not EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'TriggerOnCreate_AutomaticLastUpdateDateCustomPickLists'))
 Begin
 declare @SQLCustomPickLists  as varchar(8000)
         SET @SQLCustomPickLists ='CREATE TRIGGER TriggerOnCreate_AutomaticLastUpdateDateCustomPickLists ON CustomPickLists AFTER Insert  AS  BEGIN UPDATE CustomPickLists SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;'
         EXEC (@SQLCustomPickLists);
 end




    --Regions
 IF not EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'TriggerOnCreate_AutomaticLastUpdateDateRegions'))
 Begin
 declare @SQLRegions  as varchar(8000)
         SET @SQLRegions ='CREATE TRIGGER TriggerOnCreate_AutomaticLastUpdateDateRegions ON Regions AFTER Insert  AS  BEGIN UPDATE Regions SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;'
         EXEC (@SQLRegions);
 end



 
    --CustomerSizes
 IF not EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'TriggerOnCreate_AutomaticLastUpdateDateCustomerSizes'))
 Begin
 declare @SQLCustomerSizes  as varchar(8000)
         SET @SQLCustomerSizes ='CREATE TRIGGER TriggerOnCreate_AutomaticLastUpdateDateCustomerSizes ON CustomerSizes AFTER Insert  AS  BEGIN UPDATE CustomerSizes SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;'
         EXEC (@SQLCustomerSizes);
 end


 
    --Industries
 IF not EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'TriggerOnCreate_AutomaticLastUpdateDateIndustries'))
 Begin
 declare @SQLIndustries  as varchar(8000)
         SET @SQLIndustries ='CREATE TRIGGER TriggerOnCreate_AutomaticLastUpdateDateIndustries ON Industries AFTER Insert  AS  BEGIN UPDATE Industries SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;'
         EXEC (@SQLIndustries);
 end

 
 
    --ShipmentComputedFields
 IF not EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'TriggerOnCreate_AutomaticLastUpdateDateShipmentComputedFields'))
 Begin
 declare @SQLShipmentComputedFields  as varchar(8000)
         SET @SQLShipmentComputedFields ='CREATE TRIGGER TriggerOnCreate_AutomaticLastUpdateDateShipmentComputedFields ON ShipmentComputedFields AFTER Insert  AS  BEGIN UPDATE ShipmentComputedFields SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;'
         EXEC (@SQLShipmentComputedFields);
 end


  --____________________________________ Fact Charge_____________________________________
      --ShipmentPayables
 IF not EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'TriggerOnCreate_AutomaticLastUpdateDateShipmentPayables'))
 Begin
 declare @SQLShipmentPayables  as varchar(8000)
         SET @SQLShipmentPayables ='CREATE TRIGGER TriggerOnCreate_AutomaticLastUpdateDateShipmentPayables ON ShipmentPayables AFTER Insert  AS  BEGIN UPDATE ShipmentPayables SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;'
         EXEC (@SQLShipmentPayables);
 end
 --APInvoiceLines
  IF not EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'TriggerOnCreate_AutomaticLastUpdateDateAPInvoiceLines'))
 Begin
 declare @SQLAPInvoiceLines  as varchar(8000)
         SET @SQLAPInvoiceLines ='CREATE TRIGGER TriggerOnCreate_AutomaticLastUpdateDateAPInvoiceLines ON APInvoiceLines AFTER Insert  AS  BEGIN UPDATE APInvoiceLines SET AutomaticLastUpdateDate = GETDATE() WHERE APInvoiceId IN (SELECT DISTINCT APInvoiceId FROM Inserted) and LineNumber IN (SELECT DISTINCT LineNumber FROM Inserted)  END;'
         EXEC (@SQLAPInvoiceLines);
 end

 --APInvoices

  IF not EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'TriggerOnCreate_AutomaticLastUpdateDateAPInvoices'))
 Begin
 declare @SQLAPInvoices  as varchar(8000)
         SET @SQLAPInvoices ='CREATE TRIGGER TriggerOnCreate_AutomaticLastUpdateDateAPInvoices ON APInvoices AFTER Insert  AS  BEGIN UPDATE APInvoices SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;'
         EXEC (@SQLAPInvoices);
 end

 --ShipmentReceivables
 IF not EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'TriggerOnCreate_AutomaticLastUpdateDateShipmentReceivables'))
 Begin
 declare @SQLShipmentReceivables  as varchar(8000)
         SET @SQLShipmentReceivables ='CREATE TRIGGER TriggerOnCreate_AutomaticLastUpdateDateShipmentReceivables ON ShipmentReceivables AFTER Insert  AS  BEGIN UPDATE ShipmentReceivables SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;'
         EXEC (@SQLShipmentReceivables);
 end


  --ARInvoiceLines
  IF not EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'TriggerOnCreate_AutomaticLastUpdateDateARInvoiceLines'))
 Begin
 declare @SQLARInvoiceLines  as varchar(8000)
         SET @SQLARInvoiceLines ='CREATE TRIGGER TriggerOnCreate_AutomaticLastUpdateDateARInvoiceLines ON ARInvoiceLines AFTER Insert  AS  BEGIN UPDATE ARInvoiceLines SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;'
         EXEC (@SQLARInvoiceLines);
 end

 --ARInvoices

  IF not EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'TriggerOnCreate_AutomaticLastUpdateDateARInvoices'))
 Begin
 declare @SQLARInvoices  as varchar(8000)
         SET @SQLARInvoices ='CREATE TRIGGER TriggerOnCreate_AutomaticLastUpdateDateARInvoices ON ARInvoices AFTER Insert  AS  BEGIN UPDATE ARInvoices SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;'
         EXEC (@SQLARInvoices);
 end


  --ChargesTypes

  IF not EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'TriggerOnCreate_AutomaticLastUpdateDateChargesTypes'))
 Begin
 declare @SQLChargesTypes  as varchar(8000)
         SET @SQLChargesTypes ='CREATE TRIGGER TriggerOnCreate_AutomaticLastUpdateDateChargesTypes ON ChargesTypes AFTER Insert  AS  BEGIN UPDATE ChargesTypes SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;'
         EXEC (@SQLChargesTypes);
 end


  --LeadSources
 IF not EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'TriggerOnCreate_AutomaticLastUpdateDateLeadSources'))
 Begin
 declare @SQLLeadSources  as varchar(8000)
         SET @SQLLeadSources ='CREATE TRIGGER TriggerOnCreate_AutomaticLastUpdateDateLeadSources ON LeadSources AFTER Insert  AS  BEGIN UPDATE LeadSources SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;'
         EXEC (@SQLLeadSources);
 end

