
drop PROCEDURE [dbo].[usp_DeleteHybridTenantFromMainDB] 
go
Create  PROCEDURE [dbo].[usp_DeleteHybridTenantFromMainDB] 
(

@tenant int                     --Input parameter ,  tenant to delete

)
AS


BEGIN

--declare @tenant int;
--set @tenant = 34;
----delete tenants stuff
--delete from accountingsettings
--where id=@tenant

--delete from contacttenantroleset
--where tenant=@tenant

--delete from restrictions 
--where tenant =@tenant

--delete from contacttenants
--where tenantid=@tenant
 

update  tenants set currencyId = null,AddressId = null,FreightCurrencyId = null,PaymentTermId = null,ProfitCurrencyId = null,AgentId = null,OtherChargesCurrencyId = null,QuoteSaleCurrencyId = null
where id=@tenant


-------------------------------------



delete from followups
where tenant=@tenant
--delete shipments for tenant



delete from insideshipmentpackages
where tenant=@tenant

delete from shipmentpackages
where tenant=@tenant

delete from ShipmentPickUpDeliveryPackages
where tenant=@tenant

delete from shipmentpickupdeliveries
where tenant=@tenant

delete from shipmentorderpackages
where tenant=@tenant

delete from shipmentpayables
where tenant=@tenant

delete from shipmentreceivables
where tenant=@tenant

delete from shipmentawbprintonlies
where tenant=@tenant
 
delete from shipmentmasterdatas
where tenant=@tenant

delete from shipments
where tenant=@tenant

---end of shipment deletions

--delete apinvoices stuff-------------

delete from apinvoiceentities
where tenant=@tenant

delete from apinvoicelines
where tenant=@tenant

delete from apinvoicetotalvats
where tenant=@tenant

delete from apinvoicepayments
where tenant=@tenant

delete from appayments
where tenant=@tenant

delete from apinvoices
where tenant=@tenant

--delete arinvoices stuff-------------

delete from arinvoiceentities
where tenant=@tenant

delete from arinvoicelines
where tenant=@tenant

delete from arinvoicetotalvats
where tenant=@tenant

delete from arinvoicepayments
where tenant=@tenant

delete from arpayments
where tenant=@tenant

delete from arinvoices
where tenant=@tenant

-----------------------------------------------------------------

--delete quotes
delete from quotepricesteps
where tenant=@tenant

delete from quotecharges
where tenant=@tenant

delete from quotes
where tenant=@tenant

delete from CustomerAdditionalServices
where tenant =@tenant

--------------------------------------------

--delete from communicationattachments
--where tenant=@tenant

--delete from communicationlogs
--where tenant=@tenant

--delete from documentoutcopies
--where tenant=@tenant

--delete from documentouts
--where tenant=@tenant

--delete from documentins
--where tenant=@tenant

--delete from documenttypecopies
--where tenant=@tenant

--delete from documenttypecustomfields1
--where tenant=@tenant

--delete from formcustomfields1
--where tenant=@tenant

--delete from documenttypetemplates
--where tenant=@tenant

--delete from documenttypes
--where tenant=@tenant

delete from mawbstacks
where tenant=@tenant


--delete from Customs.SupplierInvoices
--where Tenant = @tenant

--delete from Customs.Consignments
--where Tenant = @tenant


--delete from Customs.Declarations
--where Tenant = @tenant

--delete from Competitors
--where Tenant = @tenant


--delete from OpportunityCompetitors
--where Tenant = @tenant


--delete from OpportunityCompetitorProducts
--where Tenant = @tenant

--delete from OpportunityProductLocations
--where Tenant = @tenant

--delete from OpportunityProducts
--where Tenant = @tenant

--delete from Opportunities
--where tenant = @tenant
----------------------------------------------------------------

--delete from Activities
--where Tenant = @tenant


-------------------delete cards but card and addresses is for last



 

delete from agents
where tenant=@tenant

delete from customers
where tenant=@tenant

delete from airlines
where tenant=@tenant

delete from truckers
where tenant=@tenant

delete from shippinglines
where tenant=@tenant

delete from vendors
where tenant=@tenant

delete from warehouses
where tenant=@tenant

delete from Customagents
where tenant=@tenant

delete from shippingagents
where tenant=@tenant

delete from potentialcustomers
where tenant=@tenant

delete from addresses
where tenant=@tenant

delete from cardcontacts
where tenant=@tenant

delete from cards
where tenant=@tenant





----------------------------------------
-- billing stuff
--delete from incoterms
--where tenant=@tenant

--delete from ratestables
--where tenant=@tenant

--delete from currencies
--where tenant=@tenant

--delete from paymentterms
--where tenant=@tenant

--delete from chargestypes
--where tenant=@tenant

--delete from VatTypePercentages
--where tenant=@tenant

--delete from vattypes
--where tenant=@tenant

--delete from Accounts1
--where tenant=@tenant
-----------------------------

-----------delete location stuff
 

--delete from ports
--where tenant=@tenant

--delete from states
--where tenant=@tenant
 
--delete from Countries
--where tenant=@tenant

--delete from globalzones
--where tenant=@tenant


--delete from tarrifsteps
--where tenant=@tenant

--delete from tarriffromtoes
--where tenant=@tenant

--delete from tarrifcharges
--where tenant=@tenant

--delete from tarrifheaders
--where tenant=@tenant
----------------------------------------
delete from PostLikes
where Tenant = @tenant

delete from Feeds
where Tenant = @tenant

delete from Posts
where Tenant = @tenant

delete from FollowEntities
where Tenant = @tenant

delete from Followers
where Tenant = @tenant

----------------------------------
---------contact users branch departments

--delete from Translations
--where Tenant = @tenant

--delete from TraceEvents
--where Tenant = @tenant

--delete from userloginlogs
--where tenant=@tenant

--delete from traceevents
--where tenant=@tenant

--delete from EntityLastActivities
--where tenant=@tenant

--delete from UserLastLogins
--where tenant=@tenant

--delete from users
--where tenant=@tenant


--delete from TermsofUseSignatures
--where tenant=@tenant

--delete from contacts
--where tenant=@tenant

--delete from branches
--where tenant=@tenant

--delete from departments
--where tenant=@tenant

delete from vessels
where tenant=@tenant

---------------------------------------

--delete from eventtypes
--where tenant=@tenant

--delete from entitystatus
--where tenant=@tenant

--delete from querycolumns
--where tenant=@tenant

--delete from advancedqueryfilters
--where tenant=@tenant

--delete from queries
--where tenant=@tenant

--delete from blobs
--where tenant=@tenant

--delete from counterdefinitions
--where tenant=@tenant

--delete from counterlastnumbers
--where tenant=@tenant

--delete from counterstats
--where tenant=@tenant

--delete from counters
--where tenant=@tenant

--delete from customtables
--where tenant=@tenant

--delete from descriptionofgoods
--where tenant=@tenant

--delete from entitylastaccesses
--where tenant=@tenant

--delete from packagetypes
--where tenant=@tenant

--delete from measurements
--where tenant=@tenant

--delete from ranks
--where tenant=@tenant


----------------------------------
-------------------metadata stuff

--delete from objecttablerulefields
--where tenant=@tenant

--delete from RuleConditionFields
--where tenant=@tenant

--delete from objecttablerules
--where tenant=@tenant

--delete from objectfieldvalidations
--where tenant=@tenant

--delete from screenfields
--where tenant=@tenant


--delete from objectfields
--where tenant=@tenant

--delete from objecttabletabs
--where tenant=@tenant

--delete from objecttablehelpercontrols
--where tenant=@tenant

--delete from menuButtons
--where tenant=@tenant

--delete from menubuttonGroups
--where tenant=@tenant

--delete from menustables
--where tenant=@tenant

--update objecttables
--set DescriptionTextCodeId=null
--where tenant=@tenant

--delete from textcodes
--where tenant=@tenant

--update objecttables
--set HeaderScreenId=null
--where tenant=@tenant

--delete from screenfields
--where tenant=@tenant

--delete from screens
--where tenant=@tenant

--delete from objecttables
--where tenant =@tenant

--delete from tips
--where tenant=@tenant

--delete from rolefeatures
--where tenant=@tenant

--delete from features
--where tenant=@tenant

--delete from roles
--where tenant=@tenant



END

