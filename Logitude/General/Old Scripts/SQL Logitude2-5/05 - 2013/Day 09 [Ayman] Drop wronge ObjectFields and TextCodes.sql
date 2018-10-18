
delete from ObjectFields where FieldName = 'AWBCustomsInfoCode'
go

delete from ObjectFields where FieldName = 'AWBCustomsInformationCode'
go

delete from TextCodes where Code like '%AWBOCI%AWBCustomsInfoCode%'
go

delete from TextCodes where Code like '%AWBOCI%AWBCustomsInformationCode%'
go