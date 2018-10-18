
declare @Tenant as int
declare @Email as varchar(100)
declare @LoggedUserId as varchar(15)
declare @BranchId as varchar(15)
set @Tenant = 1
set @Email = 'angular@fnarsoft.com'
set @LoggedUserId = (select top 1 Id from Contacts where Tenant = @Tenant and Email = @Email)
set @BranchId = '1-19'	-- Office 2

declare @OpenActivities_My as int
declare @OpenActivities_All as int
declare @ClosedActivities_My as int
declare @ClosedActivities_All as int
declare @ClosedActivities_MeetingSummary as int
declare @Others_All as int
declare @Others_Cancelled as int

if (@BranchId is null)
BEGIN
set @OpenActivities_My = (select count(*)
from Activities
where Tenant = @Tenant
and ActivityStatusCode != 'X'
and IsOpen = 1
and OwnerId = @LoggedUserId)
-------------------------------------------------
set @OpenActivities_All = (select count(*)
from Activities
where Tenant = @Tenant
and ActivityStatusCode != 'X'
and IsOpen = 1)
-------------------------------------------------
-------------------------------------------------
-------------------------------------------------
set @ClosedActivities_My = (select count(*)
from Activities
where Tenant = @Tenant
and ActivityStatusCode != 'X'
and IsOpen = 0
and OwnerId = @LoggedUserId)
-------------------------------------------------
set @ClosedActivities_All = (select count(*)
from Activities
where Tenant = @Tenant
and ActivityStatusCode != 'X'
and IsOpen = 0)
-------------------------------------------------
set @ClosedActivities_MeetingSummary = (select count(*)
from Activities
where Tenant = @Tenant
and ActivityStatusCode != 'X'
and IsOpen = 0
and ActivityTypeCode = 'AP')
-------------------------------------------------
-------------------------------------------------
-------------------------------------------------
set @Others_All = (select count(*)
from Activities
where Tenant = @Tenant
and ActivityStatusCode != 'X')
-------------------------------------------------
set @Others_Cancelled = (select count(*)
from Activities
where Tenant = @Tenant
and ActivityStatusCode = 'X')
END

else
BEGIN
set @OpenActivities_My = (select count(*)
from Activities
where Tenant = @Tenant
and ActivityStatusCode != 'X'
and IsOpen = 1
and OwnerId = @LoggedUserId
and BranchId = @BranchId)
-------------------------------------------------
set @OpenActivities_All = (select count(*)
from Activities
where Tenant = @Tenant
and ActivityStatusCode != 'X'
and IsOpen = 1
and BranchId = @BranchId)
-------------------------------------------------
-------------------------------------------------
-------------------------------------------------
set @ClosedActivities_My = (select count(*)
from Activities
where Tenant = @Tenant
and ActivityStatusCode != 'X'
and IsOpen = 0
and OwnerId = @LoggedUserId
and BranchId = @BranchId)
-------------------------------------------------
set @ClosedActivities_All = (select count(*)
from Activities
where Tenant = @Tenant
and ActivityStatusCode != 'X'
and IsOpen = 0
and BranchId = @BranchId)
-------------------------------------------------
set @ClosedActivities_MeetingSummary = (select count(*)
from Activities
where Tenant = @Tenant
and ActivityStatusCode != 'X'
and IsOpen = 0
and ActivityTypeCode = 'AP'
and BranchId = @BranchId)
-------------------------------------------------
-------------------------------------------------
-------------------------------------------------
set @Others_All = (select count(*)
from Activities
where Tenant = @Tenant
and ActivityStatusCode != 'X'
and BranchId = @BranchId)
-------------------------------------------------
set @Others_Cancelled = (select count(*)
from Activities
where Tenant = @Tenant
and ActivityStatusCode = 'X'
and BranchId = @BranchId)
END

print '[Open Activities]'
print 'My: ' + convert(varchar,@OpenActivities_My)
print 'All: ' + convert(varchar,@OpenActivities_All)
print ''

print '[Closed Activities]'
print 'My: ' + convert(varchar,@ClosedActivities_My)
print 'All: ' + convert(varchar,@ClosedActivities_All)
print 'Meeting Summary: ' + convert(varchar,@ClosedActivities_MeetingSummary)
print ''

print '[Others]'
print 'All: ' + convert(varchar,@Others_All)
print 'Cancelled: ' + convert(varchar,@Others_Cancelled)
print ''