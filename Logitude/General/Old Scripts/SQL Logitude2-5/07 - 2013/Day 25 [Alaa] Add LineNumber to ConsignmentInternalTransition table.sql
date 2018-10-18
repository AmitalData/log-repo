alter table [Customs].[ConsignmentInternalTransitions] drop constraint PK__Consignm__B8B35B040ECE1972
GO
alter table  [Customs].[ConsignmentInternalTransitions] add LineNumber int not null
GO
alter table [Customs].[ConsignmentInternalTransitions] alter column siteCode varchar(17) null
GO
alter table  [Customs].[ConsignmentInternalTransitions] add constraint pk_ConsignmentInternal primary key (DeclarationId,ConsignmentNumber,LineNumber)
GO