update Customs.Declarations  
set SearchFields  = 
isnull( DeclarationNumber ,'') + ',' +
 isnull(CustomFileNo ,'') + ','
 

 update d set SearchFields =

 isnull(SearchFields ,'') + ',' +
isnull(SecondCargoID ,'') + ',' +
isnull(ManifestNumber ,'') + ',' +
isnull(ThirdCargoID ,'') + ','

    
from Customs.Declarations as d,Customs.Consignments as c
Where c.DeclarationId = d.Id



 update d set SearchFields =

isnull(d.SearchFields ,'') + ','+
 isnull(cli.FullName,'') + ','
   
  
from Customs.Declarations as d , Customs.Clients as cli
Where    d.ImporterId = cli.Id





