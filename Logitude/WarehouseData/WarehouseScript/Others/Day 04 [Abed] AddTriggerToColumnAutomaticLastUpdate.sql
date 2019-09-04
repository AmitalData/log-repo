
--Shipments
IF not EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'Trigger_AutomaticLastUpdateDateShipments'))
 Begin
 declare @SQLShipment as varchar(8000)
         SET @SQLShipment =   'CREATE TRIGGER Trigger_AutomaticLastUpdateDateShipments ON Shipments AFTER UPDATE  AS  BEGIN UPDATE Shipments SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;'
         EXEC (@SQLShipment);
 end

 --Cards
IF not EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'Trigger_AutomaticLastUpdateDateCards'))
 Begin
 declare @SQLCard as varchar(8000)
         SET @SQLCard ='CREATE TRIGGER Trigger_AutomaticLastUpdateDateCards ON Cards AFTER UPDATE  AS  BEGIN UPDATE Cards SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted) END;'
         EXEC (@SQLCard);
 end

--Tenants
 IF not EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'Trigger_AutomaticLastUpdateDateTenants'))
 Begin
 declare @SQLTenant as varchar(8000)
         SET @SQLTenant = 'CREATE TRIGGER Trigger_AutomaticLastUpdateDateTenants ON Tenants AFTER UPDATE  AS  BEGIN UPDATE Tenants SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted) END;'
         EXEC (@SQLTenant);
 end

 --Directions
 IF not EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'Trigger_AutomaticLastUpdateDateDirections'))
 Begin
 declare @SQLDirection as varchar(8000)
         SET @SQLDirection = 'CREATE TRIGGER Trigger_AutomaticLastUpdateDateDirections ON Directions AFTER UPDATE  AS  BEGIN UPDATE Directions SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted) END;'
         EXEC (@SQLDirection);
 end

  --Ports
 IF not EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'Trigger_AutomaticLastUpdateDatePorts'))
 Begin
 declare @SQLPort as varchar(8000)
         SET @SQLPort ='CREATE TRIGGER Trigger_AutomaticLastUpdateDatePorts ON Ports AFTER UPDATE  AS  BEGIN UPDATE Ports SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted) END;'
         EXEC (@SQLPort);
 end


   --TransportMode
 IF not EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'Trigger_AutomaticLastUpdateDateTransportModes'))
 Begin
 declare @SQLTransportMode as varchar(8000)
         SET @SQLTransportMode ='CREATE TRIGGER Trigger_AutomaticLastUpdateDateTransportModes ON TransportModes AFTER UPDATE  AS  BEGIN UPDATE TransportModes SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)  END;'
         EXEC (@SQLTransportMode);
 end


 --ShipmentLevels
 IF not EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'Trigger_AutomaticLastUpdateDateShipmentLevels'))
 Begin

 declare @SQLShipmentLevel as varchar(8000)
         SET @SQLShipmentLevel = 'CREATE TRIGGER Trigger_AutomaticLastUpdateDateShipmentLevels ON ShipmentLevels AFTER UPDATE  AS  BEGIN UPDATE ShipmentLevels SET AutomaticLastUpdateDate = GETDATE() WHERE Code IN (SELECT DISTINCT Code FROM Inserted) END;'
         EXEC (@SQLShipmentLevel);
 end


  --ShipmentTypes
 IF not EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'Trigger_AutomaticLastUpdateDateShipmentTypes'))
 Begin
 declare @SQLShipmentType as varchar(8000)
         SET @SQLShipmentType ='CREATE TRIGGER Trigger_AutomaticLastUpdateDateShipmentTypes ON ShipmentTypes AFTER UPDATE  AS  BEGIN UPDATE ShipmentTypes SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted) END;'
         EXEC (@SQLShipmentType);
 end



   --Departments
 IF not EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'Trigger_AutomaticLastUpdateDateDepartments'))
 Begin
 declare @SQLDepartment as varchar(8000)
         SET @SQLDepartment ='CREATE TRIGGER Trigger_AutomaticLastUpdateDateDepartments ON Departments AFTER UPDATE  AS  BEGIN UPDATE Departments SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)  END;'
         EXEC (@SQLDepartment);
 end



    --Branches
 IF not EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'Trigger_AutomaticLastUpdateDateBranches'))
 Begin
 declare @SQLBranches as varchar(8000)
         SET @SQLBranches ='CREATE TRIGGER Trigger_AutomaticLastUpdateDateBranches ON Branches AFTER UPDATE  AS  BEGIN UPDATE Branches SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted) END;'
         EXEC (@SQLBranches);
 end

     --Incoterms
 IF not EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'Trigger_AutomaticLastUpdateDateIncoterms'))
 Begin
 declare @SQLIncoterm as varchar(8000)
         SET @SQLIncoterm = 'CREATE TRIGGER Trigger_AutomaticLastUpdateDateIncoterms ON Incoterms AFTER UPDATE  AS  BEGIN UPDATE Incoterms SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted) END;'
         EXEC (@SQLIncoterm);
 end



      --Users
 IF not EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'Trigger_AutomaticLastUpdateDateUsers'))
 Begin
 declare @SQLUser  as varchar(8000)
         SET @SQLUser ='CREATE TRIGGER Trigger_AutomaticLastUpdateDateUsers ON Users AFTER UPDATE  AS  BEGIN UPDATE Users SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;'
         EXEC (@SQLUser);
 end


   --Currencies
 IF not EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'Trigger_AutomaticLastUpdateDateCurrencies'))
 Begin
 declare @SQLCurrencies as varchar(8000)
         SET @SQLCurrencies ='CREATE TRIGGER Trigger_AutomaticLastUpdateDateCurrencies ON Currencies AFTER UPDATE  AS  BEGIN UPDATE Currencies SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted) END;'
         EXEC (@SQLCurrencies);
 end


      --EntityStatus
 IF not EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'Trigger_AutomaticLastUpdateDateEntityStatus'))
 Begin
 declare @SQLEntityStatus as varchar(8000)
         SET @SQLEntityStatus ='CREATE TRIGGER Trigger_AutomaticLastUpdateDateEntityStatus ON EntityStatus AFTER UPDATE  AS  BEGIN UPDATE EntityStatus SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted) END;'
         EXEC (@SQLEntityStatus);

 end



      --Addresses
 IF not EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'Trigger_AutomaticLastUpdateDateAddresses'))
 Begin
 declare @SQLAddresses as varchar(8000)
         SET @SQLAddresses = 'CREATE TRIGGER Trigger_AutomaticLastUpdateDateAddresses ON Addresses AFTER UPDATE  AS  BEGIN UPDATE Addresses SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)  END;'
         EXEC (@SQLAddresses);
 end


     --Countries
 IF not EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'Trigger_AutomaticLastUpdateDateCountries'))
 Begin
 declare @SQLCountries as varchar(8000)
         SET @SQLCountries ='CREATE TRIGGER Trigger_AutomaticLastUpdateDateCountries ON Countries AFTER UPDATE  AS  BEGIN UPDATE Countries SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted) END;'
         EXEC (@SQLCountries);
 end


      --Contacts
 IF not EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'Trigger_AutomaticLastUpdateDateContacts'))
 Begin
 declare @SQLContact as varchar(8000)
         SET @SQLContact ='CREATE TRIGGER Trigger_AutomaticLastUpdateDateContacts ON Contacts AFTER UPDATE  AS  BEGIN UPDATE Contacts SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted) END;'
         EXEC (@SQLContact);
 end





      --States
 IF not EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'Trigger_AutomaticLastUpdateDateStates'))
 Begin
 declare @SQLState as varchar(8000)
         SET @SQLState ='CREATE TRIGGER Trigger_AutomaticLastUpdateDateStates ON States AFTER UPDATE  AS  BEGIN UPDATE States SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted) END;'
         EXEC (@SQLState);
 end


      --Customers
 IF not EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'Trigger_AutomaticLastUpdateDateCustomers'))
 Begin
 declare @SQLCustomer as varchar(8000)
         SET @SQLCustomer = 'CREATE TRIGGER Trigger_AutomaticLastUpdateDateCustomers ON Customers AFTER UPDATE  AS  BEGIN UPDATE Customers SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted) END;'
         EXEC (@SQLCustomer);
 end


      --Ranks
 IF not EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'Trigger_AutomaticLastUpdateDateRanks'))
 Begin
 declare @SQLRank as varchar(8000)
         SET @SQLRank ='CREATE TRIGGER Trigger_AutomaticLastUpdateDateRanks ON Ranks AFTER UPDATE  AS  BEGIN UPDATE Ranks SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)  END;'
         EXEC (@SQLRank);
 end


       --PartnerTypes
 IF not EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'Trigger_AutomaticLastUpdateDatePartnerTypes'))
 Begin
 declare @SQLPartnerType as varchar(8000)
         SET @SQLPartnerType ='CREATE TRIGGER Trigger_AutomaticLastUpdateDatePartnerTypes ON PartnerTypes AFTER UPDATE  AS  BEGIN UPDATE PartnerTypes SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)  END;'
         EXEC (@SQLPartnerType);
 end


       --ShipmentMasterDatas
 IF not EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'Trigger_AutomaticLastUpdateDateShipmentMasterDatas'))
 Begin
 declare @SQLShipmentMasterData as varchar(8000)
         SET @SQLShipmentMasterData ='CREATE TRIGGER Trigger_AutomaticLastUpdateDateShipmentMasterDatas ON ShipmentMasterDatas AFTER UPDATE  AS  BEGIN UPDATE ShipmentMasterDatas SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)  END;'
         EXEC (@SQLShipmentMasterData);
 end


        --DWHSettings
 IF not EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'Trigger_AutomaticLastUpdateDateDWHSettings'))
 Begin
 declare @SQLDWHSetting as varchar(8000)
         SET @SQLDWHSetting = 'CREATE TRIGGER Trigger_AutomaticLastUpdateDateDWHSettings ON DWHSettings AFTER UPDATE  AS  BEGIN UPDATE DWHSettings SET AutomaticLastUpdateDate = GETDATE() WHERE Tenant IN (SELECT DISTINCT Tenant FROM Inserted)END;'
         EXEC (@SQLDWHSetting);
 end


         --MoveTypes
 IF not EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'Trigger_AutomaticLastUpdateDateMoveTypes'))
 Begin
 declare @SQLMoveType as varchar(8000)
         SET @SQLMoveType ='CREATE TRIGGER Trigger_AutomaticLastUpdateDateMoveTypes ON MoveTypes AFTER UPDATE  AS  BEGIN UPDATE MoveTypes SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;'
         EXEC (@SQLMoveType);
 end


  --Vessels
 IF not EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'Trigger_AutomaticLastUpdateDateVessels'))
 Begin
 declare @SQLVessel  as varchar(8000)
         SET @SQLVessel ='CREATE TRIGGER Trigger_AutomaticLastUpdateDateVessels ON Vessels AFTER UPDATE  AS  BEGIN UPDATE Vessels SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;'
         EXEC (@SQLVessel);
 end



 

  --SpecialServicesTypes
 IF not EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'Trigger_AutomaticLastUpdateDateSpecialServicesTypes'))
 Begin
 declare @SQLSpecialServicesType  as varchar(8000)
         SET @SQLSpecialServicesType ='CREATE TRIGGER Trigger_AutomaticLastUpdateDateSpecialServicesTypes ON SpecialServicesTypes AFTER UPDATE  AS  BEGIN UPDATE SpecialServicesTypes SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;'
         EXEC (@SQLSpecialServicesType);
 end


 
  --ObjectFields
 IF not EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'Trigger_AutomaticLastUpdateDateObjectFields'))
 Begin
 declare @SQLObjectFields  as varchar(8000)
         SET @SQLObjectFields ='CREATE TRIGGER Trigger_AutomaticLastUpdateDateObjectFields ON ObjectFields AFTER UPDATE  AS  BEGIN UPDATE ObjectFields SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;'
         EXEC (@SQLObjectFields);
 end


   --CustomPickLists
 IF not EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'Trigger_AutomaticLastUpdateDateCustomPickLists'))
 Begin
 declare @SQLCustomPickLists  as varchar(8000)
         SET @SQLCustomPickLists ='CREATE TRIGGER Trigger_AutomaticLastUpdateDateCustomPickLists ON CustomPickLists AFTER UPDATE  AS  BEGIN UPDATE CustomPickLists SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;'
         EXEC (@SQLCustomPickLists);
 end




    --Regions
 IF not EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'Trigger_AutomaticLastUpdateDateRegions'))
 Begin
 declare @SQLRegions  as varchar(8000)
         SET @SQLRegions ='CREATE TRIGGER Trigger_AutomaticLastUpdateDateRegions ON Regions AFTER UPDATE  AS  BEGIN UPDATE Regions SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;'
         EXEC (@SQLRegions);
 end



 
    --CustomerSizes
 IF not EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'Trigger_AutomaticLastUpdateDateCustomerSizes'))
 Begin
 declare @SQLCustomerSizes  as varchar(8000)
         SET @SQLCustomerSizes ='CREATE TRIGGER Trigger_AutomaticLastUpdateDateCustomerSizes ON CustomerSizes AFTER UPDATE  AS  BEGIN UPDATE CustomerSizes SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;'
         EXEC (@SQLCustomerSizes);
 end


 
    --Industries
 IF not EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'Trigger_AutomaticLastUpdateDateIndustries'))
 Begin
 declare @SQLIndustries  as varchar(8000)
         SET @SQLIndustries ='CREATE TRIGGER Trigger_AutomaticLastUpdateDateIndustries ON Industries AFTER UPDATE  AS  BEGIN UPDATE Industries SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;'
         EXEC (@SQLIndustries);
 end

 
 
    --ShipmentComputedFields
 IF not EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'Trigger_AutomaticLastUpdateDateShipmentComputedFields'))
 Begin
 declare @SQLShipmentComputedFields  as varchar(8000)
         SET @SQLShipmentComputedFields ='CREATE TRIGGER Trigger_AutomaticLastUpdateDateShipmentComputedFields ON ShipmentComputedFields AFTER UPDATE  AS  BEGIN UPDATE ShipmentComputedFields SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;'
         EXEC (@SQLShipmentComputedFields);
 end

