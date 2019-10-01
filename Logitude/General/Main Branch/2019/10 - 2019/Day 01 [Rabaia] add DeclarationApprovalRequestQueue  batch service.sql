--> Please run this script at Global db 
INSERT INTO [dbo].[BatchServicesDefinitions]
           ([Code]
           ,[ClassName]
           ,[Parameter1]
           ,[Parameter2]
		   ,[QueueDefinitionCode])
     VALUES
           ('DeclarationApprovalRequestWorkerRole'
           ,'DeclarationApprovalRequestWorkerRole'
           ,NULL
           ,NULL
		   ,'DeclarationApprovalRequestQueue')

INSERT INTO [dbo].[BatchServicesDefinitionMods]
           ([Code]
           ,[InActive]
           ,[NumberOfThreads])
     VALUES
           ('DeclarationApprovalRequestWorkerRole'
           ,0
           ,1)



