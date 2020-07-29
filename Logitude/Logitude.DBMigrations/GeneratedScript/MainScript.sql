-- Add New Column With Name AdditionalFactCode
ALTER TABLE [dbo].[DWObjectTables] ADD [AdditionalFactCode] VARCHAR(50) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('351bff9b-419e-41ab-8cbe-8d156d9d6d1c', 'DWObjectTable.dxml', 'DWObjectTables', 'AdditionalFactCode', 'Add Column', GETDATE(), '-- Add New Column With Name AdditionalFactCodeALTER TABLE [dbo].[DWObjectTables] ADD [AdditionalFactCode] VARCHAR(50) NULL;');

-- Add New Column With Name AdditionalFactForeignKey
ALTER TABLE [dbo].[DWObjectTables] ADD [AdditionalFactForeignKey] VARCHAR(100) NULL;

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('50fa1aff-876b-40a4-8b45-ac46f9970f12', 'DWObjectTable.dxml', 'DWObjectTables', 'AdditionalFactForeignKey', 'Add Column', GETDATE(), '-- Add New Column With Name AdditionalFactForeignKeyALTER TABLE [dbo].[DWObjectTables] ADD [AdditionalFactForeignKey] VARCHAR(100) NULL;');


-- Drop Column BookingConfirmationSent
EXEC SP_RENAME 'dbo.ShipmentComputedFields.BookingConfirmationSent', 'Drop_BookingConfirmationSent', 'COLUMN';

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('7c3ccd9c-7ca1-4ff4-9c1b-80ffcd96cdc3', 'ShipmentComputedFields.dxml', 'ShipmentComputedFields', 'BookingConfirmationSent', 'Drop Column', GETDATE(), '-- Drop Column BookingConfirmationSentEXEC SP_RENAME ''dbo.ShipmentComputedFields.BookingConfirmationSent'', ''Drop_BookingConfirmationSent'', ''COLUMN'';');

-- Drop Column PreAlertSent
EXEC SP_RENAME 'dbo.ShipmentComputedFields.PreAlertSent', 'Drop_PreAlertSent', 'COLUMN';

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('9079eea0-9a5b-4232-ba3b-e2cbc06e3bbb', 'ShipmentComputedFields.dxml', 'ShipmentComputedFields', 'PreAlertSent', 'Drop Column', GETDATE(), '-- Drop Column PreAlertSentEXEC SP_RENAME ''dbo.ShipmentComputedFields.PreAlertSent'', ''Drop_PreAlertSent'', ''COLUMN'';');

-- Drop Column DeliveryNoticeSent
EXEC SP_RENAME 'dbo.ShipmentComputedFields.DeliveryNoticeSent', 'Drop_DeliveryNoticeSent', 'COLUMN';

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('07c77e5f-96b6-4672-9aba-02175dfbb3c0', 'ShipmentComputedFields.dxml', 'ShipmentComputedFields', 'DeliveryNoticeSent', 'Drop Column', GETDATE(), '-- Drop Column DeliveryNoticeSentEXEC SP_RENAME ''dbo.ShipmentComputedFields.DeliveryNoticeSent'', ''Drop_DeliveryNoticeSent'', ''COLUMN'';');

-- Drop Column ExpectedArrivalNoticeSent
EXEC SP_RENAME 'dbo.ShipmentComputedFields.ExpectedArrivalNoticeSent', 'Drop_ExpectedArrivalNoticeSent', 'COLUMN';

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('f9ed692a-a350-47bd-9917-199b1ff19ee6', 'ShipmentComputedFields.dxml', 'ShipmentComputedFields', 'ExpectedArrivalNoticeSent', 'Drop Column', GETDATE(), '-- Drop Column ExpectedArrivalNoticeSentEXEC SP_RENAME ''dbo.ShipmentComputedFields.ExpectedArrivalNoticeSent'', ''Drop_ExpectedArrivalNoticeSent'', ''COLUMN'';');

-- Drop Column T1Received
EXEC SP_RENAME 'dbo.ShipmentComputedFields.T1Received', 'Drop_T1Received', 'COLUMN';

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('77edcd59-33ab-48a5-9d3a-f0eec4424a08', 'ShipmentComputedFields.dxml', 'ShipmentComputedFields', 'T1Received', 'Drop Column', GETDATE(), '-- Drop Column T1ReceivedEXEC SP_RENAME ''dbo.ShipmentComputedFields.T1Received'', ''Drop_T1Received'', ''COLUMN'';');

-- Drop Column ArrivalNoticeSent
EXEC SP_RENAME 'dbo.ShipmentComputedFields.ArrivalNoticeSent', 'Drop_ArrivalNoticeSent', 'COLUMN';

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('a3449ce7-17cc-4dfd-954e-9c89ae6fa416', 'ShipmentComputedFields.dxml', 'ShipmentComputedFields', 'ArrivalNoticeSent', 'Drop Column', GETDATE(), '-- Drop Column ArrivalNoticeSentEXEC SP_RENAME ''dbo.ShipmentComputedFields.ArrivalNoticeSent'', ''Drop_ArrivalNoticeSent'', ''COLUMN'';');


