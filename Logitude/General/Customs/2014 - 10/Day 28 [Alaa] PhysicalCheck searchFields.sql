Update p
set SearchFields = 
isnull( P.CheckId ,'') + ',' +
isnull( P.ContainerNubmer ,'') + ',' +
isnull( d.DeclarationNumber ,'') + ',' +
isnull( d.CustomFileNo ,'') + ',' 

from Customs.PhysicalChecks as p , Customs.Declarations as d
where p.DeclarationId = d.Id


