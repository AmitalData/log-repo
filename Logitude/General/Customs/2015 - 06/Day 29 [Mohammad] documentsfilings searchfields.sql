  update DocumentsFilings
  set SearchFields = 
  isnull(DF.Code,'') + ',' +
 isnull(DF.DirectionCode,'') + ',' +
 isnull(DF.EntityReference,'') + ',' +
 isnull(DF.ExternalEntityReference,'') + ',' +
 isnull(CO.EnglishName,'') + ',' +
 isnull(CO.LocalName,'') + ',' +
 isnull(DT.Code,'') + ',' +
 ISNull(DT.Name, '') + ','

  FROM DocumentsFilings as DF ,Contacts as CO, DocumentTypes as DT
  Where  DF.OwnerId = CO.Id and DF.DocumentTypeId = DT.Id