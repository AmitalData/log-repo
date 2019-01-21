delete from PackageFeatures where FeatureId In (Select Id from Features where Code like 'downloaddocuments')
delete from RoleFeatures where FeatureId In (Select Id from Features where Code like 'downloaddocuments')
delete from Features where NameTextCodeId In(Select Id from TextCodes where Code like '%Shipment.Features.DownloadDocuments%')
delete from TextCodes where Code like '%Shipment.Features.DownloadDocuments%'
delete from Features where Code like 'downloaddocuments'