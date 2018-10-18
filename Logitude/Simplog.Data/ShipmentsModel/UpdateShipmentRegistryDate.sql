
IF OBJECT_ID('[dbo].[usp_UpdateShipmentRegistryDate]', 'P') IS NOT NULL
drop PROCEDURE [dbo].[usp_UpdateShipmentRegistryDate]
GO

Create PROCEDURE [dbo].[usp_UpdateShipmentRegistryDate]
(
	@ShipmentId varchar(15)
)
AS

if (@ShipmentId is not null)
BEGIN
	declare @Tenant as int
	declare @HouseId as varchar(15)
	declare @MasterDataId as varchar(15)
	declare @ShipmentLevelCode as varchar(1)
	declare @RegistryDate_Master as DateTime
	declare @ARInvoiceId as varchar(15)
	declare @ApprovedDate as DateTime
	declare @IsConstituent as bit
	declare @StatusCode as varchar(2)
	declare @ConsolidationInvoiceId as varchar(15)
	declare @FirstApprovalDate as DateTime

	select 
	@Tenant = Tenant,
	@MasterDataId = MasterShipmentDataId,
	@ShipmentLevelCode = ShipmentLevelCode
	from Shipments where Id = @ShipmentId

	set @FirstApprovalDate = null

	-- Get the First Approval Date 
	-- for the Shipment from its own invoices
	BEGIN
		DECLARE ARInvoiceEntitiesCursor CURSOR READ_ONLY
		FOR
		SELECT ARInvoices.Id, ARInvoices.ApprovedDate, ARInvoices.IsConstituentInvoice, ARInvoices.StatusCode, ARInvoices.ConsolidationInvoiceId
		FROM ARInvoiceEntities
		JOIN ARInvoices on ARInvoiceEntities.ARInvoiceId = ARInvoices.Id
		WHERE ARInvoiceEntities.EntityId = @ShipmentId
		AND ARInvoiceEntities.Tenant = @Tenant
		AND ARInvoices.Tenant = @Tenant
		AND ARInvoices.IsAutoCredit = 0
		AND ARInvoices.StatusCode != 'DR'
		AND ARInvoices.StatusCode != 'LL'
		AND ARInvoices.StatusCode != 'VD'
		AND ARInvoices.StatusCode != 'NT'
		AND ARInvoices.StatusCode != 'AC'
		OPEN ARInvoiceEntitiesCursor FETCH NEXT FROM ARInvoiceEntitiesCursor INTO @ARInvoiceId, @ApprovedDate, @IsConstituent, @StatusCode, @ConsolidationInvoiceId
		WHILE @@FETCH_STATUS = 0
		BEGIN

			if (@IsConstituent = 1 AND @StatusCode = 'CN' AND @ConsolidationInvoiceId is not null)
			BEGIN
				set @ApprovedDate = (select ApprovedDate from ARInvoices 
									where IsConsolidationInvoice = 1
									AND Id = @ConsolidationInvoiceId
									AND IsAutoCredit = 0
									AND StatusCode != 'DR'
									AND StatusCode != 'LL'
									AND StatusCode != 'VD'
									AND StatusCode != 'AC'
									)			
			END			

			if (@ApprovedDate is not null)
			BEGIN
				if (@FirstApprovalDate is null)
				set @FirstApprovalDate = @ApprovedDate

				else if (@FirstApprovalDate > @ApprovedDate)
				set @FirstApprovalDate = @ApprovedDate
			END

		FETCH NEXT FROM ARInvoiceEntitiesCursor INTO @ARInvoiceId, @ApprovedDate, @IsConstituent, @StatusCode, @ConsolidationInvoiceId
		END
		CLOSE ARInvoiceEntitiesCursor
		DEALLOCATE ARInvoiceEntitiesCursor
	END

	if (@ShipmentLevelCode = 'H' AND @MasterDataId is not null)
	BEGIN
		if exists (select * from ShipmentMasterDatas where Id = @MasterDataId AND ProrateReceivables = 1)
		begin
			set @RegistryDate_Master = (select RegistryDate from Shipments where Id = @MasterDataId AND Tenant = @Tenant)
			if (@RegistryDate_Master is not null)
			begin
				if (@FirstApprovalDate is null)
				set @FirstApprovalDate = @RegistryDate_Master

				else if (@FirstApprovalDate > @RegistryDate_Master)
				set @FirstApprovalDate = @RegistryDate_Master
			end
		end
	END

	update Shipments set RegistryDate = @FirstApprovalDate where Id = @ShipmentId AND Tenant = @Tenant

	if (@ShipmentLevelCode = 'C')
	BEGIN
		DECLARE ShipmentsCursor CURSOR READ_ONLY
		FOR
		SELECT Id
		FROM Shipments
		WHERE ShipmentLevelCode = 'H' AND MasterShipmentDataId = @ShipmentId
		OPEN ShipmentsCursor FETCH NEXT FROM ShipmentsCursor INTO @HouseId
		WHILE @@FETCH_STATUS = 0
		BEGIN
		EXECUTE [usp_UpdateShipmentRegistryDate] @HouseId
		FETCH NEXT FROM ShipmentsCursor INTO @HouseId
		END
		CLOSE ShipmentsCursor
		DEALLOCATE ShipmentsCursor			
	END
END









