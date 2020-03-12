
declare @MainAddressId as varchar(15)
declare @CardId as varchar(15)
	DECLARE AddressAddress CURSOR READ_ONLY
	FOR
	SELECT CardId
	From Addresses
    where AddressTypeId = 'M' and CardId is not null
	group by CardId,AddressTypeId having Count(*)> 1
	OPEN AddressAddress FETCH NEXT FROM AddressAddress INTO @CardId
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
     set @MainAddressId = (select top(1) Id from addresses where AddressTypeId = 'M' and CardId = @CardId)
	 update Addresses set AddressTypeId = 'B' where  CardId = @CardId and AddressTypeId = 'M'  and Id !=@MainAddressId
	FETCH NEXT FROM AddressAddress INTO  @CardId
		End
	CLOSE AddressAddress
	DEALLOCATE AddressAddress
