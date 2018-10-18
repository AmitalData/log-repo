
-- Run this Script
-- Update Tenant
-- Check Quote Features Again

delete from MenuButtons where FeatureId in (select Id from Features where ObjectTableId = (select Id from ObjectTables where Name = 'Quote'))
go

delete from RoleFeatures where FeatureId in (select Id from Features where ObjectTableId = (select Id from ObjectTables where Name = 'Quote'))
go

delete from Features where Code = 'QUOTEACCEPTED'
go

delete from Features where Code = 'QUOTEDECLINED'
go

delete from TextCodes where Code = 'Quote.Features.SetAsApproved'
go