create table [dbo].[SmallDocuments] (
    [Id] [varchar](40) not null,
    [Tenant] [int] not null,
    [Content] [nvarchar](max) not null,
    primary key ([Id])
);
go

alter table Documents add SmallDocumentId varchar(40) null 
go
alter table [dbo].[Documents] add constraint [Document_SmallDocument] foreign key ([SmallDocumentId]) references [dbo].[SmallDocuments]([Id]);
go
create table [dbo].[CommunicationLogSteps] (
    [StepNumber] [int] not null,
    [CommunicationLogId] [varchar](15) not null,
    [Tenant] [int] not null,
    [Name] [varchar](100) not null,
    [Retries] [int] not null,
    [ParamIn1] [varchar](1000) null,
    [ParamOut1] [varchar](1000) null,
    [Status] [varchar](4) not null,
    [Log] [nvarchar](max) null,
    [StartDate] [datetime] not null,
    [EndDate] [datetime] not null,
    primary key ([StepNumber], [CommunicationLogId])
);
go
alter table [dbo].[CommunicationLogSteps] add constraint [CommunicationLogStep_CommunicationLog] foreign key ([CommunicationLogId]) references [dbo].[CommunicationLogs]([Id]) 
go
alter table [dbo].[CommunicationLogSteps] add constraint [CommunicationLogStep_CommunicationStatusType] foreign key ([Status]) references [dbo].[CommunicationStatusTypes]([Code])
go