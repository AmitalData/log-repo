 --System Log DB

IF not EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_FailedLoginLogs_GMTDateTime'
    AND object_id = OBJECT_ID('[dbo].[FailedLoginLogs]'))
  begin
    CREATE NONCLUSTERED INDEX [IX_FailedLoginLogs_GMTDateTime]
ON [dbo].[FailedLoginLogs]([GMTDateTime])
  end   


  IF not EXISTS (SELECT *  FROM sys.indexes  WHERE name='IX_FailedTokenLogs_GMTDateTime'
    AND object_id = OBJECT_ID('[dbo].[FailedTokenLogs]'))
  begin
    CREATE NONCLUSTERED INDEX [IX_FailedTokenLogs_GMTDateTime]
ON [dbo].[FailedTokenLogs]([GMTDateTime])
  end   