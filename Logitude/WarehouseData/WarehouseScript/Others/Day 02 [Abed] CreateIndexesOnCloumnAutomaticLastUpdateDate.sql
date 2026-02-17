
IF not EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Shipments_AutomaticLastUpdateDate' 
    AND object_id = OBJECT_ID('[dbo].[Shipments]'))
  begin
    CREATE NONCLUSTERED INDEX [IX_Shipments_AutomaticLastUpdateDate]
ON [dbo].[Shipments]([AutomaticLastUpdateDate])
  end

IF not EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Cards_AutomaticLastUpdateDate' 
    AND object_id = OBJECT_ID('[dbo].[Cards]'))
  begin
    CREATE NONCLUSTERED INDEX [IX_Cards_AutomaticLastUpdateDate]
ON [dbo].[Cards]([AutomaticLastUpdateDate])
  end

IF not EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Directions_AutomaticLastUpdateDate' 
    AND object_id = OBJECT_ID('[dbo].[Directions]'))
  begin
    CREATE NONCLUSTERED INDEX [IX_Directions_AutomaticLastUpdateDate]
ON [dbo].[Directions]([AutomaticLastUpdateDate])
  end

IF not EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Ports_AutomaticLastUpdateDate' 
    AND object_id = OBJECT_ID('[dbo].[Ports]'))
  begin
    CREATE NONCLUSTERED INDEX [IX_Ports_AutomaticLastUpdateDate]
ON [dbo].[Ports]([AutomaticLastUpdateDate])
  end

IF not EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_TransportModes_AutomaticLastUpdateDate' 
    AND object_id = OBJECT_ID('[dbo].[TransportModes]'))
  begin
    CREATE NONCLUSTERED INDEX [IX_TransportModes_AutomaticLastUpdateDate]
ON [dbo].[TransportModes]([AutomaticLastUpdateDate])
  end

IF not EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_ShipmentLevels_AutomaticLastUpdateDate' 
    AND object_id = OBJECT_ID('[dbo].[ShipmentLevels]'))
  begin
    CREATE NONCLUSTERED INDEX [IX_ShipmentLevels_AutomaticLastUpdateDate]
ON [dbo].[ShipmentLevels]([AutomaticLastUpdateDate])
  end

IF not EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_ShipmentTypes_AutomaticLastUpdateDate' 
    AND object_id = OBJECT_ID('[dbo].[ShipmentTypes]'))
  begin
    CREATE NONCLUSTERED INDEX [IX_ShipmentTypes_AutomaticLastUpdateDate]
ON [dbo].[ShipmentTypes]([AutomaticLastUpdateDate])
  end

IF not EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Departments_AutomaticLastUpdateDate' 
    AND object_id = OBJECT_ID('[dbo].[Departments]'))
  begin
    CREATE NONCLUSTERED INDEX [IX_Departments_AutomaticLastUpdateDate]
ON [dbo].[Departments]([AutomaticLastUpdateDate])
  end

IF not EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Incoterms_AutomaticLastUpdateDate' 
    AND object_id = OBJECT_ID('[dbo].[Incoterms]'))
  begin
    CREATE NONCLUSTERED INDEX [IX_Incoterms_AutomaticLastUpdateDate]
ON [dbo].[Incoterms]([AutomaticLastUpdateDate])
  end

IF not EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Users_AutomaticLastUpdateDate' 
    AND object_id = OBJECT_ID('[dbo].[Users]'))
  begin
    CREATE NONCLUSTERED INDEX [IX_Users_AutomaticLastUpdateDate]
ON [dbo].[Users]([AutomaticLastUpdateDate])
  end

IF not EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Currencies_AutomaticLastUpdateDate' 
    AND object_id = OBJECT_ID('[dbo].[Currencies]'))
  begin
    CREATE NONCLUSTERED INDEX [IX_Currencies_AutomaticLastUpdateDate]
ON [dbo].[Currencies]([AutomaticLastUpdateDate])
  end

IF not EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_EntityStatus_AutomaticLastUpdateDate' 
    AND object_id = OBJECT_ID('[dbo].[EntityStatus]'))
  begin
    CREATE NONCLUSTERED INDEX [IX_EntityStatus_AutomaticLastUpdateDate]
ON [dbo].[EntityStatus]([AutomaticLastUpdateDate])
  end

IF not EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Addresses_AutomaticLastUpdateDate' 
    AND object_id = OBJECT_ID('[dbo].[Addresses]'))
  begin
    CREATE NONCLUSTERED INDEX [IX_Addresses_AutomaticLastUpdateDate]
ON [dbo].[Addresses]([AutomaticLastUpdateDate])
  end

IF not EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Countries_AutomaticLastUpdateDate' 
    AND object_id = OBJECT_ID('[dbo].[Countries]'))
  begin
    CREATE NONCLUSTERED INDEX [IX_Countries_AutomaticLastUpdateDate]
ON [dbo].[Countries]([AutomaticLastUpdateDate])
  end

IF not EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Contacts_AutomaticLastUpdateDate' 
    AND object_id = OBJECT_ID('[dbo].[Contacts]'))
  begin
    CREATE NONCLUSTERED INDEX [IX_Contacts_AutomaticLastUpdateDate]
ON [dbo].[Contacts]([AutomaticLastUpdateDate])
  end

IF not EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_States_AutomaticLastUpdateDate' 
    AND object_id = OBJECT_ID('[dbo].[States]'))
  begin
    CREATE NONCLUSTERED INDEX [IX_States_AutomaticLastUpdateDate]
ON [dbo].[States]([AutomaticLastUpdateDate])
  end

IF not EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Customers_AutomaticLastUpdateDate' 
    AND object_id = OBJECT_ID('[dbo].[Customers]'))
  begin
    CREATE NONCLUSTERED INDEX [IX_Customers_AutomaticLastUpdateDate]
ON [dbo].[Customers]([AutomaticLastUpdateDate])
  end

IF not EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_PartnerTypes_AutomaticLastUpdateDate' 
    AND object_id = OBJECT_ID('[dbo].[PartnerTypes]'))
  begin
    CREATE NONCLUSTERED INDEX [IX_PartnerTypes_AutomaticLastUpdateDate]
ON [dbo].[PartnerTypes]([AutomaticLastUpdateDate])
  end

IF not EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_ShipmentMasterDatas_AutomaticLastUpdateDate' 
    AND object_id = OBJECT_ID('[dbo].[ShipmentMasterDatas]'))
  begin
    CREATE NONCLUSTERED INDEX [IX_ShipmentMasterDatas_AutomaticLastUpdateDate]
ON [dbo].[ShipmentMasterDatas]([AutomaticLastUpdateDate])
  end

IF not EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Tenants_AutomaticLastUpdateDate' 
    AND object_id = OBJECT_ID('[dbo].[Tenants]'))
  begin
    CREATE NONCLUSTERED INDEX [IX_Tenants_AutomaticLastUpdateDate]
ON [dbo].[Tenants]([AutomaticLastUpdateDate])
  end

IF not EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Branches_AutomaticLastUpdateDate' 
    AND object_id = OBJECT_ID('[dbo].[Branches]'))
  begin
    CREATE NONCLUSTERED INDEX [IX_Branches_AutomaticLastUpdateDate]
ON [dbo].[Branches]([AutomaticLastUpdateDate])
  end

IF not EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Ranks_AutomaticLastUpdateDate' 
    AND object_id = OBJECT_ID('[dbo].[Ranks]'))
  begin
    CREATE NONCLUSTERED INDEX [IX_Ranks_AutomaticLastUpdateDate]
ON [dbo].[Ranks]([AutomaticLastUpdateDate])
  end   

  
IF not EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_DWHSettings_AutomaticLastUpdateDate' 
    AND object_id = OBJECT_ID('[dbo].[DWHSettings]'))
  begin
    CREATE NONCLUSTERED INDEX [IX_DWHSettings_AutomaticLastUpdateDate]
ON [dbo].[DWHSettings]([AutomaticLastUpdateDate])
  end

  IF not EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_MoveTypes_AutomaticLastUpdateDate' 
    AND object_id = OBJECT_ID('[dbo].[MoveTypes]'))
  begin
    CREATE NONCLUSTERED INDEX [IX_MoveTypes_AutomaticLastUpdateDate]
ON [dbo].[MoveTypes]([AutomaticLastUpdateDate])
  end  

  IF not EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Vessels_AutomaticLastUpdateDate' 
    AND object_id = OBJECT_ID('[dbo].[Vessels]'))
  begin
    CREATE NONCLUSTERED INDEX [IX_Vessels_AutomaticLastUpdateDate]
ON [dbo].[Vessels]([AutomaticLastUpdateDate])
  end  


    IF not EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_SpecialServicesTypes_AutomaticLastUpdateDate' 
    AND object_id = OBJECT_ID('[dbo].[SpecialServicesTypes]'))
  begin
    CREATE NONCLUSTERED INDEX [IX_SpecialServicesTypes_AutomaticLastUpdateDate]
ON [dbo].[SpecialServicesTypes]([AutomaticLastUpdateDate])
  end  
  

      IF not EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_ObjectFields_AutomaticLastUpdateDate' 
    AND object_id = OBJECT_ID('[dbo].[ObjectFields]'))
  begin
    CREATE NONCLUSTERED INDEX [IX_ObjectFields_AutomaticLastUpdateDate]
ON [dbo].[ObjectFields]([AutomaticLastUpdateDate])
  end  
  

  
      IF not EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_CustomPickLists_AutomaticLastUpdateDate' 
    AND object_id = OBJECT_ID('[dbo].[CustomPickLists]'))
  begin
    CREATE NONCLUSTERED INDEX [IX_CustomPickLists_AutomaticLastUpdateDate]
ON [dbo].[CustomPickLists]([AutomaticLastUpdateDate])
  end  
  

  		  
  		  IF not EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Regions_AutomaticLastUpdateDate' 
    AND object_id = OBJECT_ID('[dbo].[Regions]'))
  begin
    CREATE NONCLUSTERED INDEX [IX_Regions_AutomaticLastUpdateDate]
ON [dbo].[Regions]([AutomaticLastUpdateDate])
  end   



		  IF not EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_CustomerSizes_AutomaticLastUpdateDate' 
    AND object_id = OBJECT_ID('[dbo].[CustomerSizes]'))
  begin
    CREATE NONCLUSTERED INDEX [IX_CustomerSizes_AutomaticLastUpdateDate]
ON [dbo].[CustomerSizes]([AutomaticLastUpdateDate])
  end   



  		  IF not EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_Industries_AutomaticLastUpdateDate' 
    AND object_id = OBJECT_ID('[dbo].[Industries]'))
  begin
    CREATE NONCLUSTERED INDEX [IX_Industries_AutomaticLastUpdateDate]
ON [dbo].[Industries]([AutomaticLastUpdateDate])
  end   


  
		  IF not EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_ShipmentComputedFields_AutomaticLastUpdateDate' 
    AND object_id = OBJECT_ID('[dbo].[ShipmentComputedFields]'))
  begin
    CREATE NONCLUSTERED INDEX [IX_ShipmentComputedFields_AutomaticLastUpdateDate]
ON [dbo].[ShipmentComputedFields]([AutomaticLastUpdateDate])
  end   
