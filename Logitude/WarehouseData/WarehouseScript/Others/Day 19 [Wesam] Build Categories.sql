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




		  --CustomFields
IF not EXISTS(SELECT 1 FROM [dbo].[DWCategories] 
          WHERE Code = 'CustomFields')
		  Begin  
				INSERT INTO [dbo].[DWCategories] ([Code],[Name],[Index]) VALUES('CustomFields','CustomFields',110)
		  End
ELSE      Begin 
				UPDATE [dbo].[DWCategories] Set [dbo].[DWCategories].[Index] = 110 WHERE [dbo].[DWCategories].[Code] = 'CustomFields'	  
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