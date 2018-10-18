begin transaction
begin

create table [Customs].[DeclarationConstraints] (
    [DeclarationID] [varchar](15) not null,
    [ConstraintNumber] [varchar](9) not null,
    [Tenant] [int] not null,
    [ConstraintTypeCode] [varchar](3) null,
    [ConstraintStatusCode] [varchar](3) null,
    [AgentExplanation] [varchar](512) null,
    [ApprovalNote] [varchar](512) null,
    [ApprovalAuthorityDate] [datetime] null,
    [ApprovalUserName] [varchar](256) null,
    [ApprovalDecision] [varchar](2) null,
    primary key ([DeclarationID], [ConstraintNumber])
); 


create table [Customs].[ConstraintTypes] (
    [Code] [varchar](3) not null,
    [LocalName] [nvarchar](100) null,
    [EnglishName] [varchar](100) null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([Code])
);


create table [Customs].[ConstraintStatuses] (
    [Code] [varchar](3) not null,
    [LocalName] [nvarchar](100) null,
    [EnglishName] [varchar](100) null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([Code])
);

alter table [Customs].[DeclarationConstraints] add constraint [DeclarationConstraint_ConstraintType] foreign key ([ConstraintTypeCode]) references [Customs].[ConstraintTypes]([Code]);

alter table [Customs].[DeclarationConstraints] add constraint [DeclarationConstraint_ConstraintStatus] foreign key ([ConstraintStatusCode]) references [Customs].[ConstraintStatuses]([Code]);


END
commit transaction