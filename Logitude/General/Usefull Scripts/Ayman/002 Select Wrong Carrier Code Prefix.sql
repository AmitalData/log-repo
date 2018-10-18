
declare @Tenant as int
declare @Id as varchar(15)
declare @ShipmentNumber as varchar(20)
declare @MainCarriageCarrierPrefix as char(2)
declare @Transshipment1CarrierPrefix as char(2)
declare @Transshipment2CarrierPrefix as char(2)
declare @Transshipment3CarrierPrefix as char(2)

declare @DataTable table
(
  Id varchar(15) not null,
  Tenant int not null,
  ShipmentNumber varchar(15) not null,
  MainCarriageCarrierPrefix char(2)
)

declare @DataTable1 table
(
  Id varchar(15) not null,
  Tenant int not null,
  ShipmentNumber varchar(15) not null,
  Transshipment1CarrierPrefix char(2)
)

declare @DataTable2 table
(
  Id varchar(15) not null,
  Tenant int not null,
  ShipmentNumber varchar(15) not null,
  Transshipment2CarrierPrefix char(2)
)

declare @DataTable3 table
(
  Id varchar(15) not null,
  Tenant int not null,
  ShipmentNumber varchar(15) not null,
  Transshipment3CarrierPrefix char(2)
)

--update Shipments set
--MainCarriageCarrierPrefix = null,
--Transshipment1CarrierPrefix = null,
--Transshipment2CarrierPrefix = null,
--Transshipment3CarrierPrefix = null
--where TransportModeId <> 'A'

BEGIN
		DECLARE DataCursor CURSOR READ_ONLY
		FOR
		SELECT Id, Tenant, ShipmentNumber, MainCarriageCarrierPrefix, Transshipment1CarrierPrefix, Transshipment2CarrierPrefix, Transshipment3CarrierPrefix
		FROM Shipments
		--where TransportModeId = 'A'
		OPEN DataCursor FETCH NEXT FROM DataCursor INTO @Id, @Tenant, @ShipmentNumber, @MainCarriageCarrierPrefix, @Transshipment1CarrierPrefix, @Transshipment2CarrierPrefix, @Transshipment3CarrierPrefix
		WHILE @@FETCH_STATUS = 0
			BEGIN

				if (@MainCarriageCarrierPrefix is not null)
				begin
					if not exists (select * from Cards where Tenant = @Tenant AND Code = @MainCarriageCarrierPrefix AND PartnerTypeId = 'AL')
					BEGIN
						insert into @DataTable(Id, Tenant, ShipmentNumber, MainCarriageCarrierPrefix)
						values (@Id, @Tenant, @ShipmentNumber, @MainCarriageCarrierPrefix)
					END
				end
				
				if (@Transshipment1CarrierPrefix is not null)
				begin
					if not exists (select * from Cards where Tenant = @Tenant AND Code = @Transshipment1CarrierPrefix AND PartnerTypeId = 'AL')
					BEGIN
						insert into @DataTable1(Id, Tenant, ShipmentNumber, Transshipment1CarrierPrefix)
						values (@Id, @Tenant, @ShipmentNumber, @Transshipment1CarrierPrefix)
					END
				end

				if (@Transshipment2CarrierPrefix is not null)
				begin
					if not exists (select * from Cards where Tenant = @Tenant AND Code = @Transshipment2CarrierPrefix AND PartnerTypeId = 'AL')
					BEGIN
						insert into @DataTable2(Id, Tenant, ShipmentNumber, Transshipment2CarrierPrefix)
						values (@Id, @Tenant, @ShipmentNumber, @Transshipment2CarrierPrefix)
					END
				end

				if (@Transshipment3CarrierPrefix is not null)
				begin
					if not exists (select * from Cards where Tenant = @Tenant AND Code = @Transshipment3CarrierPrefix AND PartnerTypeId = 'AL')
					BEGIN
						insert into @DataTable3(Id, Tenant, ShipmentNumber, Transshipment3CarrierPrefix)
						values (@Id, @Tenant, @ShipmentNumber, @Transshipment3CarrierPrefix)
					END
				end

			FETCH NEXT FROM DataCursor INTO @Id, @Tenant, @ShipmentNumber, @MainCarriageCarrierPrefix, @Transshipment1CarrierPrefix, @Transshipment2CarrierPrefix, @Transshipment3CarrierPrefix
			END
		CLOSE DataCursor
		DEALLOCATE DataCursor
END

select * from @DataTable
select * from @DataTable1
select * from @DataTable2
select * from @DataTable3

