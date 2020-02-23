
--Shipments
IF not EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'AutomaticLastUpdateDate'
          AND Object_ID = Object_ID(N'Shipments'))
		  Begin  ALTER TABLE Shipments ADD  AutomaticLastUpdateDate datetime NULL DEFAULT GETDATE(); 	  End


--Cards
IF not EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'AutomaticLastUpdateDate'
          AND Object_ID = Object_ID(N'Cards'))
		  Begin  ALTER TABLE Cards ADD  AutomaticLastUpdateDate datetime NULL DEFAULT GETDATE(); end

--Tenants
IF not EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'AutomaticLastUpdateDate'
          AND Object_ID = Object_ID(N'Tenants'))
		  Begin
		   ALTER TABLE Tenants ADD  AutomaticLastUpdateDate datetime NULL DEFAULT GETDATE();
		 END

--Directions
IF not EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'AutomaticLastUpdateDate'
          AND Object_ID = Object_ID(N'Directions'))
		  Begin
		   ALTER TABLE Directions ADD  AutomaticLastUpdateDate datetime NULL DEFAULT GETDATE();
		  END

--Ports
IF not EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'AutomaticLastUpdateDate'
          AND Object_ID = Object_ID(N'Ports'))
		  Begin
		   ALTER TABLE Ports ADD  AutomaticLastUpdateDate datetime NULL DEFAULT GETDATE();
		  
		  End

--TransportModes
IF not EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'AutomaticLastUpdateDate'
          AND Object_ID = Object_ID(N'TransportModes'))
		  Begin
		   ALTER TABLE TransportModes ADD  AutomaticLastUpdateDate datetime NULL DEFAULT GETDATE();
		  END
		 

--ShipmentLevels
IF not EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'AutomaticLastUpdateDate'
          AND Object_ID = Object_ID(N'ShipmentLevels'))
		  Begin
		   ALTER TABLE ShipmentLevels ADD  AutomaticLastUpdateDate datetime NULL DEFAULT GETDATE();
		
		  End


--ShipmentTypes
IF not EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'AutomaticLastUpdateDate'
          AND Object_ID = Object_ID(N'ShipmentTypes'))
		  Begin
		  ALTER TABLE ShipmentTypes ADD  AutomaticLastUpdateDate datetime NULL DEFAULT GETDATE();
	
		  End


 --Departments
IF not EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'AutomaticLastUpdateDate'
          AND Object_ID = Object_ID(N'Departments'))
		  Begin
		  ALTER TABLE Departments ADD  AutomaticLastUpdateDate datetime NULL DEFAULT GETDATE();
	
		  End



--Branches
IF not EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'AutomaticLastUpdateDate'
          AND Object_ID = Object_ID(N'Branches'))
		  Begin
		   ALTER TABLE Branches ADD  AutomaticLastUpdateDate datetime NULL DEFAULT GETDATE();
		 
		  End

 --Incoterms
IF not EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'AutomaticLastUpdateDate'
          AND Object_ID = Object_ID(N'Incoterms'))
		  Begin
		  ALTER TABLE Incoterms ADD  AutomaticLastUpdateDate datetime NULL DEFAULT GETDATE();
		
		  End

 --Users
IF not EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'AutomaticLastUpdateDate'
          AND Object_ID = Object_ID(N'Users'))
		  Begin
		   ALTER TABLE Users ADD  AutomaticLastUpdateDate datetime NULL DEFAULT GETDATE();
		
		  End

 --Currencies
IF not EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'AutomaticLastUpdateDate'
          AND Object_ID = Object_ID(N'Currencies'))
		  Begin
		   ALTER TABLE Currencies ADD  AutomaticLastUpdateDate datetime NULL DEFAULT GETDATE();

		  End

--EntityStatus
IF not EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'AutomaticLastUpdateDate'
          AND Object_ID = Object_ID(N'EntityStatus'))
		  Begin
		    ALTER TABLE EntityStatus ADD  AutomaticLastUpdateDate datetime NULL DEFAULT GETDATE();
		 
		  End

 --Addresses
IF not EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'AutomaticLastUpdateDate'
          AND Object_ID = Object_ID(N'Addresses'))
		  Begin
		   ALTER TABLE Addresses ADD  AutomaticLastUpdateDate datetime NULL DEFAULT GETDATE();
		 

		  End


--Countries
IF not EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'AutomaticLastUpdateDate'
          AND Object_ID = Object_ID(N'Countries'))
		  Begin
		   ALTER TABLE Countries ADD  AutomaticLastUpdateDate datetime NULL DEFAULT GETDATE();
		
		  End

 --Contacts
IF not EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'AutomaticLastUpdateDate'
          AND Object_ID = Object_ID(N'Contacts'))
		  Begin
		   ALTER TABLE Contacts ADD  AutomaticLastUpdateDate datetime NULL DEFAULT GETDATE();
	
		  End


		   --States
IF not EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'AutomaticLastUpdateDate'
          AND Object_ID = Object_ID(N'States'))
		  Begin
		  ALTER TABLE States ADD  AutomaticLastUpdateDate datetime NULL DEFAULT GETDATE();
		
		  End



		   --Customers
IF not EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'AutomaticLastUpdateDate'
          AND Object_ID = Object_ID(N'Customers'))
		  Begin
		   ALTER TABLE Customers ADD  AutomaticLastUpdateDate datetime NULL DEFAULT GETDATE();
		
		  End

		   --Ranks
IF not EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'AutomaticLastUpdateDate'
          AND Object_ID = Object_ID(N'Ranks'))
		  Begin
		   ALTER TABLE Ranks ADD  AutomaticLastUpdateDate datetime NULL DEFAULT GETDATE(); 
		  End


		   --PartnerTypes
IF not EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'AutomaticLastUpdateDate'
          AND Object_ID = Object_ID(N'PartnerTypes'))
		  Begin
		  ALTER TABLE PartnerTypes ADD  AutomaticLastUpdateDate datetime NULL DEFAULT GETDATE();
		
		  End


		   --ShipmentMasterDatas
IF not EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'AutomaticLastUpdateDate'
          AND Object_ID = Object_ID(N'ShipmentMasterDatas'))
		  Begin
		   ALTER TABLE ShipmentMasterDatas ADD  AutomaticLastUpdateDate datetime NULL DEFAULT GETDATE();
		
		  End


		  
 --DWHSettings
IF not EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'AutomaticLastUpdateDate'
          AND Object_ID = Object_ID(N'DWHSettings'))
		  Begin
		   ALTER TABLE DWHSettings ADD  AutomaticLastUpdateDate datetime NULL DEFAULT GETDATE();
		
		  End


--MoveTypes
		  IF not EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'AutomaticLastUpdateDate'
          AND Object_ID = Object_ID(N'MoveTypes'))
		  Begin
		   ALTER TABLE MoveTypes ADD  AutomaticLastUpdateDate datetime NULL DEFAULT GETDATE();
		  
		  End



--Vessels		  
  IF not EXISTS(SELECT 1 FROM sys.columns 
 WHERE Name = N'AutomaticLastUpdateDate'
          AND Object_ID = Object_ID(N'Vessels'))
		  Begin
		   ALTER TABLE Vessels ADD  AutomaticLastUpdateDate datetime NULL DEFAULT GETDATE();
		
		  End


	
--SpecialServicesTypes		  
  IF not EXISTS(SELECT 1 FROM sys.columns 
 WHERE Name = N'AutomaticLastUpdateDate'
          AND Object_ID = Object_ID(N'SpecialServicesTypes'))
		  Begin
		   ALTER TABLE SpecialServicesTypes ADD  AutomaticLastUpdateDate datetime NULL DEFAULT GETDATE();
		
		  End

--ObjectFields
		    IF not EXISTS(SELECT 1 FROM sys.columns 
 WHERE Name = N'AutomaticLastUpdateDate'
          AND Object_ID = Object_ID(N'ObjectFields'))
		  Begin
		   ALTER TABLE ObjectFields ADD  AutomaticLastUpdateDate datetime NULL DEFAULT GETDATE();
		
		  End

		  
--CustomPickLists
		    IF not EXISTS(SELECT 1 FROM sys.columns 
 WHERE Name = N'AutomaticLastUpdateDate'
          AND Object_ID = Object_ID(N'CustomPickLists'))
		  Begin
		   ALTER TABLE CustomPickLists ADD  AutomaticLastUpdateDate datetime NULL DEFAULT GETDATE();
		
		  End



		  		   --Regions
IF not EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'AutomaticLastUpdateDate'
          AND Object_ID = Object_ID(N'Regions'))
		  Begin
		   ALTER TABLE Regions ADD  AutomaticLastUpdateDate datetime NULL DEFAULT GETDATE(); 
		  End




		  		   --CustomerSizes
IF not EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'AutomaticLastUpdateDate'
          AND Object_ID = Object_ID(N'CustomerSizes'))
		  Begin
		   ALTER TABLE CustomerSizes ADD  AutomaticLastUpdateDate datetime NULL DEFAULT GETDATE(); 
		  End



		  		   --Industries
IF not EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'AutomaticLastUpdateDate'
          AND Object_ID = Object_ID(N'Industries'))
		  Begin
		   ALTER TABLE Industries ADD  AutomaticLastUpdateDate datetime NULL DEFAULT GETDATE(); 
		  End



		  		   --ShipmentComputingPartners
IF not EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'AutomaticLastUpdateDate'
          AND Object_ID = Object_ID(N'ShipmentComputedFields'))
		  Begin
		   ALTER TABLE ShipmentComputedFields ADD  AutomaticLastUpdateDate datetime NULL DEFAULT GETDATE(); 
		  End




 --____________________________________ Fact Charge_____________________________________


 		  		   --ShipmentPayables
IF not EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'AutomaticLastUpdateDate'
          AND Object_ID = Object_ID(N'ShipmentPayables'))
		  Begin
		   ALTER TABLE ShipmentPayables ADD  AutomaticLastUpdateDate datetime NULL DEFAULT GETDATE(); 
		  End



		   		  		   --APInvoiceLines
IF not EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'AutomaticLastUpdateDate'
          AND Object_ID = Object_ID(N'APInvoiceLines'))
		  Begin
		   ALTER TABLE APInvoiceLines ADD  AutomaticLastUpdateDate datetime NULL DEFAULT GETDATE(); 
		  End




		   		  		   --APInvoices
IF not EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'AutomaticLastUpdateDate'
          AND Object_ID = Object_ID(N'APInvoices'))
		  Begin
		   ALTER TABLE APInvoices ADD  AutomaticLastUpdateDate datetime NULL DEFAULT GETDATE(); 
		  End



		   		  		   --ShipmentReceivables
IF not EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'AutomaticLastUpdateDate'
          AND Object_ID = Object_ID(N'ShipmentReceivables'))
		  Begin
		   ALTER TABLE ShipmentReceivables ADD  AutomaticLastUpdateDate datetime NULL DEFAULT GETDATE(); 
		  End



		   		  		   --ARInvoiceLines
IF not EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'AutomaticLastUpdateDate'
          AND Object_ID = Object_ID(N'ARInvoiceLines'))
		  Begin
		   ALTER TABLE ARInvoiceLines ADD  AutomaticLastUpdateDate datetime NULL DEFAULT GETDATE(); 
		  End




		   		  		   --ARInvoices
IF not EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'AutomaticLastUpdateDate'
          AND Object_ID = Object_ID(N'ARInvoices'))
		  Begin
		   ALTER TABLE ARInvoices ADD  AutomaticLastUpdateDate datetime NULL DEFAULT GETDATE(); 
		  End



		   		  		   --ChargesTypes
IF not EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'AutomaticLastUpdateDate'
          AND Object_ID = Object_ID(N'ChargesTypes'))
		  Begin
		   ALTER TABLE ChargesTypes ADD  AutomaticLastUpdateDate datetime NULL DEFAULT GETDATE(); 
		  End


		  		   		  		   --ChargesGroups
--IF not EXISTS(SELECT 1 FROM sys.columns 
--          WHERE Name = N'AutomaticLastUpdateDate'
--          AND Object_ID = Object_ID(N'ChargesGroups'))
--		  Begin
--		   ALTER TABLE ChargesGroups ADD  AutomaticLastUpdateDate datetime NULL DEFAULT GETDATE(); 
--		  End

		  