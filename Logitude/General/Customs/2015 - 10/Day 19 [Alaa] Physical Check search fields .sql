Update p
set SearchFields = 
isnull( P.CheckId ,'') + ',' +
isnull( P.ContainerNubmer ,'') + ',' +
isnull( P.CargoIdentifierKey1 ,'') + ',' +
isnull( P.CargoIdentifierKey2 ,'') + ',' +
isnull( P.CargoIdentifierKey3 ,'') + ',' +

isnull( d.DeclarationNumber ,'') + ',' +
isnull( d.CustomFileNo ,'') + ',' 

from Customs.PhysicalChecks as p , Customs.Declarations as d
where p.DeclarationId = d.Id
