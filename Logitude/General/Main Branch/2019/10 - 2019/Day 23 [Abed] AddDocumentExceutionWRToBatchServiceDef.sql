		   --> Please run this script at Global db 
INSERT INTO [dbo].[BatchServicesDefinitions]
           ([Code]
           ,[ClassName]
           ,[Parameter1]
           ,[Parameter2]
		   ,[QueueDefinitionCode])
     VALUES
           ('DocumentExecutionWR'
           ,'DocumentExecutionWorkerRole'
           ,NULL
           ,NULL,
		   'DocumentExecutionQueue')

INSERT INTO [dbo].[BatchServicesDefinitionMods]
           ([Code]
           ,[InActive]
           ,[NumberOfThreads])
     VALUES
           ('DocumentExecutionWR'
           ,0
           ,1)