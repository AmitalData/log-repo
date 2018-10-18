
delete from QueryColumns where QueryId = (select Id from Queries where Code = 'In Progress Quotes')
delete from AdvancedQueryFilters where QueryId = (select Id from Queries where Code = 'In Progress Quotes')

delete from QueryColumns where QueryId = (select Id from Queries where OriginalQueryId = ( select Id from Queries where Code = 'In Progress Quotes'))
delete from AdvancedQueryFilters where QueryId = (select Id from Queries where OriginalQueryId = ( select Id from Queries where Code = 'In Progress Quotes'))

delete from Queries where OriginalQueryId = (select Id from Queries where Code = 'In Progress Quotes')
delete from Queries where Code = 'In Progress Quotes'

delete from TextCodes where Code = 'Quote.Q.InProgressQuotes'
delete from TextCodes where Code = 'Quote.Features.InProgressQuotes'

delete from PackageFeatures where FeatureId = (select Id from Features where Code = 'INPROGRESSQUOTES')
delete from RoleFeatures where FeatureId = (select Id from Features where Code = 'INPROGRESSQUOTES')
delete from Features where Code = 'INPROGRESSQUOTES'