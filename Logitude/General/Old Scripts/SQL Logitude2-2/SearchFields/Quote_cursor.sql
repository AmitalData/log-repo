DECLARE @Id varchar(15)
-----------------------
DECLARE @QuoteNumber varchar(250)
DECLARE @ShipperReference1 varchar(250)
DECLARE @ShipperReference2 varchar(250)
DECLARE @ConsigneeReference1 varchar(250)
DECLARE @ConsigneeReference2 varchar(250)
DECLARE @ShipperName varchar(250)
DECLARE @ConsigneeName varchar(250)
DECLARE @FromPortId varchar(250)
DECLARE @ToPortId varchar(250)
DECLARE @FromPortCode varchar(250)
DECLARE @FromPortName varchar(250)
DECLARE @ToPortCode varchar(250)
DECLARE @ToPortName varchar(250)
DECLARE @CustomerReference varchar(250)
DECLARE @PotentialCustomerId varchar(250)
DECLARE @PotentialCustomerName varchar(250)
DECLARE @CustomerId varchar(250)
DECLARE @CustomerName varchar(250)
DECLARE @PotentialShipperId varchar(250)
DECLARE @PotentialShipperName varchar(250)
DECLARE @PotentialConsigneeId varchar(250)
DECLARE @PotentialConsigneeName varchar(250)
DECLARE @MainCarriageCarrierId varchar(250)
DECLARE @MainCarriageCarrierCode varchar(250)
DECLARE @MainCarriageCarrierName varchar(250)


DECLARE quotecursor CURSOR READ_ONLY
FOR
SELECT Id,QuoteNumber,ShipperReference1,ShipperReference2,ConsigneeReference1,ConsigneeReference2,
ShipperName,ConsigneeName,FromPortId,ToPortId,MainCarriageCarrierId,CustomerReference,PotentialCustomerId,CustomerId,
PotentialShipperId,PotentialConsigneeId
FROM Quotes

OPEN quotecursor

	FETCH NEXT FROM quotecursor
	INTO @Id,@QuoteNumber,@ShipperReference1,@ShipperReference2,@ConsigneeReference1,@ConsigneeReference2,
	@ShipperName,@ConsigneeName,@FromPortId,@ToPortId,@MainCarriageCarrierId,@CustomerReference,@PotentialCustomerId,@CustomerId,
	@PotentialShipperId,@PotentialConsigneeId


WHILE @@FETCH_STATUS = 0

BEGIN

PRINT @Id

set @FromPortCode = (select Code from Ports
Where Id = @FromPortId)

set @FromPortName = (select EnglishName from Ports
Where Id = @FromPortId)

set @ToPortCode = (select Code from Ports
Where Id = @ToPortId)

set @ToPortName = (select EnglishName from Ports
Where Id = @ToPortId)

set @MainCarriageCarrierCode = (select Code from Cards
Where Id = @MainCarriageCarrierId)

set @MainCarriageCarrierName = (select EnglishName from Cards
Where Id = @MainCarriageCarrierId)

set @PotentialCustomerName = (select EnglishName from PotentialCustomers
Where Id = @PotentialCustomerId)

set @CustomerName = (select EnglishName from Cards
Where Id = @CustomerId)

set @PotentialShipperName = (select EnglishName from PotentialCustomers
Where Id = @PotentialShipperId)

set @PotentialConsigneeName = (select EnglishName from PotentialCustomers
Where Id = @PotentialConsigneeId)

    Update Quotes
set SearchFields = 
isnull(@QuoteNumber,'') + ',' + 
isnull(@ShipperReference1,'') + ',' + 
isnull(@ShipperReference2,'') + ',' + 
isnull(@ConsigneeReference1,'') + ',' + 
isnull(@ConsigneeReference2,'') + ',' + 
isnull(@ShipperName,'') + ',' + 
isnull(@ConsigneeName,'') + ',' + 
isnull(@CustomerReference,'') + ',' + 
isnull(@FromPortCode,'') + ',' + 
isnull(@FromPortName,'') + ',' + 
isnull(@ToPortCode,'') + ',' + 
isnull(@ToPortName,'') + ',' + 
isnull(@MainCarriageCarrierCode,'') + ',' + 
isnull(@MainCarriageCarrierName,'') + ',' + 
isnull(@PotentialCustomerName,'') + ',' + 
isnull(@PotentialShipperName,'') + ',' + 
isnull(@PotentialConsigneeName,'') + ','
	where Id = @Id

	FETCH NEXT FROM quotecursor
	INTO @Id,@QuoteNumber,@ShipperReference1,@ShipperReference2,@ConsigneeReference1,@ConsigneeReference2,
	@ShipperName,@ConsigneeName,@FromPortId,@ToPortId,@MainCarriageCarrierId,@CustomerReference,@PotentialCustomerId,@CustomerId,
	@PotentialShipperId,@PotentialConsigneeId
END

CLOSE quotecursor
DEALLOCATE quotecursor