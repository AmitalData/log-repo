--> Please run this script at Global db 
INSERT INTO [dbo].[BatchServicesDefinitions]
           ([Code]
           ,[ClassName]
           ,[Parameter1]
           ,[Parameter2])
     VALUES
           ('DocumentFilingBackupQueueBuilderWR'
           ,'DocumentFilingBackupQueueBuilderWR'
           ,NULL
           ,NULL)

INSERT INTO [dbo].[BatchServicesDefinitionMods]
           ([Code]
           ,[InActive]
           ,[NumberOfThreads])
     VALUES
           ('DocumentFilingBackupQueueBuilderWR'
           ,0
           ,1)



		   --> Please run this script at Global db 
INSERT INTO [dbo].[BatchServicesDefinitions]
           ([Code]
           ,[ClassName]
           ,[Parameter1]
           ,[Parameter2])
     VALUES
           ('DocumentFilingBackupBatchQueueWR'
           ,'DocumentFilingBackupBatchQueueWR'
           ,NULL
           ,NULL)

INSERT INTO [dbo].[BatchServicesDefinitionMods]
           ([Code]
           ,[InActive]
           ,[NumberOfThreads])
     VALUES
           ('DocumentFilingBackupBatchQueueWR'
           ,0
           ,1)






