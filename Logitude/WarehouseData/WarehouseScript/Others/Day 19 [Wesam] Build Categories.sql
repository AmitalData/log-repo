--General
IF not EXISTS(SELECT 1 FROM [dbo].[DWCategories] 
          WHERE Code = 'General')
		  Begin  
				INSERT INTO [dbo].[DWCategories] ([Code],[Name],[Index]) VALUES('General','General',10)
		  End
ELSE      Begin 
				UPDATE [dbo].[DWCategories] Set [dbo].[DWCategories].[Index] = 10 WHERE [dbo].[DWCategories].[Code] = 'General'	  
		  End

--Operational
IF not EXISTS(SELECT 1 FROM [dbo].[DWCategories] 
          WHERE Code = 'Operational')
		  Begin  
				INSERT INTO [dbo].[DWCategories] ([Code],[Name],[Index]) VALUES('Operational','Operational',20)
		  End
ELSE      Begin 
				UPDATE [dbo].[DWCategories] Set [dbo].[DWCategories].[Index] = 20 WHERE [dbo].[DWCategories].[Code] = 'Operational'	  
		  End

--References
IF not EXISTS(SELECT 1 FROM [dbo].[DWCategories] 
          WHERE Code = 'References')
		  Begin  
				INSERT INTO [dbo].[DWCategories] ([Code],[Name],[Index]) VALUES('References','References',30)
		  End
ELSE      Begin 
				UPDATE [dbo].[DWCategories] Set [dbo].[DWCategories].[Index] = 30 WHERE [dbo].[DWCategories].[Code] = 'References'	  
		  End

--Partners
IF not EXISTS(SELECT 1 FROM [dbo].[DWCategories] 
          WHERE Code = 'Partners')
		  Begin  
				INSERT INTO [dbo].[DWCategories] ([Code],[Name],[Index]) VALUES('Partners','Partners',40)
		  End
ELSE      Begin 
				UPDATE [dbo].[DWCategories] Set [dbo].[DWCategories].[Index] = 40 WHERE [dbo].[DWCategories].[Code] = 'Partners'	  
		  End
 
  
--Packages
IF not EXISTS(SELECT 1 FROM [dbo].[DWCategories] 
          WHERE Code = 'Partners')
		  Begin  
				INSERT INTO [dbo].[DWCategories] ([Code],[Name],[Index]) VALUES('Packages','Packages',50)
		  End
ELSE      Begin 
				UPDATE [dbo].[DWCategories] Set [dbo].[DWCategories].[Index] = 50 WHERE [dbo].[DWCategories].[Code] = 'Packages'	  
		  End

--Money
IF not EXISTS(SELECT 1 FROM [dbo].[DWCategories] 
          WHERE Code = 'Money')
		  Begin  
				INSERT INTO [dbo].[DWCategories] ([Code],[Name],[Index]) VALUES('Money','Money',60)
		  End
ELSE      Begin 
				UPDATE [dbo].[DWCategories] Set [dbo].[DWCategories].[Index] = 60 WHERE [dbo].[DWCategories].[Code] = 'Money'	  
		  End

--Charges
IF not EXISTS(SELECT 1 FROM [dbo].[DWCategories] 
          WHERE Code = 'Charges')
		  Begin  
				INSERT INTO [dbo].[DWCategories] ([Code],[Name],[Index]) VALUES('Charges','Charges',70)
		  End
ELSE      Begin 
				UPDATE [dbo].[DWCategories] Set [dbo].[DWCategories].[Index] = 70 WHERE [dbo].[DWCategories].[Code] = 'Charges'	  
		  End

--Dates
IF not EXISTS(SELECT 1 FROM [dbo].[DWCategories] 
          WHERE Code = 'Dates')
		  Begin  
				INSERT INTO [dbo].[DWCategories] ([Code],[Name],[Index]) VALUES('Dates','Dates',80)
		  End
ELSE      Begin 
				UPDATE [dbo].[DWCategories] Set [dbo].[DWCategories].[Index] = 80 WHERE [dbo].[DWCategories].[Code] = 'Dates'	  
		  End



		  --Routings
IF not EXISTS(SELECT 1 FROM [dbo].[DWCategories] 
          WHERE Code = 'Routings')
		  Begin  
				INSERT INTO [dbo].[DWCategories] ([Code],[Name],[Index]) VALUES('Routings','Routings',90)
		  End
ELSE      Begin 
				UPDATE [dbo].[DWCategories] Set [dbo].[DWCategories].[Index] = 90 WHERE [dbo].[DWCategories].[Code] = 'Routings'	  
		  End


		  		  --Routings
IF not EXISTS(SELECT 1 FROM [dbo].[DWCategories] 
          WHERE Code = 'InvoiceLines')
		  Begin  
				INSERT INTO [dbo].[DWCategories] ([Code],[Name],[Index]) VALUES('InvoiceLines','Invoice Lines',90)
		  End
ELSE      Begin 
				UPDATE [dbo].[DWCategories] Set [dbo].[DWCategories].[Index] = 90 WHERE [dbo].[DWCategories].[Code] = 'InvoiceLines'	  
		  End

		  --KPI
IF not EXISTS(SELECT 1 FROM [dbo].[DWCategories] 
          WHERE Code = 'KPI')
		  Begin  
				INSERT INTO [dbo].[DWCategories] ([Code],[Name],[Index]) VALUES('KPI','KPI',100)
		  End
ELSE      Begin 
				UPDATE [dbo].[DWCategories] Set [dbo].[DWCategories].[Index] = 100 WHERE [dbo].[DWCategories].[Code] = 'KPI'	  
		  End

  

		  		  --PartnerAddresses
IF not EXISTS(SELECT 1 FROM [dbo].[DWCategories] 
          WHERE Code = 'PartnerAddresses')
		  Begin  
				INSERT INTO [dbo].[DWCategories] ([Code],[Name],[Index]) VALUES('PartnerAddresses','Shipment Partner Addresses',120)
		  End
ELSE      Begin 
				UPDATE [dbo].[DWCategories] Set [dbo].[DWCategories].[Index] = 120, [dbo].[DWCategories].[Name] = 'Shipment Partner Addresses' WHERE [dbo].[DWCategories].[Code] = 'PartnerAddresses'	  
		  End


		  		  --PartnerContacts
IF not EXISTS(SELECT 1 FROM [dbo].[DWCategories] 
          WHERE Code = 'PartnerContacts')
		  Begin  
				INSERT INTO [dbo].[DWCategories] ([Code],[Name],[Index]) VALUES('PartnerContacts','Shipment Partner Contacts',130)
		  End
ELSE      Begin 
				UPDATE [dbo].[DWCategories] Set [dbo].[DWCategories].[Index] = 130, [dbo].[DWCategories].[Name] = 'Shipment Partner Contacts' WHERE [dbo].[DWCategories].[Code] = 'PartnerContacts'	  
		  End
		   
 

		  		  --CustomFields
IF not EXISTS(SELECT 1 FROM [dbo].[DWCategories] 
          WHERE Code = 'InvoiceGeneralDetails')
		  Begin  
				INSERT INTO [dbo].[DWCategories] ([Code],[Name],[Index]) VALUES('InvoiceGeneralDetails','Invoice General Details',10)
		  End
ELSE      Begin 
				UPDATE [dbo].[DWCategories] Set [dbo].[DWCategories].[Index] = 10 WHERE [dbo].[DWCategories].[Code] = 'InvoiceGeneralDetails'	  
		  End

		  		   	  --InvoicePartners
IF not EXISTS(SELECT 1 FROM [dbo].[DWCategories] 
          WHERE Code = 'InvoicePartners')
		  Begin  
				INSERT INTO [dbo].[DWCategories] ([Code],[Name],[Index]) VALUES('InvoicePartners','Invoice Partners',40)
		  End
ELSE      Begin 
				UPDATE [dbo].[DWCategories] Set [dbo].[DWCategories].[Index] = 40, [dbo].[DWCategories].[Name] = 'Invoice Partners' WHERE [dbo].[DWCategories].[Code] = 'InvoicePartners'	  
		  End


		  -- Containers Caterories

		  		  		  --Empty Pick -up
IF not EXISTS(SELECT 1 FROM [dbo].[DWCategories] 
          WHERE Code = 'EmptyPick-up')
		  Begin  
				INSERT INTO [dbo].[DWCategories] ([Code],[Name],[Index]) VALUES('EmptyPick-up','Empty Pick -up',140)
		  End
ELSE      Begin 
				UPDATE [dbo].[DWCategories] Set [dbo].[DWCategories].[Index] = 140, [dbo].[DWCategories].[Name] = 'Empty Pick -up' WHERE [dbo].[DWCategories].[Code] = 'EmptyPick-up'	  
		  End

		  		  		  		  --Empty Pick -up
IF not EXISTS(SELECT 1 FROM [dbo].[DWCategories] 
          WHERE Code = 'Pick-up')
		  Begin  
				INSERT INTO [dbo].[DWCategories] ([Code],[Name],[Index]) VALUES('Pick-up','Pick -up',150)
		  End
ELSE      Begin 
				UPDATE [dbo].[DWCategories] Set [dbo].[DWCategories].[Index] = 150, [dbo].[DWCategories].[Name] = 'Pick -up' WHERE [dbo].[DWCategories].[Code] = 'Pick-up'	  
		  End

		  		  		  		  		  --Pre-Carriage
IF not EXISTS(SELECT 1 FROM [dbo].[DWCategories] 
          WHERE Code = 'Pre-Carriage')
		  Begin  
				INSERT INTO [dbo].[DWCategories] ([Code],[Name],[Index]) VALUES('Pre-Carriage','Pre-Carriage',160)
		  End
ELSE      Begin 
				UPDATE [dbo].[DWCategories] Set [dbo].[DWCategories].[Index] = 160, [dbo].[DWCategories].[Name] = 'Pre-Carriage' WHERE [dbo].[DWCategories].[Code] = 'Pre-Carriage'	  
		  End

		  
		  		  		  		  		  --POL
IF not EXISTS(SELECT 1 FROM [dbo].[DWCategories] 
          WHERE Code = 'POL')
		  Begin  
				INSERT INTO [dbo].[DWCategories] ([Code],[Name],[Index]) VALUES('POL','POL',170)
		  End
ELSE      Begin 
				UPDATE [dbo].[DWCategories] Set [dbo].[DWCategories].[Index] = 170, [dbo].[DWCategories].[Name] = 'POL' WHERE [dbo].[DWCategories].[Code] = 'POL'	  
		  End

		  		  		  		  		  		  --Main Carriage
IF not EXISTS(SELECT 1 FROM [dbo].[DWCategories] 
          WHERE Code = 'MainCarriage')
		  Begin  
				INSERT INTO [dbo].[DWCategories] ([Code],[Name],[Index]) VALUES('MainCarriage','Main Carriage',180)
		  End
ELSE      Begin 
				UPDATE [dbo].[DWCategories] Set [dbo].[DWCategories].[Index] = 180, [dbo].[DWCategories].[Name] = 'Main Carriage' WHERE [dbo].[DWCategories].[Code] = 'MainCarriage'	  
		  End

		  		  		  		  		  		   --Transshipment - 1
IF not EXISTS(SELECT 1 FROM [dbo].[DWCategories] 
          WHERE Code = 'Transshipment-1')
		  Begin  
				INSERT INTO [dbo].[DWCategories] ([Code],[Name],[Index]) VALUES('Transshipment-1','Transshipment - 1',190)
		  End
ELSE      Begin 
				UPDATE [dbo].[DWCategories] Set [dbo].[DWCategories].[Index] = 190, [dbo].[DWCategories].[Name] = 'Transshipment - 1' WHERE [dbo].[DWCategories].[Code] = 'Transshipment-1'	  
		  End

		  		  		  		  		  		  		   --Transshipment - 2
IF not EXISTS(SELECT 1 FROM [dbo].[DWCategories] 
          WHERE Code = 'Transshipment-2')
		  Begin  
				INSERT INTO [dbo].[DWCategories] ([Code],[Name],[Index]) VALUES('Transshipment-2','Transshipment - 2',200)
		  End
ELSE      Begin 
				UPDATE [dbo].[DWCategories] Set [dbo].[DWCategories].[Index] = 200, [dbo].[DWCategories].[Name] = 'Transshipment - 2' WHERE [dbo].[DWCategories].[Code] = 'Transshipment-2'	  
		  End

		  		  		  		  		  		  		  		   --Transshipment - 3
IF not EXISTS(SELECT 1 FROM [dbo].[DWCategories] 
          WHERE Code = 'Transshipment-3')
		  Begin  
				INSERT INTO [dbo].[DWCategories] ([Code],[Name],[Index]) VALUES('Transshipment-3','Transshipment - 3',210)
		  End
ELSE      Begin 
				UPDATE [dbo].[DWCategories] Set [dbo].[DWCategories].[Index] = 210, [dbo].[DWCategories].[Name] = 'Transshipment - 3' WHERE [dbo].[DWCategories].[Code] = 'Transshipment-3'	  
		  End


		  		  		  		  		  		  		  		   --Transshipment - 4
IF not EXISTS(SELECT 1 FROM [dbo].[DWCategories] 
          WHERE Code = 'Transshipment-4')
		  Begin  
				INSERT INTO [dbo].[DWCategories] ([Code],[Name],[Index]) VALUES('Transshipment-4','Transshipment - 4',220)
		  End
ELSE      Begin 
				UPDATE [dbo].[DWCategories] Set [dbo].[DWCategories].[Index] = 220, [dbo].[DWCategories].[Name] = 'Transshipment - 4' WHERE [dbo].[DWCategories].[Code] = 'Transshipment-4'	  
		  End


		  		  		  		  		  		  --POD
IF not EXISTS(SELECT 1 FROM [dbo].[DWCategories] 
          WHERE Code = 'POD')
		  Begin  
				INSERT INTO [dbo].[DWCategories] ([Code],[Name],[Index]) VALUES('POD','POD',230)
		  End
ELSE      Begin 
				UPDATE [dbo].[DWCategories] Set [dbo].[DWCategories].[Index] = 230, [dbo].[DWCategories].[Name] = 'POD' WHERE [dbo].[DWCategories].[Code] = 'POD'	  
		  End

		  
		  		  		  		  		  		  --Availability
IF not EXISTS(SELECT 1 FROM [dbo].[DWCategories] 
          WHERE Code = 'Availability')
		  Begin  
				INSERT INTO [dbo].[DWCategories] ([Code],[Name],[Index]) VALUES('Availability','Availability',240)
		  End
ELSE      Begin 
				UPDATE [dbo].[DWCategories] Set [dbo].[DWCategories].[Index] = 240, [dbo].[DWCategories].[Name] = 'Availability' WHERE [dbo].[DWCategories].[Code] = 'Availability'	  
		  End


		  		  
		  		  		  		  		  		  --On Carriage
IF not EXISTS(SELECT 1 FROM [dbo].[DWCategories] 
          WHERE Code = 'OnCarriage')
		  Begin  
				INSERT INTO [dbo].[DWCategories] ([Code],[Name],[Index]) VALUES('OnCarriage','On Carriage',250)
		  End
ELSE      Begin 
				UPDATE [dbo].[DWCategories] Set [dbo].[DWCategories].[Index] = 250, [dbo].[DWCategories].[Name] = 'On Carriage' WHERE [dbo].[DWCategories].[Code] = 'OnCarriage'	  
		  End



			  		  		  		  		  		  --Delivery
IF not EXISTS(SELECT 1 FROM [dbo].[DWCategories] 
          WHERE Code = 'Delivery')
		  Begin  
				INSERT INTO [dbo].[DWCategories] ([Code],[Name],[Index]) VALUES('Delivery','Delivery',260)
		  End
ELSE      Begin 
				UPDATE [dbo].[DWCategories] Set [dbo].[DWCategories].[Index] = 260, [dbo].[DWCategories].[Name] = 'Delivery' WHERE [dbo].[DWCategories].[Code] = 'Delivery'	  
		  End

			  		  		  		  		  		  --Delivery Empty Return 
IF not EXISTS(SELECT 1 FROM [dbo].[DWCategories] 
          WHERE Code = 'DeliveryEmptyReturn')
		  Begin  
				INSERT INTO [dbo].[DWCategories] ([Code],[Name],[Index]) VALUES('DeliveryEmptyReturn ','Delivery Empty Return ',270)
		  End
ELSE      Begin 
				UPDATE [dbo].[DWCategories] Set [dbo].[DWCategories].[Index] = 270, [dbo].[DWCategories].[Name] = 'Delivery Empty Return ' WHERE [dbo].[DWCategories].[Code] = 'DeliveryEmptyReturn'	  
		  End

	    
		


		  --CustomFields
IF not EXISTS(SELECT 1 FROM [dbo].[DWCategories] 
          WHERE Code = 'CustomFields')
		  Begin  
				INSERT INTO [dbo].[DWCategories] ([Code],[Name],[Index]) VALUES('CustomFields','CustomFields',280)
		  End
ELSE      Begin 
				UPDATE [dbo].[DWCategories] Set [dbo].[DWCategories].[Index] = 280 WHERE [dbo].[DWCategories].[Code] = 'CustomFields'	  
		  End

		  
		  --Customs
IF not EXISTS(SELECT 1 FROM [dbo].[DWCategories] 
          WHERE Code = 'Customs')
		  Begin  
				INSERT INTO [dbo].[DWCategories] ([Code],[Name],[Index]) VALUES('Customs','Customs',290)
		  End
ELSE      Begin 
				UPDATE [dbo].[DWCategories] Set [dbo].[DWCategories].[Index] = 290 WHERE [dbo].[DWCategories].[Code] = 'Customs'	  
		  End

