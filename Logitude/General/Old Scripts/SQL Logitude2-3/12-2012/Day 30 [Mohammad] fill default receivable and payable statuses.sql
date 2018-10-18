declare @OpenReceivablesInLocalCurrency float
declare @AccountedReceivablesInLocalCurrency float
declare @OpenPayablesInLocalCurrency float
declare @AccountedPayablesInLocalCurrency float
declare @Id varchar(15)
declare @Tenant int


DECLARE updatecursor CURSOR READ_ONLY
FOR
SELECT Id,Tenant,OpenReceivablesInLocalCurrency,AccountedReceivablesInLocalCurrency,OpenPayablesInLocalCurrency,AccountedPayablesInLocalCurrency
FROM Shipments

OPEN updatecursor

	FETCH NEXT FROM updatecursor
	INTO @Id,@Tenant,@OpenReceivablesInLocalCurrency,@AccountedReceivablesInLocalCurrency,@OpenPayablesInLocalCurrency,@AccountedPayablesInLocalCurrency


WHILE @@FETCH_STATUS = 0

BEGIN

    declare @ReceivableStatus varchar(4)

    if(@OpenReceivablesInLocalCurrency=0 and @AccountedReceivablesInLocalCurrency=0)
	Begin
	set @ReceivableStatus='NORE'
	End

	if(@OpenReceivablesInLocalCurrency<>0 and @AccountedReceivablesInLocalCurrency=0)
	Begin
	set @ReceivableStatus='OPEN'
	End

	if(@OpenReceivablesInLocalCurrency=0 and @AccountedReceivablesInLocalCurrency<>0)
	Begin
	set @ReceivableStatus='CLSD'
	End

	if(@OpenReceivablesInLocalCurrency<>0 and @AccountedReceivablesInLocalCurrency<>0)
	Begin
	set @ReceivableStatus='PRIN'
	End
	update shipments set ShipmentReceivableStatusCode=@ReceivableStatus where id=@Id and tenant=@Tenant and ShipmentReceivableStatusCode is null


	  declare @PayableStatus varchar(4)

    if(@OpenPayablesInLocalCurrency=0 and @AccountedPayablesInLocalCurrency=0)
	Begin
	set @PayableStatus='NOPA'
	End

	if(@OpenPayablesInLocalCurrency<>0 and @AccountedPayablesInLocalCurrency=0)
	Begin
	set @PayableStatus='COST'
	End

	if(@OpenPayablesInLocalCurrency=0 and @AccountedPayablesInLocalCurrency<>0)
	Begin
	set @PayableStatus='PAID'
	End

	if(@OpenPayablesInLocalCurrency<>0 and @AccountedPayablesInLocalCurrency<>0)
	Begin
	set @PayableStatus='PRPD'
	End
	update shipments set ShipmentPayableStatusCode=@PayableStatus where id=@Id and tenant=@Tenant and ShipmentPayableStatusCode is null
  
	FETCH NEXT FROM updatecursor
	INTO  @Id,@Tenant,@OpenReceivablesInLocalCurrency,@AccountedReceivablesInLocalCurrency,@OpenPayablesInLocalCurrency,@AccountedPayablesInLocalCurrency

END

CLOSE updatecursor
DEALLOCATE updatecursor