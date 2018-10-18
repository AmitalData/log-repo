DECLARE @Id varchar(15)

-------------------------------
DECLARE @ShipmentNumber varchar(250)
DECLARE @ShipperReference1 varchar(250)
DECLARE @ShipperReference2 varchar(250)
DECLARE @ConsigneeReference1 varchar(250)
DECLARE @ConsigneeReference2 varchar(250)
DECLARE @House varchar(250)
DECLARE @AgentReference1 varchar(250)
DECLARE @AgentReference2 varchar(250)
DECLARE @CustomAgentImportReference varchar(250)
DECLARE @CustomAgentExportReference varchar(250)
DECLARE @FreightForwarderReference varchar(250)
DECLARE @CustomerReference varchar(250)
-------------------------------
DECLARE @ShipperId varchar(15)
DECLARE @ConsigneeId varchar(15)
DECLARE @CustomAgentImportId varchar(15)
DECLARE @Notify1Id varchar(15)
DECLARE @Notify2Id varchar(15)
DECLARE @PreCarriageFromPortId varchar(15)
DECLARE @PreCarriageToPortId varchar(15)
DECLARE @OnCarriageFromPortId varchar(15)
DECLARE @OnCarriageToPortId varchar(15)
DECLARE @AgentId varchar(15)
DECLARE @ShipperNotExporterId varchar(15)
DECLARE @ConsigneeNotImporterId varchar(15)
DECLARE @QuoteId varchar(15)
DECLARE @CustomAgentExportId varchar(15)
DECLARE @CustomerId varchar(15)
DECLARE @FromPortId varchar(15)
DECLARE @ToPortId varchar(15)
DECLARE @MasterShipmentDataId varchar(15)
----------------------------------------
DECLARE @ShipperName varchar(250)
DECLARE @ConsigneeName varchar(250)
DECLARE @MasterNumber varchar(250)
DECLARE @CustomAgentImportName varchar(250)
DECLARE @Notify1Name varchar(250)
DECLARE @Notify2Name varchar(250)
DECLARE @PreCarriageFromPortCode varchar(250)
DECLARE @PreCarriageFromPortName varchar(250)
DECLARE @PreCarriageToPortCode varchar(250)
DECLARE @PreCarriageToPortName varchar(250)
DECLARE @OnCarriageFromPortCode varchar(250)
DECLARE @OnCarriageFromPortName varchar(250)
DECLARE @OnCarriageToPortCode varchar(250)
DECLARE @OnCarriageToPortName varchar(250)
DECLARE @AgentName varchar(250)
DECLARE @ShipperNotExporterName varchar(250)
DECLARE @ConsigneeNotImporterName varchar(250)
DECLARE @CustomAgentExportName varchar(250)
DECLARE @CustomerName varchar(250)
DECLARE @QuoteNumber varchar(250)
DECLARE @MainCarriageFinalDestinationPortCode varchar(250)
DECLARE @MainCarriageFinalDestinationPortName varchar(250)
-------------------------------------------------------------



DECLARE shipmentcursor CURSOR READ_ONLY
FOR
SELECT Id,ShipmentNumber,ShipperReference1,ShipperReference2,ConsigneeReference1,
ConsigneeReference2,House,AgentReference1,AgentReference2,CustomAgentImportReference,
CustomAgentExportReference,FreightForwarderReference,CustomerReference,
ShipperId,ConsigneeId,CustomAgentImportId,Notify1Id,Notify2Id,
PreCarriageFromPortId,PreCarriageToPortId,OnCarriageFromPortId,OnCarriageToPortId,
AgentId,ShipperNotExporterId,ConsigneeNotImporterId,QuoteId,CustomAgentExportId,CustomerId,
FromPortId,ToPortId,MasterShipmentDataId
FROM Shipments

OPEN shipmentcursor

	FETCH NEXT FROM shipmentcursor
	INTO @Id,@ShipmentNumber,@ShipperReference1,@ShipperReference2,@ConsigneeReference1,
	@ConsigneeReference2,@House,@AgentReference1,@AgentReference2,@CustomAgentImportReference,
	@CustomAgentExportReference,@FreightForwarderReference,@CustomerReference,
	@ShipperId,@ConsigneeId,@CustomAgentImportId,@Notify1Id,@Notify2Id,
	@PreCarriageFromPortId,@PreCarriageToPortId,@OnCarriageFromPortId,@OnCarriageToPortId,
	@AgentId,@ShipperNotExporterId,@ConsigneeNotImporterId,@QuoteId,@CustomAgentExportId,@CustomerId,
	@FromPortId,@ToPortId,@MasterShipmentDataId


WHILE @@FETCH_STATUS = 0

BEGIN

PRINT @Id

set @ShipperName = (select EnglishName from Cards
Where Id = @ShipperId)

set @ConsigneeName = (select EnglishName from Cards
Where Id = @ConsigneeId)

set @CustomAgentImportName = (select EnglishName from Cards
Where Id = @CustomAgentImportId)

set @Notify1Name = (select EnglishName from Cards
Where Id = @Notify1Id)

set @Notify2Name = (select EnglishName from Cards
Where Id = @Notify2Id)

set @PreCarriageFromPortCode = (select Code from Ports
Where Id = @PreCarriageFromPortId)

set @PreCarriageFromPortName = (select EnglishName from Ports
Where Id = @PreCarriageFromPortId)

set @PreCarriageToPortCode = (select Code from Ports
Where Id = @PreCarriageToPortId)

set @PreCarriageToPortName = (select EnglishName from Ports
Where Id = @PreCarriageToPortId)

set @OnCarriageFromPortCode = (select Code from Ports
Where Id = @OnCarriageFromPortId)

set @OnCarriageFromPortName = (select EnglishName from Ports
Where Id = @OnCarriageFromPortId)

set @OnCarriageToPortCode = (select Code from Ports
Where Id = @OnCarriageToPortId)

set @OnCarriageToPortName = (select EnglishName from Ports
Where Id = @OnCarriageToPortId)

set @AgentName = (select EnglishName from Cards
Where Id = @AgentId)

set @ShipperNotExporterName = (select EnglishName from Cards
Where Id = @ShipperNotExporterId)

set @ConsigneeNotImporterName = (select EnglishName from Cards
Where Id = @ConsigneeNotImporterId)

set @CustomAgentExportName = (select EnglishName from Cards
Where Id = @CustomAgentExportId)

set @CustomerName = (select EnglishName from Cards
Where Id = @CustomerId)

set @QuoteNumber = (select QuoteNumber from Quotes
Where Id = @QuoteId)

---------
declare @MainCarriageFinalDestinationPortId varchar(250)
set @MainCarriageFinalDestinationPortId = (select Id from ShipmentMasterDatas
Where Id = @MasterShipmentDataId)
---------
set @MainCarriageFinalDestinationPortCode = (select Code from Ports
Where Id = @MainCarriageFinalDestinationPortId)

set @MainCarriageFinalDestinationPortName = (select EnglishName from Ports
Where Id = @MainCarriageFinalDestinationPortId)

set @MasterNumber = (select MasterShipmentNumber from ShipmentMasterDatas
Where Id = @MasterShipmentDataId)

--Container Number cursor--
DECLARE @PackageId varchar(15)
DECLARE @ContainerNumber varchar(250)
SET @ContainerNumber = ''
 DECLARE containercursor CURSOR READ_ONLY
FOR
SELECT Id
FROM ShipmentPackages
where ShipmentId=@Id

OPEN containercursor

	FETCH NEXT FROM containercursor
	INTO @PackageId

WHILE @@FETCH_STATUS = 0

BEGIN

DECLARE @localcontainernumber varchar(250)
set @localcontainernumber = (select ContainerNumber from ShipmentPackages
Where Id = @PackageId)

 set @ContainerNumber =  @ContainerNumber +  isnull(@localcontainernumber,'') + ','



	FETCH NEXT FROM containercursor
	INTO @PackageId
END

CLOSE containercursor
DEALLOCATE containercursor
--------------------

--Invoice Number cursor----
 DECLARE @InvoiceId varchar(250)
 DECLARE @InvoiceNumber varchar(250)
 SET @InvoiceNumber = ''

 DECLARE invoicecursor CURSOR READ_ONLY
FOR
SELECT InvoiceId
FROM InvoiceEntities
where EntityId=@Id

OPEN invoicecursor

	FETCH NEXT FROM invoicecursor
	INTO @InvoiceId

WHILE @@FETCH_STATUS = 0

BEGIN

DECLARE @localinvoicenumber varchar(250)
set @localinvoicenumber = (select InvoiceNumber from Invoices
Where Id = @InvoiceId)

SET @InvoiceNumber = @InvoiceNumber + isnull(@localinvoicenumber,'')  + ','

	FETCH NEXT FROM invoicecursor
	INTO @InvoiceId
END

CLOSE invoicecursor
DEALLOCATE invoicecursor
--------------------
print @InvoiceNumber

    Update Shipments
set SearchFields = 
isnull(@ShipmentNumber,'') + ',' + 
 isnull(@ShipperReference1,'') + ',' +
  isnull(@ShipperReference2,'') + ',' +
   isnull(@ConsigneeReference1,'') + ',' +
    isnull(@ConsigneeReference2,'') + ',' +
   isnull(@House,'') + ',' +
    isnull(@AgentReference1,'') + ',' +
   isnull(@AgentReference2,'') + ',' +
    isnull(@CustomAgentImportReference,'') + ',' +
	 isnull(@CustomAgentExportReference,'') + ',' +
	  isnull(@FreightForwarderReference,'') + ',' +
	   isnull(@CustomerReference,'') + ',' + 
	    isnull(@ShipperName,'') + ',' +
		 isnull(@ConsigneeName,'') + ',' +
		  isnull(@CustomAgentImportName,'') + ',' +
		   isnull(@Notify1Name,'') + ',' +
		    isnull(@Notify2Name,'') + ',' +
			isnull(@PreCarriageFromPortCode,'') + ',' +
			isnull(@PreCarriageFromPortName,'') + ',' +
			isnull(@PreCarriageToPortCode,'') + ',' +
			isnull(@PreCarriageToPortName,'') + ',' +
			isnull(@OnCarriageFromPortCode,'') + ',' +
			isnull(@OnCarriageFromPortName,'') + ',' +
			isnull(@OnCarriageToPortCode,'') + ',' +
			isnull(@OnCarriageToPortName,'') + ',' +
			isnull(@AgentName,'') + ',' +
			isnull(@ShipperNotExporterName,'') + ',' +
			isnull(@ConsigneeNotImporterName,'') + ',' +
			isnull(@CustomAgentExportName,'') + ',' +
			isnull(@CustomerName,'') + ',' +
			isnull(@QuoteNumber,'') + ',' +
			isnull(@MainCarriageFinalDestinationPortCode,'') + ',' +
			isnull(@MainCarriageFinalDestinationPortName,'') + ',' +
			isnull(@ContainerNumber,'') + ',' +
			isnull(@InvoiceNumber,'') + ',' +
		  isnull(@MasterNumber,'') + ','
	where Id = @Id


	FETCH NEXT FROM shipmentcursor
	INTO @Id,@ShipmentNumber,@ShipperReference1,@ShipperReference2,@ConsigneeReference1,
	@ConsigneeReference2,@House,@AgentReference1,@AgentReference2,@CustomAgentImportReference,
	@CustomAgentExportReference,@FreightForwarderReference,@CustomerReference,
	@ShipperId,@ConsigneeId,@CustomAgentImportId,@Notify1Id,@Notify2Id,
	@PreCarriageFromPortId,@PreCarriageToPortId,@OnCarriageFromPortId,@OnCarriageToPortId,
	@AgentId,@ShipperNotExporterId,@ConsigneeNotImporterId,@QuoteId,@CustomAgentExportId,@CustomerId,
	@FromPortId,@ToPortId,@MasterShipmentDataId
END

CLOSE shipmentcursor
DEALLOCATE shipmentcursor