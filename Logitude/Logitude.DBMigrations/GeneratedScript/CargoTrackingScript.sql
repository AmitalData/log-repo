-- Change Size From -1 To 1000 For Column SearchFields
ALTER TABLE [dbo].[CargoTrackingShipmentSearches] ALTER COLUMN [SearchFields] NVARCHAR(1000);

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('0fb1794a-a848-4634-a8fd-4065e1f47c8d', 'CargoTrackingShipmentSearch.dxml', 'CargoTrackingShipmentSearches', 'SearchFields', 'Alter Column Size', GETDATE(), '-- Change Size From -1 To 1000 For Column SearchFieldsALTER TABLE [dbo].[CargoTrackingShipmentSearches] ALTER COLUMN [SearchFields] NVARCHAR(1000);');

-- Create Index On CargoTrackingShipmentSearches Table
EXEC('CREATE NONCLUSTERED INDEX [IX_CargoTrackingShipmentSearches_Tenant_ShipmentDate_SearchFields] ON [dbo].[CargoTrackingShipmentSearches]([Tenant],[ShipmentDate],[SearchFields])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('5555861f-fdb4-45b2-b8dd-6cfc40ad6ab0', 'CargoTrackingShipmentSearch.dxml', 'CargoTrackingShipmentSearches', 'Tenant,ShipmentDate,SearchFields', 'Create Index', GETDATE(), '-- Create Index On CargoTrackingShipmentSearches TableEXEC(''CREATE NONCLUSTERED INDEX [IX_CargoTrackingShipmentSearches_Tenant_ShipmentDate_SearchFields] ON [dbo].[CargoTrackingShipmentSearches]([Tenant],[ShipmentDate],[SearchFields])'');');


-- Add Foreign Key Constraint For Column EntityType In Table CargoTrackingShipments As Reference To Column Code In Table CargoTrackingHeaderEntityTypes
EXEC('ALTER TABLE [dbo].[CargoTrackingShipments] ADD CONSTRAINT [FK_CargoTrackingShipments_CargoTrackingHeaderEntityTypes_EntityType] FOREIGN KEY([EntityType]) REFERENCES [dbo].[CargoTrackingHeaderEntityTypes]([Code])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('4300cadc-9141-4376-b39d-2b6d6ba8f997', 'CargoTrackingShipment.dxml', 'CargoTrackingShipments', 'EntityType', 'Create Relation', GETDATE(), '-- Add Foreign Key Constraint For Column EntityType In Table CargoTrackingShipments As Reference To Column Code In Table CargoTrackingHeaderEntityTypesEXEC(''ALTER TABLE [dbo].[CargoTrackingShipments] ADD CONSTRAINT [FK_CargoTrackingShipments_CargoTrackingHeaderEntityTypes_EntityType] FOREIGN KEY([EntityType]) REFERENCES [dbo].[CargoTrackingHeaderEntityTypes]([Code])'');');

-- Create Index On CargoTrackingShipments Table
EXEC('CREATE NONCLUSTERED INDEX [IX_CargoTrackingShipments_EntityType] ON [dbo].[CargoTrackingShipments]([EntityType])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('c9446756-ad75-4a6b-a58e-30e317e6d916', 'CargoTrackingShipment.dxml', 'CargoTrackingShipments', 'EntityType', 'Create Index', GETDATE(), '-- Create Index On CargoTrackingShipments TableEXEC(''CREATE NONCLUSTERED INDEX [IX_CargoTrackingShipments_EntityType] ON [dbo].[CargoTrackingShipments]([EntityType])'');');

-- Add Foreign Key Constraint For Column CurrentMilestoneCode In Table CargoTrackingShipments As Reference To Column Code In Table CargoTrackingMilestones
EXEC('ALTER TABLE [dbo].[CargoTrackingShipments] ADD CONSTRAINT [FK_CargoTrackingShipments_CargoTrackingMilestones_CurrentMilestoneCode] FOREIGN KEY([CurrentMilestoneCode]) REFERENCES [dbo].[CargoTrackingMilestones]([Code])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('de2d65da-368c-471d-9b8c-48ef1b95133f', 'CargoTrackingShipment.dxml', 'CargoTrackingShipments', 'CurrentMilestoneCode', 'Create Relation', GETDATE(), '-- Add Foreign Key Constraint For Column CurrentMilestoneCode In Table CargoTrackingShipments As Reference To Column Code In Table CargoTrackingMilestonesEXEC(''ALTER TABLE [dbo].[CargoTrackingShipments] ADD CONSTRAINT [FK_CargoTrackingShipments_CargoTrackingMilestones_CurrentMilestoneCode] FOREIGN KEY([CurrentMilestoneCode]) REFERENCES [dbo].[CargoTrackingMilestones]([Code])'');');

-- Create Index On CargoTrackingShipments Table
EXEC('CREATE NONCLUSTERED INDEX [IX_CargoTrackingShipments_CurrentMilestoneCode] ON [dbo].[CargoTrackingShipments]([CurrentMilestoneCode])');

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('507f6328-9a3a-401c-976c-579b5b32495c', 'CargoTrackingShipment.dxml', 'CargoTrackingShipments', 'CurrentMilestoneCode', 'Create Index', GETDATE(), '-- Create Index On CargoTrackingShipments TableEXEC(''CREATE NONCLUSTERED INDEX [IX_CargoTrackingShipments_CurrentMilestoneCode] ON [dbo].[CargoTrackingShipments]([CurrentMilestoneCode])'');');


