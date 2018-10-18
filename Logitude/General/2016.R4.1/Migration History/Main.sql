ALTER TABLE [dbo].[ObjectTables] ADD [MenuHeaderViewPath] [varchar](250)
ALTER TABLE [dbo].[ObjectTables] ADD [MenuHeaderViewName] [varchar](250)

ALTER TABLE [dbo].[ObjectTables] ADD [NewWizardComponentName] [varchar](250)
ALTER TABLE [dbo].[ObjectTables] ADD [NewWizardComponentPath] [varchar](max)

ALTER TABLE [dbo].[ObjectTables] ADD [HasCustomFilter] [bit] NOT NULL DEFAULT 0

DECLARE @var0 nvarchar(128)
SELECT @var0 = name
FROM sys.default_constraints
WHERE parent_object_id = object_id(N'dbo.ObjectTables')
AND col_name(parent_object_id, parent_column_id) = 'MenuHeaderViewName';
IF @var0 IS NOT NULL
    EXECUTE('ALTER TABLE [dbo].[ObjectTables] DROP CONSTRAINT [' + @var0 + ']')
ALTER TABLE [dbo].[ObjectTables] DROP COLUMN [MenuHeaderViewName]
DECLARE @var1 nvarchar(128)
SELECT @var1 = name
FROM sys.default_constraints
WHERE parent_object_id = object_id(N'dbo.ObjectTables')
AND col_name(parent_object_id, parent_column_id) = 'NewWizardComponentName';
IF @var1 IS NOT NULL
    EXECUTE('ALTER TABLE [dbo].[ObjectTables] DROP CONSTRAINT [' + @var1 + ']')
ALTER TABLE [dbo].[ObjectTables] DROP COLUMN [NewWizardComponentName]



ALTER TABLE [dbo].[Automations] ADD [IsAutomationDone] [bit] NOT NULL DEFAULT 0

ALTER TABLE [dbo].[Airlines] ALTER COLUMN [TTY] [varchar](33) NULL
ALTER TABLE [dbo].[Participants] ALTER COLUMN [TTY] [varchar](33) NULL


--ALTER TABLE [dbo].[OceanInsightsRequests] ADD [Type] [varchar](15) NOT NULL DEFAULT ''
--ALTER TABLE [dbo].[OceanInsightsRequests] ADD [BLNumber] [varchar](15) NOT NULL DEFAULT ''

ALTER TABLE [dbo].[CommunicationLogs] ADD [ChildEntityId] [varchar](15)
ALTER TABLE [dbo].[CommunicationLogs] ADD [ChildObjectTableId] [varchar](15)


---------------------

ALTER TABLE [dbo].[ObjectTables] ALTER COLUMN [NewWizardComponentPath] [varchar](250) NULL
ALTER TABLE [dbo].[ObjectTables] ADD [HasHelperComponent] [bit] NOT NULL DEFAULT 0
ALTER TABLE [dbo].[ObjectFields] ADD [HasTemplateComponent] [bit] NOT NULL DEFAULT 0

ALTER TABLE [dbo].[ObjectTables] ADD [HasHelper] [bit] NOT NULL DEFAULT 0
ALTER TABLE [dbo].[ObjectTables] ADD [HasShortTitle] [bit] NOT NULL DEFAULT 0
ALTER TABLE [dbo].[ObjectTables] ADD [HasMenuButtons] [bit] NOT NULL DEFAULT 0
ALTER TABLE [dbo].[ObjectTables] ADD [HasFiltersMenu] [bit] NOT NULL DEFAULT 0
ALTER TABLE [dbo].[ObjectFields] ADD [HasTemplate] [bit] NOT NULL DEFAULT 0
DECLARE @var0 nvarchar(128)
SELECT @var0 = name
FROM sys.default_constraints
WHERE parent_object_id = object_id(N'dbo.ObjectTables')
AND col_name(parent_object_id, parent_column_id) = 'FilterMenuComponentPath';
IF @var0 IS NOT NULL
    EXECUTE('ALTER TABLE [dbo].[ObjectTables] DROP CONSTRAINT [' + @var0 + ']')
ALTER TABLE [dbo].[ObjectTables] DROP COLUMN [FilterMenuComponentPath]
DECLARE @var1 nvarchar(128)
SELECT @var1 = name
FROM sys.default_constraints
WHERE parent_object_id = object_id(N'dbo.ObjectTables')
AND col_name(parent_object_id, parent_column_id) = 'ShortTitleComponentPath';
IF @var1 IS NOT NULL
    EXECUTE('ALTER TABLE [dbo].[ObjectTables] DROP CONSTRAINT [' + @var1 + ']')
ALTER TABLE [dbo].[ObjectTables] DROP COLUMN [ShortTitleComponentPath]
DECLARE @var2 nvarchar(128)
SELECT @var2 = name
FROM sys.default_constraints
WHERE parent_object_id = object_id(N'dbo.ObjectTables')
AND col_name(parent_object_id, parent_column_id) = 'HasHelperComponent';
IF @var2 IS NOT NULL
    EXECUTE('ALTER TABLE [dbo].[ObjectTables] DROP CONSTRAINT [' + @var2 + ']')
ALTER TABLE [dbo].[ObjectTables] DROP COLUMN [HasHelperComponent]
DECLARE @var3 nvarchar(128)
SELECT @var3 = name
FROM sys.default_constraints
WHERE parent_object_id = object_id(N'dbo.ObjectTables')
AND col_name(parent_object_id, parent_column_id) = 'MenuButtonsComponentPath';
IF @var3 IS NOT NULL
    EXECUTE('ALTER TABLE [dbo].[ObjectTables] DROP CONSTRAINT [' + @var3 + ']')
ALTER TABLE [dbo].[ObjectTables] DROP COLUMN [MenuButtonsComponentPath]
DECLARE @var4 nvarchar(128)
SELECT @var4 = name
FROM sys.default_constraints
WHERE parent_object_id = object_id(N'dbo.ObjectFields')
AND col_name(parent_object_id, parent_column_id) = 'HasTemplateComponent';
IF @var4 IS NOT NULL
    EXECUTE('ALTER TABLE [dbo].[ObjectFields] DROP CONSTRAINT [' + @var4 + ']')
ALTER TABLE [dbo].[ObjectFields] DROP COLUMN [HasTemplateComponent]


ALTER TABLE [dbo].[Tenants] ADD [IsWebAccessActivated] [bit] NOT NULL DEFAULT 0


----------------- 28/9/2016

ALTER TABLE [dbo].[BlobFiles] DROP CONSTRAINT [PK_dbo.BlobFiles]
ALTER TABLE [dbo].[BlobFiles] ALTER COLUMN [Id] [varchar](60) NOT NULL
ALTER TABLE [dbo].[BlobFiles] ADD CONSTRAINT [PK_dbo.BlobFiles] PRIMARY KEY ([Id])

ALTER TABLE [dbo].[BlobFiles] DROP CONSTRAINT [PK_dbo.BlobFiles]
ALTER TABLE [dbo].[BlobFiles] ALTER COLUMN [Id] [varchar](20) NOT NULL
ALTER TABLE [dbo].[BlobFiles] ADD CONSTRAINT [PK_dbo.BlobFiles] PRIMARY KEY ([Id])


ALTER TABLE [dbo].[OceanInsightsRequests] ALTER COLUMN [BLNumber] [varchar](15) NULL

ALTER TABLE [dbo].[Correspondences] ADD [RightToLeft] [bit] NOT NULL DEFAULT 0

ALTER TABLE [dbo].[Tenants] ADD [IsRightToLeftEnabled] [bit] NOT NULL DEFAULT 0

ALTER TABLE [dbo].[Airlines] ADD [OldTTY] [varchar](33)
update Airlines set OldTTY = TTY

ALTER TABLE [dbo].[InsideShipmentPackages] ALTER COLUMN [Description] [varchar](700) NULL
ALTER TABLE [dbo].[ShipmentPackages] ALTER COLUMN [Description] [varchar](700) NULL

CREATE TABLE [dbo].[Category1] (
    [Id] [varchar](15) NOT NULL,
    [Tenant] [int] NOT NULL,
    [EnglishName] [varchar](max) NOT NULL,
    [LocalName] [varchar](max),
    [Inactive] [bit] NOT NULL,
    CONSTRAINT [PK_dbo.Category1] PRIMARY KEY ([Id])
)
CREATE TABLE [dbo].[Category2] (
    [Id] [varchar](15) NOT NULL,
    [Tenant] [int] NOT NULL,
    [EnglishName] [varchar](max) NOT NULL,
    [LocalName] [varchar](max),
    [Inactive] [bit] NOT NULL,
    CONSTRAINT [PK_dbo.Category2] PRIMARY KEY ([Id])
)
CREATE TABLE [dbo].[Category3] (
    [Id] [varchar](15) NOT NULL,
    [Tenant] [int] NOT NULL,
    [EnglishName] [varchar](max) NOT NULL,
    [LocalName] [varchar](max),
    [Inactive] [bit] NOT NULL,
    CONSTRAINT [PK_dbo.Category3] PRIMARY KEY ([Id])
)
CREATE TABLE [dbo].[Category4] (
    [Id] [varchar](15) NOT NULL,
    [Tenant] [int] NOT NULL,
    [EnglishName] [varchar](max) NOT NULL,
    [LocalName] [varchar](max),
    [Inactive] [bit] NOT NULL,
    CONSTRAINT [PK_dbo.Category4] PRIMARY KEY ([Id])
)
CREATE TABLE [dbo].[Category5] (
    [Id] [varchar](15) NOT NULL,
    [Tenant] [int] NOT NULL,
    [EnglishName] [varchar](max) NOT NULL,
    [LocalName] [varchar](max),
    [Inactive] [bit] NOT NULL,
    CONSTRAINT [PK_dbo.Category5] PRIMARY KEY ([Id])
)

ALTER TABLE [dbo].[Tenants] ADD [AccountingActivationDate] [datetime]
ALTER TABLE [dbo].[Tenants] ADD [AccountingActivated] [bit] NOT NULL DEFAULT 0

-----------

ALTER TABLE [dbo].[Shipments] ALTER COLUMN [ExceptionDescription] [nvarchar](500) NULL
ALTER TABLE [dbo].[Shipments] ALTER COLUMN [ExceptionResolvedDescription] [nvarchar](500) NULL


---

ALTER TABLE [dbo].[OceanInsightsRequests] ALTER COLUMN [BLNumber] [varchar](18) NULL




--6/11/2016
ALTER TABLE [dbo].[QuoteTemplateSettings] ADD [ShowChargeDescriptionPackages] [bit] NOT NULL DEFAULT 0
ALTER TABLE [dbo].[QuoteTemplateSettings] ADD [ShowChargeDescriptionContainers] [bit] NOT NULL DEFAULT 0

----16/11/2016

ALTER TABLE [dbo].[Shipments] ALTER COLUMN [MainHarmonize] [varchar](18) NULL

---24/11/2016

IF object_id(N'[dbo].[FK_dbo.QueueMessageMoreDetails_dbo.QueueMessages_Id]', N'F') IS NOT NULL
    ALTER TABLE [dbo].[QueueMessageMoreDetails] DROP CONSTRAINT [FK_dbo.QueueMessageMoreDetails_dbo.QueueMessages_Id]
IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'IX_Id' AND object_id = object_id(N'[dbo].[QueueMessageMoreDetails]', N'U'))
    DROP INDEX [IX_Id] ON [dbo].[QueueMessageMoreDetails]
ALTER TABLE [dbo].[QueueMessageMoreDetails] DROP CONSTRAINT [PK_dbo.QueueMessageMoreDetails]
ALTER TABLE [dbo].[QueueMessageMoreDetails] ALTER COLUMN [Id] [bigint] NOT NULL
ALTER TABLE [dbo].[QueueMessageMoreDetails] ADD CONSTRAINT [PK_dbo.QueueMessageMoreDetails] PRIMARY KEY ([Id])
