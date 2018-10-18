update documents set FileName = null,Extension = null,HasFile = 0 where FileSize = 0 and id in (select documentid from DocumentsFilings where DirectionCode = 'I')

