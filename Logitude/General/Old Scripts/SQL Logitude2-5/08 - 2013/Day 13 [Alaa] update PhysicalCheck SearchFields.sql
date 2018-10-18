Update [Customs].[PhysicalChecks]
set SearchFields = 
isnull(CargoIdentifierKey1,'') + ',' +
 isnull(CargoIdentifierKey2,'') + ',' +
  isnull(CargoIdentifierKey3,'') + ',' +
 isnull(CargoIdentifierTypeCode,'') + ',' + 
 
isnull((Select EnglishName from [Customs].[CargoIdentifireTypes] where [Customs].[CargoIdentifireTypes].Code = CargoIdentifierTypeCode), '')+ ','+

   isnull(CargoTypeCode,'') + ',' + 
   isnull(CheckId,'') + ',' + 
   isnull(CheckSiteCode,'') + ',' + 
  isnull((Select EnglishName from [Customs].[SiteLookups] where [Customs].[SiteLookups].Code = CheckSiteCode),'')+ ','+
   isnull(ContainerNubmer,'') + ',' + 
   
  isnull((select EnglishName  from [dbo].[Cards]  where  [dbo].[Cards].Id = (Select CustomerId from [Customs].[Declarations] where [Customs].[Declarations].Id = DeclarationId)), '') + ','+
  isnull((select Code  from [dbo].[Cards]  where  [dbo].[Cards].Id = (Select CustomerId from [Customs].[Declarations] where [Customs].[Declarations].Id = DeclarationId)), '') + ','+

  isnull((Select DeclarationNumber from [Customs].[Declarations] where [Customs].[Declarations].Id = DeclarationId), '')+ ','+
   isnull(ImporterNumber,'') + ',' + 
    isnull(InitiatorTypeCode,'') + ',' + 
   isnull(OperationCode,'') + ',' + 
 isnull((Select EnglishName from [Customs].[PhysicalCheckOperations] where [Customs].[PhysicalCheckOperations].Code = OperationCode), '')+ ','+
   isnull(QueueTypeCode,'') + ',' + 
 isnull((Select EnglishName from [Customs].[CheckQueueTypes] where [Customs].[CheckQueueTypes].Code = QueueTypeCode) , '')+ ','+
   isnull(RowNumber,'') + ',' + 
  isnull(StatusMessageCode,'') + ',' + 
   isnull(StorageSiteCode,'') + ',' + 
 isnull((Select EnglishName from [Customs].[SiteLookups] where [Customs].[SiteLookups].Code = StorageSiteCode), '')+ ','+

isnull(CheckEssence,'') + ','


select Searchfields from [Customs].[PhysicalChecks]
