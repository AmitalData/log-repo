-- Hello from Logitudoo //
------------------------------------------------------------------------------------------------
---- Shipment join Master join cards -----------------------------------------------------------
------------------------------------------------------------------------------------------------
select te.id,te.company,sh.ShipmentNumber,ca.EnglishName, ma.MasterShipmentNumber,ma.MainCarriageCarrierNumber from shipments as sh, ShipmentMasterDatas as ma,cards as ca,tenants as te
where sh.tenant = te.id and ma.MainCarriageCarrierId = ca.id and ma.id = sh.id
------------------------------------------------------------------------------------------------
-- Modify document template   
------------------------------------------------------------------------------------------------
DECLARE @Id AS varchar(15)
DECLARE @Tenant AS INT 
DECLARE @TemplateBody AS varbinary(max)  
DECLARE @TemplateBody_varchar AS varchar(max)

BEGIN; 
	DECLARE DocumentTypeTemplates_Cursor CURSOR READ_ONLY
	FOR	
	SELECT Id,Tenant,TemplateBody
	FROM DocumentTypeTemplates 
	OPEN DocumentTypeTemplates_Cursor FETCH NEXT FROM DocumentTypeTemplates_Cursor INTO @Id,@Tenant,@TemplateBody
	WHILE @@FETCH_STATUS = 0
	BEGIN

set @TemplateBody_varchar = (select convert(varchar(max),TemplateBody,0) from DocumentTypeTemplates where id =@Id)
--print @TemplateBody_varchar
--set @TemplateBody_varchar = replace(@TemplateBody_varchar,'[FromPortId.EnglishName]','[FromLocation]')
--set @TemplateBody_varchar = replace(@TemplateBody_varchar,'[ToPortId.EnglishName]','[ToLocation]')
--set @TemplateBody_varchar = replace(@TemplateBody_varchar,'[MainCarriageFromPortId.EnglishName]','[FromLocation]')
--set @TemplateBody_varchar = replace(@TemplateBody_varchar,'[MainCarriageToPortId.EnglishName]','[ToLocation]')
--set @TemplateBody_varchar = replace(@TemplateBody_varchar,'[MainCarriageCarrierNumber]','[MainCarriageFullCarrierNumber]')

--set @TemplateBody_varchar = replace(@TemplateBody_varchar,'[Transshipment1carriernumber]','[Transshipment1fullcarriernumber]')
--set @TemplateBody_varchar = replace(@TemplateBody_varchar,'[Transshipment2carriernumber]','[Transshipment2fullcarriernumber]')
--set @TemplateBody_varchar = replace(@TemplateBody_varchar,'[Transshipment3carriernumber]','[Transshipment3fullcarriernumber]')

set @TemplateBody_varchar = replace(@TemplateBody_varchar,'[Transshipment1fullcarriernumber]','[Transshipment1FullCarrierNumber]')
set @TemplateBody_varchar = replace(@TemplateBody_varchar,'[Transshipment2fullcarriernumber]','[Transshipment2FullCarrierNumber]')
set @TemplateBody_varchar = replace(@TemplateBody_varchar,'[Transshipment3fullcarriernumber]','[Transshipment3FullCarrierNumber]')

set @TemplateBody = convert(varbinary(max),@TemplateBody_varchar,0)
--print @TemplateBody_varchar

update DocumentTypeTemplates
set TemplateBody = @TemplateBody
where id = @Id

	FETCH NEXT FROM DocumentTypeTemplates_Cursor INTO @Id,@Tenant,@TemplateBody
	END
	CLOSE DocumentTypeTemplates_Cursor
	DEALLOCATE DocumentTypeTemplates_Cursor
END
------------------------------------------------------------------------------------------------
-- check counter states
------------------------------------------------------------------------------------------------
select co.Tenant,co.Code,co.Name,ot.name,cs.prefix,cs.lastvalue,cd.parameter1,cd.parameter2,cd.prefix,cd.uniqueperprefix,cd.startnumber from counters as co,counterstats as cs,counterdefinitions as cd,objecttables as ot
where co.tenant = 1 and cs.counterid = co.id and cd.counterid = co.id and co.objecttableid = ot.id
------------------------------------------------------------------------------------------------
-- Update Version and Isactive
------------------------------------------------------------------------------------------------
select * from globaltenants 
where version <> -1 and isactive = 1

update globaltenants 
set version = 8 , isactive = 0
where version <> -1 and isactive = 1 and (id <> 0 and id <> 1 and id <> 190 and id <> 188 and id <> 42)


select * from globaltenants 
where version <> -1 and isactive = 1 and (id = 0 or id = 1 or id = 190 or id = 188 or id = 42)


update globaltenants 
set version = 7 , isactive = 1
where version <> -1 and isactive = 0 and (id <> 0 and id <> 1 and id <> 190 and id <> 188 and id <> 42)

update globaltenants 
set ver
where version = 8
------------------------------------------------------------------------------------------------
-- 
------------------------------------------------------------------------------------------------
update queries set originalqueryid=NULL
delete from querycolumns where queryid in (select id from queries where tenant=0)
delete from Advancedqueryfilters where queryid in (select id from queries where tenant=0)
delete from queries where tenant=0


delete from screenfields where tenant = 0
delete from MenuButtons
delete from menustables

-------------------------------------------------------------------------------------------------
--
-------------------------------------------------------------------------------------------------
declare @string as varchar(max)

set @string = convert(varchar(max),0x3C3F786D6C2076657273696F6E3D22312E302220656E636F64696E673D225554462D3822207374616E64616C6F6E653D22796573223F3E3C456E76656C6F706520526563697069656E743D2255534342325853222053656E6465723D22514946464D58532220786D6C6E733D22687474703A2F2F7777772E6368616D702E6165726F2F474343532F436172676F584D4C223E3C546578744D6573736167653E3C5374616E646172644D6573736167654964656E74696669636174696F6E204D6573736167655479706556657273696F6E4E756D6265723D223122205374616E646172644D6573736167654964656E7469666965723D22464654222F3E3C546578743E3935302D31373031313330314652414C48522F54324B3535205243462F58533937302F31324A414E303835362F4C48522F5432204445502F5853323937352F31344A414E2F4652414C48522F5432203C2F546578743E3C2F546578744D6573736167653E3C2F456E76656C6F70653E,0)

print @string


-------------------------------------------------------------------------------------------------
--
-------------------------------------------------------------------------------------------------

select * from features

select * from roles

select * from RoleFeatures

select fe.code,fety.name as FeatureType,fety.code,fe.packagable,ro.code,ro.Name as Role,ob.name as objecttable,txt.defaulttext as DefaultText from features as fe, RoleFeatures as rofe, roles as ro,objecttables as ob, textcodes as txt,FeatureTypes as fety
where rofe.roleid = ro.id and rofe.featureid = fe.id and fe.objecttableid = ob.id and fe.nametextcodeid = txt.id and fe.featuretypecode = fety.code
order by FeatureType
-------------------------------------------------------------------------------------------------
--
-------------------------------------------------------------------------------------------------

select * from ObjectFields
where FieldName = 'Searchfields' and ObjectTableId = (select id from ObjectTables where Name = 'ErrorLog')

update ObjectFields
set MaxLength = 8000
where FieldName = 'Searchfields' and ObjectTableId = (select id from ObjectTables where Name = 'ErrorLog')

----------------------------------------
select * from ContactActivityLogs
order by GMTLogDateTime desc
----------------------------------------
select top 100 * from UserLoginLogs
order by GMTDateTime desc 
----------------------------------------
select top 100 contacts.EnglishName,UserLoginLogs.* from UserLoginLogs,Contacts
where IP = '82.213.2.230' and GMTDateTime > '2015-03-11 12:00' and contacts.Id = UserId
order by GMTDateTime desc 
---------------------------------------------

select gc.*,cp.* from globalcontacts as gc, ContactPasswords as cp
where  gc.GlobalTenantId = 1 and gc.Email = cp.Email and cp.email = '3bod@mail.com'

----------------------------------------
select top 1000 * from PerformanceLogs
order by LogDateTimeGMT desc
----------------------------------------

update Contacts
set DontShowLocalLabels = 1
where email = 'jalal@logitudeworld.com'

update Contacts
set DontShowLocalLabels = 1
where email = 'jalal@mail.com' and Tenant =  1
----------------------------------------------------------
select 
	  sum(reserved_page_count) * 8.0 / 1024 
from 
	  sys.dm_db_partition_stats 
GO

select 
	  sys.objects.name, sum(reserved_page_count) * 8.0 / 1024 
from 
	  sys.dm_db_partition_stats, sys.objects 
where 
	  sys.dm_db_partition_stats.object_id = sys.objects.object_id

group by sys.objects.name

select UserName,Exception,COUNT(Exception) from ErrorLogs
group by Exception,UserName
HAVING COUNT(Exception) > 1000
order by COUNT(Exception) desc

select COUNT(*) from ErrorLogs
where Exception = '   at System.ServiceModel.DomainServices.Client.DomainContext.<>c__DisplayClass38.<InvokeOperation>b__34(Object )'
--order by LogDate desc
select COUNT(*) from ErrorLogs
where Exception = '   at System.ServiceModel.DomainServices.Client.DomainContext.<>c__DisplayClass1b.<Load>b__17(Object )'

select COUNT(*) from ErrorLogs
where Exception = 'Debugging resource strings are unavailable. Often the key and arguments provide sufficient information to diagnose the problem. See http://go.microsoft.com/fwlink/?linkid=106663&Version=5.1.20125.0&File=System.Windows.dll&Key=HttpWebRequest_WebException_RemoteServer)     at System.ServiceModel.DomainServices.Client.OperationBase.Complete(Exception error).   at System.ServiceModel.DomainServices.Client.InvokeOperation.Complete(Exception error).   at System.ServiceModel.DomainServices.Client.DomainContext.CompleteInvoke(IAsyncResult asyncResult).   at System.ServiceModel.DomainServices.Client.DomainContext.<>c__DisplayClass38.<InvokeOperation>b__34(Object )'

delete from ErrorLogs
where exception = '   at System.ServiceModel.DomainServices.Client.DomainContext.<>c__DisplayClass38.<InvokeOperation>b__34(Object )'

select COUNT(*) from ErrorLogs
where exception = '   at System.ServiceModel.DomainServices.Client.DomainContext.<>c__DisplayClass1b.<Load>b__17(Object )'

delete from ErrorLogs
where exception like '%was thrown.%'

delete from ErrorLogs
where exception like '%DataBackupWR%'

delete from ErrorLogs
where exception like '%Access to operation%'

delete from ErrorLogs
where Exception like 
'%Debugging resource strings are unavailable. Often the key and arguments provide sufficient information to diagnose the problem. See %'

delete from ErrorLogs
where Exception like 'Debugging resource strings are unavailable%'

delete from ErrorLogs
where Exception like '%WebFreight.Web.Security.AutenticationException%'
------------------------------------------------------------

select te.id,te.Company,ct.Code,ct.EnglishName from ShipmentAWBPrintOnlies as spo,ChargesTypes as ct,Tenants as te
where spo.IATACodeCode is null  and spo.ChargesTypeId = ct.Id and spo.Tenant = te.Id

------------------------------------------------------------
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
CREATE NONCLUSTERED INDEX IX_ErrorLogs_LogDate ON dbo.ErrorLogs
	(
	LogDate
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

--ALTER TABLE dbo.ErrorLogs SET (LOCK_ESCALATION = TABLE)
GO
COMMIT

----------------------------------------------------
select *
from INFORMATION_SCHEMA.COLUMNS
where TABLE_NAME like '%Addresses%'



----------------------------------
select * from querycolumns 
where tenant = 0 and queryid = (select id from Queries where tenant = 0 and code like '%All Tenant Managements%') and UserId = '1-1'

select * from queries 
where tenant = 0 and code like '%All Tenant Managements%'

update QueryColumns
set IndexOrder = 13
where id = '1-385928'

select * from  sys.database_connection_stats
order by throttled_connection_count desc

select * from  sys.database_connection_stats
where start_time >= '2013-10-29' and total_failure_count > 0
order by start_time 

select top 100 * from sys.event_log 
where  start_time >= '2013-10-29' and severity > 0 -- and description <> 'Connected successfully to database.'
--order by throttled_connection_count desc
----------------------------------------------------------------
select * from TextCodes
where ObjectTableId in (select id from ObjectTables where name like '%customs.%')



select * from ObjectFields
where ObjectTableId in (select id from ObjectTables where name like '%customs.declaration%')

delete from ObjectFields
where ObjectTableId in (select id from ObjectTables where name like '%customs.declaration%')

delete from TextCodes
where ObjectTableId in (select id from ObjectTables where name like '%customs.declaration%')

select * from ObjectTables
where name like '%customs.%' and EnableSecurity = 0
--where id = '1-18024'

update ObjectTables
set EnableSecurity = 1
where name like '%customs.%' and EnableSecurity = 0

--------------
select * from arinvoices
where ARInvoiceTypeCode = 'TX'

update ARInvoices
set ARInvoiceTypeCode = 'IN'
where ARInvoiceTypeCode = 'TX'

-----------------------------------------------------drop primary key script by mohammad--------------------------------------------------------------------------------
DECLARE @SQL VARCHAR(4000)
SET @SQL = 'ALTER TABLE Customs.OrganizationUnitTypes DROP CONSTRAINT |ConstraintName| '

SET @SQL = REPLACE(@SQL, '|ConstraintName|', ( SELECT   name
											   FROM     sysobjects
											   WHERE    xtype = 'PK'
														AND parent_obj = OBJECT_ID('Customs.OrganizationUnitTypes')
											 ))

EXEC (@SQL)
-----------------------------------------------------------------------------------------------------------------
declare @tenant int;
declare @table varchar(40);

set @tenant =  308;
set @table = 'Shipment';

select * from CounterDefinitions
where tenant = @tenant and CounterId = (select id from Counters
where tenant = @tenant and name = @table)

select * from CounterStats
where tenant = @tenant and CounterId = (select id from Counters 
where tenant = @tenant and name = @table)

--update CounterStats
--set LastValue = 1044
--where tenant = 194 and CounterId = (select id from Counters
--where tenant = 194 and name = 'ARInvoice') and Prefix = 'CRE 2013-'


select * from CounterDefinitions as c,Counters as cc
 where c.Tenant = 1  and c.CounterId = cc.Id

select * from CounterStats as c,Counters as cc
 where c.Tenant = 1  and c.CounterId = cc.Id
-----------------------------------------------------------------------------------------------------------------
select distinct arlin.id,arinv.InvoiceNumber,arinv.InvoiceDate,arinv.StatusCode,arinv.BillToId,arlin.VatPercentage from ARInvoices as arinv, ARInvoiceLines as arlin
where arinv.id = arlin.ARInvoiceId and arinv.Tenant = 198 and arinv.InvoiceDate >= '2013-06-01'
and arlin.VatPercentage = 17 and arinv.StatusCode <> 'VD'
--ORDER BY InvoiceNumber ASC
order by arlin.id

select * from ARInvoiceLines
where ARInvoiceId in (select id from ARInvoices where Tenant = 198 and InvoiceDate >= '2013-06-01' and StatusCode <> 'VD')
and VatPercentage = 18 and Tenant = 198


--update ARInvoiceLines
set VatPercentage = 18
where ARInvoiceId in (select id from ARInvoices where Tenant = 198 and InvoiceDate >= '2013-06-01' and StatusCode <> 'VD')
and VatPercentage = 17 and Tenant = 198

-----------------------------------------------------------------------------------------------------------------
select ar.InvoiceNumber,* from ARInvoiceLines as li,ARInvoices as ar
where ar.id = li.ARInvoiceId and li.Tenant = 198 and li.VatPercentage = 17 and ar.StatusCode <> 'VD' and ar.InvoiceDate >= '2013-06-01 00:00:00.000'

select ap.InvoiceNumber,* from APInvoiceLines as li,APInvoices as ap
where ap.id = li.APInvoiceId and li.Tenant = 198 and li.VatPercentage = 17 and ap.StatusCode <> 'VD' and ap.InvoiceDate >= '2013-06-01 00:00:00.000'
-----------------------------------------------------------------------------------------------------------------
update Contacts
set DontShowLocalLabels = 1
where UserType = 'R' and inactive = 0
-----------------------------------------------------------------------------------------------------------------
update AdvancedQueryFilters
set IsPredefined = 0,PredefinedValue = null
where QueryId = '1-41824' and Tenant = 1 
-----------------------------------------------------------------------------------------------------------------
select * from  Contactpasswords
where email = 'jiffsales2-ruh@jifflogistics.com'

update Contactpasswords
set islocked = 0,NumberOfRetries = 0
where email = 'jiffsales2-ruh@jifflogistics.com'
-----------------------------------------------------------------------------------------------------------------
select top 100 * from CommunicationLogs
where CommunicationStatusTypeCode = 'F'
order by CreateDate desc
-----------------------------------------------------------------------------------------------------------------
select ca.EnglishName,ar.InvoiceNumber,ar.MainEntityReference,ar.CreateDate,ar.StatusCode,ar.SubTotalInInvoiceCurrency,ar.SubTotalInLocalCurrency,ar.AmountInInvoiceCurrency,ar.AmountInLocalCurrency,cu.Code from APInvoices as ar, cards as ca,Currencies as cu
where ar.Tenant = 189 and ar.vendorid = ca.Id  and cu.Id = ar.InvoiceCurrencyId
-----------------------------------------------------------------------------------------------------------------
select * from CommunicationLogs
where CommunicationStatusTypeCode = 'F' and ([from] ='champ' or [To]='champ')
order by CreateDate desc
-----------------------------------------------------------------------------------------------------------------
update globaltenants
set Version = Version+1 where Id = 0

update globaltenants
set Version = Version+1 where (Id <> 1 and Id <>42 and id<> 222 and id <>242 and id<>237)

select * from GlobalTenants

select * from GlobalTenants
where isactive = 1 and Version= -1
-----------------------------------------------------------------------------------------------------------------
--After create new branch, run this script to copy migration history
insert into   [2014R1_Main].[dbo].[__MigrationHistory]
select * from  [Logitude2-5_Main].[dbo].[__MigrationHistory]

-----------------------------------------------------------------------------------------------------------------
delete from ErrorLogs
where Exception like '%WebFreight.Web.Security.AutenticationException%' and LogDate > '2014-02-17'
-----------------------------------------------------------------------------------------------------------------
CREATE NONCLUSTERED INDEX [IX_Tenant_UserId_ObjectTableId] ON [dbo].[EntityLastActivities] 
(
	[Tenant] ASC,
	[UserId] ASC,
	[ObjectTableId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
-----------------------------------------------------------------------------------------------------------------
insert into EmailProviders
values ('1','retail.smtp.com','ihab@simplogworld.com','Saas256','2525','Active',Null,Null);

insert into EmailProviders
values ('2','smtp.sendgrid.net','system@logitudeworld.com','log!@##@!','587','Active',Null,Null);
-----------------------------------------------------------------------------------------------------------------
select * from performancelogs
where logdatetimegmt > '2014-05-12 11:42:02.000'
and userip like '62.24%'
-----------------------------------------------------------------------------------------------------------------
select column_name,* from information_schema.columns
 where table_name = 'Shipments'
order by ordinal_position
-----------------------------------------------------------------------------------------------------------------
insert into Settings(Id,LogitudeURL,ChampURL,DeploymentStage,ChampEnv,CustomerCareIP,TotangoServiceId,UsingAzure,IsLogEnabled,storageaccountname,storageaccountkey,storagetype)
values('1','http://system.logitudeworld.com','http://54.200.22.12:80','Simplog','PROD','82.213.2.230,213.6.5.182,213.6.5.176,192.116.221.91','SP-1146-01',1,1,'simplog','4TBEStcAfMhUqpZOfuJby9OAAj2b2CHnAxAd3xeTeuC1IhjOkanV3iAEv4u6N8IiAGDjK04t0PqsiFCnybk+og==','azure')
-----------------------------------------------------------------------------------------------------------------
insert into Settings(Id,LogitudeURL,ChampURL,DeploymentStage,ChampEnv,CustomerCareIP,TotangoServiceId,UsingAzure,IsLogEnabled,storageaccountname,storageaccountkey,storagetype)
values('1','http://amitalcloud.cloudapp.net/','http://54.200.22.12:80','amitalcloud','TEST','82.213.2.230,213.6.5.182,213.6.5.176,192.116.221.91','SP-11460-01',1,1,'amitalcloud','czvnH30OagkI4VLkkjJiaPp1n6XwMgfy6qurGxwGT7AxfPE9ns+KLOp87lrEQoRKCx6jeJIRx2cTxytUiFK4pA==','azure')
-----------------------------------------------------------------------------------------------------------------
DECLARE @dbid INT ; --what's the dbid for DBNAME?
SET @dbid = DB_ID() ;

DECLARE @objectid INT ; --what's objectid for our demo table?
SET @objectid = OBJECT_ID('dbo.') ;

 
--limit the results to only the offending table
SELECT  *
FROM    sys.dm_tran_locks
WHERE   resource_database_id = @dbid --AND
		--resource_associated_entity_id = @objectid ;
		
		select     *--session_id, program_name
from sys.dm_exec_sessions

-- then kill the locked session
-----------------------------------------------------------------------------------------------------------------
select * from settings

update settings
set
LogitudeURL = 'http://logitudewe1.cloudapp.net/',
DeploymentStage = 'simplogtest1',
ChampEnv = 'TEST',
TotangoServiceId = 'SP-11460-01',
StorageAccountName = 'simplogtest1',
StorageAccountKey = '3TMyh2uVpfmx5u9PqNMUXQ73yjqWADDJiGQzu/tWRnRECRHtQ2kMmTxZr7/Z8LXcY3b8Abus3xczvCp0OO6/rA==',
AutoSignupEmail ='',
AutoSignupPassword = '',
glshkurl='',
glshkenv=''

update settings
set
LogitudeURL = 'http://logitudetest.cloudapp.net/',
DeploymentStage = 'logitudetest',
ChampEnv = 'TEST',
TotangoServiceId = 'SP-11460-01',
StorageAccountName = 'logitudetest',
StorageAccountKey = '+IZs9LFwrJGrWydcvg4XZMS8pvlw3kP+0pG2E+GVpasnrdjJzSokANY8evNwl8adi403AeP9LJw9F2bRVggo7A==',
AutoSignupEmail ='',
AutoSignupPassword = ''

update settings
set
LogitudeURL = 'http://logitudetest2.cloudapp.net/',
DeploymentStage = 'logitudetest2',
ChampEnv = 'TEST',
TotangoServiceId = 'SP-11460-01',
StorageAccountName = 'logitudetest2',
StorageAccountKey = 'LUo/OhtqQ4Nk8ynFpdHaXu1X9UyHZ8gxos9mfaZiq1nEUCblj1m8FyZyQqBy0WsjKm16x47S6UxlqRnRtI1NiQ==',
AutoSignupEmail ='',
AutoSignupPassword = '',
GLSHKEnv = '',
GLSHKURL = ''

update settings
set
LogitudeURL = 'http://preproduction1.cloudapp.net/',
DeploymentStage = 'logitudepreprod',
ChampEnv = 'TEST',
TotangoServiceId = 'SP-11460-01',
StorageAccountName = 'logitudepreproduction',
StorageAccountKey = 'Goq5YhqhIQRfkXMm8r2eNLsZ/x1ojpa3E7GLjE2FzpsinLtU+YgkRubP5y8awegb0jVgIgilWhLgS5Dk9A1VBg==',
AutoSignupEmail ='',
AutoSignupPassword = '',
GLSHKURL = '',
ChampURL = '',
customertenantsurl = LogitudeURL,
ForwarderTenantsURL = LogitudeURL

select * from GlobalDBs

update GlobalDBs 
set DBConnection = 'SimplogMain-Test,logitudemanager,S@@s256.0,jubwciho7q.database.windows.net'

update GlobalDBs 
set DBConnection = 'LogitudeMain-Test,logitudemanager,!LO852456,ebup282itq.database.windows.net'

update globaldbs
set dbconnection = 'LogitudeMain-2016R2,logitudeuser,!LU789456,ek5cxrtgus.database.windows.net'

update GlobalDBs 
set DBConnection = '2016R3_Main,logitudemanager,!LO009008,logitudetest.database.windows.net'

update globaldbs
set dbconnection = 'LogitudeMain-Pre,logitudemanager,!LO009008,logitudetest.database.windows.net'

-----------------------------------------------------------------------------------------------------------------
update  settings
set checkconnectionurl = 'http://logitudetest.cloudapp.net/webservices/CheckConnectivityWebService.asmx'
-----------------------------------------------------------------------------------------------------------------
select Contacts.Email,Contacts.englishname,Tenants.Company,Roles.Name from ContactTenantRoleSet,Tenants,Contacts,Roles,Users
where ContactTenantRoleSet.contacttenantid = Contacts.id and ContactTenantRoleSet.tenant = tenants.id and ContactTenantRoleSet.roleid = roles.id  and Users.Id = ContactTenantRoleSet.ContactTenantId
and Contacts.Email not like '%system%' and Contacts.Email not like '%customercare@logitude%' and Users.LicencedUser = 1 and Contacts.InActive = 0 and Contacts.Id = Users.Id and Contacts.Tenant = ContactTenantRoleSet.Tenant
order by Company

-----------------------------------------------------------------------------------------------------------------
select year(createdatetime),month(createdatetime),count(*) from shipments
where createdatetime >= '2014-01-01' and createdatetime <= '2014-09-01'
group by year(createdatetime),month(createdatetime)
order by year(createdatetime),month(createdatetime) desc

select year(CreateDate),month(CreateDate),count(*) from ARInvoices
where CreateDate >= '2014-01-01' and CreateDate <= '2014-09-01'
group by year(CreateDate),month(CreateDate)
order by year(CreateDate),month(CreateDate) desc
-----------------------------------------------------------------------------------------------------------------
select Tenants.Id,Tenants.Company,Countries.EnglishName from Tenants,addresses,countries
where Tenants.Id = Addresses.Tenant and Tenants.AddressId = Addresses.Id and Addresses.CountryId = Countries.Id and Countries.EnglishName = 'Israel' 
and Tenants.Company not like '%Empty%' and Tenants.Company not like '%delete%' and Tenants.Company not like '%test%'
order by Tenants.Id
-----------------------------------------------------------------------------------------------------------------
select Id,SUM(CASE WHEN FreeUsers is null
                 THEN NumberOfUsers
                 ELSE FreeUsers+NumberOfUsers END) from TenantManagements
where NumberOfUsers <> 999 
group by Id
------------------
select cards.englishName,cards.accountingcard,addresses.Address1,addresses.City,addresses.zipcode,countries.Englishname from cards,addresses,countries
where cards.tenant = 327 and cards.partnertypeid = 'CS' and cards.iscustomer = 1 and addresses.addresstypeid = 'M' and cards.id = addresses.cardid and addresses.countryid = countries.id

-----------
select * from contactpasswords
where email = 'all@nordic-group.eu' or 
email = 'asaf@tvlteam.com' or 
email = 'cag@cab-forwarding.com' or 
email = 'dirk.derooij@champ.aero' or 
email = 'emerald.huang@thmason.com' or 
email = 'fraser@wadoye.com' or 
email = 'gemma@acumenaircargo.com' or 
email = 'dane.croft@goworldcargo.com'
----------------
select * from INFORMATION_SCHEMA.COLUMNS

select * from INFORMATION_SCHEMA.COLUMNS -- table_name,column_name,is_nullable,Data_Type,Character_maximum_length
where table_name = 'Shipments' or
table_name = 'ARInvoices' or
table_name = 'ShipmentMasterDatas' or
table_name = 'Contacts' or
table_name = 'ShipmentReceivables' or
table_name = 'ShipmentPayables' or
table_name = 'ARInvoiceLines' or
table_name = 'ShipmentPackages' or
table_name = 'Cards' or
table_name = 'ShipmentPickUpDeliveries' or
table_name = 'APInvoices' or
table_name = 'Addresses' or
table_name = 'Customers' or
table_name = 'Ports' or
table_name = 'Countries' or
table_name = 'APInvoiceLines' or
table_name = 'ARPayments' or
table_name = 'ChargesTypes' or
table_name = 'Users' or
table_name = 'Airlines' or
table_name = 'APPayments' or
table_name = 'InsideShipmentPackages' or
table_name = 'PaymentTerms' or
table_name = 'ShippingLines' or
table_name = 'Agents' or
table_name = 'Truckers' or
table_name = 'Vendors'
----------------------------------
select apinvoices.id,apinvoices.InvoiceDate,apinvoices.InvoiceCurrencyExchangeRate,Currencies1.Code as invoicecurr,Currencies2.Code as localcurr,SubTotalInInvoiceCurrency,SubTotalInLocalCurrency,AmountInLocalCurrency,AmountInInvoiceCurrency,ProfitCurrencyExchangeRate,AmountInProfitCurrency,Currencies3.Code as profitcurr,AmountDue,AmountDueInLocalCurrency,AmountDueInProfitCurrency  from apinvoices,Currencies as Currencies1,Currencies as Currencies2,Currencies as Currencies3
where apinvoices.Tenant =327 and apinvoices.InvoiceCurrencyId = Currencies1.Id and APInvoices.LocalCurrencyId = Currencies2.Id and APInvoices.ProfitCurrencyId = Currencies3.Id  and APInvoices.InvoiceNumber = '5527'
----------------------------------
--Type	Name	VAT Number	Address 1	Address 2	Zip	 City	State	Country	 Phone	Fax	 Email	Contact Name  code
select 'Customer' as Type,Cards.EnglishName,cards.VatNumber,Addresses.Address1,addresses.Address2,Addresses.ZipCode,Addresses.City,States.Code as state,cards.CountryCode,Addresses.PhoneNumber,Addresses.FaxNumber,Contacts.Email,Contacts.EnglishName as Contact,Cards.AccountingCard from Cards
left join Addresses on Addresses.CardId = Cards.Id and Addresses.AddressTypeId = 'M'
left join States on States.Id = Addresses.StateId 
left join Contacts on Contacts.Id = cards.PrimaryContactId
where cards.Tenant = 343 and Cards.PartnerTypeId = 'CS'
---------------------------------------------------
update SystemMetadataLastUpdates
set ObjectFieldsUpdateDateGMT = GETDATE(),TranslationsUpdateDateGMT = GETDATE()
------------------------------------
 drop index IX_TraceEvents_Tenant_LogDateTime on traceevents
 ---------------
 SELECT *
FROM INFORMATION_SCHEMA.COLUMNS
WHERE 
     TABLE_NAME = 'shipmentmasterdatas' AND 
     COLUMN_NAME = 'Prefix'
------------------------------------------------- TOP SQLS
SELECT TOP 10 SUBSTRING(qt.TEXT, (qs.statement_start_offset/2)+1,
((CASE qs.statement_end_offset
WHEN -1 THEN DATALENGTH(qt.TEXT)
ELSE qs.statement_end_offset
END - qs.statement_start_offset)/2)+1),
qs.execution_count,
qs.total_logical_reads, qs.last_logical_reads,
qs.total_logical_writes, qs.last_logical_writes,
qs.total_worker_time,
qs.last_worker_time,
qs.total_elapsed_time/1000000 total_elapsed_time_in_S,
qs.last_elapsed_time/1000000 last_elapsed_time_in_S,
qs.last_execution_time,
qp.query_plan
FROM sys.dm_exec_query_stats qs
CROSS APPLY sys.dm_exec_sql_text(qs.sql_handle) qt
CROSS APPLY sys.dm_exec_query_plan(qs.plan_handle) qp
ORDER BY qs.total_logical_reads DESC -- logical reads
-- ORDER BY qs.total_logical_writes DESC -- logical writes
-- ORDER BY qs.total_worker_time DESC -- CPU time
------------------------------------------------------Queries taking longest elapsed time
SELECT TOP 10
qs.total_elapsed_time / qs.execution_count / 1000000.0 AS average_seconds,
qs.total_elapsed_time / 1000000.0 AS total_seconds,
qs.execution_count,
SUBSTRING (qt.text,qs.statement_start_offset/2,
(CASE WHEN qs.statement_end_offset = -1
THEN LEN(CONVERT(NVARCHAR(MAX), qt.text)) * 2
ELSE qs.statement_end_offset END - qs.statement_start_offset)/2) AS individual_query,
o.name AS object_name,
DB_NAME(qt.dbid) AS database_name
FROM sys.dm_exec_query_stats qs
CROSS APPLY sys.dm_exec_sql_text(qs.sql_handle) as qt
LEFT OUTER JOIN sys.objects o ON qt.objectid = o.object_id
where qt.dbid = DB_ID()
ORDER BY average_seconds DESC;
-----------------------------------------------------------------------Queries doing most I/O:
SELECT TOP 10
(total_logical_reads + total_logical_writes) / qs.execution_count AS average_IO,
(total_logical_reads + total_logical_writes) AS total_IO,
qs.execution_count AS execution_count,
SUBSTRING (qt.text,qs.statement_start_offset/2,
(CASE WHEN qs.statement_end_offset = -1
THEN LEN(CONVERT(NVARCHAR(MAX), qt.text)) * 2
ELSE qs.statement_end_offset END - qs.statement_start_offset)/2) AS indivudual_query,
o.name AS object_name,
DB_NAME(qt.dbid) AS database_name
FROM sys.dm_exec_query_stats qs
CROSS APPLY sys.dm_exec_sql_text(qs.sql_handle) as qt
LEFT OUTER JOIN sys.objects o ON qt.objectid = o.object_id
where qt.dbid = DB_ID()
ORDER BY average_IO DESC;


declare @RoleId as varchar(15)
EXECUTE usp_GetNextTableIdValue @RoleId OUTPUT,'Role'
					insert into Roles(Id, Name, Tenant,Code, RoleTypeCode, [Description])
					values(@RoleId, 'Freigh operation + AP invoices entry',0, 'FOAI', 'AC', 'Freigh operation + AP invoices entry')

--------------------------------------------------
select * 
into errorlogs_history
from ErrorLogs where LogDate >= '2015-10-01' and LogDate < '2015-10-20'

--------------------------------------------------
--------------------------------------------------
SELECT KU.table_name as tablename,column_name as primarykeycolumn
FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS TC
INNER JOIN
INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS KU
ON TC.CONSTRAINT_TYPE = 'PRIMARY KEY' AND
TC.CONSTRAINT_NAME = KU.CONSTRAINT_NAME
and ku.table_name='ContactMobileDevices'
ORDER BY KU.TABLE_NAME, KU.ORDINAL_POSITION;
-------------------------------------
select * from CommunicationLogs
where entityreference = '1068' and Tenant = 545
order by CreateDate desc
----
select * From analyzequeues
where entityreference = '1068' and Tenant = 545
order by CreateDate desc

----------------------
select Bookings.*,BookingStatus.Code as status,Contacts.EnglishName as CreatedBy,p1.Code as fromport,p2.Code as toport,cards.EnglishName as IssuingCarrierAgent from Bookings
join BookingStatus on BookingStatusCode = BookingStatus.Code
join Contacts on Contacts.Id = CreatedByUserId 
join ports p1 on p1.id = MainCarriageFromPortId
join ports p2 on p2.id = MainCarriageToPortId 
join Cards on cards.Id = IssuingCarrierAgentId
order by CreateDate desc
------------------------------
select Bookings.BookingNumber as BookingNo,Tenants.Company,Bookings.AirlinePrefix,Bookings.Master,Bookings.ChargeableWeightInKG as Weight,BookingStatus.Code as Status,Contacts.EnglishName as CreatedBy,p1.Code as FromPort,p2.Code as ToPort,bookings.CreateDate from Bookings
join BookingStatus on BookingStatusCode = BookingStatus.Code
join Contacts on Contacts.Id = CreatedByUserId 
join ports p1 on p1.id = MainCarriageFromPortId
join ports p2 on p2.id = MainCarriageToPortId 
join Tenants on Tenants.Id = Bookings.tenant
order by CreateDate desc
-------------------------------- Fina all indexes
SELECT 
     TableName = t.name,
     IndexName = ind.name,
     IndexId = ind.index_id,
     ColumnId = ic.index_column_id,
     ColumnName = col.name,
     ind.*,
     ic.*,
     col.* 
FROM 
     sys.indexes ind 
INNER JOIN 
     sys.index_columns ic ON  ind.object_id = ic.object_id and ind.index_id = ic.index_id 
INNER JOIN 
     sys.columns col ON ic.object_id = col.object_id and ic.column_id = col.column_id 
INNER JOIN 
     sys.tables t ON ind.object_id = t.object_id 
WHERE 
     ind.is_primary_key = 0 
     AND ind.is_unique = 0 
     AND ind.is_unique_constraint = 0 
     AND t.is_ms_shipped = 0 
ORDER BY 
     t.name, ind.name, ind.index_id, ic.index_column_id 


----------------------------Clean errologs table
drop table errorlogs_history

select * 
into ErrorLogs
from errorlogs_history where LogDate >= '2015-11-01' and LogDate < '2015-11-23'

TRUNCATE TABLE ErrorLogs

insert into ErrorLogs
select * from errorlogs_history
--------------------------- Find constraint in a table
SELECT * FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
WHERE TABLE_NAME='cards'

select * from INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE where CONSTRAINT_NAME = 'UQ_Tenant_Code_PartnerTypeId_Cards'

---------------------------------
(select tenants.Company as Company ,month(quotes.OpenDate) as month,quotes.Id as quoteid,null as shipmentid from quotes
join tenants on quotes.tenant  = tenants.id
where quotes.OpenDate >= '2015-11-01' and quotes.OpenDate < '2015-12-01')
union 
(select tenants.Company , month(Shipments.createdatetime) as month,null,Shipments.Id as shipmentid from Shipments
join tenants on Shipments.tenant  = tenants.id
where Shipments.createdatetime >= '2015-11-01' and Shipments.createdatetime < '2015-12-01')
--------------------------
SELECT *
FROM sys.columns c
    JOIN sys.tables t ON c.object_id = t.object_id

	-------------------------------------

	ALTER TABLE settings ALTER COLUMN deploymentstage [varchar](40) NOT NULL


select top 10 * from autosignupemails order by createdate desc

update autosignupemails
set status = 'New',retries = 0 where id = '3de84531-3135-475a-ba70-ca12370c9a53'

---------------------------------------------------- Count of Rows per table -----
SELECT 
    t.NAME AS TableName,
    s.Name AS SchemaName,
    p.rows AS RowCounts,
    SUM(a.total_pages) * 8 AS TotalSpaceKB, 
    SUM(a.used_pages) * 8 AS UsedSpaceKB, 
    (SUM(a.total_pages) - SUM(a.used_pages)) * 8 AS UnusedSpaceKB
FROM 
    sys.tables t
INNER JOIN      
    sys.indexes i ON t.OBJECT_ID = i.object_id
INNER JOIN 
    sys.partitions p ON i.object_id = p.OBJECT_ID AND i.index_id = p.index_id
INNER JOIN 
    sys.allocation_units a ON p.partition_id = a.container_id
LEFT OUTER JOIN 
    sys.schemas s ON t.schema_id = s.schema_id
WHERE 
    t.NAME NOT LIKE 'dt%' 
    AND t.is_ms_shipped = 0
    AND i.OBJECT_ID > 255 
GROUP BY 
    t.Name, s.Name, p.Rows
ORDER BY 
    p.rows desc
--------------delete large tables

SELECT 1
WHILE @@ROWCOUNT > 0	BEGIN	
DELETE TOP (500)
--from Competitors
FROM PerformanceLogs
--from CustomerAdditionalServices
--from CustomerCompetitors
--where Tenant <> 0
END

SELECT 1
WHILE @@ROWCOUNT > 0	BEGIN	
DELETE TOP (500)
--from Competitors
FROM CommunicationAttachments
--from CustomerAdditionalServices
--from CustomerCompetitors
where communicationlogid in (select id from communicationlogs where createdateutc < '2016-09-01')
END

update ARInvoices
set IsCancelled = 1,CancelledByARInvoiceId = '1-115637',StatusCode = 'AR',AmountDue = 0,AmountDueInProfitCurrency = 0,AmountDueInLocalCurrency = 0
where InvoiceNumber = '1460' and id = '1-110678' and Tenant = 478


------Change Business unit : Quotes, Opportunities, Activities

update Quotes set BusinessUnitId = '3-1'
where SalesmanUserId in ('1-205674','1-205675','1-69766')

update Opportunities set BusinessUnitId = '3-1'
where OwnerId in ('1-205674','1-205675','1-69766')

update Activities set BusinessUnitId = '3-1'
where OwnerId in ('1-205674','1-205675','1-69766')


update Quotes set BusinessUnitId = '3-1'
--select * from quotes
where SalesmanUserId = '1-3816' and tenant = 3 and BusinessUnitId <> '3-1'

update Opportunities set BusinessUnitId = '3-1'
--select * from Opportunities
where OwnerId  = '1-3816' and tenant = 3 and BusinessUnitId <> '3-1'

update Activities set BusinessUnitId = '3-1'
--select * from activities
where OwnerId  = '1-3816' and tenant = 3 and BusinessUnitId <> '3-1'

select BusinessUnitId from Users where Id = (select id from contacts where email = 'ilank@ruthcargo.co.il' and tenant = 3)


--Check users with no Roles
select * from Users where  id in ( select id from contacts where id in ( select ContactId from ContactTenants where Id not in ( select contacttenantid from ContactTenantRoleSet)))


-------------------Open connections, Kill connections 
DECLARE @kill varchar(8000) = '';

SELECT @kill = @kill + 'KILL ' + CONVERT(varchar(5), c.session_id) + ';'

FROM sys.dm_exec_connections AS c
JOIN sys.dm_exec_sessions AS s
    ON c.session_id = s.session_id
WHERE c.session_id <> @@SPID
--WHERE status = 'sleeping'
ORDER BY c.connect_time ASC

EXEC(@kill)


SELECT
    c.session_id, c.net_transport, c.encrypt_option,
    s.status,
    c.auth_scheme, s.host_name, s.program_name,
    s.client_interface_name, s.login_name, s.nt_domain,
    s.nt_user_name, s.original_login_name, c.connect_time,
    s.login_time
FROM sys.dm_exec_connections AS c
JOIN sys.dm_exec_sessions AS s
    ON c.session_id = s.session_id
--WHERE c.session_id = @@SPID;
--WHERE status = 'sleeping'
ORDER BY c.connect_time ASC

-----------------Recent shipments, and other activities , clean 
insert into  EntityLastActivities 
select * from EntityLastActivities2
--where ActivityDate  >= '2016-03-201'

select count(*) from EntityLastActivities

truncate table EntityLastActivities


----- User login in last 5 mintues
select  u.LoginDateTime,c.EnglishName,t.Company from UserLastLogins as u,Contacts as c,Tenants as t
where u.Id = c.Id and c.Tenant = u.Tenant and c.Tenant = t.Id and u.LoginDateTime >= DATEADD(minute, -5, GETDATE()) -- '2016-03-27 8:30:00'
order by t.company

-- find - Used indexes from each table 
SELECT   OBJECT_NAME(S.[OBJECT_ID]) AS [OBJECT NAME], 
         I.[NAME] AS [INDEX NAME], 
         USER_SEEKS, 
         USER_SCANS, 
         USER_LOOKUPS, 
         USER_UPDATES 
FROM     SYS.DM_DB_INDEX_USAGE_STATS AS S 
         INNER JOIN SYS.INDEXES AS I 
           ON I.[OBJECT_ID] = S.[OBJECT_ID] 
              AND I.INDEX_ID = S.INDEX_ID 
			  And OBJECT_NAME(S.[OBJECT_ID]) = 'Documenttypes'--ShipmentMasterDatas
			  And s.database_id = DB_ID()
WHERE    OBJECTPROPERTY(S.[OBJECT_ID],'IsUserTable') = 1
order by I.[Name] 

----Queue messages check 
select top 100 * from queuemessages
where QueueDefinitionCode = 'Emailqueue' 
order by CreateDateTime desc  


select * from queuemessages
where messagebody like '%1-4073%'


--declare @LogId as varchar(15)

--EXECUTE usp_GetNextTableIdValue @LogId OUTPUT,'QueueMessage'
insert into QueueMessages values('EmailQueue',SYSDATETIME(),0,'{"CommunicationLogId":"1-3784","Tenant":"2"}',SYSDATETIME(),null,null,0)

--LOG PASRER STUDIO
SELECT TOP 10 * FROM '[LOGFILEPATH]'
where cs-uri-stem like '%translations%' and sc-bytes > 10000
order by sc-bytes desc


--Update transfer status - change blocked invoices AP 
select * from apinvoices where invoicedate between '2015-04-01' and '2015-06-01' and transferstatuscode = 'BL' and tenant = 244
select * from apinvoices where invoicedate between '2015-04-01' and '2015-06-01' and transferstatuscode = 'NR' and tenant = 244

select * from APInvoiceTransferStatus

update apinvoices
set transferstatuscode = 'NR'
where invoicedate between '2015-04-01' and '2015-06-01' and transferstatuscode = 'BL' and tenant = --244

---CONNECTIONS IN POOL SQL SERVER - AZURE
SELECT * FROM sys.dm_exec_connections
order by connect_time desc



-- Update ObjectTableLastUpdates , Add new record and update old records


declare @Tenant as int
declare @NewStageId as varchar(15)



	DECLARE TenantsCursor CURSOR READ_ONLY
	FOR	
	SELECT Id
	FROM Tenants
	OPEN TenantsCursor FETCH NEXT FROM TenantsCursor INTO @Tenant
	WHILE @@FETCH_STATUS = 0
	BEGIN

	if not exists ( select id from ObjectTableLastUpdates where tenant = @Tenant and ObjectTableId = '1-57')

			begin

			EXECUTE usp_GetNextTableIdValue @NewStageId OUTPUT,'ObjectTableLastUpdate'
			insert into ObjectTableLastUpdates (Id, Tenant, LastUpdateDate, UpdatedByUserId, ObjectTableId)
			values(@NewStageId, @Tenant, GETDATE() , '1-1', '1-57')

			end 


	FETCH NEXT FROM TenantsCursor INTO @Tenant
	END
	CLOSE TenantsCursor
	DEALLOCATE TenantsCursor


	--update ObjectTableLastUpdates
	--set LastUpdateDate = GETDATE()
	--where ObjectTableId = '1-57' and LastUpdateDate < '2016-05-09'


	---------- Shipment search fields without containers 
	select shipments.CreateDateTime,shipments.ShipmentNumber,shipments.TransportModeId,shipments.SearchFields from shipments 
join shipmentpackages  on shipmentpackages.ShipmentId = shipments.id and shipmentpackages.ContainerNumber is not null
where shipments.tenant = 558  and shipments.SearchFields not like CONCAT('%',shipmentpackages.ContainerNumber,'%')

--- Check what delete from database

SELECT 
    [Transaction ID],
    Operation,
    Context,
    AllocUnitName
    
FROM 
    fn_dblog(NULL, NULL) 
WHERE 
    Operation = 'LOP_DELETE_ROWS' and allocunitname like '%AirlineMessagingRules%'


    ------- Find locks and errors in database 
select cmd,* from sys.sysprocesses
where blocked > 0


USE Master
GO
EXEC sp_who2 
GO


 DECLARE @sqltext VARBINARY(128)
SELECT @sqltext = sql_handle
FROM sys.sysprocesses
WHERE spid = 140   
SELECT TEXT
FROM ::fn_get_sql(@sqltext)
GO


SELECT 
    CONVERT (varchar(30), GETDATE(), 121) as [RunTime],
    DATEADD (ms, rbf.[timestamp] - tme.ms_ticks, GETDATE()) as [Notification_Time],
    CAST(record as xml).value('(//SPID)[1]', 'bigint') as SPID,
    CAST(record as xml).value('(//ErrorCode)[1]', 'varchar(255)') as Error_Code,
    CAST(record as xml).value('(//CallingAPIName)[1]', 'varchar(255)') as [CallingAPIName],
    CAST(record as xml).value('(//APIName)[1]', 'varchar(255)') as [APIName],
    CAST(record as xml).value('(//Record/@id)[1]', 'bigint') AS [Record Id],
    CAST(record as xml).value('(//Record/@type)[1]', 'varchar(30)') AS [Type],
    CAST(record as xml).value('(//Record/@time)[1]', 'bigint') AS [Record Time],
    tme.ms_ticks as [Current Time]
from sys.dm_os_ring_buffers rbf
cross join sys.dm_os_sys_info tme
where rbf.ring_buffer_type = 'RING_BUFFER_SECURITY_ERROR' --and cast(record as xml).value('(//SPID)[1]', 'int') = XspidNo
ORDER BY rbf.timestamp ASC


    SELECT CONVERT (varchar(30), GETDATE(), 121) as [RunTime],
    dateadd (ms, (rbf.[timestamp] - tme.ms_ticks), GETDATE()) as Time_Stamp,
    cast(record as xml).value('(//Record/ConnectivityTraceRecord/RecordType)[1]', 'varchar(50)') AS [Action], 
    cast(record as xml).value('(//Record/ConnectivityTraceRecord/RecordSource)[1]', 'varchar(50)') AS [Source], 
    cast(record as xml).value('(//Record/ConnectivityTraceRecord/Spid)[1]', 'int') AS [SPID],
    cast(record as xml).value('(//Record/ConnectivityTraceRecord/RemoteHost)[1]', 'varchar(100)') AS [RemoteHost],
    cast(record as xml).value('(//Record/ConnectivityTraceRecord/RemotePort)[1]', 'varchar(25)') AS [RemotePort],
    cast(record as xml).value('(//Record/ConnectivityTraceRecord/LocalPort)[1]', 'varchar(25)') AS [LocalPort],
    cast(record as xml).value('(//Record/ConnectivityTraceRecord/TdsBuffersInformation/TdsInputBufferError)[1]', 'varchar(25)') AS [TdsInputBufferError],
    cast(record as xml).value('(//Record/ConnectivityTraceRecord/TdsBuffersInformation/TdsOutputBufferError)[1]', 'varchar(25)') AS [TdsOutputBufferError],
    cast(record as xml).value('(//Record/ConnectivityTraceRecord/TdsBuffersInformation/TdsInputBufferBytes)[1]', 'varchar(25)') AS [TdsInputBufferBytes],
    cast(record as xml).value('(//Record/ConnectivityTraceRecord/TdsDisconnectFlags/PhysicalConnectionIsKilled)[1]', 'int') AS [isPhysConnKilled], 
    cast(record as xml).value('(//Record/ConnectivityTraceRecord/TdsDisconnectFlags/DisconnectDueToReadError)[1]', 'int') AS [DisconnectDueToReadError],
    cast(record as xml).value('(//Record/ConnectivityTraceRecord/TdsDisconnectFlags/NetworkErrorFoundInInputStream)[1]', 'int') AS [NetworkErrorFound],
    cast(record as xml).value('(//Record/ConnectivityTraceRecord/TdsDisconnectFlags/ErrorFoundBeforeLogin)[1]', 'int') AS [ErrorBeforeLogin],
    cast(record as xml).value('(//Record/ConnectivityTraceRecord/TdsDisconnectFlags/SessionIsKilled)[1]', 'int') AS [isSessionKilled],
    cast(record as xml).value('(//Record/ConnectivityTraceRecord/TdsDisconnectFlags/NormalDisconnect)[1]', 'int') AS [NormalDisconnect],
    cast(record as xml).value('(//Record/ConnectivityTraceRecord/TdsDisconnectFlags/NormalLogout)[1]', 'int') AS [NormalLogout],
    cast(record as xml).value('(//Record/@id)[1]', 'bigint') AS [Record Id], 
    cast(record as xml).value('(//Record/@type)[1]', 'varchar(30)') AS [Type], 
    cast(record as xml).value('(//Record/@time)[1]', 'bigint') AS [Record Time],
    tme.ms_ticks as [Current Time]
FROM sys.dm_os_ring_buffers rbf
cross join sys.dm_os_sys_info tme
where rbf.ring_buffer_type = 'RING_BUFFER_CONNECTIVITY' and cast(record as xml).value('(//Record/ConnectivityTraceRecord/Spid)[1]', 'int') <> 0
ORDER BY rbf.timestamp ASC


SELECT CONVERT (varchar(30), GETDATE(), 121) as [RunTime],
    dateadd (ms, (rbf.[timestamp] - tme.ms_ticks), GETDATE()) as Time_Stamp,
    cast(record as xml).value('(//Exception//Error)[1]', 'varchar(255)') as [Error],
    cast(record as xml).value('(//Exception/Severity)[1]', 'varchar(255)') as [Severity],
    cast(record as xml).value('(//Exception/State)[1]', 'varchar(255)') as [State],
    msg.description,
    cast(record as xml).value('(//Exception/UserDefined)[1]', 'int') AS [isUserDefinedError],
    cast(record as xml).value('(//Record/@id)[1]', 'bigint') AS [Record Id],
    cast(record as xml).value('(//Record/@type)[1]', 'varchar(30)') AS [Type], 
    cast(record as xml).value('(//Record/@time)[1]', 'bigint') AS [Record Time],
    tme.ms_ticks as [Current Time]
from sys.dm_os_ring_buffers rbf
cross join sys.dm_os_sys_info tme
cross join sys.sysmessages msg
where rbf.ring_buffer_type = 'RING_BUFFER_EXCEPTION' --and cast(record as xml).value('(//SPID)[1]', 'int') <> 0--in (122,90,161,179)
and msg.error = cast(record as xml).value('(//Exception//Error)[1]', 'varchar(500)') and msg.msglangid = 1033 --and [Error] = 4002
ORDER BY rbf.timestamp ASC


SELECT CONVERT (varchar(30), GETDATE(), 121) as [RunTime],
    dateadd (ms, (rbf.[timestamp] - tme.ms_ticks), GETDATE()) as [Notification_Time], 
    cast(record as xml).value('(//Record/ResourceMonitor/Notification)[1]', 'varchar(30)') AS [Notification_type], 
    cast(record as xml).value('(//Record/MemoryRecord/MemoryUtilization)[1]', 'bigint') AS [MemoryUtilization %], 
    cast(record as xml).value('(//Record/MemoryNode/@id)[1]', 'bigint') AS [Node Id], 
    cast(record as xml).value('(//Record/ResourceMonitor/IndicatorsProcess)[1]', 'int') AS [Process_Indicator],
    cast(record as xml).value('(//Record/ResourceMonitor/IndicatorsSystem)[1]', 'int') AS [System_Indicator],
    cast(record as xml).value('(//Record/MemoryNode/ReservedMemory)[1]', 'bigint') AS [SQL_ReservedMemory_KB], 
    cast(record as xml).value('(//Record/MemoryNode/CommittedMemory)[1]', 'bigint') AS [SQL_CommittedMemory_KB], 
    cast(record as xml).value('(//Record/MemoryNode/AWEMemory)[1]', 'bigint') AS [SQL_AWEMemory], 
    cast(record as xml).value('(//Record/MemoryNode/SinglePagesMemory)[1]', 'bigint') AS [SinglePagesMemory], 
    cast(record as xml).value('(//Record/MemoryNode/MultiplePagesMemory)[1]', 'bigint') AS [MultiplePagesMemory], 
    cast(record as xml).value('(//Record/MemoryRecord/TotalPhysicalMemory)[1]', 'bigint') AS [TotalPhysicalMemory_KB], 
    cast(record as xml).value('(//Record/MemoryRecord/AvailablePhysicalMemory)[1]', 'bigint') AS [AvailablePhysicalMemory_KB], 
    cast(record as xml).value('(//Record/MemoryRecord/TotalPageFile)[1]', 'bigint') AS [TotalPageFile_KB], 
    cast(record as xml).value('(//Record/MemoryRecord/AvailablePageFile)[1]', 'bigint') AS [AvailablePageFile_KB], 
    cast(record as xml).value('(//Record/MemoryRecord/TotalVirtualAddressSpace)[1]', 'bigint') AS [TotalVirtualAddressSpace_KB], 
    cast(record as xml).value('(//Record/MemoryRecord/AvailableVirtualAddressSpace)[1]', 'bigint') AS [AvailableVirtualAddressSpace_KB], 
    cast(record as xml).value('(//Record/@id)[1]', 'bigint') AS [Record Id], 
    cast(record as xml).value('(//Record/@type)[1]', 'varchar(30)') AS [Type],
    cast(record as xml).value('(//Record/@time)[1]', 'bigint') AS [Record Time],
    tme.ms_ticks as [Current Time]
FROM sys.dm_os_ring_buffers rbf
cross join sys.dm_os_sys_info tme
where rbf.ring_buffer_type = 'RING_BUFFER_RESOURCE_MONITOR' --and cast(record as xml).value('(//Record/ResourceMonitor/Notification)[1]', 'varchar(30)') = 'RESOURCE_MEMPHYSICAL_LOW'
ORDER BY rbf.timestamp ASC



------------------information about individual events -----------SQL AZURE -------------
SELECT e.*
FROM sys.event_log AS e
WHERE e.database_name = 'myDbName'
AND e.event_category = 'connectivity'
AND 2 >= DateDiff
  (hour, e.end_time, GetUtcDate())
ORDER BY e.event_category,
  e.event_type, e.end_time;
  --------------------------------------------aggregated counts of event types
  SELECT c.*
FROM sys.database_connection_stats AS c
WHERE c.database_name = 'myDbName'
AND 24 >= DateDiff
  (hour, c.end_time, GetUtcDate())
ORDER BY c.end_time;

---------- Mobile contacts 
select GlobalTenants.CompanyName,ContactMobileDevices.Platform,ContactMobileDevices.Devicetype,ContactMobileDevices.createdate,ContactMobileDevices.email from ContactMobileDevices
join globalcontacts on globalcontacts.email = ContactMobileDevices.email
join GlobalTenants on GlobalTenants.id = globalcontacts.globaltenantid
where ContactMobileDevices.createdate >= '2016-09-01' and globalcontacts.inactive = 0 and globalcontacts.internetaccess = 1

select * from globalcontacts where email in (select email from ContactMobileDevices
where createdate >= '2016-09-01')

select  * From MobileNotificationLogs where email in (select email from ContactMobileDevices
where createdate >= '2016-09-01')

----------------------- Queries - Views With filters - Advance fitlers
select contacts.email,textcodes.defaulttext,queries.*,Advancedqueryfilters.*,objectfields.fieldname from queries
join Advancedqueryfilters on Advancedqueryfilters.QueryId = queries.Id
join textcodes on textcodes.id = queries.nametextcodeid
join contacts on contacts.id = queries.userid
join objectfields on objectfields.id = Advancedqueryfilters.objectfieldid
 where queries.tenant = 383  

 --- Create error logs databse - 19/9/2016
 
CREATE DATABASE [LogitudeSystemLogs-Pre]
GO
/****** Object:  Table [dbo].[BatchServicesLogs]    Script Date: 19/9/2016 10:01:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BatchServicesLogs](
	[Id] [varchar](40) NOT NULL,
	[BatchServiceCode] [varchar](40) NOT NULL,
	[LastActivity] [datetime] NULL,
	[CPU] [decimal](18, 2) NULL,
	[CreateDate] [datetime] NULL,
	[NumberOfDoneItems] [int] NULL,
	[DoneItemsInOneMinute] [int] NOT NULL DEFAULT ((0)),
	[DoneItemsInFiveMinutes] [int] NOT NULL DEFAULT ((0)),
	[DoneItemsInOneHour] [int] NOT NULL DEFAULT ((0)),
 CONSTRAINT [PK_dbo.BatchServicesLogs] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ContactActivityLogs]    Script Date: 19/9/2016 10:01:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ContactActivityLogs](
	[Id] [varchar](40) NOT NULL,
	[Tenant] [int] NOT NULL,
	[ContactId] [varchar](15) NOT NULL,
	[LogDateTime] [datetime] NOT NULL,
	[GMTLogDateTime] [datetime] NOT NULL,
	[Module] [varchar](100) NOT NULL,
	[Activity] [varchar](150) NOT NULL,
	[IsSharedLogisticsContact] [bit] NOT NULL,
	[CardId] [varchar](15) NULL,
	[PartnerTypeId] [varchar](2) NULL,
	[Via] [varchar](20) NULL,
 CONSTRAINT [PK__ContactA__3214EC0703317E3D] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ErrorLogs]    Script Date: 19/9/2016 10:01:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ErrorLogs](
	[Id] [varchar](40) NOT NULL,
	[Tenant] [int] NOT NULL,
	[UserName] [varchar](100) NOT NULL,
	[LogDate] [datetime] NOT NULL,
	[ClientDate] [datetime] NOT NULL,
	[Tier] [varchar](40) NOT NULL,
	[Exception] [varchar](7000) NULL,
	[StackTrace] [varchar](7000) NULL,
	[SearchFields] [varchar](8000) NULL,
	[IP] [varchar](15) NULL,
 CONSTRAINT [PK__ErrorLog__3214EC077F60ED59] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Index [IX_ErrorLogs_LogDate]    Script Date: 19/9/2016 10:01:14 AM ******/
CREATE NONCLUSTERED INDEX [IX_ErrorLogs_LogDate] ON [dbo].[ErrorLogs]
(
	[LogDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[usp_GetForeignKeyName]    Script Date: 19/9/2016 10:01:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [dbo].[usp_GetForeignKeyName]
(
    @keyName as varchar(500) output,
	@baseTableName as varchar(500) ,
	@foreignTableName as varchar(500) ,
	@foreignColumnName as varchar(500)	
)
AS

set @keyName = (select g.ForeignKey from
(
SELECT 
    f.name AS ForeignKey,
    OBJECT_NAME(f.parent_object_id) AS TableName,
    COL_NAME(fc.parent_object_id,
    fc.parent_column_id) AS ColumnName,
    OBJECT_NAME (f.referenced_object_id) AS ReferenceTableName,
    COL_NAME(fc.referenced_object_id,
    fc.referenced_column_id) AS ReferenceColumnName
FROM 
    sys.foreign_keys AS f
    INNER JOIN sys.foreign_key_columns AS fc ON f.OBJECT_ID = fc.constraint_object_id
) as g

where g.TableName = @baseTableName
and g.ReferenceTableName = @foreignTableName
and g.ColumnName = @foreignColumnName
)

GO


------- Active users for active custmers from CRM tennat ---
select Tenants.Id,Tenants.Company,contacts.englishname,contacts.email from contacts 
join tenants on tenants.id = contacts.tenant
 where tenants.id in (select accountingcard From cards where id in (select  id from customers where tenant = 341 and iscustomer = 1 and customerstatuscode = 'ACT'))
and contacts.email is not null  and contacts.usertype  = 'R' and contacts.inactive = 0
and contacts.id in ( select id from users where tenant = contacts.tenant)
order by Tenants.Company

-----------Active EAWB+Booking users
select Globaltenants.Id,Globaltenants.CompanyName,globalcontacts.email from globalcontacts 
join Globaltenants on Globaltenants.id = globalcontacts.globaltenantid
 where Globaltenants.id in (select  id from tenantmanagements where packagecode = 'BUBK' or packagecode = 'BUCH' or packagecode = 'BUEC' or packagecode = 'EACR' or packagecode = 'EAWB' 
or packagecode ='ECEA' or packagecode = 'EECM')
and globalcontacts.email is not null  and globalcontacts.isuser  = 1 and globalcontacts.inactive = 0
order by Globaltenants.CompanyName

--Then select from Main database to get the contact's english name. 

------------ Comm.logs by airline code
select top 20 * from CommunicationLogs
inner join Shipments on Shipments.id = CommunicationLogs.EntityId 
inner join ShipmentMasterDatas on ShipmentMasterDatas.id = Shipments.Id 
inner join cards on cards.id = ShipmentMasterDatas.MainCarriageCarrierId  and cards.code = 'QR'
order by CommunicationLogs.createdateutc desc

-----------
(select tenants.id as tenant ,year(quotes.OpenDate) as year,month(quotes.OpenDate) as month,quotes.Id as quoteid,null as shipmentid from quotes
join tenants on quotes.tenant  = tenants.id
where quotes.OpenDate >= '2010-01-01' and quotes.OpenDate < '2020-10-01')
union 
(select tenants.id , year(Shipments.createdatetime) as year,month(Shipments.createdatetime) as month,null,Shipments.Id as shipmentid from Shipments
join tenants on Shipments.tenant  = tenants.id
where Shipments.createdatetime >= '2010-01-01' and Shipments.createdatetime < '2020-10-01')

----------
select Technology From users where id in (
select id from contacts where 
email = 'h.elgartit@timar.ma' or
email = 'n.abhri@timar.ma') and tenant not in (1,65)

update users
set technology = 'PR'
where id in (
select id from contacts where 
email = 'h.elgartit@timar.ma' or
email = 'n.abhri@timar.ma') and tenant not in (1,65)


----------
select  * from DocumentTypeTemplates
where DocumentTypeId in ( select id from DocumentTypes where code = '784' and Tenant = 1)

ALTER TABLE DocumentTypeTemplates ADD [Col_Varbinary] VARBINARY(MAX)
GO

SELECT 'UPDATE DocumentTypeTemplates SET [Col_Varbinary]='
+ CONVERT(VARCHAR(MAX),'000000') 

--Take the update command from above and execute, add the where 
UPDATE DocumentTypeTemplates SET [Col_Varbinary]='000000'
where DocumentTypeId in ( select id from DocumentTypes where code = '784' and Tenant = 1)


DECLARE @SQLIMG VARCHAR(MAX),
    @IMG_PATH VARBINARY(MAX),
    @TIMESTAMP VARCHAR(MAX),
    @ObjectToken INT

DECLARE IMGPATH CURSOR FAST_FORWARD FOR 
        select  Col_Varbinary from DocumentTypeTemplates where DocumentTypeId in ( select id from DocumentTypes where code = '784' and Tenant  = 1)

OPEN IMGPATH 

FETCH NEXT FROM IMGPATH INTO @IMG_PATH 

WHILE @@FETCH_STATUS = 0
    BEGIN
        SET @TIMESTAMP = 'C:\temp\' + replace(replace(replace(replace(convert(varchar,getdate(),121),'-',''),':',''),'.',''),' ','') + '.mrt'

        PRINT @TIMESTAMP
        PRINT @SQLIMG

        EXEC sp_OACreate 'ADODB.Stream', @ObjectToken OUTPUT
        EXEC sp_OASetProperty @ObjectToken, 'Type', 1
        EXEC sp_OAMethod @ObjectToken, 'Open'
        EXEC sp_OAMethod @ObjectToken, 'Write', NULL, @IMG_PATH
        EXEC sp_OAMethod @ObjectToken, 'SaveToFile', NULL, @TIMESTAMP, 2
        EXEC sp_OAMethod @ObjectToken, 'Close'
        EXEC sp_OADestroy @ObjectToken

        FETCH NEXT FROM IMGPATH INTO @IMG_PATH 
    END 

CLOSE IMGPATH
DEALLOCATE IMGPATH

--Convert varbinary to varchar 
declare @temp as varbinary(max)
set @temp = (select templatebody from DocumentTypeTemplates where DocumentTypeId in ( select id from DocumentTypes where code = '784' and Tenant = 558  ) and id = '1-43967')
select convert(varchar(max),@temp ,0)

-- Update template from varchar to varbinary 
UPDATE DocumentTypeTemplates SET templatebody= convert(varbinary(max),'',0)
where DocumentTypeId in ( select id from DocumentTypes where code = '784' and Tenant = 558  ) and id = '1-43967'


-----Compare 2 VarBinary columns - Document types -- between tenant 0 and other tenants - ARP

DECLARE @Id AS varchar(15)
DECLARE @Tenant AS INT 
DECLARE @TemplateBody AS varbinary(max)  
DECLARE @TemplateBody_Zero AS varchar(max)

BEGIN; 
	DECLARE DocumentTypeTemplates_Cursor CURSOR READ_ONLY
	FOR	
	SELECT Id,Tenant,TemplateBody
	FROM DocumentTypeTemplates where DocumentTypeId in ( select id from DocumentTypes where code = 'ARP')
	OPEN DocumentTypeTemplates_Cursor FETCH NEXT FROM DocumentTypeTemplates_Cursor INTO @Id,@Tenant,@TemplateBody
	WHILE @@FETCH_STATUS = 0
	BEGIN

	set @TemplateBody_Zero = (select templatebody from DocumentTypeTemplates where DocumentTypeId in ( select id from DocumentTypes where code = 'ARP' and Tenant = 0  ))

	if(@TemplateBody_Zero <> @TemplateBody)
	print convert(varchar(20),@Tenant)

	FETCH NEXT FROM DocumentTypeTemplates_Cursor INTO @Id,@Tenant,@TemplateBody
	END
	CLOSE DocumentTypeTemplates_Cursor
	DEALLOCATE DocumentTypeTemplates_Cursor
END

--Available space on db after shrink
SELECT
     LogicalName = dbf.name
    ,FileType = dbf.type_desc
    ,FilegroupName = fg.name
    ,PhysicalFileLocation = dbf.physical_name
    ,FileSizeMB = CONVERT(DECIMAL(10,2),dbf.size/128.0)
    ,UsedSpaceMB = CONVERT(DECIMAL(10,2),dbf.size/128.0 - ((dbf.size/128.0)
               - CAST(FILEPROPERTY(dbf.name, 'SPACEUSED') AS INT) /128.0))
    ,FreeSpaceMB = CONVERT(DECIMAL(10,2),dbf.size/128.0
           - CAST(FILEPROPERTY(dbf.name, 'SPACEUSED') AS INT)/128.0)
FROM sys.database_files dbf
LEFT JOIN sys.filegroups fg ON dbf.data_space_id = fg.data_space_id
ORDER BY dbf.type DESC, dbf.name;

---------
---Active contacts with EAWB or Booking package
---------
select tenantmanagements.distributorcode,tenantmanagements.packagecode,tenantmanagements.Name,globalcontacts.Email from globalcontacts 
join globaltenants on globaltenants.id = globalcontacts.globaltenantid 
join tenantmanagements on globaltenants.id = tenantmanagements.id
where globalcontacts.inactive = 0 and tenantmanagements.packagecode in('EAWB','BUBK') and globalcontacts.isuser = 1
and globaltenants.isactive = 1 and globalcontacts.Email not like ('%system@tenant%')
and tenantmanagements.Name not in('AHtml 05','Zaki Sand-Box') 
group by tenantmanagements.distributorcode,tenantmanagements.packagecode,tenantmanagements.Name,globalcontacts.Email
order by tenantmanagements.name

--------
-------- Active Users using Firefox
select * from UserLoginLogs 
join contacts on contacts.id = UserLoginLogs.userid
join tenants on tenants.id = UserLoginLogs.Tenant
where UserLoginLogs.browser like '%Fire%' and contacts.inactive = 0 and UserLoginLogs.Tenant not in(0,1,42,65) 
and contacts.email not like '%logitudeworld%'
and contacts.email not like '%fnarsoft%'
and contacts.email not like '%amital%'
and contacts.email not like '%champ.aero%'
and contacts.email not like '%@mail.com%'

-------------- Find constraint and drop
DECLARE @ConstraintName nvarchar(200)
SELECT @ConstraintName = Name FROM sys.foreign_keys WHERE parent_object_id = OBJECT_ID('CardExternalCodeByCurrencies') and referenced_object_id =  OBJECT_ID('ExternalSystemsTablesCodes')
IF @ConstraintName IS NOT NULL
EXEC('ALTER TABLE CardExternalCodeByCurrencies DROP CONSTRAINT ' + @ConstraintName)
----------------------Spanish translation
select objecttables.name,textcodes.code,textcodes.defaulttext,textcodes.tenant from textcodes 
join objecttables on objecttables.id = textcodes.objecttableid
where textcodes.objecttableid in(select id from objecttables where name like '%receivable%' or name like '%transfer%' or name like '%ARinvoice%')
and textcodes.defaulttext <> '' and textcodes.tenant = 0
and textcodes.id not in ( select textcodeid from translations where tenant = 1128)
order by objecttables.name

------------  all index that have FRAGMENT_PERCENT over 30% most rebuild.

SELECT  OBJECT_NAME(DMV.object_id) AS TABLE_NAME ,
    SI.NAME AS INDEX_NAME ,
    avg_fragmentation_in_percent AS FRAGMENT_PERCENT ,
    DMV.record_count
FROM    sys.dm_db_index_physical_stats(DB_ID(), NULL, NULL, NULL, 'SAMPLED')
    AS DMV
    LEFT OUTER JOIN SYS.INDEXES AS SI ON DMV.OBJECT_ID = SI.OBJECT_ID
                                         AND DMV.INDEX_ID = SI.INDEX_ID
WHERE   avg_fragmentation_in_percent > 10
    AND index_type_desc IN ( 'CLUSTERED INDEX', 'NONCLUSTERED INDEX' )
    AND DMV.record_count >= 2000
ORDER BY TABLE_NAME DESC

----------ShipmentAWBPrintOnlies
select tenants.company,shipmentmasterdatas.AirlinePrefix,cards.englishname,* from ShipmentAWBPrintOnlies
join tenants on tenants.id = ShipmentAWBPrintOnlies.tenant
join shipmentmasterdatas on shipmentmasterdatas.id = ShipmentAWBPrintOnlies.shipmentid 
join airlines on airlines.prefix = shipmentmasterdatas.AirlinePrefix and airlines.tenant = 0
join cards on cards.id = airlines.id
 where ShipmentAWBPrintOnlies.iatacodeid in (select id From IATACodes where code = 'ci')--where iatacodecode = 'CI'
 ------------------
 update countries
set hasstates = 1,isstaterequired = 1
where code in('AU','CA','US') and hasstates = 0
and contacts.email not like '%@mail.com%'


-------
-------
select UserLoginLogs.Browser,contacts.Email,contacts.englishname from UserLoginLogs 
join contacts on contacts.id = UserLoginLogs.userid
join tenants on tenants.id = UserLoginLogs.Tenant
where UserLoginLogs.browser like '%Fire%' and contacts.inactive = 0 and UserLoginLogs.Tenant not in(0,1,42,65) 
and contacts.email not like '%logitudeworld%'
and contacts.email not like '%fnarsoft%'
and contacts.email not like '%amital%'
and contacts.email not like '%champ.aero%'
and contacts.email not like '%@mail.com%'
and UserLoginLogs.Gmtdatetime between '2016-09-01' and '2017-03-01'
group by  UserLoginLogs.Browser,contacts.Email,contacts.englishname


-- Morrocian Tenant - inactive some doc. types
update documenttypes 
set inactive = 1
where code in ('08F.EXP','861','863','AVDE','AVIS') and tenant not in 
('232',
'235',
'237',
'239',
'240',
'242',
'247',
'248',
'249',
'250',
'251',
'252',
'253',
'255',
'261',
'266',
'267',
'268',
'269',
'270',
'271',
'272',
'273',
'274',
'275',
'276',
'277',
'278',
'279',
'280',
'281',
'282',
'283',
'284',
'285',
'286',
'287',
'288',
'289',
'291',
'292',
'293',
'295',
'331',
'468',
'530',
'565',
'799',
'802',
'807',
'992',
'1056',
'1116',
'1117',
'1129',
'1130',
'1131',
'1132',
'1133',
'1145',
'1149',
'1151',
'1197',
'1250',
'1259',
'1273',
'1281',
'1287',
'1294',
'1296')