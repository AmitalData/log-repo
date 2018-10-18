CREATE TABLE [dbo].[Packages] (
    [Code]varchar(4)   NOT NULL,
    [Name]varchar(40)   NOT NULL,
    [SearchFields]nvarchar(1000)   NULL
);

CREATE TABLE [dbo].[PackageFeatures] (
    [Id] varchar(15) not null,
	[Tenant] int not null,
	[PackageCode] varchar(4) not null,
	[FeatureId] varchar(15) not null,
);

-- Creating primary key on [Code] in table 'Packages'
ALTER TABLE [dbo].[Packages]
ADD CONSTRAINT [PK_Packages]
    PRIMARY KEY CLUSTERED ([Code] ASC);
GO

-- Creating primary key on [Id] in table 'PackageFeatures'
ALTER TABLE [dbo].[PackageFeatures]
ADD CONSTRAINT [PK_PackageFeatures]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating foreign key on [PackageCode] in table 'PackageFeatures'
ALTER TABLE [dbo].[PackageFeatures]
ADD CONSTRAINT [FK_PackageFeaturePackage]
    FOREIGN KEY ([PackageCode])
    REFERENCES [dbo].[Packages]
        ([Code])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_PackageFeaturePackage'
CREATE INDEX [IX_FK_PackageFeaturePackage]
ON [dbo].[PackageFeatures]
    ([PackageCode]);
GO

-- Creating foreign key on [FeatureId] in table 'PackageFeatures'
ALTER TABLE [dbo].[PackageFeatures]
ADD CONSTRAINT [FK_PackageFeatureFeature]
    FOREIGN KEY ([FeatureId])
    REFERENCES [dbo].[Features]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_PackageFeatureFeature'
CREATE INDEX [IX_FK_PackageFeatureFeature]
ON [dbo].[PackageFeatures]
    ([FeatureId]);
GO