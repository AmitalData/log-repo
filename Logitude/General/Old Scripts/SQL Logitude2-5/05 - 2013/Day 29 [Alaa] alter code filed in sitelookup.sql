-- with errors

--alter table [Customs].[SiteLookups] drop PK__Sites__A25C5AA65C229E14
--go

alter table [Customs].[SiteLookups] alter column code [varchar] (17) not null
go

-- Creating primary key
--ALTER TABLE [Customs].[SiteLookups]
--ADD CONSTRAINT [PK_SiteLookups]
--    PRIMARY KEY CLUSTERED ([Code] ASC);
--GO

