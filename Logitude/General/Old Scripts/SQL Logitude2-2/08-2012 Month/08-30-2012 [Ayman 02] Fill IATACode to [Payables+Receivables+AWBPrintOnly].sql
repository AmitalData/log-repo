

DECLARE @EntityTenant AS INT
DECLARE @EntityId AS varchar(15)	
DECLARE @EntityIATACodeCode AS varchar(5)
DECLARE @EntityMeasurementId AS varchar(15)
DECLARE @EntityChargesTypeId AS varchar(15)

DECLARE @ChargeIATACodeCode AS varchar(5)
DECLARE @ChargeMeasurementId AS varchar(15)

BEGIN; -- Update AWBPrint	
	DECLARE AWBPrintCursor CURSOR READ_ONLY
	FOR	
	SELECT Id,Tenant,ChargesTypeId,IATACodeCode,MeasurementId
	FROM ShipmentAWBPrintOnlies 
	OPEN AWBPrintCursor FETCH NEXT FROM AWBPrintCursor INTO @EntityId,@EntityTenant,@EntityChargesTypeId,@EntityIATACodeCode,@EntityMeasurementId
	WHILE @@FETCH_STATUS = 0
	BEGIN

		set @ChargeIATACodeCode = (SELECT IATACodeCode From ChargesTypes where Id = @EntityChargesTypeId AND Tenant = @EntityTenant)
		set @ChargeMeasurementId = (SELECT MeasurementId From ChargesTypes where Id = @EntityChargesTypeId AND Tenant = @EntityTenant)	
	
		if (@EntityIATACodeCode is null)
		begin;
		update ShipmentAWBPrintOnlies set IATACodeCode = @ChargeIATACodeCode where Id = @EntityId
		end

		if (@EntityMeasurementId is null)
		begin;
		update ShipmentAWBPrintOnlies set MeasurementId = @ChargeMeasurementId where Id = @EntityId
		end

	FETCH NEXT FROM AWBPrintCursor INTO @EntityId,@EntityTenant,@EntityChargesTypeId,@EntityIATACodeCode,@EntityMeasurementId
	END
	CLOSE AWBPrintCursor
	DEALLOCATE AWBPrintCursor
END

BEGIN; -- Update Payables	
	DECLARE PayablesCursor CURSOR READ_ONLY
	FOR	
	SELECT Id,Tenant,ChargesTypeId,IATACodeCode
	FROM ShipmentPayables 
	OPEN PayablesCursor FETCH NEXT FROM PayablesCursor INTO @EntityId,@EntityTenant,@EntityChargesTypeId,@EntityIATACodeCode
	WHILE @@FETCH_STATUS = 0
	BEGIN

			set @ChargeIATACodeCode = (SELECT IATACodeCode From ChargesTypes where Id = @EntityChargesTypeId AND Tenant = @EntityTenant)
			
			if (@EntityIATACodeCode is null)
			begin;
			update ShipmentPayables set IATACodeCode = @ChargeIATACodeCode where Id = @EntityId
			end

	FETCH NEXT FROM PayablesCursor INTO @EntityId,@EntityTenant,@EntityChargesTypeId,@EntityIATACodeCode
	END
	CLOSE PayablesCursor
	DEALLOCATE PayablesCursor
END

BEGIN; -- Update Receivables	
	DECLARE ReceivablesCursor CURSOR READ_ONLY
	FOR	
	SELECT Id,Tenant,ChargesTypeId,IATACodeCode
	FROM ShipmentReceivables 
	OPEN ReceivablesCursor FETCH NEXT FROM ReceivablesCursor INTO @EntityId,@EntityTenant,@EntityChargesTypeId,@EntityIATACodeCode
	WHILE @@FETCH_STATUS = 0
	BEGIN

			set @ChargeIATACodeCode = (SELECT IATACodeCode From ChargesTypes where Id = @EntityChargesTypeId AND Tenant = @EntityTenant)
			
			if (@EntityIATACodeCode is null)
			begin;
			update ShipmentReceivables set IATACodeCode = @ChargeIATACodeCode where Id = @EntityId
			end

	FETCH NEXT FROM ReceivablesCursor INTO @EntityId,@EntityTenant,@EntityChargesTypeId,@EntityIATACodeCode
	END
	CLOSE ReceivablesCursor
	DEALLOCATE ReceivablesCursor
END