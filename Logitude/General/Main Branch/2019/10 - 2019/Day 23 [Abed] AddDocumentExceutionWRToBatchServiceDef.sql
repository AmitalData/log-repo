--> Please run this script at Global db 

delete BatchServicesDefinitionMods where Code = 'DocumentExecutionWR'
delete BatchServicesDefinitions where Code = 'DocumentExecutionWR'

INSERT INTO [dbo].[BatchServicesDefinitions]
           ([Code]
           ,[ClassName]
           ,[Parameter1]
           ,[Parameter2]
		   ,[QueueDefinitionCode])
     VALUES
           ('DocumentsExecutionWR'
           ,'DocumentsExecutionWorkerRole'
           ,NULL
           ,NULL,
		   'DocumentsExecutionQueue')

INSERT INTO [dbo].[BatchServicesDefinitionMods]
           ([Code]
           ,[InActive]
           ,[NumberOfThreads])
     VALUES
           ('DocumentsExecutionWR'
           ,0
           ,1)
