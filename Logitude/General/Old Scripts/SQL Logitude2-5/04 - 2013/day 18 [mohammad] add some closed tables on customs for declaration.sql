-- use main db

create table [Customs].[GovernmentProcedureTypes] (
    [Code] [varchar](7) not null,
    [EnglishName] [varchar](60) null,
    [LocalName] [nvarchar](60) null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([Code]))