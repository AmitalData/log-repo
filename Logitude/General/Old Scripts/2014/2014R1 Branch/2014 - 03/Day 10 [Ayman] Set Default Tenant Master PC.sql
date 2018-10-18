
update Tenants set MasterExportFreightPrepaidCollectId = 'P' where MasterExportFreightPrepaidCollectId is null
go

update Tenants set MasterExportOtherPrepaidCollectId = 'P' where MasterExportOtherPrepaidCollectId is null
go

update Tenants set MasterImportFreightPrepaidCollectId = 'P' where MasterImportFreightPrepaidCollectId is null
go

update Tenants set MasterImportOtherPrepaidCollectId = 'P' where MasterImportOtherPrepaidCollectId is null
go