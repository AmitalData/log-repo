


DECLARE @Tenant AS INT
declare @invoiceid varchar(15)
declare @mainentityid varchar(15)
declare @house varchar(20)
declare @master varchar(20)
declare @direction varchar(2)
declare @shipmentlevelcode varchar(2)
declare @port varchar(4)
declare @portid varchar(15)
declare @description varchar(255)
BEGIN;
	DECLARE TEUCursor CURSOR READ_ONLY
	FOR	
	SELECT Id,Tenant,MainEntityId
	FROM ARInvoices
	OPEN TEUCursor FETCH NEXT FROM TEUCursor INTO @invoiceid,@Tenant,@mainentityid
	WHILE @@FETCH_STATUS = 0
	BEGIN
	print @invoiceid
	print @mainentityid

	set @shipmentlevelcode=(select shipmentlevelCode from shipments where id=@mainentityid)
	set @direction=(select directionId from shipments where id =@mainentityid)
	set @house=(select house from Shipments where id=@mainentityid)
	set @master=(select Master from ShipmentMasterDatas where id=@mainentityid)

	print @direction
	print @house
	print @master

	--update ARInvoices set HouseNumber=@house where id=@invoiceid
	--update ARInvoices set MasterNumber=@master where id=@invoiceid
	if (@direction='I')
	BEGIN
	
	if(@shipmentlevelcode ='D' or @shipmentlevelcode='C')
	begin
	set @portid=(select MaincarriageFromPortId from ShipmentMasterDatas where id =@mainentityid)
	end
	else
	begin
	set @portid=(select FromPortId from Shipments where id =@mainentityid)
	end
	
	set @port=(select Code from Ports where id=@portid)
	print @shipmentlevelcode
	print @direction
	print @port
	set @description='Import from '+@port
	--update ARInvoices set Description='Import from '+@port
	END

	if(@direction='E')
	begin
	
	if(@shipmentlevelcode ='D' or @shipmentlevelcode='C')
	begin
	set @portid=(select MaincarriageToPortId from ShipmentMasterDatas where id =@mainentityid)
	end
	else
	begin
	set @portid=(select ToPortId from Shipments where id =@mainentityid)
	end
	set @port=(select Code from Ports where id=@portid)
	print @shipmentlevelcode
	print @direction
	print @port
	set @description='Export to '+@port
	--update ARInvoices set Description='Export to '+@port
	end

	if(@direction='D')
	begin
	
	if(@shipmentlevelcode ='D' or @shipmentlevelcode='C')
	begin
	set @portid=(select MaincarriageToPortId from ShipmentMasterDatas where id =@mainentityid)
	end
	else
	begin
	set @portid=(select ToPortId from Shipments where id =@mainentityid)
	end

	set @port=(select Code from Ports where id=@portid)
	print @shipmentlevelcode
	print @direction
	print @port
	set @description='Ship to '+@port
	--update ARInvoices set Description='Ship to '+@port
	end
	update arinvoices set SearchFields=SearchFields+','+Description+','+HouseNumber+','+MasterNumber,HouseNumber=@house,MasterNumber=@master,Description=@description where id=@invoiceid
	FETCH NEXT FROM TEUCursor INTO @invoiceid,@Tenant,@mainentityid
	END
	CLOSE TEUCursor
	DEALLOCATE TEUCursor	
END



