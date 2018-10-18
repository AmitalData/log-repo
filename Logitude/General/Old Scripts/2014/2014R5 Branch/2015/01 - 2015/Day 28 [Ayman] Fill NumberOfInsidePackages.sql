
SET NOCOUNT ON;
GO

declare @IsLCL as bit
declare @Tenant as int
declare @ShipmentId as varchar(15)
declare @TransportModeId as varchar(1)
declare @ShipmentTypeId as varchar(5)
declare @ShipmentPackageId as varchar(15)
declare @InsidePackageQuantity as int
declare @InsidePackageTypeId as varchar(15)

declare @ShipmentLevelNumberOfInsides as int
declare @PackageLevelNumberOfInsides as int
declare @ShipmentLevelNumberOfInsidesDetails as varchar(500)
declare @PackageLevelNumberOfInsidesDetails as varchar(500)

declare @MemoryQuantity as int
declare @MemoryPackageTypeName as varchar(40)
declare @MemoryTable table
(
  ShipmentId varchar(15),
  ShipmentPackageId varchar(15),
  PackageTypeId varchar(15),
  PackageTypeName varchar(40),
  Quantity int,
  IsOthers bit
)

BEGIN
		DECLARE ShipmentsCursor CURSOR READ_ONLY
		FOR
		SELECT Id, Tenant, TransportModeId, ShipmentTypeId
		FROM Shipments
		OPEN ShipmentsCursor FETCH NEXT FROM ShipmentsCursor INTO @ShipmentId, @Tenant, @TransportModeId, @ShipmentTypeId
		WHILE @@FETCH_STATUS = 0
		BEGIN

			set @IsLCL = 0

			if (@TransportModeId = 'A')
			set @IsLCL = 1

			else if(@TransportModeId = 'O' AND @ShipmentTypeId = 'LCLD')
			set @IsLCL = 1

			else if(@TransportModeId = 'I' AND @ShipmentTypeId = 'LTL')
			set @IsLCL = 1

			if (@IsLCL = 1)
			BEGIN
				update Shipments
				set
				NumberOfInsidePackages = 0,
				NumberOfInsidePackagesDetails = null
				where Tenant = @Tenant AND Id = @ShipmentId

				update ShipmentPackages
				set
				NumberOfInsidePackages = 0,
				NumberOfInsidePackagesDetails = null
				where Tenant = @Tenant AND ShipmentId = @ShipmentId
			END

			else
			BEGIN

				delete from @MemoryTable
				set @ShipmentLevelNumberOfInsides = 0
				set @ShipmentLevelNumberOfInsidesDetails = ''

					-- Loop Packages
					BEGIN
					DECLARE ShipmentPackagesCursor CURSOR READ_ONLY
					FOR
					SELECT Id
					FROM ShipmentPackages
					where Tenant = @Tenant AND ShipmentId = @ShipmentId
					OPEN ShipmentPackagesCursor FETCH NEXT FROM ShipmentPackagesCursor INTO @ShipmentPackageId
					WHILE @@FETCH_STATUS = 0
					BEGIN
												
						set @PackageLevelNumberOfInsides = 0
						set @PackageLevelNumberOfInsidesDetails = ''

							-- Loop Insides
							BEGIN
							DECLARE InsidePackagesCursor CURSOR READ_ONLY
							FOR
							SELECT Quantity, PackageTypeId
							FROM InsideShipmentPackages
							where Tenant = @Tenant AND ShipmentPackageId = @ShipmentPackageId
							OPEN InsidePackagesCursor FETCH NEXT FROM InsidePackagesCursor INTO @InsidePackageQuantity, @InsidePackageTypeId
							WHILE @@FETCH_STATUS = 0
							BEGIN

								if (@InsidePackageQuantity is null)
								set @InsidePackageQuantity = 0

								if (@InsidePackageTypeId is null)
								begin
									insert into @MemoryTable
									values
									(
									@ShipmentId,
									@ShipmentPackageId,
									null,
									'Others',
									@InsidePackageQuantity,
									1
									)
								end

								else
								begin
									insert into @MemoryTable
									values
									(
									@ShipmentId,
									@ShipmentPackageId,
									@InsidePackageTypeId,
									(select EnglishName from PackageTypes where Tenant = @Tenant AND Id = @InsidePackageTypeId),
									@InsidePackageQuantity,
									0
									)
								end

							FETCH NEXT FROM InsidePackagesCursor INTO @InsidePackageQuantity, @InsidePackageTypeId
							END
							CLOSE InsidePackagesCursor
							DEALLOCATE InsidePackagesCursor
							END
						
							-- Loop @MemoryTable:PackageLevel
							BEGIN
							DECLARE MemoryTablePackageLevelCursor CURSOR READ_ONLY
							FOR
							SELECT PackageTypeName, Sum(Quantity)
							FROM @MemoryTable
							where ShipmentPackageId = @ShipmentPackageId
							group by IsOthers, PackageTypeName
							Order by IsOthers, PackageTypeName
							OPEN MemoryTablePackageLevelCursor FETCH NEXT FROM MemoryTablePackageLevelCursor INTO @MemoryPackageTypeName, @MemoryQuantity
							WHILE @@FETCH_STATUS = 0
							BEGIN

								set @PackageLevelNumberOfInsides = @PackageLevelNumberOfInsides + @MemoryQuantity

								if (@PackageLevelNumberOfInsidesDetails = '') set @PackageLevelNumberOfInsidesDetails = convert(varchar,@MemoryQuantity) + ' ' + @MemoryPackageTypeName
								else set @PackageLevelNumberOfInsidesDetails = @PackageLevelNumberOfInsidesDetails + ', ' + convert(varchar,@MemoryQuantity) + ' ' + @MemoryPackageTypeName	
								
							FETCH NEXT FROM MemoryTablePackageLevelCursor INTO @MemoryPackageTypeName, @MemoryQuantity
							END
							CLOSE MemoryTablePackageLevelCursor
							DEALLOCATE MemoryTablePackageLevelCursor
							END							

						if (@PackageLevelNumberOfInsidesDetails = '')
						set @PackageLevelNumberOfInsidesDetails = null

						update ShipmentPackages
						set
						NumberOfInsidePackages = @PackageLevelNumberOfInsides,
						NumberOfInsidePackagesDetails = @PackageLevelNumberOfInsidesDetails
						where Tenant = @Tenant ANd ShipmentId = @ShipmentId AND Id = @ShipmentPackageId

					FETCH NEXT FROM ShipmentPackagesCursor INTO @ShipmentPackageId
					END
					CLOSE ShipmentPackagesCursor
					DEALLOCATE ShipmentPackagesCursor
					END

					-- Loop @MemoryTable:ShipmentLevel
					BEGIN
					DECLARE MemoryTableShipmentLevelCursor CURSOR READ_ONLY
					FOR
					SELECT PackageTypeName, Sum(Quantity)
					FROM @MemoryTable
					where ShipmentId = @ShipmentId
					group by IsOthers, PackageTypeName
					Order by IsOthers, PackageTypeName
					OPEN MemoryTableShipmentLevelCursor FETCH NEXT FROM MemoryTableShipmentLevelCursor INTO @MemoryPackageTypeName, @MemoryQuantity
					WHILE @@FETCH_STATUS = 0
					BEGIN

						set @ShipmentLevelNumberOfInsides = @ShipmentLevelNumberOfInsides + @MemoryQuantity

						if (@ShipmentLevelNumberOfInsidesDetails = '') set @ShipmentLevelNumberOfInsidesDetails = convert(varchar,@MemoryQuantity) + ' ' + @MemoryPackageTypeName
						else set @ShipmentLevelNumberOfInsidesDetails = @ShipmentLevelNumberOfInsidesDetails + ', ' + convert(varchar,@MemoryQuantity) + ' ' + @MemoryPackageTypeName	
								
					FETCH NEXT FROM MemoryTableShipmentLevelCursor INTO @MemoryPackageTypeName, @MemoryQuantity
					END
					CLOSE MemoryTableShipmentLevelCursor
					DEALLOCATE MemoryTableShipmentLevelCursor
					END

				if (@ShipmentLevelNumberOfInsidesDetails = '')
				set @ShipmentLevelNumberOfInsidesDetails = null

				update Shipments
				set
				NumberOfInsidePackages = @ShipmentLevelNumberOfInsides,
				NumberOfInsidePackagesDetails = @ShipmentLevelNumberOfInsidesDetails
				where Tenant = @Tenant AND Id = @ShipmentId
				
			END

		FETCH NEXT FROM ShipmentsCursor INTO @ShipmentId, @Tenant, @TransportModeId, @ShipmentTypeId
		END
		CLOSE ShipmentsCursor
		DEALLOCATE ShipmentsCursor
END

SET NOCOUNT Off;