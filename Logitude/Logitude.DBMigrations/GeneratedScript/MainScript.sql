-- Add New Column With Name PartnerObjectFieldCode
ALTER TABLE [dbo].[AutomationResultEmailRecipients] ADD [PartnerObjectFieldCode] VARCHAR(200) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('948bba7a-27ba-4b6e-adc9-638865887ff4', 'AutomationResultEmailRecipient.dxml', 'AutomationResultEmailRecipients', 'PartnerObjectFieldCode', 'Add Column', GETDATE(), '-- Add New Column With Name PartnerObjectFieldCodeALTER TABLE [dbo].[AutomationResultEmailRecipients] ADD [PartnerObjectFieldCode] VARCHAR(200) NULL;');


