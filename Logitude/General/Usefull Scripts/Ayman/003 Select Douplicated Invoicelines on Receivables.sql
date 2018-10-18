
--select ReceivableId, count(*) from ARInvoiceLines
--where ReceivableId is not null
--group by ReceivableId
--having count(*) > 1

--AC,Auto Credit
------------------------AD,Unpaid
--AR,Auto Credited
------------------------CN,Connected
------------------------DR,Draft
------------------------NT,Not Connected
------------------------PD,Paid
------------------------PP,Partially Paid
--VD,Void

declare @Tenant as int
declare @Count as int
declare @ShipmentId as varchar(15)
declare @ReceivableId as varchar(15)
declare @ARInvoiceId as varchar(15)
declare @StatusCode varchar(2)
declare @MainARInvoiceId as varchar(15)
declare @MainARInvoiceLineId as varchar(15)
declare @UpdateDate as datetime

declare @MemoryTable table
(
  ReceivableId varchar(15) not null,
  ShipmentId varchar(15) not null,
  Tenant integer not null,
  Count integer not null,
  InvoiceId varchar(15),
  InvoiceStatus varchar(2),
  InvoiceLineId varchar(15),
  AllInvoicesIds varchar(500),
  AllInvoicesStatus varchar(500),
  UpdateDate datetime null
)

BEGIN
		DECLARE DataCursor1 CURSOR READ_ONLY
		FOR
		SELECT Id, Tenant, ShipmentId, ARInvoiceId, ARInvoiceLineId, UpdateDate
		FROM ShipmentReceivables
		where ARInvoiceLineId is not null
		OPEN DataCursor1 FETCH NEXT FROM DataCursor1 INTO @ReceivableId, @Tenant, @ShipmentId, @MainARInvoiceId, @MainARInvoiceLineId, @UpdateDate
		WHILE @@FETCH_STATUS = 0
		BEGIN

			set @Count = (select count(*) from ARInvoiceLines where ReceivableId = @ReceivableId and Tenant = @Tenant)
			if (@Count > 1)
			begin

				set @StatusCode = (select StatusCode from ARInvoices where Id = @MainARInvoiceId and Tenant = @Tenant)

				insert into @MemoryTable
				values (
				@ReceivableId,
				@ShipmentId,
				@Tenant, 
				1,
				@MainARInvoiceId,
				@StatusCode,
				@MainARInvoiceLineId,
				@MainARInvoiceId,
				@StatusCode,
				@UpdateDate
				)

				BEGIN
					DECLARE DataCursor2 CURSOR READ_ONLY
					FOR
					SELECT ARInvoiceId
					FROM ARInvoiceLines
					WHERE ReceivableId = @ReceivableId and Tenant = @Tenant
					OPEN DataCursor2 FETCH NEXT FROM DataCursor2 INTO @ARInvoiceId
					WHILE @@FETCH_STATUS = 0
					BEGIN

						if (@ARInvoiceId <> @MainARInvoiceId)
						begin

						set @StatusCode = (select StatusCode from ARInvoices where Id = @ARInvoiceId and Tenant = @Tenant)

						update @MemoryTable
						set
						Count = Count + 1,
						AllInvoicesIds = AllInvoicesIds + ',' + @ARInvoiceId,
						AllInvoicesStatus = AllInvoicesStatus + ',' + @StatusCode
						where ReceivableId = @ReceivableId
						end

					FETCH NEXT FROM DataCursor2 INTO @ARInvoiceId
					END
					CLOSE DataCursor2
					DEALLOCATE DataCursor2
				END

			end

		FETCH NEXT FROM DataCursor1 INTO @ReceivableId, @Tenant, @ShipmentId, @MainARInvoiceId, @MainARInvoiceLineId, @UpdateDate
		END
		CLOSE DataCursor1
		DEALLOCATE DataCursor1
END

select * from @MemoryTable


--select * from ARInvoiceLines where ReceivableId = '1-16'
