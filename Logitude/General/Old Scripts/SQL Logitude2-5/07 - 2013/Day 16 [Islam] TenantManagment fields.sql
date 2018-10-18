-- do not run online
-- run on global database
alter table TenantManagements add TTY varchar(15) null

alter table TenantManagements add LastFWBSentDate datetime null
alter table TenantManagements add LastFHLSentDate datetime null
alter table TenantManagements add StatisticsUpdateDate datetime null


alter table TenantManagements add ShipmentLastDate datetime null
alter table TenantManagements add ShipmentTotalLastWeek int not null default(0)
alter table TenantManagements add ShipmentTotalLastMonth int not null default(0)

alter table TenantManagements add QuoteLastDate datetime null
alter table TenantManagements add QuoteTotalLastWeek int not null default(0)
alter table TenantManagements add QuoteTotalLastMonth int not null default(0)

alter table TenantManagements add ARInvoiceLastDate datetime null
alter table TenantManagements add ARInvoiceTotalLastWeek int not null default(0)
alter table TenantManagements add ARInvoiceTotalLastMonth int  not null default(0)


alter table TenantManagements add APInvoiceLastDate datetime null
alter table TenantManagements add APInvoiceTotalLastWeek int not null default(0)
alter table TenantManagements add APInvoiceTotalLastMonth int not null default(0)


alter table TenantManagements add CustomerLastDate datetime null
alter table TenantManagements add CustomerTotalLastWeek int not null default(0)
alter table TenantManagements add CustomerTotalLastMonth int not null default(0)
 
 