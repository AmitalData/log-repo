-- Create New Table With Name CargoTrackingShipmentMasters
CREATE TABLE [dbo].[CargoTrackingShipmentMasters](
[Id] VARCHAR(15) NOT NULL,
[Master] VARCHAR(20) NULL,
CONSTRAINT [PK_CargoTrackingShipmentMasters] PRIMARY KEY([Id])
);

INSERT INTO [dbo].[DBMigrationsHistory]([Id], [DxmlFileName], [TableName], [ColumnName], [MigrationType], [ExecutionDate], [MigrationScript])VALUES('cf6a2d18-b72b-4812-9e62-316b84b1c279', 'CargoTrackingShipmentMaster.dxml', 'CargoTrackingShipmentMasters', NULL, 'Create Table', GETDATE(), '-- Create New Table With Name CargoTrackingShipmentMastersCREATE TABLE [dbo].[CargoTrackingShipmentMasters]([Id] VARCHAR(15) NOT NULL,[Master] VARCHAR(20) NULL,CONSTRAINT [PK_CargoTrackingShipmentMasters] PRIMARY KEY([Id]));');


