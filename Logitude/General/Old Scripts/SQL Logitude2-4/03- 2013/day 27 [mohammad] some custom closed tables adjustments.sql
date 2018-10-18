-- errors when run it online 

alter table Customs.CargoIdentifireTypes alter column LocalName nvarchar(40) null
go

alter table Customs.CargoIdentifireTypes alter column EnglishName varchar(40) null
go

alter table Customs.CheckEntityTypes alter column LocalName nvarchar(40) null
go

alter table Customs.CheckEntityTypes alter column EnglishName varchar(40) null
go

alter table Customs.CheckQueueTypes alter column LocalName nvarchar(40) null
go

alter table Customs.CheckQueueTypes alter column EnglishName varchar(40) null
go

alter table Customs.CheckRepresentativeTypes alter column LocalName nvarchar(40) null
go

alter table Customs.CheckRepresentativeTypes alter column EnglishName varchar(40) null
go

alter table Customs.PhysicalCheckOperations alter column LocalName nvarchar(40) null
go

alter table Customs.PhysicalCheckOperations alter column EnglishName varchar(40) null
go

alter table Customs.PhysicalCheckStatusMessages alter column LocalName nvarchar(40) null
go

alter table Customs.PhysicalCheckStatusMessages alter column EnglishName varchar(40) null
go

alter table Customs.Sites alter column LocalName nvarchar(100) null
go

alter table Customs.Sites alter column EnglishName varchar(100) null
go

alter table Customs.PhysicalChecks drop [PhysicalCheck_CheckSite]
go

alter table Customs.PhysicalChecks drop [PhysicalCheck_StorageSite] 
go

alter table Customs.PhysicalChecks alter column StorageSiteCode varchar(40) null
go

alter table Customs.PhysicalChecks alter column CheckSiteCode varchar(40) null
go

alter table Customs.Sites alter column Code varchar(40) not null
go

alter table [Customs].[PhysicalChecks] add constraint [PhysicalCheck_CheckSite] foreign key ([CheckSiteCode]) references [Customs].[Sites]([Code]);

alter table [Customs].[PhysicalChecks] add constraint [PhysicalCheck_StorageSite] foreign key ([StorageSiteCode]) references [Customs].[Sites]([Code]);