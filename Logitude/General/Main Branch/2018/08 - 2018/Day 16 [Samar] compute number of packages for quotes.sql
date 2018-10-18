
declare @PackagesQuantity as int
declare @Quantity1 as int
declare @Quantity2 as int
declare @Quantity3 as int
declare @Quantity4 as int
declare @Quantity5 as int
declare @Id as varchar(15)
declare @ShipmentTypeId as varchar(5)
declare @TransportModeId as varchar(1)

BEGIN 
	DECLARE QuotesCursor CURSOR READ_ONLY
	FOR
	SELECT Id, ShipmentTypeId, TransportModeId, PackageType1Quantity, PackageType2Quantity, PackageType3Quantity, PackageType4Quantity, PackageType5Quantity
	FROM Quotes
	OPEN QuotesCursor FETCH NEXT FROM QuotesCursor INTO @Id, @ShipmentTypeId, @TransportModeId, @Quantity1, @Quantity2, @Quantity3, @Quantity4, @Quantity5
	WHILE @@FETCH_STATUS = 0
		BEGIN

			set @PackagesQuantity = 0			

			if(@TransportModeId = 'A' OR (@TransportModeId = 'O' AND @ShipmentTypeId = 'LCLD') OR (@TransportModeId = 'I' AND @ShipmentTypeId = 'LTL'))
			begin
				set @PackagesQuantity = (select SUM(Quantity) from QuotePackages where QuoteId = @Id)
				
				update Quotes set NumberOfPackages = @PackagesQuantity	where Id = @Id	
			end

			else
			begin
				set @PackagesQuantity = isnull(@Quantity1,0) + isnull(@Quantity2,0) + isnull(@Quantity3,0) + isnull(@Quantity4,0) + isnull(@Quantity5,0)
				
				update Quotes set NumberOfContainers = @PackagesQuantity where Id = @Id	
			end
		
           FETCH NEXT FROM QuotesCursor INTO @Id, @ShipmentTypeId, @TransportModeId, @Quantity1, @Quantity2, @Quantity3, @Quantity4, @Quantity5
        END
	CLOSE QuotesCursor
	DEALLOCATE QuotesCursor
END