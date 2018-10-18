INSERT INTO [dbo].[BatchServicesDefinitions]
           (Code
           ,ClassName
           ,Parameter1
           ,Parameter2)
     VALUES
           ('SendToCustoms'
           ,'SendToCustomsWorkerRole'
           ,NULL          
           ,NULL)

INSERT INTO [dbo].[BatchServicesDefinitionMods]
           (Code
           ,InActive
           ,NumberOfThreads)
     VALUES
           ('SendToCustoms'
           ,0
           ,1)