
Update [Customs].[PhysicalChecks]
set SearchFields = 
isnull(DeclarationId,'') + ',' +
 (Select FileNo from [Customs].[Declarations] where [Customs].[Declarations].Id = DeclarationId)+ ','+
 ISNULL (ContainerNubmer, '')+ ',' 

