using Logitude.CargoTracking.BL.CargoTrackingServices.HelperClasses;
using Logitude.CargoTracking.BL.CargoTrackingServices.Services.ServicesHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CargoTracking.BL.CargoTrackingServices.Services.MainService
{
    public static class CargoTrackingQueriesService
	{

        public static string GetQuery(CargoTrackingUpdateDataBaseArgs cargoTrackingUpdateDataBaseArgs)
        {
			var shipmentOrdersCondition = "";
			var customShipmentCondition = "";
			var forwardingShipmentCondition = "";

			if (cargoTrackingUpdateDataBaseArgs.CargoTrackingArguments == null)
			{
				var LastUpdate = ServiceHelper.GetTableLastUpdate(cargoTrackingUpdateDataBaseArgs.BuildCargoArgs.Table.Main_CargoTracking_TableName, cargoTrackingUpdateDataBaseArgs.BuildCargoArgs.DestinationConnectionString);
				forwardingShipmentCondition = GetForwardingShipmentTableIncrementalConditions(LastUpdate);
				customShipmentCondition = GetCustomShipmentTableIncrementalConditions(LastUpdate);
				shipmentOrdersCondition = GetOrderShipmentTableIncrementalConditions(LastUpdate);
			}
			else
			{
				forwardingShipmentCondition = GetForwardingShipmentTableBuildConditions(cargoTrackingUpdateDataBaseArgs.CargoTrackingArguments);
				customShipmentCondition = GetCustomShipmentTableBuildConditions(cargoTrackingUpdateDataBaseArgs.CargoTrackingArguments);
				shipmentOrdersCondition = GetOrderShipmentTableBuildConditions(cargoTrackingUpdateDataBaseArgs.CargoTrackingArguments);
			}

			var query = $@"
				select * from (
					select 
					OrderTable.id as OrderId, 
					OrderTable.Tenant as OrderTenant , 
					OrderTable.ShipmentId as OrderForwardingShipmentHeaderId , 
					OrderTable.CustomerId as OrderCustomerId , 
					OrderTable.TransportModeId as OrderTransportModeId , 
					OrderTable.Master as OrderMaster , 
					OrderTable.House as OrderHouse , 
					OrderTable.OrderNumber as OrderShipmentNumber , 
					OrderTable.OriginPortId as OrderFromPortId , 
					OrderTable.DescriptionOfGoods as OrderDescriptionOfGoods ,
					OrderTable.SupplyDateTime as OrderSupplyDateTime ,
					OrderTable.DestinationPortId as OrderToPortId , 
					OrderTable.ShipperId as OrderShipperId , 
					OrderTable.ConsigneeId as OrderConsigneeId , 
					OrderTable.GrossWeight as OrderGrossWeight , 
					OrderTable.Volume as OrderVolume , 
					OrderTable.PickupActualDateTime as OrderPickupDate , 
					OrderTable.CreateDate as OrderCreateDate , 
					OrderTable.SecurityKey as OrderSecurityKey , 
					OrderTable.CustomerReferences as OrderCustomerReference , 
					OrderTable.PickupEstimatedDateTime as OrderPickupEstimationDate , 
					OrderTable.ATD as OrderDepartureDate , 
					OrderTable.ETD as OrderDepartureEstimationDate , 
					OrderTable.ATA as OrderArrivalDate , 
					OrderTable.ETA as OrderArrivalEstimationDate , 
					OrderTable.Quantity as OrderPackagesQuantity , 
					OrderTable.DirectionId as OrderDirectionId , 
					OrderTable.ShipmentLevelCode as OrderShipmentLevelCode , 
					OrderTable.ShipmentNumber as OrderForwardingShipmentNumber , 
					OrderTable.LastExceptionDate as OrderLastExceptionDate , 
					OrderTable.LastExceptionDescription as OrderLastExceptionDescription , 
					OrderTable.BookingConfirmationDate as OrderBookingDate , 
					OrderTable.UpdateDate as OrderAutomaticLastUpdateDate,
					(case 
						when OrderTable.DirectionId = 'E' AND OrderTable.ConsigneeId IS NOT NULL 
							then ConsigneeCardForOrder.EnglishName 
						when OrderTable.DirectionId = 'E' AND OrderTable.ConsigneeId IS NULL 
							then OrderTable.CasualSupplierName end
					) as OrderConsigneeName,
					(case 
						when OrderTable.DirectionId = 'I' AND OrderTable.ShipperId IS NOT NULL 
							then ShipperCardForOrder.EnglishName	
						when OrderTable.DirectionId = 'I' AND OrderTable.ShipperId IS NULL 
							then OrderTable.CasualImporterName end 
					) as OrderShipperName 
					
					from ShipmentOrders OrderTable
					LEFT OUTER JOIN dbo.Cards ConsigneeCardForOrder    ON ConsigneeCardForOrder.Id = ConsigneeId
					LEFT OUTER JOIN dbo.Cards ShipperCardForOrder    ON ShipperCardForOrder.Id = ShipperId
				
					where 
					
					{shipmentOrdersCondition}
				
				) as OrderShipmentTable
				full join 
				(
					select ForwardingShipmentTable.Id as ForwardingId, 
					ForwardingShipmentTable.Tenant as ForwardingTenant , 
					ForwardingShipmentTable.CustomFileId as ForwardingCustomsShipmentHeaderId , 
					ForwardingShipmentTable.CustomerId as ForwardingCustomerId , 
					ForwardingShipmentTable.TransportModeId as ForwardingTransportModeId , 
					ForwardingShipmentTable.House as ForwardingHouse , 
					ForwardingShipmentTable.ShipmentNumber as ForwardingShipmentNumber , 
					ForwardingShipmentTable.FromPortId as ForwardingFromPortId , 
					ForwardingShipmentTable.ToPortId as ForwardingToPortId , 
					ForwardingShipmentTable.ShipperId as ForwardingShipperId , 
					ForwardingShipmentTable.ConsigneeId as ForwardingConsigneeId , 
					ForwardingShipmentTable.GrossWeight as ForwardingGrossWeight , 
					ForwardingShipmentTable.VolumetricWeight as ForwardingVolume , 
					ForwardingShipmentTable.CreateDateTime as ForwardingCreateDate , 
					ForwardingShipmentTable.SecurityKey as ForwardingSecurityKey , 
					(case 
						when ForwardingShipmentTable.DirectionId = 'E' AND ForwardingShipmentTable.ConsigneeId IS NOT NULL 
							then ConsigneeCardForForward.EnglishName 
						when ForwardingShipmentTable.DirectionId = 'E' AND ForwardingShipmentTable.ConsigneeId IS NULL 
							then ForwardingShipmentTable.ConsigneeName end
					) as ForwardingConsigneeName,
					(case 
						when ForwardingShipmentTable.DirectionId = 'I' AND ForwardingShipmentTable.ShipperId IS NOT NULL 
							then ShipperCardForForward.EnglishName	
						when ForwardingShipmentTable.DirectionId = 'I' AND ForwardingShipmentTable.ShipperId IS NULL 
							then ForwardingShipmentTable.ShipperName end 
					) as ForwardingShipperName , 
					ForwardingShipmentTable.CustomerReference1 as ForwardingCustomerReference1 , 
					ForwardingShipmentTable.CustomerReference2 as ForwardingCustomerReference2 , 
					ForwardingShipmentTable.FirstPickupETD as ForwardingPickupEstimationDate , 
					ForwardingShipmentTable.WarehouseLegExpectedEntryDate as ForwardingFromWarehouseEstimationDate , 
					ForwardingShipmentTable.WarehouseLegRemarks as ForwardingFromWarehouseNotes , 
					ForwardingShipmentTable.WarehouseLegExpectedEntryDate  as ForwardingToWarehouseEstimationDate , 
					ForwardingShipmentTable.WarehouseLegRemarks  as ForwardingToWarehouseNotes , 
					ForwardingShipmentTable.FirstPickupETD as ForwardingFirstPickupETD , 
					ForwardingShipmentTable.WarehouseLegActualEntryDate as ForwardingWarehouseLegActualEntryDate , 
					ForwardingShipmentTable.DeclarationDate as ForwardingDeclarationDate , 
					ForwardingShipmentTable.PackagesQuantity as ForwardingPackagesQuantity , 
					ForwardingShipmentTable.DirectionId as ForwardingDirectionId , 
					ForwardingShipmentTable.ShipmentLevelCode as ForwardingShipmentLevelCode , 
					ForwardingShipmentTable.AssignedToTruckerDate  as ForwardingAssignedTruckerDate , 
					ForwardingShipmentTable.AssginedToCustomsAgentDate  as ForwardingAssignedCustomsAgentDate , 
					ForwardingShipmentTable.GrossWeightUnitCode as ForwardingGrossWeightUnitCode , 
					ForwardingShipmentTable.ShipmentTypeId as ForwardingShipmentTypeCode , 
					ForwardingShipmentTable.ExceptionDate as ForwardingExceptionDate , 
					ForwardingShipmentTable.LastExceptionDescription as ForwardingCurrentMilestoneExceptionDescription , 
					ForwardingShipmentTable.AutomaticLastUpdateDate as ForwardingAutomaticLastUpdateDate, 
					MasterTableForForward.ImportManifest as ForwardingImportManifest , 
					MasterTableForForward.MainCarriageATD as ForwardingDepartureDate , 
					MasterTableForForward.MainCarriageETD as ForwardingDepartureEstimationDate , 
					MasterTableForForward.MainCarriageATA  as ForwardingArrivalDate , 
					MasterTableForForward.MainCarriageETA  as ForwardingArrivalEstimationDate , 
					MasterTableForForward.Master as ForwardingMaster , 
					ComputedTableForForward.FinalDeliveryATD as ForwardingDeliveryDate , 
					ComputedTableForForward.FinalDeliveryETD  as ForwardingDeliveryEstimationDate , 
					ComputedTableForForward.ContainersNumbers as ForwardingContainersNumbers , 
					ComputedTableForForward.FinalDeliveryATA  as ForwardingDeliveredDate , 
					ComputedTableForForward.FinalDeliveryETA  as ForwardingDeliveredEstimationDate , 
					ComputedTableForForward.FirstPickupATD as ForwardingPickupDate , 
					CarrierCardForForward.LocalName as ForwardingCarrierLocalName , 
					CarrierCardForForward.EnglishName as ForwardingCarrierEnglishName 
				
					from Shipments as ForwardingShipmentTable
				
					LEFT OUTER JOIN dbo.ShipmentMasterDatas MasterTableForForward ON MasterTableForForward.Id = ForwardingShipmentTable.MasterShipmentDataId
					LEFT OUTER JOIN dbo.ShipmentComputedFields ComputedTableForForward ON ComputedTableForForward.Id = ForwardingShipmentTable.Id 
					LEFT OUTER JOIN dbo.ShipmentPickUpDeliveries ShipmentDeliveriesForForward    ON ShipmentDeliveriesForForward.ShipmentId = ForwardingShipmentTable.Id
					LEFT OUTER JOIN dbo.Cards CarrierCardForForward ON CarrierCardForForward.Id = ShipmentDeliveriesForForward.CarrierId
					LEFT OUTER JOIN dbo.Cards ConsigneeCardForForward    ON ConsigneeCardForForward.Id = ForwardingShipmentTable.ConsigneeId
					LEFT OUTER JOIN dbo.Cards ShipperCardForForward    ON ShipperCardForForward.Id = ForwardingShipmentTable.ShipperId
					
					where
					
					{forwardingShipmentCondition}
				
				) as  ForwardingShipmentTable1 on OrderShipmentTable.OrderForwardingShipmentHeaderId = ForwardingShipmentTable1.ForwardingId
				full join 
				(
					select CustomShipmentTable.Id as CustomId, 
					CustomShipmentTable.Tenant as CustomTenant , 
					CustomShipmentTable.CustomerId as CustomCustomerId , 
					CustomShipmentTable.TransportModeId as CustomTransportModeId , 
					CustomShipmentTable.House as CustomHouse  , 
					CustomShipmentTable.House as CustomsDeclarationNumber ,  
					CustomShipmentTable.ShipmentNumber as CustomShipmentNumber , 
					CustomShipmentTable.FromPortId as CustomFromPortId , 
					CustomShipmentTable.ToPortId as CustomToPortId , 
					CustomShipmentTable.ShipperId as CustomShipperId , 
					CustomShipmentTable.ConsigneeId as CustomConsigneeId , 
					CustomShipmentTable.GrossWeight as CustomGrossWeight , 
					CustomShipmentTable.VolumetricWeight as CustomVolume , 
					CustomShipmentTable.CreateDateTime as CustomCreateDate , 
					CustomShipmentTable.SecurityKey as CustomSecurityKey , 
					(case 
						when CustomShipmentTable.DirectionId = 'E' AND CustomShipmentTable.ConsigneeId IS NOT NULL 
							then ConsigneeCardForCustom.EnglishName 
						when CustomShipmentTable.DirectionId = 'E' AND CustomShipmentTable.ConsigneeId IS NULL 
							then CustomShipmentTable.ConsigneeName end
					) as CustomConsigneeName, 
					(case 
						when CustomShipmentTable.DirectionId = 'I' AND CustomShipmentTable.ShipperId IS NOT NULL 
							then ShipperCardForCustom.EnglishName 
						when CustomShipmentTable.DirectionId = 'I' AND CustomShipmentTable.ShipperId IS NULL 
							then CustomShipmentTable.ShipperName end
					) as ForwardingShipperName , 
					CustomShipmentTable.ShipperName as CustomShipperName , 
					CustomShipmentTable.CustomerReference1 as CustomCustomerReference1 , 
					CustomShipmentTable.CustomerReference2 as CustomCustomerReference2 , 
					CustomShipmentTable.FirstPickupETD as CustomPickupEstimationDate , 
					CustomShipmentTable.WarehouseLegExpectedEntryDate as CustomFromWarehouseEstimationDate , 
					CustomShipmentTable.WarehouseLegRemarks as CustomFromWarehouseNotes , 
					CustomShipmentTable.WarehouseLegExpectedEntryDate  as CustomToWarehouseEstimationDate , 
					CustomShipmentTable.WarehouseLegRemarks  as CustomToWarehouseNotes , 
					CustomShipmentTable.FirstPickupETD as CustomFirstPickupETD , 
					CustomShipmentTable.WarehouseLegActualEntryDate as CustomWarehouseLegActualEntryDate , 
					CustomShipmentTable.CustomsClearanceDate as CustomsClearanceDate , 
					CustomShipmentTable.PackagesQuantity as CustomPackagesQuantity , 
					CustomShipmentTable.DirectionId as CustomDirectionId , 
					CustomShipmentTable.ShipmentLevelCode as CustomShipmentLevelCode , 
					CustomShipmentTable.AssignedToTruckerDate  as CustomAssignedTruckerDate , 
					CustomShipmentTable.AssginedToCustomsAgentDate  as CustomAssignedCustomsAgentDate , 
					CustomShipmentTable.AutomaticLastUpdateDate as CustomAutomaticLastUpdateDate, 
					CustomShipmentTable.GrossWeightUnitCode as CustomGrossWeightUnitCode , 
					CustomShipmentTable.ExceptionDate as CustomExceptionDate , 
					CustomShipmentTable.LastExceptionDescription as CustomCurrentMilestoneExceptionDescription , 
					CustomShipmentTable.ShipmentTypeId as CustomShipmentTypeCode , 
					MasterTableForCustom.ImportManifest as CustomImportManifest, 
					MasterTableForCustom.MainCarriageATD as CustomDepartureDate , 
					MasterTableForCustom.MainCarriageETD as CustomDepartureEstimationDate , 
					MasterTableForCustom.MainCarriageATA  as CustomArrivalDate , 
					MasterTableForCustom.MainCarriageETA  as CustomArrivalEstimationDate , 
					MasterTableForCustom.Master as CustomMaster , 
					AdditionalDataTableForCustom.DocumentInspection as CustomDocumentInspectionDate , 
					AdditionalDataTableForCustom.GoodsClassification as CustomGoodsClassificationDate , 
					AdditionalDataTableForCustom.IsPaymentRequired as CustomIsPaymentRequired , 
					AdditionalDataTableForCustom.PaymentDateTime as CustomPaymentDateTime , 
					AdditionalDataTableForCustom.PaymentRequestDateTime as CustomPaymentReceivedDate , 
					AdditionalDataTableForCustom.GatepassDocumentsReady as CustomGatepassArrivedDate , 
					ComputedTableForCustom.FinalDeliveryATD as CustomDeliveryDate , 
					ComputedTableForCustom.FinalDeliveryETD  as CustomDeliveryEstimationDate , 
					ComputedTableForCustom.ContainersNumbers as CustomContainersNumbers , 
					ComputedTableForCustom.FinalDeliveryATA  as CustomDeliveredDate , 
					ComputedTableForCustom.FinalDeliveryETA  as CustomDeliveredEstimationDate , 
					ComputedTableForCustom.FirstPickupATD as CustomPickupDate , 
					CarrierCardForCustom.LocalName as CustomCarrierLocalName , 
					CarrierCardForCustom.EnglishName as CustomCarrierEnglishName
				
					from Shipments as CustomShipmentTable
				
					LEFT OUTER JOIN dbo.ShipmentMasterDatas MasterTableForCustom ON MasterTableForCustom.Id = CustomShipmentTable.MasterShipmentDataId
					LEFT OUTER JOIN dbo.ShipmentComputedFields ComputedTableForCustom ON ComputedTableForCustom.Id = CustomShipmentTable.Id 
					LEFT OUTER JOIN dbo.ShipmentAdditionalCloudDatas AdditionalDataTableForCustom    ON AdditionalDataTableForCustom.Id = CustomShipmentTable.Id
					LEFT OUTER JOIN dbo.ShipmentPickUpDeliveries ShipmentDeliveriesForCustom    ON ShipmentDeliveriesForCustom.ShipmentId = AdditionalDataTableForCustom.Id
					LEFT OUTER JOIN dbo.Cards CarrierCardForCustom ON CarrierCardForCustom.Id = ShipmentDeliveriesForCustom.CarrierId
					LEFT OUTER JOIN dbo.Cards ConsigneeCardForCustom    ON ConsigneeCardForCustom.Id = CustomShipmentTable.ConsigneeId
					LEFT OUTER JOIN dbo.Cards ShipperCardForCustom    ON ShipperCardForCustom.Id = CustomShipmentTable.ShipperId
				
					where
				
					{customShipmentCondition}
				
				) as CustomShipmentTable1 on ForwardingShipmentTable1.ForwardingCustomsShipmentHeaderId = CustomShipmentTable1.CustomId  


            ";
			return query;
        }


		private static string GetForwardingShipmentTableBuildConditions(CargoTrackingArguments cargoTrackingArguments)
		{
			var tenant = cargoTrackingArguments.Tenant;
			var fromDate = cargoTrackingArguments.FromDate;
			var toDate = cargoTrackingArguments.ToDate;
			var tenantCondishin = "";
			if (tenant != null)
			{
				tenantCondishin = $"ForwardingShipmentTable.Tenant = {tenant} and";
			}
			var condishins = $@"
            
				ForwardingShipmentTable.ShipmentLevelCode != 'A' and
				ForwardingShipmentTable.ShipmentLevelCode != 'C' and 
				ForwardingShipmentTable.IsCancelled != 1 and 
				ForwardingShipmentTable.CreateDateTime >= '{fromDate.Value.Date.ToString("MM/dd/yyyy hh:mm:ss.fff tt")}' and 
				ForwardingShipmentTable.CreateDateTime <= '{toDate.Value.Date.ToString("MM/dd/yyyy hh:mm:ss.fff tt")}'
                {tenantCondishin}
			
            ";
			return condishins;
		}

		private static string GetCustomShipmentTableBuildConditions(CargoTrackingArguments cargoTrackingArguments)
		{
			var tenant = cargoTrackingArguments.Tenant;
			var fromDate = cargoTrackingArguments.FromDate;
			var toDate = cargoTrackingArguments.ToDate;
			var tenantCondishin = "";
			if (tenant != null)
			{
				tenantCondishin = $"CustomShipmentTable.Tenant = {tenant}  and ";
			}
			var condishins = $@"
            
				CustomShipmentTable.ShipmentLevelCode = 'A' and
				CustomShipmentTable.IsCancelled != 1 and 
				CustomShipmentTable.CreateDateTime >= '{fromDate.Value.Date.ToString("MM/dd/yyyy hh:mm:ss.fff tt")}' and 
				CustomShipmentTable.CreateDateTime <= '{toDate.Value.Date.ToString("MM/dd/yyyy hh:mm:ss.fff tt")}'
                {tenantCondishin}
			
            ";
			return condishins;
		}

		private static string GetOrderShipmentTableBuildConditions(CargoTrackingArguments cargoTrackingArguments)
		{
			var tenant = cargoTrackingArguments.Tenant;
			var fromDate = cargoTrackingArguments.FromDate;
			var toDate = cargoTrackingArguments.ToDate;
			var tenantCondishin = "";
			if (tenant != null)
			{
				tenantCondishin = $"OrderTable.Tenant = {tenant}  and ";
			}
			var condishins = $@"
            
				OrderTable.CreateDate >= '{fromDate.Value.Date.ToString("MM/dd/yyyy hh:mm:ss.fff tt")}' and
			    OrderTable.CreateDate <= '{toDate.Value.Date.ToString("MM/dd/yyyy hh:mm:ss.fff tt")}'
                {tenantCondishin}
			
            ";
			return condishins;
		}

		private static string GetCustomShipmentTableIncrementalConditions(string lastUpdate)
		{
			var condishins = $@"
            
				CustomShipmentTable.ShipmentLevelCode = 'A' and
				CustomShipmentTable.IsCancelled != 1 and 
                CustomShipmentTable.AutomaticLastUpdateDate > '{lastUpdate}'
			
            ";
			return condishins;
		}

		private static string GetForwardingShipmentTableIncrementalConditions(string lastUpdate)
		{

			var condishins = $@"
            
				ForwardingShipmentTable.ShipmentLevelCode != 'A' and
				ForwardingShipmentTable.ShipmentLevelCode != 'C' and 
				ForwardingShipmentTable.IsCancelled != 1 and 
				ForwardingShipmentTable.AutomaticLastUpdateDate > '{lastUpdate}'
			
            ";
			return condishins;
		}
		private static string GetOrderShipmentTableIncrementalConditions(string lastUpdate)
		{

			var condishins = $@"
            
				OrderTable.UpdateDate > '{lastUpdate}'
			
            ";
			return condishins;
		}

		public static string GetDeleteSearchesQuery(List<CargoTrackingShipmentResources> cargoTrackingShipments, string tableName)
        {
			if (cargoTrackingShipments.Count <= 0)
				return "";
			var ids = "";
			foreach (var item in cargoTrackingShipments)
			{
				ids += $"'{item.CargoTrackingShipment.EntityId}' ,";
			}
			ids = ids.Substring(0,ids.Length - 1);
			var query = $@"
			delete from {tableName} where ShipmentId  in 
			(
				{ids}
			)
			";
            
			return query;

		}
	}
}
