



update ObjectTables
set LookUp2 = NULL, LookUp1 = 'LocalName'
where Name = 'Customs.MeasurmentUnit' OR Name = 'Customs.ModificationAndDiscountType'
go
