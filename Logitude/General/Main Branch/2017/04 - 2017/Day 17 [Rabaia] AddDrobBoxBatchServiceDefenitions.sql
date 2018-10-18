--> Please run this script at Global db 
INSERT INTO [dbo].[BatchServicesDefinitions]
           ([Code]
           ,[ClassName]
           ,[Parameter1]
           ,[Parameter2])
     VALUES
           ('DropBoxFileToAnalyzeQueue'
           ,'AddDropBoxFileToAnalyzeQueueWR'
           ,NULL
           ,NULL)

INSERT INTO [dbo].[BatchServicesDefinitionMods]
           ([Code]
           ,[InActive]
           ,[NumberOfThreads])
     VALUES
           ('DropBoxFileToAnalyzeQueue'
           ,0
           ,1)

INSERT INTO [dbo].[BatchServicesDefinitions]
           ([Code]
           ,[ClassName]
           ,[Parameter1]
           ,[Parameter2])
     VALUES
           ('AddFileToDropBoxWR'
           ,'AddFileToDropBoxWR'
           ,NULL
           ,NULL)

INSERT INTO [dbo].[BatchServicesDefinitionMods]
           ([Code]
           ,[InActive]
           ,[NumberOfThreads])
     VALUES
           ('AddFileToDropBoxWR'
           ,0
           ,1)


