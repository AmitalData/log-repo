
IF object_id(N'GetDateFormateAsNumber', N'FN') IS NOT NULL
  BEGIN DROP FUNCTION GetDateFormateAsNumber   end


IF object_id('GetDateFormateAsNumber') IS  NULL
BEGIN
 declare @dateString as varchar(3000)

 set @dateString = 'CREATE FUNCTION [dbo].[GetDateFormateAsNumber](@dateTime datetime) RETURNS int WITH EXECUTE AS CALLER AS BEGIN   declare @Result  as int declare @dateString as varchar(100) set @Result = -1	 if(@dateTime is not null)   begin    if(@dateTime < ''2008-01-01 00:00:00.000'')  begin set @Result = -2  end  else if(@dateTime> ''2027-12-31 00:00:00.000'')  begin  set @Result = -3	 end     else  begin set @dateString = CONVERT(VARCHAR(10), @dateTime, 120);  set @dateString = REPLACE(@dateString, ''-'', '''');   set @dateString = REPLACE(@dateString, ''/'', '''');   set @Result = @dateString     end  end   RETURN(@Result);  END;';

 EXEC(@dateString)
			
END


--If(OBJECT_ID('tempdb..#Fact_ShipmentsTemp') Is Not Null)
--Begin
--    Drop Table #Fact_ShipmentsTemp
--End


--CREATE TABLE #Fact_ShipmentsTemp (
--	Id_Number int not null identity(1,1) primary key,
--    Id varchar(15) not null,
--   	[Source Tenant]  int,
--    [Parent Tenant]  int,
--	Direction varchar(1) not null,
--	[Transport Mode]  varchar(1) not null,
--	Level  varchar(1) not null,
--	Type  varchar(4) not null,
--	Department  int not null,
--	Branch  int not null,
--    [Shipment Number] varchar(15) not null,
--	House varchar(20),
--	Master varchar(30),
--	Shipper  int,
--	Consignee int,
--	Agent int,
--	Customer int,
--	Incoterm int,
--	[Gross Weight (KG)] float,
--	[Chargeable Weight (KG)] float,
--	[Total Volume (CBM)] float,
--	[Number of Packages] int ,
--	[Number of Containers] int ,
--	Salesman int,
--	[Account Manager] int,

--	[Profit ( Local )] float,
--	Profit  float,
	
--	[Local Currency ] int,
--    [Profit Currency]  int,
--	[Number of Invoices]  int,
--	[Operationally Closed]  bit,
--	[Accounting Closed] bit ,
--	Status int , 
--	Location nvarchar(40),
--	Origin int,
--	[Final Destination] int,
--	[Is Departed] bit,
--	[Departed Date] datetime,
--	[Is Arrived] bit,
--	[Arrived Date] datetime,
--	[Is Customs Cleared] bit,
--	[Customs Clearence Date] int ,
--	[Total Shipments] int ,
--    [Create Date] datetime ,
--	[Last Update Date] datetime ,
--	[Operational Date] int ,
--	[Operational Close Date] int ,
--	[Accounting Close Date] int ,
	
--	[Open Receivables ( Local )] float ,
--	[Open Receivables ( Profit )] float,
--	[Accounted Receivables ( Local )] float,
--	[Accounted Receivables ( Profit )] float,
--    [Open Payables ( Local )] float,
--	[Open Payables ( Profit )] float,
--    [Accounted Payables ( Local )] float,
--	[Accounted Payables ( Profit )] float,

--);


   declare @Id as varchar(15)
   declare @SourceTenant as int
   declare @ParentTenant as int
   declare @Direction as varchar(1)
   declare @TransportMode as varchar(1)
   declare @Level as varchar(1)
   declare @Type as varchar(4)
   declare @Department as int
   declare @Branch as int
   declare @ShipmentNumber as varchar(15)
   declare @House as varchar(20)
   declare @Master as varchar(30)
   declare @Shipper as int
   declare @Consignee as int
   declare @Agent as int
   declare @Customer as int
   declare @Incoterm as int
   declare @TotalGrossWeightInKG as float
   declare @TotalChargeableWeightInKG as float
   declare @TotalVolumeInCBM as float
   declare @NumberOfPackages as int
   declare @NumberOfContainers as int
   declare @Salesman as int
   declare @AccountManager as int
   declare @TotalProfitInLocalCurrency as float
   declare @TotalProfitInProfitCurrency as float
 
   declare @LocalCurrency as int
   declare @ProfitCurrency as int
   declare @NumberOfInvoices as int
   declare @OperationallyClosed as bit
   declare @AccountingClosed as bit
   declare @Status as int
   declare @Location as nvarchar(40)
   declare @Origin as int
   declare @FinalDestination as int
   declare @IsDeparted as bit
   declare @DepartedDate as datetime
   declare @IsArrived as bit
   declare @ArrivedDate as datetime
   declare @IsCustomsCleared as bit
   declare @CustomsClearenceDate as date


   declare @Tenant as int
   declare @MasterDataId as varchar(15)
   declare @Transshipment3ToPortId  as int
   declare @Transshipment2ToPortId  as int
   declare @Transshipment1ToPortId  as int
   declare @MainCarriageToPortId  as int
   declare @DirectionId as varchar(15)
   declare @TransportModeId as varchar(1)
   declare @ToPortId as int
 
   declare @CreateDate as datetime
   declare @LastUpdateDate as datetime
   declare @OperationalDate as datetime
   declare @OperationalCloseDate as datetime
   declare @AccountingCloseDate as datetime

   declare @OpenReceivablesInLocalCurrency as float
   declare @OpenReceivablesInProfitCurrency as float
   declare @AccountedReceivablesInLocalCurrency as float 
   declare @AccountedReceivablesInProfitCurrency as float
   declare @OpenPayablesInLocalCurrency as float
   declare @OpenPayablesInProfitCurrency as float
   declare @AccountedPayablesInLocalCurrency as float
   declare @AccountedPayablesInProfitCurrency as float 


	DECLARE ShipmentsCursor CURSOR READ_ONLY
	FOR
	SELECT dw_Shipments.Id, SourceTenant.[Tenant Number], ParentTenant.[Tenant Number] , NewDIM_Directions.Code, NewDIM_TransportModes.Code ,NewDIM_Levels.Code,  NewDIM_Types.Code , NewDIM_Departments.Id_Number ,NewDIM_Branches.Id_Number , dw_Shipments.ShipmentNumber, dw_Shipments.House ,dw_ShipmentMasterDatas.Master,shipperPartners.Id_Number, consigneePartners.Id_Number,
	agentPartners.Id_Number,customerPartners.Id_Number , NewDIM_Incoterms.Id_Number, dw_Shipments.GrossWeightInKG ,  dw_Shipments.ChargeableWeightInKG , dw_Shipments.VolumeInCBM ,  dw_Shipments.NumberOfPackages, dw_Shipments.NumberOfContainers,SalesmanUser.Id_Number, AccountManagerUser.Id_Number,
	dw_Shipments.ProfitInLocalCurrency,dw_Shipments.ProfitInProfitCurrency,  LocalCurrency.Id_Number  ,  ProfitCurrency.Id_Number,0,dw_Shipments.IsOperationalClosed,
	dw_Shipments.IsAccountingClosed , NewDIM_ShipmentStatuses.Id_Number , dw_Shipments.StatusLocation  ,  dw_ShipmentMasterDatas.MainCarriageATD ,dw_Shipments.FinalArrivalDate, dw_Shipments.CustomsClearanceDate , dw_ShipmentMasterDatas.Id ,mainCarriageToPort.Id_Number ,  transshipment1ToPort.Id_Number ,transshipment2ToPort.Id_Number,transshipment3ToPort.Id_Number,fromPort.Id_Number, toPort.Id_Number , dw_Shipments.DirectionId ,dw_Shipments.TransportModeId ,dw_Shipments.Tenant,
	dw_Shipments.CreateDateTime ,dw_Shipments.LastUpdateDate,  dw_Shipments.OperationalDate,dw_Shipments.OperationalCloseDate, dw_Shipments.AccountingCloseDate,
	dw_Shipments.OpenReceivablesInLocalCurrency,dw_Shipments.OpenReceivablesInProfitCurrency,dw_Shipments.AccountedReceivablesInLocalCurrency,dw_Shipments.AccountedReceivablesInProfitCurrency , dw_Shipments.OpenPayablesInLocalCurrency,dw_Shipments.OpenPayablesInProfitCurrency,dw_Shipments.AccountedPayablesInLocalCurrency,dw_Shipments.AccountedPayablesInProfitCurrency 

	From dw_Shipments
	inner JOIN NewDIM_Tenants SourceTenant ON dw_Shipments.Tenant = SourceTenant.[Tenant Number]
	inner JOIN dw_DWHSettings ON dw_Shipments.Tenant = dw_DWHSettings.Tenant
	inner JOIN NewDIM_Tenants ParentTenant ON dw_DWHSettings.ParentTenant = ParentTenant.[Tenant Number]
	inner JOIN NewDIM_Directions ON dw_Shipments.DirectionId = NewDIM_Directions.Code
    inner JOIN NewDIM_TransportModes ON dw_Shipments.TransportModeId = NewDIM_TransportModes.Code
	inner JOIN NewDIM_Levels ON dw_Shipments.ShipmentLevelCode = NewDIM_Levels.Code
	inner JOIN NewDIM_Types ON dw_Shipments.ShipmentTypeId = NewDIM_Types.Code
	inner JOIN NewDIM_Departments ON dw_Shipments.DepartmentId = NewDIM_Departments.Id
	inner JOIN NewDIM_Branches ON dw_Shipments.BranchId =NewDIM_Branches.Id
    inner JOIN dw_ShipmentMasterDatas ON dw_Shipments.MasterShipmentDataId = dw_ShipmentMasterDatas.Id
	inner JOIN NewDIM_Partners shipperPartners ON dw_Shipments.ShipperId = shipperPartners.Id
	inner JOIN NewDIM_Partners consigneePartners ON dw_Shipments.ConsigneeId = consigneePartners.Id
	inner JOIN NewDIM_Partners agentPartners ON dw_Shipments.AgentId = agentPartners.Id
	inner JOIN NewDIM_Partners customerPartners ON dw_Shipments.CustomerId = customerPartners.Id
	inner JOIN NewDIM_Incoterms  ON dw_Shipments.IncotermId = NewDIM_Incoterms.Id
	inner JOIN NewDIM_Users SalesmanUser ON dw_Shipments.SalesmanUserId = SalesmanUser.Id
	inner JOIN NewDIM_Users AccountManagerUser ON dw_Shipments.AccountManagerUserId = AccountManagerUser.Id
	
	inner JOIN NewDIM_Currencies ProfitCurrency ON dw_Shipments.ProfitCurrencyId = ProfitCurrency.Id
	inner JOIN NewDIM_ShipmentStatuses  ON dw_Shipments.StatusId = NewDIM_ShipmentStatuses.Id
	inner JOIN dw_Tenants  ON dw_Shipments.Tenant = dw_Tenants.Id
	inner JOIN NewDIM_Currencies LocalCurrency ON dw_Tenants.CurrencyId = LocalCurrency.Id
    inner JOIN NewDIM_Ports fromPort  ON dw_Shipments.FromPortId = FromPort.Id
	inner JOIN NewDIM_Ports toPort  ON dw_Shipments.ToPortId = toPort.Id
	inner JOIN NewDIM_Ports mainCarriageToPort  ON dw_ShipmentMasterDatas.MainCarriageToPortId = mainCarriageToPort.Id
	inner JOIN NewDIM_Ports transshipment1ToPort  ON dw_ShipmentMasterDatas.Transshipment1ToPortId = transshipment1ToPort.Id
	inner JOIN NewDIM_Ports transshipment2ToPort  ON dw_ShipmentMasterDatas.Transshipment2ToPortId = transshipment2ToPort.Id
	inner JOIN NewDIM_Ports transshipment3ToPort  ON dw_ShipmentMasterDatas.Transshipment3ToPortId = transshipment3ToPort.Id
	
	

	OPEN ShipmentsCursor FETCH NEXT FROM ShipmentsCursor INTO    @Id ,@SourceTenant, @ParentTenant ,@Direction , @TransportMode, @Level , @Type , @Department , @Branch , @ShipmentNumber , @House , @Master , @Shipper , @Consignee , @Agent, @Customer 
	,@Incoterm , @TotalGrossWeightInKG, @TotalChargeableWeightInKG , @TotalVolumeInCBM , @NumberOfPackages , @NumberOfContainers , @Salesman ,@AccountManager,@TotalProfitInLocalCurrency ,
	 @TotalProfitInProfitCurrency ,  @LocalCurrency , @ProfitCurrency , @NumberOfInvoices , @OperationallyClosed , @AccountingClosed , @Status , @Location   ,
    @DepartedDate ,  @ArrivedDate ,  @CustomsClearenceDate ,@MasterDataId ,@MainCarriageToPortId, @Transshipment1ToPortId,@Transshipment2ToPortId ,@Transshipment3ToPortId, @Origin ,@ToPortId,@DirectionId,@TransportModeId , @Tenant ,@CreateDate,@LastUpdateDate,@OperationalDate,@OperationalCloseDate,@AccountingCloseDate,
    @OpenReceivablesInLocalCurrency,@OpenReceivablesInProfitCurrency,@AccountedReceivablesInLocalCurrency,@AccountedReceivablesInProfitCurrency,@OpenPayablesInLocalCurrency,@OpenPayablesInProfitCurrency,@AccountedPayablesInLocalCurrency,@AccountedPayablesInProfitCurrency
	WHILE @@FETCH_STATUS = 0
	BEGIN

	
		 if(@DirectionId != 'D' or @TransportModeId != 'I')
				BEGIN

				--Master
				  if(@MasterDataId is not null)
				
				  BEGIN

					if(@MainCarriageToPortId is not null and @MainCarriageToPortId!=1 )BEGIN set @ToPortId = @MainCarriageToPortId;END
					if(@Transshipment1ToPortId is not null and @Transshipment1ToPortId!=1 )BEGIN set @ToPortId = @Transshipment1ToPortId;END
		        	if(@Transshipment2ToPortId is not null and @Transshipment2ToPortId!=1 )BEGIN set @ToPortId = @Transshipment2ToPortId;END
		    	    if(@Transshipment3ToPortId is not null and @Transshipment3ToPortId!=1 )BEGIN set @ToPortId = @Transshipment3ToPortId;END
				  End
		        End
	

		 set @FinalDestination = @ToPortId;
		 if(@FinalDestination is null) begin set @FinalDestination= 1; end

		 set @IsArrived = 1;set @IsDeparted = 1;set @IsCustomsCleared = 1;
		 if(@ArrivedDate is null) begin set @IsArrived= 0; end
	     if(@DepartedDate is null) begin set @IsDeparted=0; end
		 if(@CustomsClearenceDate is null) begin set @IsCustomsCleared= 0 end



	    insert into #Fact_ShipmentsTemp ([Id],[Source Tenant],[Parent Tenant],[Direction],[Transport Mode],[Level],[Type],[Department],[Branch],[Shipment Number],[House],[Master],[Shipper],[Consignee],[Agent],[Customer],[Incoterm],[Gross Weight (KG)],[Chargeable Weight (KG)],[Total Volume (CBM)],[Number of Packages],[Number of Containers],[Salesman],[Account Manager],[Profit ( Local )],[Profit],[Local Currency ],[Profit Currency],[Number of Invoices],[Operationally Closed],[Accounting Closed],[Status],[Location],[Origin],[Final Destination],[Is Departed],[Departed Date],[Is Arrived],[Arrived Date],[Is Customs Cleared],[Customs Clearence Date],[Total Shipments],[Create Date],[Last Update Date],[Operational Date],[Operational Close Date],[Accounting Close Date],[Open Receivables ( Local )],[Open Receivables ( Profit )],[Accounted Receivables ( Local )],[Accounted Receivables ( Profit )],[Open Payables ( Local )],[Open Payables ( Profit )],[Accounted Payables ( Local )],[Accounted Payables ( Profit )]) values(@Id, @SourceTenant,@ParentTenant,@Direction,@TransportMode, @Level, @Type , @Department ,@Branch , @ShipmentNumber , @House ,@Master , @Shipper,  @Consignee , @Agent,@Customer,@Incoterm ,@TotalGrossWeightInKG,@TotalChargeableWeightInKG, @TotalVolumeInCBM,  @NumberOfPackages, @NumberOfContainers, @Salesman , @AccountManager ,    @TotalProfitInLocalCurrency , @TotalProfitInProfitCurrency , @LocalCurrency,@ProfitCurrency,@NumberOfInvoices ,@OperationallyClosed,@AccountingClosed, @Status, @Location,  @Origin , @FinalDestination , @IsDeparted , @DepartedDate ,@IsArrived , @ArrivedDate , @IsCustomsCleared , dbo.GetDateFormateAsNumber(@CustomsClearenceDate) , 1 ,@CreateDate , @LastUpdateDate ,dbo.GetDateFormateAsNumber(@OperationalDate),dbo.GetDateFormateAsNumber(@OperationalCloseDate),dbo.GetDateFormateAsNumber(@AccountingCloseDate) ,@OpenReceivablesInLocalCurrency , @OpenReceivablesInProfitCurrency ,@AccountedReceivablesInLocalCurrency,@AccountedReceivablesInProfitCurrency, @OpenPayablesInLocalCurrency ,@OpenPayablesInProfitCurrency , @AccountedPayablesInLocalCurrency ,@AccountedPayablesInProfitCurrency )

	FETCH NEXT FROM ShipmentsCursor    INTO   @Id ,@SourceTenant, @ParentTenant ,@Direction , @TransportMode, @Level , @Type , @Department , @Branch , @ShipmentNumber , @House , @Master , @Shipper , @Consignee , @Agent, @Customer 
	,@Incoterm , @TotalGrossWeightInKG, @TotalChargeableWeightInKG , @TotalVolumeInCBM , @NumberOfPackages , @NumberOfContainers , @Salesman ,@AccountManager ,@TotalProfitInLocalCurrency , 
	 @TotalProfitInProfitCurrency ,  @LocalCurrency , @ProfitCurrency , @NumberOfInvoices , @OperationallyClosed , @AccountingClosed , @Status , @Location   ,
	 @DepartedDate ,  @ArrivedDate ,  @CustomsClearenceDate ,@MasterDataId ,@MainCarriageToPortId, @Transshipment1ToPortId,@Transshipment2ToPortId ,@Transshipment3ToPortId, @Origin ,@ToPortId,@DirectionId,@TransportModeId , @Tenant ,@CreateDate,@LastUpdateDate,@OperationalDate,@OperationalCloseDate,@AccountingCloseDate,
     @OpenReceivablesInLocalCurrency,@OpenReceivablesInProfitCurrency,@AccountedReceivablesInLocalCurrency,@AccountedReceivablesInProfitCurrency,@OpenPayablesInLocalCurrency,@OpenPayablesInProfitCurrency,@AccountedPayablesInLocalCurrency,@AccountedPayablesInProfitCurrency
		End
	CLOSE ShipmentsCursor
	DEALLOCATE ShipmentsCursor

 IF OBJECT_ID ('NewFact_Shipments', 'U')  IS NOT NULL begin drop table NewFact_Shipments end	
 	
SELECT *  INTO NewFact_Shipments FROM #Fact_ShipmentsTemp
If(OBJECT_ID('tempdb..#Fact_ShipmentsTemp') Is Not Null)
Begin
    Drop Table #Fact_ShipmentsTemp

End


 IF OBJECT_ID ('Fact_Shipments', 'U')  IS NOT NULL and  OBJECT_ID ('NewFact_Shipments', 'U')  IS NOT NULL begin EXEC sp_rename 'Fact_Shipments', 'OldFact_Shipments' end
 IF OBJECT_ID ('NewFact_Shipments', 'U')  IS NOT NULL begin EXEC sp_rename 'NewFact_Shipments', 'Fact_Shipments' end
 IF OBJECT_ID ('OldFact_Shipments', 'U')  IS NOT NULL begin drop table OldFact_Shipments end


 --DIM_Branches
 IF OBJECT_ID ('DIM_Branches', 'U')  IS NOT NULL and  OBJECT_ID ('NewDIM_Branches', 'U')  IS NOT NULL begin EXEC sp_rename 'DIM_Branches', 'OldDIM_Branches' end
 IF OBJECT_ID ('NewDIM_Branches', 'U')  IS NOT NULL begin EXEC sp_rename 'NewDIM_Branches', 'DIM_Branches' end
 IF OBJECT_ID ('OldDIM_Branches', 'U')  IS NOT NULL begin drop table OldDIM_Branches end
 IF  EXISTS (SELECT * FROM sys.key_constraints WHERE [type] = 'PK' and   [parent_object_id] = Object_id('dbo.DIM_Branches') and name = 'PK_NewDIM_Branches_Id_Number')  begin ALTER TABLE DIM_Branches DROP CONSTRAINT PK_NewDIM_Branches_Id_Number end
 ALTER TABLE DIM_Branches ADD CONSTRAINT PK_DIM_Branches_Id_Number PRIMARY KEY CLUSTERED (Id_Number);
 



 IF OBJECT_ID ('DIM_Partners', 'U')  IS NOT NULL and OBJECT_ID ('NewDIM_Partners', 'U')  IS NOT NULL begin EXEC sp_rename 'DIM_Partners', 'OldDIM_Partners' end
 IF OBJECT_ID ('NewDIM_Partners', 'U')  IS NOT NULL begin EXEC sp_rename 'NewDIM_Partners', 'DIM_Partners' end
 IF OBJECT_ID ('OldDIM_Partners', 'U')  IS NOT NULL begin drop table OldDIM_Partners end
 IF  EXISTS (SELECT * FROM sys.key_constraints WHERE [type] = 'PK' and   [parent_object_id] = Object_id('dbo.DIM_Partners') and name = 'PK_NewDIM_Partners_Id_Number')  begin ALTER TABLE DIM_Partners DROP CONSTRAINT PK_NewDIM_Partners_Id_Number end
 ALTER TABLE DIM_Partners ADD CONSTRAINT PK_DIM_Partners_Id_Number PRIMARY KEY CLUSTERED (Id_Number);



 --DIM_Currencies
 IF OBJECT_ID ('DIM_Currencies', 'U')  IS NOT NULL and OBJECT_ID ('NewDIM_Currencies', 'U')  IS NOT NULL begin EXEC sp_rename 'DIM_Currencies', 'OldDIM_Currencies' end
 IF OBJECT_ID ('NewDIM_Currencies', 'U')  IS NOT NULL begin EXEC sp_rename 'NewDIM_Currencies', 'DIM_Currencies' end
 IF OBJECT_ID ('OldDIM_Currencies', 'U')  IS NOT NULL begin drop table OldDIM_Currencies end
 IF  EXISTS (SELECT * FROM sys.key_constraints WHERE [type] = 'PK' and   [parent_object_id] = Object_id('dbo.DIM_Currencies') and name = 'PK_NewDIM_Currencies_Id_Number')  begin ALTER TABLE DIM_Currencies DROP CONSTRAINT PK_NewDIM_Currencies_Id_Number end
 ALTER TABLE DIM_Currencies ADD CONSTRAINT PK_DIM_Currencies_Id_Number PRIMARY KEY CLUSTERED (Id_Number);



 --DIM_Departments
 IF OBJECT_ID ('DIM_Departments', 'U')  IS NOT NULL and OBJECT_ID ('NewDIM_Departments', 'U')  IS NOT NULL begin EXEC sp_rename 'DIM_Departments', 'OldDIM_Departments' end
 IF OBJECT_ID ('NewDIM_Departments', 'U')  IS NOT NULL begin EXEC sp_rename 'NewDIM_Departments', 'DIM_Departments' end
 IF OBJECT_ID ('OldDIM_Departments', 'U')  IS NOT NULL begin drop table OldDIM_Departments end
 IF  EXISTS (SELECT * FROM sys.key_constraints WHERE [type] = 'PK' and   [parent_object_id] = Object_id('dbo.DIM_Departments') and name = 'PK_NewDIM_Departments_Id_Number')  begin ALTER TABLE DIM_Departments DROP CONSTRAINT PK_NewDIM_Departments_Id_Number end
 ALTER TABLE DIM_Departments ADD CONSTRAINT PK_DIM_Departments_Id_Number PRIMARY KEY CLUSTERED (Id_Number);



  --DIM_Directions
 IF OBJECT_ID ('DIM_Directions', 'U')  IS NOT NULL and  OBJECT_ID ('NewDIM_Directions', 'U')  IS NOT NULL begin EXEC sp_rename 'DIM_Directions', 'OldDIM_Directions' end
 IF OBJECT_ID ('NewDIM_Directions', 'U')  IS NOT NULL begin EXEC sp_rename 'NewDIM_Directions', 'DIM_Directions' end
 IF OBJECT_ID ('OldDIM_Directions', 'U')  IS NOT NULL begin drop table OldDIM_Directions end
 IF  EXISTS (SELECT * FROM sys.key_constraints WHERE [type] = 'PK' and   [parent_object_id] = Object_id('dbo.DIM_Directions') and name = 'PK_NewDIM_Directions_Code')  begin ALTER TABLE DIM_Directions DROP CONSTRAINT PK_NewDIM_Directions_Code end
 ALTER TABLE DIM_Directions ADD CONSTRAINT PK_DIM_Directions_Code PRIMARY KEY CLUSTERED (Code);

  --DIM_ShipmentStatuses
 IF OBJECT_ID ('DIM_ShipmentStatuses', 'U')  IS NOT NULL and  OBJECT_ID ('NewDIM_ShipmentStatuses', 'U')  IS NOT NULL begin EXEC sp_rename 'DIM_ShipmentStatuses', 'OldDIM_ShipmentStatuses' end
 IF OBJECT_ID ('NewDIM_ShipmentStatuses', 'U')  IS NOT NULL begin EXEC sp_rename 'NewDIM_ShipmentStatuses', 'DIM_ShipmentStatuses' end
 IF OBJECT_ID ('OldDIM_ShipmentStatuses', 'U')  IS NOT NULL begin drop table OldDIM_ShipmentStatuses end
 IF  EXISTS (SELECT * FROM sys.key_constraints WHERE [type] = 'PK' and   [parent_object_id] = Object_id('dbo.DIM_ShipmentStatuses') and name = 'PK_NewDIM_ShipmentStatuses_Id_Number')  begin ALTER TABLE DIM_ShipmentStatuses DROP CONSTRAINT PK_NewDIM_ShipmentStatuses_Id_Number end
 ALTER TABLE DIM_ShipmentStatuses ADD CONSTRAINT PK_DIM_ShipmentStatuses_Id_Number PRIMARY KEY CLUSTERED (Id_Number);

  --DIM_Incoterms
 IF OBJECT_ID ('DIM_Incoterms', 'U')  IS NOT NULL and OBJECT_ID ('NewDIM_Incoterms', 'U')  IS NOT NULL begin EXEC sp_rename 'DIM_Incoterms', 'OldDIM_Incoterms' end
 IF OBJECT_ID ('NewDIM_Incoterms', 'U')  IS NOT NULL begin EXEC sp_rename 'NewDIM_Incoterms', 'DIM_Incoterms' end
 IF OBJECT_ID ('OldDIM_Incoterms', 'U')  IS NOT NULL begin drop table OldDIM_Incoterms end
 IF  EXISTS (SELECT * FROM sys.key_constraints WHERE [type] = 'PK' and   [parent_object_id] = Object_id('dbo.DIM_Incoterms') and name = 'PK_NewDIM_Incoterms_Id_Number')  begin ALTER TABLE DIM_Incoterms DROP CONSTRAINT PK_NewDIM_Incoterms_Id_Number end
 ALTER TABLE DIM_Incoterms ADD CONSTRAINT PK_DIM_Incoterms_Id_Number PRIMARY KEY CLUSTERED (Id_Number);



  --DIM_Ports
 IF OBJECT_ID ('DIM_Ports', 'U')  IS NOT NULL and OBJECT_ID ('NewDIM_Ports', 'U')  IS NOT NULL begin EXEC sp_rename 'DIM_Ports', 'OldDIM_Ports' end
 IF OBJECT_ID ('NewDIM_Ports', 'U')  IS NOT NULL begin EXEC sp_rename 'NewDIM_Ports', 'DIM_Ports' end
 IF OBJECT_ID ('OldDIM_Ports', 'U')  IS NOT NULL begin drop table OldDIM_Ports end
 IF  EXISTS (SELECT * FROM sys.key_constraints WHERE [type] = 'PK' and   [parent_object_id] = Object_id('dbo.DIM_Ports') and name = 'PK_NewDIM_Ports_Id_Number')  begin ALTER TABLE DIM_Ports DROP CONSTRAINT PK_NewDIM_Ports_Id_Number end
 ALTER TABLE DIM_Ports ADD CONSTRAINT PK_DIM_Ports_Id_Number PRIMARY KEY CLUSTERED (Id_Number);

 --DIM_Levels
 IF OBJECT_ID ('DIM_Levels', 'U')  IS NOT NULL and OBJECT_ID ('NewDIM_Levels', 'U')  IS NOT NULL begin EXEC sp_rename 'DIM_Levels', 'OldDIM_Levels' end
 IF OBJECT_ID ('NewDIM_Levels', 'U')  IS NOT NULL begin EXEC sp_rename 'NewDIM_Levels', 'DIM_Levels' end
 IF OBJECT_ID ('OldDIM_Levels', 'U')  IS NOT NULL begin drop table OldDIM_Levels end
 IF  EXISTS (SELECT * FROM sys.key_constraints WHERE [type] = 'PK' and   [parent_object_id] = Object_id('dbo.DIM_Levels') and name = 'PK_NewDIM_Levels_Code')  begin ALTER TABLE DIM_Levels DROP CONSTRAINT PK_NewDIM_Levels_Code end
 ALTER TABLE DIM_Levels ADD CONSTRAINT PK_DIM_Levels_Code PRIMARY KEY CLUSTERED (Code);


 --DIM_Types
 IF OBJECT_ID ('DIM_Types', 'U')  IS NOT NULL and  OBJECT_ID ('NewDIM_Types', 'U')  IS NOT NULL begin EXEC sp_rename 'DIM_Types', 'OldDIM_Types' end
 IF OBJECT_ID ('NewDIM_Types', 'U')  IS NOT NULL begin EXEC sp_rename 'NewDIM_Types', 'DIM_Types' end
 IF OBJECT_ID ('OldDIM_Types', 'U')  IS NOT NULL begin drop table OldDIM_Types end
 IF  EXISTS (SELECT * FROM sys.key_constraints WHERE [type] = 'PK' and   [parent_object_id] = Object_id('dbo.DIM_Types') and name = 'PK_NewDIM_Types_Code')  begin ALTER TABLE DIM_Types DROP CONSTRAINT PK_NewDIM_Types_Code end
 ALTER TABLE DIM_Types ADD CONSTRAINT PK_DIM_Types_Code PRIMARY KEY CLUSTERED (Code);


  --DIM_Tenants
 IF OBJECT_ID ('DIM_Tenants', 'U')  IS NOT NULL and OBJECT_ID ('NewDIM_Tenants', 'U')  IS NOT NULL begin EXEC sp_rename 'DIM_Tenants', 'OldDIM_Tenants' end
 IF OBJECT_ID ('NewDIM_Tenants', 'U')  IS NOT NULL begin EXEC sp_rename 'NewDIM_Tenants', 'DIM_Tenants' end
 IF OBJECT_ID ('OldDIM_Tenants', 'U')  IS NOT NULL begin drop table OldDIM_Tenants end
 IF  EXISTS (SELECT * FROM sys.key_constraints WHERE [type] = 'PK' and   [parent_object_id] = Object_id('dbo.DIM_Tenants') and name = 'PK_NewDIM_Tenants_TenantNumber')  begin ALTER TABLE DIM_Tenants DROP CONSTRAINT PK_NewDIM_Tenants_TenantNumber end
 ALTER TABLE DIM_Tenants ADD CONSTRAINT PK_DIM_Tenants_TenantNumber PRIMARY KEY CLUSTERED ([Tenant Number]);

  --DIM_TransportModes
 IF OBJECT_ID ('DIM_TransportModes', 'U')  IS NOT NULL and OBJECT_ID ('NewDIM_TransportModes', 'U')  IS NOT NULL begin EXEC sp_rename 'DIM_TransportModes', 'OldDIM_TransportModes' end
 IF OBJECT_ID ('NewDIM_TransportModes', 'U')  IS NOT NULL begin EXEC sp_rename 'NewDIM_TransportModes', 'DIM_TransportModes' end
 IF OBJECT_ID ('OldDIM_TransportModes', 'U')  IS NOT NULL begin drop table OldDIM_TransportModes end
 IF  EXISTS (SELECT * FROM sys.key_constraints WHERE [type] = 'PK' and   [parent_object_id] = Object_id('dbo.DIM_TransportModes') and name = 'PK_NewDIM_TransportModes_Code')  begin ALTER TABLE DIM_TransportModes DROP CONSTRAINT PK_NewDIM_TransportModes_Code end
 ALTER TABLE DIM_TransportModes ADD CONSTRAINT PK_DIM_TransportModes_Code PRIMARY KEY CLUSTERED (Code);



   --DIM_Users
 IF OBJECT_ID ('DIM_Users', 'U')  IS NOT NULL  and OBJECT_ID ('NewDIM_Users', 'U')  IS NOT NULL begin  EXEC sp_rename 'DIM_Users', 'OldDIM_Users' end
 IF OBJECT_ID ('NewDIM_Users', 'U')  IS NOT NULL begin EXEC sp_rename 'NewDIM_Users', 'DIM_Users' end
 IF OBJECT_ID ('OldDIM_Users', 'U')  IS NOT NULL begin drop table OldDIM_Users end
 IF  EXISTS (SELECT * FROM sys.key_constraints WHERE [type] = 'PK' and   [parent_object_id] = Object_id('dbo.DIM_Users') and name = 'PK_NewDIM_Users_Id_Number')  begin ALTER TABLE DIM_Users DROP CONSTRAINT PK_NewDIM_Users_Id_Number end
 ALTER TABLE DIM_Users ADD CONSTRAINT PK_DIM_Users_Id_Number PRIMARY KEY CLUSTERED (Id_Number);

 IF OBJECT_ID ('Fact_Shipments', 'U')  IS NOT NULL

 begin

 ALTER TABLE Fact_Shipments    ADD CONSTRAINT PK_Fact_Shipments_Id_Number PRIMARY KEY CLUSTERED (Id_Number);
 ALTER TABLE Fact_Shipments ADD CONSTRAINT FK_Fact_Shipment_DIM_Branches_Branch  FOREIGN KEY (Branch) REFERENCES DIM_Branches(Id_Number);
 ALTER TABLE Fact_Shipments ADD CONSTRAINT FK_Fact_Shipment_DIM_Partners_Shipper  FOREIGN KEY (Shipper) REFERENCES DIM_Partners(Id_Number);
 ALTER TABLE Fact_Shipments ADD CONSTRAINT FK_Fact_Shipment_DIM_Partners_Consignee  FOREIGN KEY (Consignee) REFERENCES DIM_Partners(Id_Number);
 ALTER TABLE Fact_Shipments ADD CONSTRAINT FK_Fact_Shipment_DIM_Partners_Agent  FOREIGN KEY (Agent) REFERENCES DIM_Partners(Id_Number);
 ALTER TABLE Fact_Shipments ADD CONSTRAINT FK_Fact_Shipment_DIM_Partners_Customer  FOREIGN KEY (Customer) REFERENCES DIM_Partners(Id_Number);
 ALTER TABLE Fact_Shipments ADD CONSTRAINT FK_Fact_Shipment_DIM_Currencies_LocalCurrency  FOREIGN KEY ([Local Currency]) REFERENCES DIM_Currencies(Id_Number);
 ALTER TABLE Fact_Shipments ADD CONSTRAINT FK_Fact_Shipment_DIM_Currencies_ProfitCurrency  FOREIGN KEY ([Profit Currency]) REFERENCES DIM_Currencies(Id_Number);
 ALTER TABLE Fact_Shipments ADD CONSTRAINT FK_Fact_Shipment_DIM_Departments_Department  FOREIGN KEY (Department) REFERENCES DIM_Departments(Id_Number);
 ALTER TABLE Fact_Shipments ADD CONSTRAINT FK_Fact_Shipment_DIM_Directions_Direction  FOREIGN KEY (Direction) REFERENCES DIM_Directions(Code);
 ALTER TABLE Fact_Shipments ADD CONSTRAINT FK_Fact_Shipment_DIM_ShipmentStatuses_Status  FOREIGN KEY (Status) REFERENCES DIM_ShipmentStatuses(Id_Number);
 ALTER TABLE Fact_Shipments ADD CONSTRAINT FK_Fact_Shipment_DIM_Incoterms_Incoterm  FOREIGN KEY (Incoterm) REFERENCES DIM_Incoterms(Id_Number);
 ALTER TABLE Fact_Shipments ADD CONSTRAINT FK_Fact_Shipment_DIM_Ports_Origin  FOREIGN KEY (Origin) REFERENCES DIM_Ports(Id_Number);
 ALTER TABLE Fact_Shipments ADD CONSTRAINT FK_Fact_Shipment_DIM_Ports_FinalDestination  FOREIGN KEY ([Final Destination]) REFERENCES DIM_Ports(Id_Number);
 ALTER TABLE Fact_Shipments ADD CONSTRAINT FK_Fact_Shipment_DIM_Levels_Level  FOREIGN KEY (Level) REFERENCES DIM_Levels(Code);
 ALTER TABLE Fact_Shipments ADD CONSTRAINT FK_Fact_Shipment_DIM_Types_Type  FOREIGN KEY (Type) REFERENCES DIM_Types(Code);
 ALTER TABLE Fact_Shipments ADD CONSTRAINT FK_Fact_Shipment_DIM_Tenants_SourceTenant  FOREIGN KEY ([Source Tenant]) REFERENCES DIM_Tenants([Tenant Number]);
 ALTER TABLE Fact_Shipments ADD CONSTRAINT FK_Fact_Shipment_DIM_Tenants_ParentTenant  FOREIGN KEY ([Parent Tenant]) REFERENCES DIM_Tenants([Tenant Number]);
 ALTER TABLE Fact_Shipments ADD CONSTRAINT FK_Fact_Shipment_DIM_TransportModes_TransportMode  FOREIGN KEY ([Transport Mode]) REFERENCES DIM_TransportModes(Code);
 ALTER TABLE Fact_Shipments ADD CONSTRAINT FK_Fact_Shipment_DIM_Users_Salesman  FOREIGN KEY (Salesman) REFERENCES DIM_Users(Id_Number);
 ALTER TABLE Fact_Shipments ADD CONSTRAINT FK_Fact_Shipment_DIM_Users_AccountManager  FOREIGN KEY ([Account Manager]) REFERENCES DIM_Users(Id_Number);
 ALTER TABLE Fact_Shipments ADD CONSTRAINT FK_Fact_Shipment_DIM_Dates_CustomsClearenceDate  FOREIGN KEY ([Customs Clearence Date]) REFERENCES DIM_Dates([Date Key]);
 ALTER TABLE Fact_Shipments ADD CONSTRAINT FK_Fact_Shipment_DIM_Dates_OperationalDate  FOREIGN KEY ([Operational Date]) REFERENCES DIM_Dates([Date Key]);
 ALTER TABLE Fact_Shipments ADD CONSTRAINT FK_Fact_Shipment_DIM_Dates_OperationalCloseDate  FOREIGN KEY ([Operational Close Date]) REFERENCES DIM_Dates([Date Key]);
 ALTER TABLE Fact_Shipments ADD CONSTRAINT FK_Fact_Shipment_DIM_Dates_AccountingCloseDate  FOREIGN KEY ([Accounting Close Date]) REFERENCES DIM_Dates([Date Key]);







 CREATE NONCLUSTERED INDEX [IX_Fact_Shipments_Id] ON[dbo].[Fact_Shipments]([Id]);

 end


