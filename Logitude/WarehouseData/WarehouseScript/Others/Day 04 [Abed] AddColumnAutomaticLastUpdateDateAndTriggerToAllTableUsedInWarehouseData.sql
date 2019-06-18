
--Shipments
IF not EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'AutomaticLastUpdateDate'
          AND Object_ID = Object_ID(N'Shipments'))
		  Begin
		  		  ALTER TABLE Shipments ADD  AutomaticLastUpdateDate datetime NULL DEFAULT GETDATE();
		  declare @SQLShipment as varchar(8000)

BEGIN TRY
    BEGIN TRANSACTION

    BEGIN
            ;

        SET @SQLShipment = 
            'CREATE TRIGGER Trigger_AutomaticLastUpdateDateShipments ON Shipments AFTER UPDATE  AS  BEGIN UPDATE Shipments SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;'

        EXEC (@SQLShipment);

       
    END

    COMMIT TRANSACTION;
END TRY

BEGIN CATCH
    ROLLBACK TRANSACTION
END CATCH;


		  End

--Cards
IF not EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'AutomaticLastUpdateDate'
          AND Object_ID = Object_ID(N'Cards'))
		  Begin
		  ALTER TABLE Cards ADD  AutomaticLastUpdateDate datetime NULL DEFAULT GETDATE();
		  declare @SQLCard as varchar(8000)
BEGIN TRY
    BEGIN TRANSACTION

    BEGIN
            ;

        SET @SQLCard = 
            'CREATE TRIGGER Trigger_AutomaticLastUpdateDateCards ON Cards AFTER UPDATE  AS  BEGIN UPDATE Cards SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted) END;'

        EXEC (@SQLCard);

       
    END

    COMMIT TRANSACTION;
END TRY

BEGIN CATCH
    ROLLBACK TRANSACTION
END CATCH;


		  End
--Tenants
IF not EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'AutomaticLastUpdateDate'
          AND Object_ID = Object_ID(N'Tenants'))
		  Begin
		   ALTER TABLE Tenants ADD  AutomaticLastUpdateDate datetime NULL DEFAULT GETDATE();
		  declare @SQLTenant as varchar(8000)
		 
BEGIN TRY
    BEGIN TRANSACTION

    BEGIN
            ;

        SET @SQLTenant = 
            'CREATE TRIGGER Trigger_AutomaticLastUpdateDateTenants ON Tenants AFTER UPDATE  AS  BEGIN UPDATE Tenants SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted) END;'

        EXEC (@SQLTenant);

       
    END

    COMMIT TRANSACTION;
END TRY

BEGIN CATCH
    ROLLBACK TRANSACTION
END CATCH;


		  End


--Directions
IF not EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'AutomaticLastUpdateDate'
          AND Object_ID = Object_ID(N'Directions'))
		  Begin
		   ALTER TABLE Directions ADD  AutomaticLastUpdateDate datetime NULL DEFAULT GETDATE();
		  declare @SQLDirection as varchar(8000)
		 
BEGIN TRY
    BEGIN TRANSACTION

    BEGIN
            ;

        SET @SQLDirection = 
            'CREATE TRIGGER Trigger_AutomaticLastUpdateDateDirections ON Directions AFTER UPDATE  AS  BEGIN UPDATE Directions SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted) END;'

        EXEC (@SQLDirection);

       
    END

    COMMIT TRANSACTION;
END TRY

BEGIN CATCH
    ROLLBACK TRANSACTION
END CATCH;


		  End

--Ports
IF not EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'AutomaticLastUpdateDate'
          AND Object_ID = Object_ID(N'Ports'))
		  Begin
		   ALTER TABLE Ports ADD  AutomaticLastUpdateDate datetime NULL DEFAULT GETDATE();
		  declare @SQLPort as varchar(8000)
		 
BEGIN TRY
    BEGIN TRANSACTION

    BEGIN
            ;

        SET @SQLPort = 
            'CREATE TRIGGER Trigger_AutomaticLastUpdateDatePorts ON Ports AFTER UPDATE  AS  BEGIN UPDATE Ports SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted) END;'

        EXEC (@SQLPort);

       
    END

    COMMIT TRANSACTION;
END TRY

BEGIN CATCH
    ROLLBACK TRANSACTION
END CATCH



		  End

--TransportModes
IF not EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'AutomaticLastUpdateDate'
          AND Object_ID = Object_ID(N'TransportModes'))
		  Begin
		   ALTER TABLE TransportModes ADD  AutomaticLastUpdateDate datetime NULL DEFAULT GETDATE();
		  declare @SQLTransportMode as varchar(8000)
		 
		 
BEGIN TRY
    BEGIN TRANSACTION

    BEGIN
            ;

        SET @SQLTransportMode = 
            'CREATE TRIGGER Trigger_AutomaticLastUpdateDateTransportModes ON TransportModes AFTER UPDATE  AS  BEGIN UPDATE TransportModes SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)  END;'

        EXEC (@SQLTransportMode);

       
    END

    COMMIT TRANSACTION;
END TRY

BEGIN CATCH
    ROLLBACK TRANSACTION
END CATCH;


		  End

--ShipmentLevels
IF not EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'AutomaticLastUpdateDate'
          AND Object_ID = Object_ID(N'ShipmentLevels'))
		  Begin
		   ALTER TABLE ShipmentLevels ADD  AutomaticLastUpdateDate datetime NULL DEFAULT GETDATE();
		  declare @SQLShipmentLevel as varchar(8000)
		 
BEGIN TRY
    BEGIN TRANSACTION

    BEGIN
            ;

        SET @SQLShipmentLevel = 
            'CREATE TRIGGER Trigger_AutomaticLastUpdateDateShipmentLevels ON ShipmentLevels AFTER UPDATE  AS  BEGIN UPDATE ShipmentLevels SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted) END;'

        EXEC (@SQLShipmentLevel);

       
    END

    COMMIT TRANSACTION;
END TRY

BEGIN CATCH
    ROLLBACK TRANSACTION
END CATCH;


		  End


--ShipmentTypes
IF not EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'AutomaticLastUpdateDate'
          AND Object_ID = Object_ID(N'ShipmentTypes'))
		  Begin
		  ALTER TABLE ShipmentTypes ADD  AutomaticLastUpdateDate datetime NULL DEFAULT GETDATE();
		  declare @SQLShipmentType as varchar(8000)

BEGIN TRY
    BEGIN TRANSACTION

    BEGIN
            ;

        SET @SQLShipmentType = 
            'CREATE TRIGGER Trigger_AutomaticLastUpdateDateShipmentTypes ON ShipmentTypes AFTER UPDATE  AS  BEGIN UPDATE ShipmentTypes SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted) END;'

        EXEC (@SQLShipmentType);

       
    END

    COMMIT TRANSACTION;
END TRY

BEGIN CATCH
    ROLLBACK TRANSACTION
END CATCH;


		  End


 --Departments
IF not EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'AutomaticLastUpdateDate'
          AND Object_ID = Object_ID(N'Departments'))
		  Begin
		  ALTER TABLE Departments ADD  AutomaticLastUpdateDate datetime NULL DEFAULT GETDATE();
		  declare @SQLDepartment as varchar(8000)
		  
BEGIN TRY
    BEGIN TRANSACTION

    BEGIN
            ;

        SET @SQLDepartment = 
            'CREATE TRIGGER Trigger_AutomaticLastUpdateDateDepartments ON Departments AFTER UPDATE  AS  BEGIN UPDATE Departments SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)  END;'

        EXEC (@SQLDepartment);

       
    END

    COMMIT TRANSACTION;
END TRY

BEGIN CATCH
    ROLLBACK TRANSACTION
END CATCH;


		  End



--Branches
IF not EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'AutomaticLastUpdateDate'
          AND Object_ID = Object_ID(N'Branches'))
		  Begin
		   ALTER TABLE Branches ADD  AutomaticLastUpdateDate datetime NULL DEFAULT GETDATE();
		  declare @SQLBranche as varchar(8000)
		 
BEGIN TRY
    BEGIN TRANSACTION

    BEGIN
            ;

        SET @SQLBranche = 
            'CREATE TRIGGER Trigger_AutomaticLastUpdateDateBranches ON Branches AFTER UPDATE  AS  BEGIN UPDATE Branches SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted) END;'

        EXEC (@SQLBranche);

       
    END

    COMMIT TRANSACTION;
END TRY

BEGIN CATCH
    ROLLBACK TRANSACTION
END CATCH;


		  End

 --Incoterms
IF not EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'AutomaticLastUpdateDate'
          AND Object_ID = Object_ID(N'Incoterms'))
		  Begin
		  ALTER TABLE Incoterms ADD  AutomaticLastUpdateDate datetime NULL DEFAULT GETDATE();
		  declare @SQLIncoterm as varchar(8000)
		  
BEGIN TRY
    BEGIN TRANSACTION

    BEGIN
            ;

        SET @SQLIncoterm = 
            'CREATE TRIGGER Trigger_AutomaticLastUpdateDateIncoterms ON Incoterms AFTER UPDATE  AS  BEGIN UPDATE Incoterms SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted) END;'

        EXEC (@SQLIncoterm);

       
    END

    COMMIT TRANSACTION;
END TRY

BEGIN CATCH
    ROLLBACK TRANSACTION
END CATCH;


		  End

 --Users
IF not EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'AutomaticLastUpdateDate'
          AND Object_ID = Object_ID(N'Users'))
		  Begin
		   ALTER TABLE Users ADD  AutomaticLastUpdateDate datetime NULL DEFAULT GETDATE();
		  declare @SQLUser as varchar(8000)
		 
BEGIN TRY
    BEGIN TRANSACTION

    BEGIN
            ;

        SET @SQLUser = 
            'CREATE TRIGGER Trigger_AutomaticLastUpdateDateUsers ON Users AFTER UPDATE  AS  BEGIN UPDATE Users SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;'

        EXEC (@SQLUser);

       
    END

    COMMIT TRANSACTION;
END TRY

BEGIN CATCH
    ROLLBACK TRANSACTION
END CATCH;


		  End

 --Currencies
IF not EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'AutomaticLastUpdateDate'
          AND Object_ID = Object_ID(N'Currencies'))
		  Begin
		   ALTER TABLE Currencies ADD  AutomaticLastUpdateDate datetime NULL DEFAULT GETDATE();
		  declare @SQLCurrencies as varchar(8000)
		 
BEGIN TRY
    BEGIN TRANSACTION

    BEGIN
            ;

        SET @SQLCurrencies = 
            'CREATE TRIGGER Trigger_AutomaticLastUpdateDateCurrencies ON Currencies AFTER UPDATE  AS  BEGIN UPDATE Currencies SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted) END;'

        EXEC (@SQLCurrencies);

       
    END

    COMMIT TRANSACTION;
END TRY

BEGIN CATCH
    ROLLBACK TRANSACTION
END CATCH;


		  End

--EntityStatus
IF not EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'AutomaticLastUpdateDate'
          AND Object_ID = Object_ID(N'EntityStatus'))
		  Begin
		    ALTER TABLE EntityStatus ADD  AutomaticLastUpdateDate datetime NULL DEFAULT GETDATE();
		  declare @SQLEntityStatus as varchar(8000)
		
BEGIN TRY
    BEGIN TRANSACTION

    BEGIN
            ;

        SET @SQLEntityStatus = 
            'CREATE TRIGGER Trigger_AutomaticLastUpdateDateEntityStatus ON EntityStatus AFTER UPDATE  AS  BEGIN UPDATE EntityStatus SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted) END;'

        EXEC (@SQLEntityStatus);

       
    END

    COMMIT TRANSACTION;
END TRY

BEGIN CATCH
    ROLLBACK TRANSACTION
END CATCH;


		  End

 --Addresses
IF not EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'AutomaticLastUpdateDate'
          AND Object_ID = Object_ID(N'Addresses'))
		  Begin
		   ALTER TABLE Addresses ADD  AutomaticLastUpdateDate datetime NULL DEFAULT GETDATE();
		  declare @SQLAddress as varchar(8000)
		 
BEGIN TRY
    BEGIN TRANSACTION

    BEGIN
            ;

        SET @SQLAddress = 
            'CREATE TRIGGER Trigger_AutomaticLastUpdateDateAddresses ON Addresses AFTER UPDATE  AS  BEGIN UPDATE Addresses SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)  END;'

        EXEC (@SQLAddress);

       
    END

    COMMIT TRANSACTION;
END TRY

BEGIN CATCH
    ROLLBACK TRANSACTION
END CATCH;


		  End


--Countries
IF not EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'AutomaticLastUpdateDate'
          AND Object_ID = Object_ID(N'Countries'))
		  Begin
		   ALTER TABLE Countries ADD  AutomaticLastUpdateDate datetime NULL DEFAULT GETDATE();
		  declare @SQLCountries as varchar(8000)
		 
BEGIN TRY
    BEGIN TRANSACTION

    BEGIN
            ;

        SET @SQLCountries = 
            'CREATE TRIGGER Trigger_AutomaticLastUpdateDateCountries ON Countries AFTER UPDATE  AS  BEGIN UPDATE Countries SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted) END;'

        EXEC (@SQLCountries);

       
    END

    COMMIT TRANSACTION;
END TRY

BEGIN CATCH
    ROLLBACK TRANSACTION
END CATCH;


		  End

 --Contacts
IF not EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'AutomaticLastUpdateDate'
          AND Object_ID = Object_ID(N'Contacts'))
		  Begin
		   ALTER TABLE Contacts ADD  AutomaticLastUpdateDate datetime NULL DEFAULT GETDATE();
		  declare @SQLContact as varchar(8000)
		 
BEGIN TRY
    BEGIN TRANSACTION

    BEGIN
            ;

        SET @SQLContact = 
            'CREATE TRIGGER Trigger_AutomaticLastUpdateDateContacts ON Contacts AFTER UPDATE  AS  BEGIN UPDATE Contacts SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted) END;'

        EXEC (@SQLContact);

       
    END

    COMMIT TRANSACTION;
END TRY

BEGIN CATCH
    ROLLBACK TRANSACTION
END CATCH;



		  End


		   --States
IF not EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'AutomaticLastUpdateDate'
          AND Object_ID = Object_ID(N'States'))
		  Begin
		  ALTER TABLE States ADD  AutomaticLastUpdateDate datetime NULL DEFAULT GETDATE();
		  declare @SQLState as varchar(8000)
		  
BEGIN TRY
    BEGIN TRANSACTION

    BEGIN
            ;

        SET @SQLState = 
            'CREATE TRIGGER Trigger_AutomaticLastUpdateDateStates ON States AFTER UPDATE  AS  BEGIN UPDATE States SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted) END;'

        EXEC (@SQLState);

       
    END

    COMMIT TRANSACTION;
END TRY

BEGIN CATCH
    ROLLBACK TRANSACTION
END CATCH;


		  End



		   --Customers
IF not EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'AutomaticLastUpdateDate'
          AND Object_ID = Object_ID(N'Customers'))
		  Begin
		   ALTER TABLE Customers ADD  AutomaticLastUpdateDate datetime NULL DEFAULT GETDATE();
		  declare @SQLCustomer as varchar(8000)
		 
BEGIN TRY
    BEGIN TRANSACTION

    BEGIN
            ;

        SET @SQLCustomer = 
            'CREATE TRIGGER Trigger_AutomaticLastUpdateDateCustomers ON Customers AFTER UPDATE  AS  BEGIN UPDATE Customers SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted) END;'

        EXEC (@SQLCustomer);

       
    END

    COMMIT TRANSACTION;
END TRY

BEGIN CATCH
    ROLLBACK TRANSACTION
END CATCH;


		  End

		   --Ranks
IF not EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'AutomaticLastUpdateDate'
          AND Object_ID = Object_ID(N'Ranks'))
		  Begin
		   ALTER TABLE Ranks ADD  AutomaticLastUpdateDate datetime NULL DEFAULT GETDATE();
		  declare @SQLRanks as varchar(8000)
		 
BEGIN TRY
    BEGIN TRANSACTION

    BEGIN
            ;

        SET @SQLRanks = 
            'CREATE TRIGGER Trigger_AutomaticLastUpdateDateRanks ON Ranks AFTER UPDATE  AS  BEGIN UPDATE Ranks SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)  END;'

        EXEC (@SQLRanks);

       
    END

    COMMIT TRANSACTION;
END TRY

BEGIN CATCH
    ROLLBACK TRANSACTION
END CATCH;

		  End


		   --PartnerTypes
IF not EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'AutomaticLastUpdateDate'
          AND Object_ID = Object_ID(N'PartnerTypes'))
		  Begin
		  ALTER TABLE PartnerTypes ADD  AutomaticLastUpdateDate datetime NULL DEFAULT GETDATE();
		  declare @SQLPartnerType as varchar(8000)
		  
BEGIN TRY
    BEGIN TRANSACTION

    BEGIN
            ;

        SET @SQLPartnerType = 
            'CREATE TRIGGER Trigger_AutomaticLastUpdateDatePartnerTypes ON PartnerTypes AFTER UPDATE  AS  BEGIN UPDATE PartnerTypes SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)  END;'

        EXEC (@SQLPartnerType);

       
    END

    COMMIT TRANSACTION;
END TRY

BEGIN CATCH
    ROLLBACK TRANSACTION
END CATCH;

		  End


		   --ShipmentMasterDatas
IF not EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'AutomaticLastUpdateDate'
          AND Object_ID = Object_ID(N'ShipmentMasterDatas'))
		  Begin
		   ALTER TABLE ShipmentMasterDatas ADD  AutomaticLastUpdateDate datetime NULL DEFAULT GETDATE();
		  declare @SQLShipmentMasterData as varchar(8000)
		 
BEGIN TRY
    BEGIN TRANSACTION

    BEGIN
            ;

        SET @SQLShipmentMasterData = 
            'CREATE TRIGGER Trigger_AutomaticLastUpdateDateShipmentMasterDatas ON ShipmentMasterDatas AFTER UPDATE  AS  BEGIN UPDATE ShipmentMasterDatas SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)  END;'

        EXEC (@SQLShipmentMasterData);

       
    END

    COMMIT TRANSACTION;
END TRY

BEGIN CATCH
    ROLLBACK TRANSACTION
END CATCH;


		  End


		  
 --DWHSettings
IF not EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'AutomaticLastUpdateDate'
          AND Object_ID = Object_ID(N'DWHSettings'))
		  Begin
		   ALTER TABLE DWHSettings ADD  AutomaticLastUpdateDate datetime NULL DEFAULT GETDATE();
		  declare @SQLDWHSetting as varchar(8000)
		 
BEGIN TRY
    BEGIN TRANSACTION

    BEGIN
            ;

        SET @SQLDWHSetting = 
            'CREATE TRIGGER Trigger_AutomaticLastUpdateDateDWHSettings ON DWHSettings AFTER UPDATE  AS  BEGIN UPDATE DWHSettings SET AutomaticLastUpdateDate = GETDATE() WHERE Id IN (SELECT DISTINCT Id FROM Inserted)END;'

        EXEC (@SQLDWHSetting);

       
    END

    COMMIT TRANSACTION;
END TRY

BEGIN CATCH
    ROLLBACK TRANSACTION
END CATCH;


		  End