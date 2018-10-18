
declare @Tenant as int
declare @Email as varchar(100)
declare @todayDate as date
declare @LoggedUserId as varchar(15)

set @Tenant = 1
set @Email = 'angular@fnarsoft.com'
set @todayDate = (SELECT DATEADD(DAY, DATEDIFF(DAY, 0, GETDATE()), 0))
set @LoggedUserId = (select top 1 Id from Contacts where Tenant = @Tenant and Email = @Email)

if (@LoggedUserId is null)
begin
	print 'This Email does not exists'
end

else
BEGIN
declare @QuoteQueries_CreatedQuotes as int
declare @QuoteQueries_DraftQuotes as int
declare @QuoteQueries_SentQuotes as int
declare @QuoteQueries_AcceptedWithoutShipments as int
declare @QuoteQueries_AcceptedQuotes as int
declare @QuoteQueries_CancelledQuotes as int
declare @QuoteQueries_MyQuotes as int
declare @Others_ExpiredQuotes as int
declare @Others_AllFollowups as int
declare @Others_MyFollowups as int
declare @Others_AllQuotes as int

-------------------------------------------------
set @QuoteQueries_CreatedQuotes = (select count(*)
from Quotes join QuoteStages on Quotes.StageId = QuoteStages.Id
where Quotes.Tenant = @Tenant
and Quotes.IsCancelled = 0
and QuoteStages.Code = 'QTCR')
-------------------------------------------------
set @QuoteQueries_DraftQuotes = (select count(*)
from Quotes join QuoteStages on Quotes.StageId = QuoteStages.Id
where Quotes.Tenant = @Tenant
and Quotes.IsCancelled = 0
and QuoteStages.Code = 'QTDR')
-------------------------------------------------
set @QuoteQueries_SentQuotes = (select count(*)
from Quotes join QuoteStages on Quotes.StageId = QuoteStages.Id
where Quotes.Tenant = @Tenant
and Quotes.IsCancelled = 0
and QuoteStages.Code = 'QTST')
-------------------------------------------------
set @QuoteQueries_AcceptedWithoutShipments = (select count(*)
from Quotes join QuoteStages on Quotes.StageId = QuoteStages.Id
where Quotes.Tenant = @Tenant
and Quotes.IsCancelled = 0
and QuoteStages.Code = 'QTAC'
and (Quotes.UsageCount is null OR Quotes.UsageCount = 0))
-------------------------------------------------
set @QuoteQueries_AcceptedQuotes = (select count(*)
from Quotes join QuoteStages on Quotes.StageId = QuoteStages.Id
where Quotes.Tenant = @Tenant
and Quotes.IsCancelled = 0
and QuoteStages.Code = 'QTAC')
-------------------------------------------------
set @QuoteQueries_CancelledQuotes = (select count(*)
from Quotes
where Tenant = @Tenant
and IsCancelled = 1)
-------------------------------------------------
set @QuoteQueries_MyQuotes = (select count(*)
from Quotes
where Tenant = @Tenant
and IsCancelled = 0
and SalesmanUserId = @LoggedUserId)
------------------------------------------------
set @Others_ExpiredQuotes = (select count(*)
from Quotes
where Tenant = @Tenant
and IsClosed = 0
and IsCancelled = 0
and ExpirationDate is not null
and CAST(ExpirationDate AS DATE) <= @todayDate)
------------------------------------------------
set @Others_AllFollowups = (select count(*)
from FollowUps join Quotes on FollowUps.QuoteId = Quotes.Id
where
FollowUps.Tenant = 1
and FollowUps.QuoteId is not null
and Quotes.IsCancelled = 0)
-------------------------------------------------
set @Others_MyFollowups = (select count(*)
from FollowUps join Quotes on FollowUps.QuoteId = Quotes.Id
where
FollowUps.Tenant = 1
and FollowUps.QuoteId is not null
and Quotes.IsCancelled = 0
and FollowUps.OwnerUserId = @LoggedUserId)
-------------------------------------------------
set @Others_AllQuotes = (select count(*)
from Quotes
where Tenant = @Tenant
and IsCancelled = 0)
-------------------------------------------------

print '[Quote Queries]'
print 'Created Quotes: ' + convert(varchar,@QuoteQueries_CreatedQuotes)
print 'Draft Quotes: ' + convert(varchar,@QuoteQueries_DraftQuotes)
print 'Sent Quotes: ' + convert(varchar,@QuoteQueries_SentQuotes)
print 'Accepted Without Shipments: ' + convert(varchar,@QuoteQueries_AcceptedWithoutShipments)
print 'Accepted Quotes: ' + convert(varchar,@QuoteQueries_AcceptedQuotes)
print 'Cancelled Quotes: ' + convert(varchar,@QuoteQueries_CancelledQuotes)
print 'My Quotes: ' + convert(varchar,@QuoteQueries_MyQuotes)
print ''
print '[Others]'
print 'Expired Quotes: ' + convert(varchar,@Others_ExpiredQuotes)
print 'All Followups: ' + convert(varchar,@Others_AllFollowups)
print 'My Followups: ' + convert(varchar,@Others_MyFollowups)
print 'All Quotes: ' + convert(varchar,@Others_AllQuotes)
END
