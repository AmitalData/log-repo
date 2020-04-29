--> run this script at Global db 

delete BatchServicesDefinitionMods where Code = 'ImporterDigitalShipments'
delete BatchServicesDefinitions where Code = 'ImporterDigitalShipments'

INSERT INTO [dbo].[BatchServicesDefinitions]
           ([Code]
           ,[ClassName]
           ,[Parameter1]
           ,[Parameter2]
		   ,[QueueDefinitionCode])
     VALUES
           ('ImporterDigitalShipments'
           ,'ImporterShipmentsWorkerRole'
           ,0
           ,'Digital',
		   'ImportersDigitalShipmentQueue')

INSERT INTO [dbo].[BatchServicesDefinitionMods]
           ([Code]
           ,[InActive]
           ,[NumberOfThreads])
     VALUES
           ('ImporterDigitalShipments'
           ,0
           ,1)
