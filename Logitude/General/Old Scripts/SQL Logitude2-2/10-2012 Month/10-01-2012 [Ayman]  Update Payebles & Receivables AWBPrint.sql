

DECLARE @EntityId AS varchar(15)
DECLARE @EntityCurrencyId AS varchar(15)
DECLARE @ShipmentId AS varchar(15)
DECLARE @ShipmentAWBCurrencyId AS varchar(15)

BEGIN; -- Update Payables	
	DECLARE PayablesCursor CURSOR READ_ONLY
	FOR	
	SELECT Id,CurrencyId,ShipmentId
	FROM ShipmentPayables
	Where AWBPrint = 1
	OPEN PayablesCursor FETCH NEXT FROM PayablesCursor INTO @EntityId,@EntityCurrencyId,@ShipmentId
	WHILE @@FETCH_STATUS = 0
	BEGIN;
		
		set @ShipmentAWBCurrencyId = (Select AWBCurrencyId from Shipments where Id = @ShipmentId)

		if(@ShipmentAWBCurrencyId <> @EntityCurrencyId)
		begin;
			update ShipmentPayables set AWBPrint = 0 where Id = @EntityId
		end


	FETCH NEXT FROM PayablesCursor INTO @EntityId,@EntityCurrencyId,@ShipmentId
	END
CLOSE PayablesCursor
DEALLOCATE PayablesCursor
END


BEGIN; -- Update Receivables	
	DECLARE ReceivablesCursor CURSOR READ_ONLY
	FOR	
	SELECT Id,CurrencyId,ShipmentId
	FROM ShipmentReceivables
	Where AWBPrint = 1
	OPEN ReceivablesCursor FETCH NEXT FROM ReceivablesCursor INTO @EntityId,@EntityCurrencyId,@ShipmentId
	WHILE @@FETCH_STATUS = 0
	BEGIN;
		
		set @ShipmentAWBCurrencyId = (Select AWBCurrencyId from Shipments where Id = @ShipmentId)

		if(@ShipmentAWBCurrencyId <> @EntityCurrencyId)
		begin;
			update ShipmentReceivables set AWBPrint = 0 where Id = @EntityId
		end


	FETCH NEXT FROM ReceivablesCursor INTO @EntityId,@EntityCurrencyId,@ShipmentId
	END
CLOSE ReceivablesCursor
DEALLOCATE ReceivablesCursor

END