alter table shipments add Routing varchar(100) null
go

declare @Id varchar(15)
declare @Tenant int
declare @FromPortId varchar(15)
declare @ToPortId varchar(15)
declare @OnCarriageFromPortId varchar(15)
declare @OnCarriageToPortId varchar(15)
declare @PreCarriageFromPortId varchar(15)
declare @PreCarriageToPortId varchar(15)
declare @PreCarriageFromPortCode varchar(3)
declare @PreCarriageToPortCode varchar(3)
declare @OnCarriageFromPortCode varchar(3)
declare @OnCarriageToPortCode varchar(3)
declare @FromPortCode varchar(3)
declare @ToPortCode varchar(3)
declare @ShipmentLevelCode varchar(2)
declare @Routing varchar (100)


DECLARE updatecursor CURSOR READ_ONLY
FOR
SELECT Id,Tenant,FromPortId,ToPortId,OnCarriageFromPortId,OnCarriageToPortId,PreCarriageFromPortId,PreCarriageToPortId,ShipmentLevelCode
FROM Shipments

OPEN updatecursor

	FETCH NEXT FROM updatecursor
	INTO @Id,@Tenant,@FromPortId,@ToPortId,@OnCarriageFromPortId,@OnCarriageToPortId,@PreCarriageFromPortId,@PreCarriageToPortId,@ShipmentLevelCode


WHILE @@FETCH_STATUS = 0

BEGIN


    if(@ShipmentLevelCode ='C' or @ShipmentLevelCode='D')
	Begin
	declare @MainCarriageFromPortId varchar(15)
	declare @MainCarriageToPortId varchar(15)

	set @MainCarriageFromPortId=(select MainCarriageFromPortId from shipmentMasterDatas where id=@Id)
	set @MainCarriageToPortId=(select Transshipment3ToPortId from shipmentMasterDatas where id=@Id)
	if(@MainCarriageToPortId is null)
	begin
	set @MainCarriageToPortId=(select Transshipment2ToPortId from shipmentMasterDatas where id=@Id)
	end
	if(@MainCarriageToPortId is null)
	begin
	set @MainCarriageToPortId=(select Transshipment1ToPortId from shipmentMasterDatas where id=@Id)
	end
	if(@MainCarriageToPortId is null)
	begin
	set @MainCarriageToPortId=(select MainCarriageToPortId from shipmentMasterDatas where id=@Id)
	end
	set @FromPortCode=(select code from [ports] where id=@MainCarriageFromPortId)
	set @ToPortCode=(select code from [ports] where id=@MainCarriageToPortId)
	end

	else
	begin 
	set @FromPortCode=(select code from [ports] where id=@FromPortId)
	set @ToPortCode=(select code from [ports] where id=@ToPortId)
	end
	
	set @Routing=@FromPortCode+' , '+@ToPortCode
	print @Routing

	if(@PreCarriageFromPortId is not null)
	begin
	print 'pre carriage'
	print @ShipmentLevelCode
	set @PreCarriageFromPortCode=(select code from [ports] where id=@PreCarriageFromPortId)
	set @Routing=@PreCarriageFromPortCode+' , '+@FromPortCode+' , '+@ToPortCode
	end
	if(@OnCarriageToPortId is not null)
	begin
	print 'on carriage'
	print @ShipmentLevelCode
	set @OnCarriageToPortCode=(select code from [ports] where id=@OnCarriageToPortId)
	set @Routing=@PreCarriageFromPortCode+' , '+@FromPortCode+' , '+@ToPortCode+' , '+@OnCarriageToPortCode
	end
	
	print @Routing
   
   update shipments set Routing=@Routing
   where id=@Id and tenant=@Tenant
	
	FETCH NEXT FROM updatecursor
	INTO  @Id,@Tenant,@FromPortId,@ToPortId,@OnCarriageFromPortId,@OnCarriageToPortId,@PreCarriageFromPortId,@PreCarriageToPortId,@ShipmentLevelCode
END

CLOSE updatecursor
DEALLOCATE updatecursor






